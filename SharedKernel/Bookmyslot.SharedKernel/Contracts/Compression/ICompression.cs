namespace Bookmyslot.SharedKernel.Contracts.Compression
{
    public interface ICompression
    {
        T Decompress<T>(string key);

        string Compress<T>(T model);
    }
}
