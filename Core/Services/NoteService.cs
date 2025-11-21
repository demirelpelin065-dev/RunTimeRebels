using System;
using System.Threading.Tasks;

public class NoteService
{
    private readonly NoteSpeechRecognitionService _speechRecognitionService;
    private readonly IDiskIO _diskIO;

    public NoteService(IDiskIO diskIO, TimeSpan recognitionTimeout)
    {
        _diskIO = diskIO;
        _speechRecognitionService = new NoteSpeechRecognitionService(diskIO, recognitionTimeout);
    }

    public async Task<string> StartNewSessionAsync(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Session title cannot be empty", nameof(title));

        try
        {
            Console.WriteLine($"Starting new session: {title}");
            return await _speechRecognitionService.StartNewSessionAsync(title);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error starting session: {ex.Message}");
            throw;
        }
    }

    public async Task<string> RecordToSessionAsync()
    {
        try
        {
            Console.WriteLine("Recording...");
            return await _speechRecognitionService.RecordToCurrentSessionAsync();
        }
        catch (TimeoutException tex)
        {
            Console.WriteLine($"Recording timeout: {tex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error recording: {ex.Message}");
            throw;
        }
    }

    public async Task<string> EndSessionAsync()
    {
        try
        {
            Console.WriteLine("Ending session...");
            return await _speechRecognitionService.EndCurrentSessionAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error ending session: {ex.Message}");
            throw;
        }
    }

    public string GetCurrentSessionInfo()
    {
        return _speechRecognitionService.GetCurrentSessionInfo();
    }

    public async Task<string[]> SearchSessionsAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return Array.Empty<string>();
            
        return await _speechRecognitionService.SearchSessionsAsync(searchTerm);
    }
}