using System.Threading.Tasks;

public interface IDiskIO
{
    Task<string> ReadAllTextAsync(string filePath);
    Task WriteAllTextAsync(string filePath, string content);
    Task<bool> FileExistsAsync(string filePath);
    Task<string[]> GetFilesAsync(string directoryPath, string searchPattern);
    Task CreateDirectoryAsync(string directoryPath);

    Task<string> CreateNewSessionFileAsync(string directoryPath, string title);
    
    Task<string> GenerateUniqueFileNameAsync(string directoryPath, string title, string extension = ".txt");
    Task AppendToSessionFileAsync(string filePath, string content);
    Task<string> ReadSessionFileAsync(string filePath);
}