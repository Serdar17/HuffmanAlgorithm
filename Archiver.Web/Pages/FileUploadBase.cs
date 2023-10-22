using System.IO.Compression;
using System.Net.Http.Headers;
using Archiver.Web.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace Archiver.Web.Pages;

public class FileUploadBase : ComponentBase, IAsyncDisposable
{
    [Inject] public IFileService FileService { get; set; }
    [Inject] private IJSRuntime JSRuntime { get; set; }

    private IJSObjectReference? _filePasteModule;
    private IJSObjectReference? _filePasteFunctionReference;

    protected string HiddenButton = "hidden-content";
    [Parameter] public CompressionMode Mode { get; set; }
    
    protected ElementReference FileDropContainer;
    protected InputFile InputFile;
    protected string HoverClass;
    protected string ArchiveFileName;
    protected bool IsLoading;
    protected string ErrorMessage = String.Empty;
    protected string BaseAddress = "http://localhost:5252";

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _filePasteModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./js/filePaste.js");
            _filePasteFunctionReference = await _filePasteModule.InvokeAsync<IJSObjectReference>(
                "initializeFilePaste",
                FileDropContainer, 
                InputFile.Element);
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_filePasteFunctionReference is not null)
        {
            await _filePasteFunctionReference.InvokeVoidAsync("dispose");
            await _filePasteFunctionReference.DisposeAsync();
        }

        if (_filePasteModule is not null)
        {
            await _filePasteModule.DisposeAsync();
        }
    }

    protected async Task LoadFiles(InputFileChangeEventArgs e)
    {
        IsLoading = true;
        try
        {
            var file = e.GetMultipleFiles(1).FirstOrDefault();
            var content = new MultipartFormDataContent();
            var fileContent = new StreamContent(file.OpenReadStream());
            // fileContent.Headers.ContentType = new MediaTypeHeaderValue(GetContentTypeFromFile(file));
            content.Add(fileContent, "file", file.Name);
            ArchiveFileName = await FileService.SendFileAsync(content, Mode);
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            return;
        }

        if (string.IsNullOrEmpty(ArchiveFileName))
        {
            ErrorMessage = "Произошла ошибка: Не удалось разархивировать файл";
            IsLoading = false;
            return;
        }
        
        HiddenButton = string.Empty;
        IsLoading = false;
    }

    protected async Task OnClick()
    {
        try
        {
            await FileService.DownloadFileAsync("f683b5d2-031a-49f2-b9e4-bc9d36d91723.huffman");

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    protected void OnDragEnter(DragEventArgs e) => HoverClass = "hover";
    
    protected void OnDragLeave(DragEventArgs e) => HoverClass = string.Empty;
}