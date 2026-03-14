using FileManager.Abstractions;
using FileManager.Core;
using FileManager.Services;
using FileManager.Services.Abstraction;
using Microsoft.Extensions.DependencyInjection;

namespace FileManager.Extensions;




public static class FileManagerServiceCollectionExtensions
{






    public static IServiceCollection AddFileManager(this IServiceCollection services, string? rootDirectory = null)
    {
        services.AddSingleton<IFileManager>(provider => new EnhancedFileManager(rootDirectory));
        services.AddScoped<IFilesManagerService, FilesManagerService>();
        return services;
    }







    public static IServiceCollection AddFileManager(this IServiceCollection services, Action<FileManagerOptions> configureOptions)
    {
        var options = new FileManagerOptions();
        configureOptions(options);

        services.AddSingleton<IFileManager>(provider => new EnhancedFileManager(options.RootDirectory));
        services.AddScoped<IFilesManagerService, FilesManagerService>();
        return services;
    }
}




public class FileManagerOptions
{



    public string? RootDirectory { get; set; }




    public List<string> AllowedExtensions { get; set; } = new();




    public long MaxFileSize { get; set; } = 100 * 1024 * 1024;




    public bool EnableAutoCleanup { get; set; } = false;




    public int CleanupOlderThanDays { get; set; } = 30;
}