using Core.Interfaces;
using Core.Services;
using Infrastructure.Repositories;

namespace CLI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Ange sökvägen till JSON-filen i Infrastructure/Storage
            var storagePath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "..", "..", "..",
                "Infrastructure", "Storage", "notes.json"
            );

            storagePath = Path.GetFullPath(storagePath);

            // Repository (data access)
            INoteRepository repo = new JsonNoteRepository(storagePath);

            // Services (business logic)
            var noteService = new NoteService(repo);

            // UI (Spectre.Console menus, etc.)
            var menu = new MenuHelper(noteService);

            menu.MainMenu();
        }
    }
}