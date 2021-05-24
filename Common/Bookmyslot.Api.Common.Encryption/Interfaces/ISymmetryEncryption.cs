namespace Bookmyslot.Api.Common.Encryption.Interfaces
{
    public interface ISymmetryEncryption
    {
        string Encrypt(string message);
        string Decrypt(string encryptedMessage);
    }
}
