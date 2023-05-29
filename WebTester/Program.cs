using Microsoft.AspNetCore.Mvc;
using WebTester.Repositories;
using WebTester.Repositories.Entities;
using WebTester.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IFileSizeRepository, FileSizeRepository>(provider => 
{ return new FileSizeRepository(SizeLists.FileSizes); });
builder.Services.AddTransient<IChunkSizeRepository, ChunkSizeRepository>(provider =>
{ return new ChunkSizeRepository(SizeLists.ChunkSizes); });

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();

app.MapFallback(() =>
{
    return Results.BadRequest("Invalid request.");
});

app.MapGet("/download-data", async ([FromQuery] short fileSizeId,
    [FromQuery] short chunkSizeId,
    [FromServices] IFileSizeRepository fileSizeRepository,
    [FromServices] IChunkSizeRepository chunkSizeRepository,
    HttpContext context) =>
{
    var fileSize = fileSizeRepository.GetById(fileSizeId);
    var chunkSize = chunkSizeRepository.GetById(chunkSizeId);
    long dataSizeInBytes = (long)fileSize.SizeInMb * 1024 * 1024; //1024 bytes = 1KB; 1KB * 1024 = 1MB
    long chunkSizeInBytes = (long)chunkSize.SizeInKb * 1024; 

    byte[] data = new byte[chunkSizeInBytes]; // create a buffer of 64KB

    context.Response.ContentType = "application/octet-stream";
    context.Response.Headers.Add("Content-Disposition", "attachment; filename=data.bin");
    context.Response.ContentLength = dataSizeInBytes;

    long bytesWritten = 0;

    while (bytesWritten < dataSizeInBytes)
    {
        await context.Response.Body.WriteAsync(data, 0, data.Length); // write the buffer to the response stream
        await context.Response.Body.FlushAsync(); // flush the data to the client
        bytesWritten += data.Length;
    }
});

app.MapGet("/file-sizes", ([FromServices]IFileSizeRepository fileSizeRepository) =>
{
    return fileSizeRepository.GetAll();
});

app.MapGet("/chunk-sizes", ([FromServices] IChunkSizeRepository chunkSizeRepository) =>
{
    return chunkSizeRepository.GetAll();
});


app.Run();