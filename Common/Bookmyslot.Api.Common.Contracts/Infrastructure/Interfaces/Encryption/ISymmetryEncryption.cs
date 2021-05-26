namespace Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Encryption
{
    public interface ISymmetryEncryption
    {
        string Encrypt(string message);
        string Decrypt(string encryptedMessage);
    }
}
