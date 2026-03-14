using System.Text;
using Microsoft.AspNetCore.Http;
using AlAshmar.Domain.Commons;
using FileManager.Abstractions;

namespace FileManager.Services.Abstraction;




public class FileMetadata
{
    public string FilePath { get; set; } = string.Empty;
    public string OriginalFileName { get; set; } = string.Empty;
    public string StoredFileName { get; set; } = string.Empty;
    public string FileExtension { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string ContentType { get; set; } = string.Empty;
    public string FileType { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
}




public class CleanupResult
{
    public int FilesDeleted { get; set; }
    public long BytesFreed { get; set; }
    public List<string> Errors { get; set; } = new();
}




public interface IFilesManagerService
{







    Task<Result<FileMetadata>> SaveFileAsync(IFormFile file, string? subDirectory = null, string? customFileName = null);






    Task<Result<byte[]>> GetFileBytesAsync(string filePath);






    Task<Result<Stream>> GetFileStreamAsync(string filePath);






    Task<Result<Success>> DeleteFileAsync(string filePath);






    new bool FileExists(string filePath);






    Result<FileMetadata> GetFileInfo(string filePath);






    new string GetContentType(string fileExtension);






    new string GetFileTypeFromExtension(string fileExtension);







    bool IsValidFileExtension(string fileExtension, string[]? allowedExtensions = null);






    new string GenerateSafeFileName(string originalFileName);




    string UploadsDirectory { get; }






    Task<Result<CleanupResult>> CleanupOldFilesAsync(int olderThanDays = 30);
}
