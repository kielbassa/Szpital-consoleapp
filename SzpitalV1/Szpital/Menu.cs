namespace Program
{
    public class Menu
    {
        SoundEffects sound = new SoundEffects();
        ASCII_Graphics graphics = new ASCII_Graphics();

        private int windowHeight = 30;
        int windowWidth = 100; // wartosc 100 zapewnia poprawne centrowanie interfejsu

        public void ConsoleRefresh() // zamiast Console.Clear(), żeby nie było mrugania interfejsu
        {
            Console.ResetColor();
            SetWindowSize();
            Console.SetCursorPosition(0, 0);
        }
        public void SetWindowSize()  // ustawianie odpowieniego rozmiaru okna konsoli ze sprawdzeniem czy nie jest wiekszy od dostepnego ekranu
        {
            
            int maxWindowHeight = Console.LargestWindowHeight;
            int maxWindowWidth = Console.LargestWindowWidth;
            if(maxWindowHeight > windowHeight || maxWindowWidth > windowWidth)
            {
                Console.SetWindowSize(windowWidth, windowHeight);
            }
            else
            {
                Console.WriteLine("Skala konsoli nie może być ustawiona");
                Environment.Exit(1);
            }
        }
        private void LoginScreenGraphic()
        {
            Console.Title = "Aplikacja Szpital";
            graphics.MainLogo();
        }

        private int SelectedIndex;
        private string[] Options;
        private string Prompt;

        public Menu(string prompt, string[] options)
        {
            Prompt = prompt;
            Options = options;
            SelectedIndex = 0;
        }

        private void DisplayOptions()
        {
            Console.WriteLine(Prompt);
            Console.WriteLine();
            for(int indexer = 0; indexer < Options.Length; indexer++)
            {
                if(indexer == SelectedIndex)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                }

                string CurrentOption = Options[indexer];
                Console.WriteLine($"<< {CurrentOption} >>");
            }

        }
        public int Run()
        {
            ConsoleKey KeyPressed;
            do
            {
                ConsoleRefresh();
                LoginScreenGraphic();
                DisplayOptions();

                ConsoleKeyInfo KeyInfo= Console.ReadKey(true);
                KeyPressed=KeyInfo.Key;

                // odźwież wybrany index opcji SelectedIndex na podstawie klawiszy strzałek.

                if (KeyPressed == ConsoleKey.DownArrow)
                {
                    sound.CycleOption();
                    SelectedIndex++;
                    if (SelectedIndex == Options.Length)
                    {
                        SelectedIndex = 0;
                    }
                }else if (KeyPressed == ConsoleKey.UpArrow)
                {
                    sound.CycleOption();
                    SelectedIndex--;
                    if (SelectedIndex == -1)
                    {
                        SelectedIndex = Options.Length - 1;
                    }
                }

            } while (KeyPressed != ConsoleKey.Enter);
            sound.StartJingle();
            return SelectedIndex;
        }
    }
}
