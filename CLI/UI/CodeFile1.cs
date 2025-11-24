using Spectre.Console;
using System;
using System.Collections.Generic;
using System.IO;

namespace CLI.UI
{
    public class UIManager
    {
        // ---------------- SNOW ENGINE ----------------
        private SnowEngine snow;  // <--- Added

        private string SavePath = "notes/";
        private string SelectedModel = "None";

        // ---------------- MAIN MENU ----------------
        public void MainMenu()
        {
            Directory.CreateDirectory(SavePath);

            // Start snow engine
            snow = new SnowEngine(maxFlakes: 30, bottomClearRows: 6);
            snow.Start();

            while (true)
            {
                // Clear only snow first to avoid ghost trails
                snow.ClearBeforeUIClear();
                Console.Clear();

                AsciiRenderer.ShowBanner();
                snow.DrawSnowman(); // optional decoration

                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[yellow]Main Menu[/]")
                        .AddChoices("* Studio", "* Notes", "* Settings", "* Quit"));

                switch (choice)
                {
                    case "* Studio": StudioMenu(); break;
                    case "* Notes": NoteMenu(); break;
                    case "* Settings": SettingsMenu(); break;
                    case "* Quit":
                        snow.Stop();
                        AnsiConsole.Markup("[green]Goodbye![/]\n");
                        return;
                }
            }
        }

        // ---------------- STUDIO MENU ----------------
        public void StudioMenu()
        {
            while (true)
            {
                snow.ClearBeforeUIClear();
                Console.Clear();
                AsciiRenderer.ShowSubBanner("Studio");
                snow.DrawSnowman();

                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[green]Select mode[/]")
                        .AddChoices("* Manual Recording", "* Voice Activated", "* Back"));

                if (choice == "* Manual Recording") ManualRecording();
                if (choice == "* Voice Activated") VoiceActivated();
                if (choice == "* Back") return;
            }
        }

        private void ManualRecording()
        {
            snow.ClearBeforeUIClear();
            Console.Clear();
            AsciiRenderer.ShowSubBanner("Manual Recording");
            snow.DrawSnowman();

            var panel = new Panel("Press ENTER to simulate Start/Stop recording.");
            panel.Border = BoxBorder.Rounded;
            AnsiConsole.Write(panel);

            AnsiConsole.Markup("[blue]Recording...[/]\n");
            Console.ReadLine();
            Console.ReadLine();

            AnsiConsole.Markup("[green]Recording stopped! Saved note placeholder.[/]\n");
            Console.ReadLine();
        }

        private void VoiceActivated()
        {
            snow.ClearBeforeUIClear();
            Console.Clear();
            AsciiRenderer.ShowSubBanner("Voice Activated Mode");
            snow.DrawSnowman();

            AnsiConsole.Markup("[blue]Listening (mock)... Press ENTER to stop.[/]\n");
            Console.ReadLine();
            AnsiConsole.Markup("[red]Stopped listening.[/]\n");
            Console.ReadLine();
        }

        // ---------------- NOTES MENU ----------------
        private void NoteMenu()
        {
            while (true)
            {
                snow.ClearBeforeUIClear();
                Console.Clear();
                AsciiRenderer.ShowSubBanner("Notes");
                snow.DrawSnowman();

                var files = Directory.GetFiles(SavePath, "*.txt");
                var fileList = new List<string>();

                fileList.Add("Create New Note");
                foreach (var file in files)
                    fileList.Add(Path.GetFileName(file));
                fileList.Add("Back");

                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[yellow]Choose a note[/]")
                        .AddChoices(fileList));

                if (choice == "Create New Note") CreateNote();
                else if (choice == "Back") return;
                else OpenNote(choice);
            }
        }

        private void CreateNote()
        {
            snow.ClearBeforeUIClear();
            Console.Clear();
            AsciiRenderer.ShowSubBanner("New Note");
            snow.DrawSnowman();

            var text = AnsiConsole.Ask<string>("Write your note:");
            var fileName = $"{SavePath}note_{DateTime.Now.Ticks}.txt";

            File.WriteAllText(fileName, text);

            AnsiConsole.Markup("[green]Saved![/]\n");
            Console.ReadLine();
        }

        private void OpenNote(string fileName)
        {
            snow.ClearBeforeUIClear();
            Console.Clear();
            AsciiRenderer.ShowSubBanner(fileName);
            snow.DrawSnowman();

            var content = File.ReadAllText(Path.Combine(SavePath, fileName));
            AnsiConsole.Write(new Panel(content));

            var action = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Choose action")
                    .AddChoices("Delete", "Back"));

            if (action == "Delete")
            {
                File.Delete(Path.Combine(SavePath, fileName));
                AnsiConsole.Markup("[red]Deleted.[/]\n");
                Console.ReadLine();
            }
        }

        // ---------------- SETTINGS ----------------
        private void SettingsMenu()
        {
            while (true)
            {
                snow.ClearBeforeUIClear();
                Console.Clear();
                AsciiRenderer.ShowSubBanner("Settings");
                snow.DrawSnowman();

                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[yellow]Choose a setting[/]")
                        .AddChoices("* Set save path", "* Model selection", "* Back"));

                if (choice == "* Set save path")
                {
                    var path = AnsiConsole.Ask<string>("Enter new save path:");
                    SavePath = path.EndsWith("/") ? path : path + "/";
                    Directory.CreateDirectory(SavePath);
                    AnsiConsole.Markup($"[green]Saved path:[/] {SavePath}\n");
                    Console.ReadLine();
                }
                else if (choice == "* Model selection")
                {
                    SelectModel();
                }
                else if (choice == "* Back") return;
            }
        }

        private void SelectModel()
        {
            var model = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[green]Choose model[/]")
                    .AddChoices("Llama2-7B", "Llama3-8B", "Custom", "Back"));

            if (model == "Back") return;

            SelectedModel = model;
            AnsiConsole.Markup($"[green]Model selected:[/] {model}\n");
            Console.ReadLine();
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
                    .Color(Color.Aqua));
        }

        public static void ShowSubBanner(string text)
        {
            AnsiConsole.Write(
                new FigletText(text)
                    .Centered()
                    .Color(Color.Yellow));
        }
    }
}
