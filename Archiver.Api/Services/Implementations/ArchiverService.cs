using Archiver.Api.Dtos;
using Archiver.Api.HuffmanAlgorithm;
using Archiver.Api.HuffmanAlgorithm.Intrefaces;
using Archiver.Api.Services.Interfaces;
using Ardalis.Result;

namespace Archiver.Api.Services.Implementations;

public class ArchiverService : IArchiverService
{
    private readonly HuffmanCompressor _huffman = new HuffmanCompressor();

    
    public async Task<Result<string>> Compress(UploadFileDto file)
    {
        try
        {
            var bytes = new List<byte>();
            using (var br = new BinaryReader(file.File.OpenReadStream()))
                for (var i = 0; i < br.BaseStream.Length; i++)
                    bytes.Add(br.ReadByte());
            var archiveFileName = $"{Guid.NewGuid()}.{file.File.FileName.Split('.')[^1]}.huffman";
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "FileStorage", archiveFileName);
            _huffman.Compress(bytes.ToArray(), filePath);

            return archiveFileName;
        }
        catch (Exception e)
        {
            return Result.Error(e.Message);
        }
    }

    public async Task<Result<string>> Decompress(UploadFileDto file)
    {
        try
        {
            var bytes = new List<byte>();
            using (var br = new BinaryReader(file.File.OpenReadStream()))
                for (var i = 0; i < br.BaseStream.Length; i++)
                    bytes.Add(br.ReadByte());

            var parts = file.File.FileName.Split('.');
            var archiveFileName = $"{parts[0]}.{parts[^2]}";
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "FileStorage", archiveFileName);
            _huffman.Decompress(bytes.ToArray(), filePath);

            return archiveFileName;
        }
        catch (Exception e)
        {
            return Result.Error(e.Message);
        }
    }

    public async Task<Result<CustomFileDto>> DownloadFile(string filePath)
    {
        var path = Path.Combine("Filestorage", filePath);
        var bytes = await File.ReadAllBytesAsync(path);
        return new CustomFileDto(
            bytes,
            GetContentType(filePath),
            filePath);
    }

    private string GetContentType(string path)
    {
        var types = GetMimeTypes();
        var ext = Path.GetExtension(path).ToLowerInvariant();
        return types[ext];
    }

    private Dictionary<string, string> GetMimeTypes()
    {
        return new Dictionary<string, string>
        {
            {".txt", "text/plain"},
            {".huffman", "text/plain"},
            {".pdf", "application/pdf"},
            {".doc", "application/vnd.ms-word"},
            {".docx", "application/vnd.ms-word"},
            {".xls", "application/vnd.ms-excel"},
            {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
            {".png", "image/png"},
            {".jpg", "image/jpeg"},
            {".jpeg", "image/jpeg"},
            {".gif", "image/gif"},
            {".csv", "text/csv"}
        };
    }
}