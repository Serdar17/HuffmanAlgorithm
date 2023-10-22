using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Archiver.Web;
using Archiver.Web.Services.Implementations;
using Archiver.Web.Services.Interfaces;

var baseAddress = "http://localhost:5252";
var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(baseAddress) });

builder.Services.AddScoped<IFileService, FileService>();

await builder.Build().RunAsync();