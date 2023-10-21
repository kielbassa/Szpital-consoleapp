namespace Program
{
    class Szpital
    {
        SoundEffects sound = new SoundEffects();
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

            int selectedIndex = mainMenu.Run(); // przywołuje klasę Run z Menu.cs

            switch(selectedIndex) 
            {
                case 0: // login
                    LoginScreen();
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
                case 4: // info
                    ExitProgram();
                    break;
            }
        }

        private void LoginScreen()
        {

        }
        private void RegisterScreen()
        {

        }
        private void QuestAccess()
        {

        }
        private void DisplayInfo()
        {
            Console.Clear();
            Console.WriteLine("Aplikacja konsoli do wyświetlania bazy danych szpitali oraz rezerwowania wizyt lekarskich.");
            Console.WriteLine("Naciśnij klawisz, żeby wrócić do głównego Menu.");
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
