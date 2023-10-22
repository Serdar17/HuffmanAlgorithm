namespace Archiver.Api.Dtos;

public class DownloadFileDto
{
    /// <summary>
    /// Downloadable file
    /// </summary>
    public IFormFile File { get; set; }
}