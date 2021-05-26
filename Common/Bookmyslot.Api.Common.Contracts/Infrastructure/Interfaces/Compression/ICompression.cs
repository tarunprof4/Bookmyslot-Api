namespace Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Compression
{
    public interface ICompression
    {
        T Decompress<T>(string key);

        string Compress<T>(T model);
    }
}
