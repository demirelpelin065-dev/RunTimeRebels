using System;

namespace CLI
{
    internal class MenuHelper
    {
        public void MainMenu()
        {
            bool isOn = true; // Creates a bool that is used in the while loop below. As long as this is true, the loop continues. When set to false, it breaks out of the loop.

            while (isOn)
            {
                // This is the Main Menu, what the user will see when they run the program
                Console.WriteLine("1. Settings Menu");
                Console.WriteLine("2. Display Note Repository");
                Console.WriteLine("3. Recording Studio");
                Console.WriteLine("Q. Quit the program");

                string userSelected = Console.ReadLine().Trim().ToUpper(); // Waits for user input and stores it as a string called userSelected. Trim removes extra spaces and ToUpper makes input case-insensitive.

                // A switch case that compares what is stored in userSelected with the cases below.
                // When it finds a match, it executes the code and then breaks out of the switch. Default runs if nothing matches.
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
                        isOn = false; // Sets isOn to false and breaks the while loop, quitting the program.
                        break;

                    default:
                        Console.Clear();
                        Console.WriteLine("Ivalid Input. Please try again, enter the number (or Q to quit the program) shown below");
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

                string userSelected = Console.ReadLine().Trim().ToUpper();

                switch (userSelected)
                {
                    case "1":
                        // Placeholder: logic for starting recording goes here
                        break;

                    case "2":
                        // Placeholder: logic for stopping recording goes here
                        break;

                    case "3":
                        Console.WriteLine("Back to prev menu"); // Inform the user they are returning
                        isOn = false; // Exit this menu
                        break;

                    default:
                        Console.WriteLine("Invalid input. Please try again.");
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
                Console.WriteLine("1. Display Note Repository");
                Console.WriteLine("2. Back to previous menu");

                string userSelected = Console.ReadLine().Trim().ToUpper();

                switch (userSelected)
                {
                    case "1":
                        Console.WriteLine("Shows Repository"); // Placeholder: code to display repository goes here
                        break;

                    case "2":
                        isOn = false; // Go back to previous menu
                        break;

                    default:
                        Console.WriteLine("Invalid input. Please try again.");
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
                Console.WriteLine("1. Change Settings");
                Console.WriteLine("2. Back to previous Menu");

                string userSelected = Console.ReadLine().Trim().ToUpper();

                switch (userSelected)
                {
                    case "1":
                        // Placeholder: logic for changing settings goes here
                        break;

                    case "2":
                        isOn = false; // Go back to previous menu
                        break;

                    default:
                        Console.WriteLine("Invalid input. Please try again.");
                        break;
                }
            }
        }
    }
}
