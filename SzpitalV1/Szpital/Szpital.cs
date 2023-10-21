namespace Program
{
    class Szpital
    {
        SoundEffects sound = new SoundEffects();
        public void Start()
        {
            string Prompt = "Witaj w wielkiej bazie polskich szpitali! Wybierz opcję klikając klawisz ENTER:";
            string[] Options = { "Zaloguj się", "Zarejestruj się", "Dostęp gościa" };
            Menu mainMenu = new Menu(Prompt,Options);
            sound.StartJingle();
            mainMenu.Run();
            int selectedIndex = mainMenu.Run();
        }
    }
}
