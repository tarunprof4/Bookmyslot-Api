namespace Bookmyslot.Api.Common.Compression.Interfaces
{
    public interface ICompression
    {
        T Decompress<T>(string key);

        string Compress<T>(T model);
    }
}
