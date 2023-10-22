using Archiver.Api.Dtos;
using Archiver.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Archiver.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class FileController: ControllerBase
{
    private readonly IArchiverService _service;

    public FileController(IArchiverService service)
    {
        _service = service;
    }

    [HttpPost("compress")]
    [ProducesResponseType(typeof(File), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CompressAsync([FromForm] UploadFileDto uploadFile)
    {
        var result = await _service.Compress(uploadFile);

        if (!result.IsSuccess)
            return BadRequest($"Something went wrong: {result.Errors.FirstOrDefault()}");

        var filePath = result.Value;
        return Ok(filePath);
        // return File(file.FileContents, file.ContentType);
    }
    
    [HttpPost("decompress")]
    [ProducesResponseType(typeof(File), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DecompressAsync([FromForm] UploadFileDto uploadFile)
    {
        var result = await _service.Decompress(uploadFile);

        if (!result.IsSuccess)
            return BadRequest($"Something went wrong: {result.Errors.FirstOrDefault()}");

        var filePath = result.Value;
        return Ok(filePath);
        // return File(file.FileContents, file.ContentType);
    }

    [HttpGet("download/{fileName}")]
    [ProducesResponseType(typeof(File), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DownloadFile([FromRoute] string fileName)
    {
        var result = await _service.DownloadFile(fileName);

        if (!result.IsSuccess)
            return BadRequest($"Something went wrong: {result.Errors.FirstOrDefault()}");
        var file = result.Value;
        return File(file.FileContents, file.ContentType);
    }
}