namespace Program
{
    public class Menu
    {
        private readonly int windowHeight = 30;
        private readonly int windowWidth = 100; // wartosc 100 zapewnia poprawne centrowanie interfejsu
        private int SelectedIndex;
        private readonly string Prompt;

        private readonly Dictionary<int, MenuEntry> options; // indeksowany słownik z wierszami menu


        // Domyślny konstruktor
        public Menu(string prompt)
        {
            this.options = new Dictionary<int, MenuEntry>();
            this.Prompt = prompt;
            this.SelectedIndex = 0;
        }

        public Menu(string prompt, string[] captions)
        {
            this.options = new Dictionary<int, MenuEntry>();
            foreach (string caption in captions)
            {
                MenuEntry row = new MenuEntry(caption);
                this.options.Add(this.options.Count, row);
            }
            this.Prompt = prompt;
            this.SelectedIndex = 0;
        }

        // Konstruktor menu dla kolekcji
        public Menu(string prompt, Tuple<string, int>[] tuples)
        {
            this.options = new Dictionary<int, MenuEntry>();
            foreach (Tuple<string, int> t in tuples)
            {
                this.AddRow(t.Item1, t.Item2);
            }

            this.Prompt = prompt;
            this.SelectedIndex = 0;
        }

#pragma warning disable CA1416
        private void SetWindowSize()  // ustawianie odpowieniego rozmiaru okna konsoli ze sprawdzeniem czy nie jest wiekszy od dostepnego ekranu
        {

            int maxWindowHeight = Console.LargestWindowHeight;
            int maxWindowWidth = Console.LargestWindowWidth;
            if (maxWindowHeight > windowHeight || maxWindowWidth > windowWidth)
            {
                Console.SetWindowSize(windowWidth, windowHeight);
            }
            else
            {
                Console.WriteLine("Skala konsoli nie może być ustawiona");
                Environment.Exit(1);
            }
        }
        public void ConsoleRefresh() // zamiast Console.Clear(), żeby nie było mrugania interfejsu
        {
            Console.ResetColor();
            
            // safeguard w razie uruchamiania programu na innym systemie niż windows
            if (System.OperatingSystem.IsWindows())
            {
                SetWindowSize();
            }

            Console.SetCursorPosition(0, 0);
        }
#pragma warning restore CA1416

        private void DisplayOptions()
        {
            Console.WriteLine(Prompt);
            Console.WriteLine();
            for (int indexer = 0; indexer < options.Count; indexer++)
            {
                if (indexer == SelectedIndex)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                }

                string CurrentOption = options[indexer].caption;
                Console.WriteLine($"<< {CurrentOption} >>");
            }

        }

        public int AddRow(string caption, int value = -1)
        {
            int index = this.options.Count;
            MenuEntry row = new MenuEntry(caption, value);

            this.options.Add(index, row);

            return index;
        }

        public void ReplaceRow(int index, MenuEntry row)
        {
            this.options[index] = row;
        }

        public int Run()
        {
            Console.CursorVisible = false;
            ConsoleKey KeyPressed;
            do
            {
                ConsoleRefresh();
                DisplayOptions();

                ConsoleKeyInfo KeyInfo = Console.ReadKey(true);
                KeyPressed = KeyInfo.Key;

                // odźwież wybrany index opcji SelectedIndex na podstawie klawiszy strzałek.

                if (KeyPressed == ConsoleKey.DownArrow)
                {
                    SelectedIndex++;
                    if (SelectedIndex == options.Count)
                    {
                        SelectedIndex = 0;
                    }
                }
                else if (KeyPressed == ConsoleKey.UpArrow)
                {
                    SelectedIndex--;
                    if (SelectedIndex == -1)
                    {
                        SelectedIndex = options.Count - 1;
                    }
                }

            } while (KeyPressed != ConsoleKey.Enter);
            // po wciśnięciu klawisza ENTER
            ConsoleRefresh();
            Console.ResetColor();
            return SelectedIndex;
        }
    }

    public class MenuEntry
    {
        public int value;
        public string caption;

        public MenuEntry(string caption, int value = -1)
        {
            this.caption = caption;
            this.value = value;
        }

        public bool HasValue()
        {
            return this.value > 0;
        }

    }
}