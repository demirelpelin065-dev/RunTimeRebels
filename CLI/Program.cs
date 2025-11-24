using System;

namespace CLI.UI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Säkerställ layout mot aktuell konsolstorlek
            Layout.Initialize();

            var ui = new UIManager();
            ui.MainMenu();
        }
    }
}
