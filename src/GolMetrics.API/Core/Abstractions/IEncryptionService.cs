namespace GolMetrics.API.Core.Abstractions;

public interface IEncryptionService
{
    string Encrypt(string plainText);
    string Decrypt(string cipherText);
}