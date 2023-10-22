namespace Archiver.Api.Dtos;

public class CustomFileDto
{
    public CustomFileDto(byte[] fileContents, string contentType, string fileName)
    {
        FileContents = fileContents;
        ContentType = contentType;
        FileName = fileName;
    }
    
    /// <summary>
    /// Array of bytes file
    /// </summary>
    public byte[] FileContents { get; set; }
    
    /// <summary>
    /// File content type
    /// </summary>
    public string ContentType { get; set; }
    
    /// <summary>
    /// File name
    /// </summary>
    public string FileName { get; set; }
}