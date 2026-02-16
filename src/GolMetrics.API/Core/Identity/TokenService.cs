using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using GolMetrics.API.Core.Abstractions;
using GolMetrics.API.Core.Authorization;
using GolMetrics.API.Core.Persistence;
using GolMetrics.API.Core.Results;
using GolMetrics.API.Features.Auth;
using GolMetrics.API.Features.UserManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace GolMetrics.API.Core.Identity;

internal sealed class TokenService(
    IConfiguration configuration,
    GolMetricsDbContext dbContext) : ITokenService
{
    private static readonly string[] AllPermissions =
    [
        Permissions.Conversations.Read,
        Permissions.Conversations.Write,
        Permissions.Users.Read,
        Permissions.Users.Write
    ];

    public string GenerateAccessToken(User user)
    {
        var tokenOptions = configuration.GetSection("TokenOptions");
        var secretKey = tokenOptions["SecretKey"]!;
        var issuer = tokenOptions["Issuer"];
        var audience = tokenOptions["Audience"];

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email!)
        };

        foreach (var permission in AllPermissions)
        {
            claims.Add(new Claim("permissions", permission));
        }

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<string> GenerateRefreshTokenAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var tokenValue = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        var refreshToken = new RefreshTokenEntity
        {
            Id = Guid.NewGuid(),
            Token = tokenValue,
            UserId = userId,
            ExpiresAtUtc = DateTime.UtcNow.AddDays(30),
            IsRevoked = false,
            CreatedBy = userId,
            CreatedAtUtc = DateTime.UtcNow
        };

        dbContext.RefreshTokens.Add(refreshToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return tokenValue;
    }

    public async Task<Result<Guid>> ValidateRefreshTokenAsync(string token,
        CancellationToken cancellationToken = default)
    {
        var refreshToken = await dbContext.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == token, cancellationToken);

        if (refreshToken is null || refreshToken.IsRevoked || refreshToken.ExpiresAtUtc <= DateTime.UtcNow)
        {
            return Result<Guid>.Failure(AuthErrors.InvalidRefreshToken);
        }

        return refreshToken.UserId;
    }

    public async Task RevokeRefreshTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        var refreshToken = await dbContext.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == token, cancellationToken);

        if (refreshToken is not null)
        {
            refreshToken.IsRevoked = true;
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}