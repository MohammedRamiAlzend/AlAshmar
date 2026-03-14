using FileManager.Models;

namespace FileManager.Abstractions;




public interface IFileManager
{



    string RootDirectory { get; }

    #region File Operations








    Task<FileOperationResult> CopyFileAsync(string sourcePath, string destinationPath, bool overwrite = false);








    Task<FileOperationResult> MoveFileAsync(string sourcePath, string destinationPath, bool overwrite = false);







    Task<FileOperationResult> RenameFileAsync(string currentPath, string newName);






    Task<FileOperationResult> DeleteFileAsync(string filePath);






    Task<FileContentResult> ReadFileAsync(string filePath);








    Task<FileOperationResult> WriteFileAsync(string filePath, byte[] content, bool createDirectory = true);






    bool FileExists(string filePath);






    Task<FileInfoResult> GetFileInfoAsync(string filePath);

    #endregion

    #region Directory Operations






    Task<DirectoryOperationResult> CreateDirectoryAsync(string directoryPath);








    Task<DirectoryOperationResult> CopyDirectoryAsync(string sourcePath, string destinationPath, bool overwrite = false);







    Task<DirectoryOperationResult> MoveDirectoryAsync(string sourcePath, string destinationPath);







    Task<DirectoryOperationResult> RenameDirectoryAsync(string currentPath, string newName);







    Task<DirectoryOperationResult> DeleteDirectoryAsync(string directoryPath, bool recursive = false);






    bool DirectoryExists(string directoryPath);






    Task<DirectoryInfoResult> GetDirectoryInfoAsync(string directoryPath);

    #endregion

    #region Search and Listing








    Task<ListingResult> ListDirectoryAsync(string directoryPath, string? searchPattern = null, bool includeSubdirectories = false);








    Task<SearchResult> SearchFilesAsync(string searchPath, FileSearchCriteria searchCriteria, SearchOption searchOption = SearchOption.TopDirectoryOnly);

    #endregion

    #region Utilities






    string GetContentType(string fileExtension);






    string GetFileTypeFromExtension(string fileExtension);







    bool IsValidFileExtension(string fileExtension, IEnumerable<string>? allowedExtensions = null);






    string GenerateSafeFileName(string originalFileName);







    Task<CleanupResult> CleanupOldFilesAsync(int olderThanDays = 30, string? cleanupPattern = null);

    #endregion
}