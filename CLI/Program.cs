using Spectre.Console;
using System;

namespace VoiceJournalSpectre
{
    class Program
    {
        static void Main(string[] args)
        {
            AnsiConsole.Markup("[cyan]Loading Voice Journal...[/]\n");

            var ui = new UIManager();
            ui.MainMenu();
        }
    }

    public class UIManager
    {
        public void MainMenu()
        {
            while (true)
            {
                Console.Clear();
                AsciiRenderer.ShowBanner();

                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[yellow]Main Menu[/]")
                        .AddChoices("* Studio", "* Notes", "* Settings", "* Quit"));

                switch (choice)
                {
                    case "* Studio":
                        StudioMenu();
                        break;

                    case "* Notes":
                        NoteMenu();
                        break;

                    case "* Settings":
                        SettingsMenu();
                        break;

                    case "* Quit":
                        AnsiConsole.Markup("[green]Goodbye![/]\n");
                        return;
                }
            }
        }

        // ---------------- STUDIO MENU ----------------

        public void StudioMenu()
        {
            Console.Clear();
            AsciiRenderer.ShowSubBanner("Studio");

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("[green]Select mode[/]")
                .AddChoices("* Manual Recording", "* Voice Activated", "* Back"));

            if (choice == "* Manual Recording")
                ManualRecording();

            if (choice == "* Voice Activated")
                VoiceActivated();
        }

        private void ManualRecording()
        {
            Console.Clear();
            AsciiRenderer.ShowSubBanner("Manual Recording");

            var panel = new Panel("Press ENTER to simulate Start/Stop recording.");
            panel.Border = BoxBorder.Rounded;
            AnsiConsole.Write(panel);

            Console.ReadLine();
            Console.ReadLine();

            AnsiConsole.Markup("[green]Recording stopped![/]\n");
            Console.ReadLine();
        }

        private void VoiceActivated()
        {
            Console.Clear();
            AsciiRenderer.ShowSubBanner("Voice Activated Mode");

            AnsiConsole.Markup("[blue]Listening (mock)... Press ENTER to stop.[/]\n");
            Console.ReadLine();
        }

        // ---------------- NOTES MENU ----------------

        private void NoteMenu()
        {
            Console.Clear();
            AsciiRenderer.ShowSubBanner("Notes");

            var table = new Table();
            table.AddColumn("Title");
            table.AddColumn("Preview");

            table.AddRow("Note 1", "This is a placeholder note");
            table.AddRow("Note 2", "Another example note");

            AnsiConsole.Write(table);

            AnsiConsole.Markup("[grey]Press ENTER to go back.[/]");
            Console.ReadLine();
        }

        // ---------------- SETTINGS MENU ----------------

        private void SettingsMenu()
        {
            Console.Clear();
            AsciiRenderer.ShowSubBanner("Settings");

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("[yellow]Choose a setting[/]")
                .AddChoices("* Set save path", "* Model selection", "* Back"));

            if (choice == "* Set save path")
            {
                var path = AnsiConsole.Ask<string>("Enter new save path:");
                AnsiConsole.Markup($"[green]Saved:[/] {path}\n");
                Console.ReadLine();
            }
        }
    }

    // ---------------- ASCII RENDERER ----------------

    public static class AsciiRenderer
    {
        public static void ShowBanner()
        {
            AnsiConsole.Write(
                new FigletText("Voice Journal")
                .Centered()
                .Color(Spectre.Console.Color.Aqua)
            );
        }

        public static void ShowSubBanner(string text)
        {
            AnsiConsole.Write(
                new FigletText(text)
                .Centered()
                .Color(Spectre.Console.Color.Yellow)
            );
        }
    }
}
