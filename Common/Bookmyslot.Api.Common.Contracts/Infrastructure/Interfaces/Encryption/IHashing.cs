namespace Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Encryption
{
    public interface IHashing
    {
        public string Create(string message);
    }
}
