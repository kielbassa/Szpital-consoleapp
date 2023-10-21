using System.Reflection.Metadata;

namespace Program
{
    class Szpital
    {
        SoundEffects sound = new SoundEffects();
        ASCII_Graphics graphics = new ASCII_Graphics();
        public void Start()
        {
            sound.StartJingle();
            Console.Title = "Aplikacja Szpital";
            RunMainMenu();
        }
        private void RunMainMenu()
        {
            Console.Clear();

            string Prompt = "Witaj w wielkiej bazie polskich szpitali! Wybierz opcję klikając klawisz ENTER:";
            string[] Options = { "Zaloguj się", "Zarejestruj się", "Dostęp gościa", "Info", "Wyjdź z aplikacji" };

            Menu mainMenu = new Menu(Prompt, Options);

            int selectedIndex = mainMenu.Run(); // przywołuje klasę Run z Menu.cs, która zwraca index wybranej opcji

            switch(selectedIndex) 
            {
                case 0: // login
                    RunLoginScreen();
                    break;
                case 1: // rejestracja
                    RegisterScreen();
                    break;
                case 2: // dostęp gościa
                    QuestAccess();
                    break;
                case 3: // info
                    DisplayInfo();
                    break;
                case 4: // wyjście
                    ExitProgram();
                    break;
            }
        }

        private void RunLoginScreen()
        {
            Console.Clear();
            string Prompt = "Wybierz opcję klikając klawisz ENTER:";
            string[] Options = { "login", "hasło", "zapomniałem hasła", "wróć" };
            Menu LoginMenu = new Menu(Prompt, Options);
            LoginMenu.ConsoleRefresh();
            int selectedIndex = LoginMenu.Run();

            switch (selectedIndex)
            {
                case 0: // login
                    Console.Clear();
                    graphics.MainLogo();
                    Console.WriteLine("Podaj Login: ");
                    Console.WriteLine("\nNaciśnij klawisz, żeby wrócić do poprzedniego Menu.");
                    Console.ReadKey(true);
                    sound.BackOption();
                    RunLoginScreen();
                    break;
                case 1: // hasło
                    Console.Clear();
                    graphics.MainLogo();
                    Console.WriteLine("Podaj Hasło: ");
                    Console.WriteLine("\nNaciśnij klawisz, żeby wrócić do poprzedniego Menu.");
                    Console.ReadKey(true);
                    sound.BackOption();
                    RunLoginScreen();
                    break;
                case 2: // zapomniałem hasła
                    Console.Clear();
                    graphics.MainLogo();
                    Console.WriteLine("placeholder");
                    Console.WriteLine("\nNaciśnij klawisz, żeby wrócić do poprzedniego Menu.");
                    Console.ReadKey(true);
                    sound.BackOption();
                    RunLoginScreen();
                    break;
                case 3: // wróc do głównego ekranu
                    Console.Clear();
                    sound.BackOption();
                    RunMainMenu();
                    break;
            }
        }
        private void RegisterScreen()
        {
            Console.Clear();
            graphics.MainLogo();
            Console.WriteLine("rejestracja placeholder\n");
            Console.WriteLine("Naciśnij klawisz, żeby wrócić do głównego Menu.");
            Console.ReadKey(true);
            sound.BackOption();
            RunMainMenu();
        }
        private void QuestAccess()
        {
            Console.Clear();
            graphics.MainLogo();
            Console.WriteLine("questaccess placeholder\n");
            Console.WriteLine("Naciśnij klawisz, żeby wrócić do głównego Menu.");
            Console.ReadKey();
            sound.BackOption();
            RunMainMenu();
        }
        private void DisplayInfo()
        {
            Console.Clear();
            graphics.MainLogo();
            Console.WriteLine("Aplikacja konsoli do wyświetlania bazy danych szpitali oraz rezerwowania wizyt lekarskich.");
            Console.WriteLine("\nNaciśnij klawisz, żeby wrócić do głównego Menu.");
            Console.ReadKey(true);
            sound.BackOption();
            RunMainMenu();
        }
        private void ExitProgram()
        {
            Console.Clear();
            sound.StopJingle();
            Environment.Exit(0);
        }
    }
}
