using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public class DiskIO : IDiskIO
{
    public async Task<string> ReadAllTextAsync(string filePath)
    {
        try
        {
            using var reader = File.OpenText(filePath);
            return await reader.ReadToEndAsync();
        }
        catch (Exception ex) when (IsFileSystemException(ex))
        {
            throw new IOException($"Failed to read file: {filePath}", ex);
        }
    }

    public async Task WriteAllTextAsync(string filePath, string content)
    {
        try
        {
            var directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory))
            {
                await CreateDirectoryAsync(directory);
            }
            
            using var writer = new StreamWriter(filePath);
            await writer.WriteAsync(content);
        }
        catch (Exception ex) when (IsFileSystemException(ex))
        {
            throw new IOException($"Failed to write file: {filePath}", ex);
        }
    }

    public async Task<bool> FileExistsAsync(string filePath)
    {
        return await Task.Run(() => File.Exists(filePath));
    }

    public async Task<string[]> GetFilesAsync(string directoryPath, string searchPattern)
    {
        return await Task.Run(() => 
        {
            if (!Directory.Exists(directoryPath))
                return Array.Empty<string>();
                
            return Directory.GetFiles(directoryPath, searchPattern);
        });
    }

    public async Task CreateDirectoryAsync(string directoryPath)
    {
        await Task.Run(() => 
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        });
    }

    public async Task<string> CreateNewSessionFileAsync(string directoryPath, string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty", nameof(title));

        await CreateDirectoryAsync(directoryPath);

        string fileName = await GenerateUniqueFileNameAsync(directoryPath, title, ".txt");
        string filePath = Path.Combine(directoryPath, fileName);

        // Create file with title header
        string header = $"TITLE: {title}\n";
        string sessionHeader = $"SESSION START: {DateTime.Now:yyyy-MM-dd HH:mm:ss}\n";
        string separator = new string('=', 50) + "\n\n";
        
        string initialContent = header + sessionHeader + separator;
        
        await WriteAllTextAsync(filePath, initialContent);
        
        return filePath;
    }

    public async Task<string> GenerateUniqueFileNameAsync(string directoryPath, string title, string extension = ".txt")
    {
        return await Task.Run(() =>
        {
            // Clean the title for filename use
            string cleanTitle = CleanFileName(title);
            
            // Try the base name first
            string baseFileName = $"{cleanTitle}{extension}";
            string baseFilePath = Path.Combine(directoryPath, baseFileName);
            
            if (!File.Exists(baseFilePath))
                return baseFileName;

            // If base name exists, append timestamp
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string timestampFileName = $"{cleanTitle}_{timestamp}{extension}";
            string timestampFilePath = Path.Combine(directoryPath, timestampFileName);
            
            if (!File.Exists(timestampFilePath))
                return timestampFileName;

            // If timestamp exists (unlikely), append counter
            int counter = 1;
            while (true)
            {
                string counterFileName = $"{cleanTitle}_{timestamp}_{counter:00}{extension}";
                string counterFilePath = Path.Combine(directoryPath, counterFileName);
                
                if (!File.Exists(counterFilePath))
                    return counterFileName;
                    
                counter++;
                
                // Safety check to prevent infinite loop
                if (counter > 1000)
                    throw new InvalidOperationException("Could not generate unique filename after 1000 attempts");
            }
        });
    }

    public async Task AppendToSessionFileAsync(string filePath, string content)
    {
        try
        {
            using var writer = new StreamWriter(filePath, append: true);
            await writer.WriteAsync(content);
        }
        catch (Exception ex) when (IsFileSystemException(ex))
        {
            throw new IOException($"Failed to append to file: {filePath}", ex);
        }
    }

    public async Task<string> ReadSessionFileAsync(string filePath)
    {
        return await ReadAllTextAsync(filePath);
    }

    private string CleanFileName(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            return "untitled";

        // Remove invalid file name characters
        var invalidChars = Path.GetInvalidFileNameChars();
        var cleanName = new string(fileName
            .Where(ch => !invalidChars.Contains(ch))
            .ToArray());

        // Replace spaces with underscores and limit length
        cleanName = cleanName.Replace(' ', '_');
        
        // Trim to reasonable length
        if (cleanName.Length > 50)
            cleanName = cleanName.Substring(0, 50);
            
        // Ensure not empty after cleaning
        if (string.IsNullOrWhiteSpace(cleanName))
            cleanName = "untitled";

        return cleanName.Trim();
    }

    private bool IsFileSystemException(Exception ex)
    {
        return ex is IOException ||
               ex is UnauthorizedAccessException ||
               ex is ArgumentException ||
               ex is PathTooLongException ||
               ex is DirectoryNotFoundException ||
               ex is NotSupportedException;
    }
}