using System.IO.Compression;
using Archiver.Web.Services.Interfaces;
using Microsoft.AspNetCore.Components.Forms;

namespace Archiver.Web.Services.Implementations;

public class FileService : IFileService
{
    private readonly HttpClient _client;
    private const string CompressPath = "File/compress";
    private const string DecompressPath = "File/decompress";

    public FileService(HttpClient client)
    {
        _client = client;
    }

    public async Task<string> SendFileAsync(MultipartFormDataContent content, CompressionMode mode)
    {
        HttpResponseMessage? message = null;
        switch (mode)
        {
            case CompressionMode.Compress:
                message = await _client.PostAsync(CompressPath, content);
                break;
            case CompressionMode.Decompress:
                message = await _client.PostAsync(DecompressPath, content);
                break;
                
        }

        if (message is not null && message.IsSuccessStatusCode)
            return await message.Content.ReadAsStringAsync();

        return string.Empty;
    }


    public async Task DownloadFileAsync(string fileName)
    {
        var result = await _client.GetAsync($"File/download/{fileName}");

        if (result.IsSuccessStatusCode)
        {
            var bytes = await result.Content.ReadAsByteArrayAsync();
        }
    }
}