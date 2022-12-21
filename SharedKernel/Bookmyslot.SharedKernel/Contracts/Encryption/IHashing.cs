namespace Bookmyslot.SharedKernel.Contracts.Encryption
{
    public interface IHashing
    {
        public string Create(string message);
    }
}
