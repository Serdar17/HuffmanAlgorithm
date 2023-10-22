namespace Archiver.Api.Dtos;

public class UploadFileDto
{
    /// <summary>
    /// Downloadable file
    /// </summary>
    public IFormFile File { get; set; }
}