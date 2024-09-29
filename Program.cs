using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using System.IO;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Настройка доступа к статическим файлам
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "wwwroot")),
    RequestPath = ""
});

// Обработка GET запросов для HTML файлов
app.MapGet("/{fileName}", async (HttpContext context, string fileName) =>
{
    var filePath = Path.Combine(builder.Environment.ContentRootPath, "wwwroot", $"{fileName}.html");

    if (File.Exists(filePath))
    {
        var fileContent = await File.ReadAllTextAsync(filePath);
        context.Response.ContentType = "text/html";
        await context.Response.WriteAsync(fileContent);
    }
    else
    {
        context.Response.StatusCode = 404;
        var errorFilePath = Path.Combine(builder.Environment.ContentRootPath, "wwwroot", "404.html");
        var errorContent = await File.ReadAllTextAsync(errorFilePath);
        context.Response.ContentType = "text/html";
        await context.Response.WriteAsync(errorContent);
    }
});

app.Run();