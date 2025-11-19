using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace CLI
{
    internal class MenuHelper
    {
        public void MainMenu()
        {
            bool isOn = true;
            while (isOn)
            {
                Console.WriteLine("1. Settings Menu"); // fixed typo
                Console.WriteLine("2. Display Note Repository");
                Console.WriteLine("3. Recording Studio");
                Console.WriteLine("Q. Quit the program"); // fixed typo
                string userSelected = Console.ReadLine();

                switch (userSelected)
                {
                    case "1":
                        SettingsMenu();
                        break;

                    case "2":
                        RepositoryView();
                        break;

                    case "3":
                        RecordingStudio();
                        break;

                    case "Q":
                    case "q":
                        isOn = false;
                        break;

                    default:
                        break;
                }
            }
        }

        public void RecordingStudio()
        {
            bool isOn = true;
            while (isOn)
            {
                Console.WriteLine("1. Start Recording");
                Console.WriteLine("2. Stop Recording");
                Console.WriteLine("3. Back to previous Menu");

                string userSelected = Console.ReadLine();

                switch (userSelected)
                {
                    case "1":
                        break;

                    case "2":
                        break;

                    case "3":
                        Console.WriteLine("Back to prev menu");
                        isOn = false;
                        break;
                }
            }
        }

        public void RepositoryView()
        {
            bool isOn = true;
            while (isOn)
            {
                Console.WriteLine("Display Note Repository");
                Console.WriteLine("1. Display Note Repository"); // fixed typo
                Console.WriteLine("2. Back to previous menu ");

                string userSelected = Console.ReadLine();

                switch (userSelected)
                {
                    case "1":
                        Console.WriteLine("Shows Repository");
                        break;

                    case "2":
                        isOn = false;
                        break;

                    default:
                        break;
                }
            }
        }

        public void SettingsMenu()
        {
            bool isOn = true;
            while (isOn)
            {
                Console.WriteLine("Settings Menu");
                Console.WriteLine("1. Change Settings"); // fixed typo
                Console.WriteLine("2. Back to previous Menu");

                string userSelected = Console.ReadLine();

                switch (userSelected)
                {
                    case "1":
                        break;

                    case "2":
                        isOn = false;
                        break;

                    default:
                        break;
                }
            }
        }
    }
}
