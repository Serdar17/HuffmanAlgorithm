using Archiver.Api.HuffmanAlgorithm;
using Archiver.Api.HuffmanAlgorithm.Intrefaces;
using Archiver.Api.Services.Implementations;
using Archiver.Api.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IArchiverService, ArchiverService>();
// builder.Services.AddScoped<ICompressor, HuffmanCompressor>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("EnableCORS", cfg => 
    { 
        cfg.WithOrigins("*")
            .AllowAnyHeader()
            .AllowAnyMethod(); 
    });
});
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("EnableCORS");
// app.UseAuthorization();

app.MapControllers();

app.Run();