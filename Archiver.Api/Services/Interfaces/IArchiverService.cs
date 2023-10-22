using Archiver.Api.Dtos;
using Ardalis.Result;

namespace Archiver.Api.Services.Interfaces;

public interface IArchiverService
{
    Task<Result<string>> Compress(UploadFileDto file);

    Task<Result<string>> Decompress(UploadFileDto file);

    Task<Result<CustomFileDto>> DownloadFile(string filePath);
}