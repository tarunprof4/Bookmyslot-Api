namespace Bookmyslot.SharedKernel.Contracts.Encryption
{
    public interface ISymmetryEncryption
    {
        string Encrypt(string message);
        string Decrypt(string encryptedMessage);
    }
}
