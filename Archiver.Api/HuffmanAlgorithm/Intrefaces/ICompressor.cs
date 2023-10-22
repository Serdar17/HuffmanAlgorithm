namespace Archiver.Api.HuffmanAlgorithm.Intrefaces;

public interface ICompressor
{
    void Compress(byte[] bytes, string archName);
    void Decompress(byte[] bytes, string fileName);
}