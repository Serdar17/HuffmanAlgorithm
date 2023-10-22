using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace Archiver.Web.Pages;

public class CompressorBase : ComponentBase, IAsyncDisposable
{
    [Inject] private IJSRuntime JSRuntime { get; set; }

    private IJSObjectReference _filePasteModule;
    private IJSObjectReference _filePasteFunctionReference;
    protected string HoverClass;
    protected ElementReference FileDropContainer;
    protected InputFile InputFile;
    protected string TextFromFile;
    protected bool IsLoading;

    protected async Task WriteFile(InputFileChangeEventArgs e)
    {
        IsLoading = true;
        var file = e.GetMultipleFiles(1).FirstOrDefault();

        byte[] bytes;
        using (var fileContent = new StreamContent(file.OpenReadStream()))
            bytes = await fileContent.ReadAsByteArrayAsync();

        TextFromFile = Encoding.Default.GetString(bytes.ToArray());
        IsLoading = false;
    }
    
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
    
    protected void OnDragEnter(DragEventArgs e) => HoverClass = "hover";
    
    protected void OnDragLeave(DragEventArgs e) => HoverClass = string.Empty;
}