using System.IO.Compression;

namespace Archiver.Web.Services.Interfaces;

public interface IFileService
{
    Task<string> SendFileAsync(MultipartFormDataContent content, CompressionMode mode);

    Task DownloadFileAsync(string fileName);
}