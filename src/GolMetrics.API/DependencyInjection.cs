using System.Text;
using FluentValidation;
using GolMetrics.API.Core.Abstractions;
using GolMetrics.API.Core.Authorization;
using GolMetrics.API.Core.Behaviors;
using GolMetrics.API.Core.Exceptions;
using GolMetrics.API.Core.Extensions;
using GolMetrics.API.Core.Identity;
using GolMetrics.API.Core.Persistence;
using GolMetrics.API.Features.FootballData;
using GolMetrics.API.Features.UserManagement;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.SemanticKernel;
using Serilog;

namespace GolMetrics.API;

public static class DependencyInjection
{
    extension(WebApplicationBuilder builder)
    {
        public WebApplicationBuilder AddApiServices()
        {
            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });

            builder.Services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
            builder.Services.AddSlices();
            builder.Services.AddOpenApi();
            builder.Services.AddSingleton(TimeProvider.System);
            builder.Host.UseSerilog((context, configuration) =>
                configuration.ReadFrom.Configuration(context.Configuration));

            return builder;
        }

        public WebApplicationBuilder AddDatabase()
        {
            builder.Services.AddScoped<AuditableEntityInterceptor>();

            builder.Services.AddDbContext<GolMetricsDbContext>((sp, options) =>
                options
                    .UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
                    .UseSnakeCaseNamingConvention()
                    .AddInterceptors(sp.GetRequiredService<AuditableEntityInterceptor>()));

            return builder;
        }

        public WebApplicationBuilder AddAuthenticationServices()
        {
            builder.Services.AddIdentity<User, IdentityRole<Guid>>()
                .AddEntityFrameworkStores<GolMetricsDbContext>()
                .AddDefaultTokenProviders();

            var tokenOptions = builder.Configuration.GetSection("TokenOptions");

            builder.Services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = tokenOptions["Issuer"],
                        ValidAudience = tokenOptions["Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(tokenOptions["SecretKey"]!))
                    };
                });

            builder.Services.AddAuthorization();
            builder.Services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
            builder.Services.AddScoped<ITokenService, TokenService>();

            return builder;
        }

        public WebApplicationBuilder AddErrorHandling()
        {
            builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
            builder.Services.AddExceptionHandler<DatabaseExceptionHandler>();
            builder.Services.AddExceptionHandler<JsonExceptionHandler>();
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddProblemDetails();

            return builder;
        }

        public WebApplicationBuilder AddCors()
        {
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            return builder;
        }

        public WebApplicationBuilder AddSemanticKernel()
        {
            var geminiSection = builder.Configuration.GetSection("Gemini");
            var modelId = geminiSection["ModelId"] ?? "gemini-2.0-flash";
            var apiKey = geminiSection["ApiKey"]!;

            builder.Services.AddSingleton(_ =>
            {
                var kernelBuilder = Kernel.CreateBuilder();
                kernelBuilder.AddGoogleAIGeminiChatCompletion(modelId, apiKey);
                return kernelBuilder.Build();
            });

            return builder;
        }

        public WebApplicationBuilder AddEncryptionServices()
        {
            builder.Services.AddSingleton<IEncryptionService>(sp =>
            {
                var encryptionKey = builder.Configuration["Encryption:Key"]!;
                return ActivatorUtilities.CreateInstance<EncryptionService>(sp, encryptionKey);
            });

            return builder;
        }

        public WebApplicationBuilder AddFootballServices()
        {
            builder.Services.AddHttpClient<IFootballApiClient, FootballApiClient>(client =>
            {
                var baseUrl = builder.Configuration["ApiFootball:BaseUrl"]
                              ?? "https://v3.football.api-sports.io";
                client.BaseAddress = new Uri(baseUrl);
            });

            builder.Services.AddScoped<ICacheService, CacheService>();

            return builder;
        }
    }
}