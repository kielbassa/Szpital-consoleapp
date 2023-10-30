// Baza danych postawiona jest lokalnie za pomocą aplikacji XAMPP
// Dane bazowe: localhost, szpitaltest, root, (bez hasła)
// W bazie znajdują się następujące tabele:
// - pacjenci (id (AI), login (varchat 100), haslo (varchar 100), pin (varchar 4)

using System.Data;
using System.Reflection.Metadata;
using MySql.Data.MySqlClient;

namespace Program
{
    class Szpital
    {

        public void Start()
        {
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

            switch (selectedIndex)
            {
                case 0: // login
                    RunLoginScreen();
                    break;
                case 1: // rejestracja
                    RegisterScreen();
                    break;
                case 2: // dostęp gościa
                    BrowserForQuest();
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
            string[] Options = { "Logowanie", "Zapomniałem hasła", "Wróć" };
            Menu LoginMenu = new Menu(Prompt, Options);
            LoginMenu.ConsoleRefresh();
            int selectedIndex = LoginMenu.Run();

            switch (selectedIndex)
            {
                case 0: // login
                    GiveLoginDate();
                    break;
                case 1: // zapomniałem hasła
                    PasswordForgot();
                    break;
                case 2: // wróc do głównego ekranu
                    Console.Clear();
                    RunMainMenu();
                    break;
            }
        }


        private void GiveLoginDate()
        {
            Console.Clear();
            Console.WriteLine("Podaj Login: ");
            string userLogin = Console.ReadLine();
            Console.WriteLine("Podaj Hasło: ");
            string userPassword = Console.ReadLine();

            string connectionString = "Server=localhost;Database=szpitaltest;Uid=root;Pwd=;";   //dane bazy
            MySqlConnection connection = new MySqlConnection(connectionString);                 //połączenie z bazą
            MySqlCommand cmd = new MySqlCommand();                                              //połączenie z bazą
            bool userExists = false;                                                            //wartość do sprawdzania czy user istnieje

            connection.Open();                                                                  //otwarcie pola bazy
                string query = "SELECT COUNT(*) FROM pacjenci WHERE login = @Login AND haslo = @Haslo";
                cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Login", userLogin);
                cmd.Parameters.AddWithValue("@Haslo", userPassword);

                int userCount = Convert.ToInt32(cmd.ExecuteScalar());           //przypisanie wartości liczbowej sprawdzającej istneinie usera
                if (userCount > 0)
                {
                    userExists = true;
                }
            connection.Close();                                             //zamknięcie połączenia

            if (userExists)
            {
                Console.Clear();
                Console.WriteLine("Zalogowano pomyślnie!");
                Console.ReadKey();
                BrowserForLogged();
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Podane dane są nieprawidłowe!");
                Console.ReadKey();
                RunLoginScreen();
            }
        }   


        private void PasswordForgot()
        {
            Console.Clear();
            Console.WriteLine("Podaj Login: ");
            string userLogin = Console.ReadLine();
            Console.WriteLine("Podaj PIN: ");
            string userPin = Console.ReadLine();

            string connectionString = "Server=localhost;Database=szpitaltest;Uid=root;Pwd=;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand cmd = new MySqlCommand();
            bool isPinCorrect = false;


            connection.Open();
                string query = "SELECT COUNT(*) FROM pacjenci WHERE login = @Login AND pin = @Pin";
                cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Login", userLogin);
                cmd.Parameters.AddWithValue("@Pin", userPin);

                int pinCount = Convert.ToInt32(cmd.ExecuteScalar());
                if (pinCount > 0)
                {
                    isPinCorrect = true;
                }
            connection.Close();
                
            

            if (isPinCorrect)
            {
                Console.Clear();
                Console.WriteLine("PIN poprawny. Możesz zmienić hasło.");
                ChangeForgottenPassword(userLogin);
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Podane dane są nieprawidłowe!");
                Console.ReadKey();
                RunLoginScreen();
            }
        }

        private void ChangeForgottenPassword(string userLogin)
        {
            Console.WriteLine("Podaj nowe hasło:");
            string newPassword = Console.ReadLine();

            string connectionString = "Server=localhost;Database=szpitaltest;Uid=root;Pwd=;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand cmd = new MySqlCommand();

            connection.Open();

                
                string updateQuery = "UPDATE pacjenci SET haslo = @NoweHaslo WHERE login = @Login";
                cmd = new MySqlCommand(updateQuery, connection);
                cmd.Parameters.AddWithValue("@NoweHaslo", newPassword);
                cmd.Parameters.AddWithValue("@Login", userLogin);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Console.Clear();
                    Console.WriteLine("Hasło zostało zmienione!");
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Nie udało się zmienić hasła.");
                }

                Console.ReadKey();
                RunLoginScreen();
            connection.Close();  
        }


        private void RegisterScreen()
        {
            Console.Clear();

            Console.WriteLine("Podaj nowy login:");
            string newLogin = Console.ReadLine();
            Console.WriteLine("Podaj nowe hasło:");
            string newPassword = Console.ReadLine();
            Console.WriteLine("Powtórz nowe hasło:");
            string newPasswordRepeat = Console.ReadLine();
            Console.WriteLine("Podaj nowy PIN (musi zawierać dokładnie 4 cyfry):");
            string newPin = Console.ReadLine();

            if (newPin.Length == 4 && newPassword == newPasswordRepeat)
            {
                string connectionString = "Server=localhost;Database=szpitaltest;Uid=root;Pwd=;";
                MySqlConnection connection = new MySqlConnection(connectionString);
                MySqlCommand cmd = new MySqlCommand();

                connection.Open();
                    string checkQuery = "SELECT COUNT(*) FROM pacjenci WHERE login = @Login";
                    cmd = new MySqlCommand(checkQuery, connection);
                    cmd.Parameters.AddWithValue("@Login", newLogin);
                    int userCount = Convert.ToInt32(cmd.ExecuteScalar());

                    if (userCount == 0)
                    {
                        // Konto o podanym loginie nie istnieje, więc można je dodać
                        string insertQuery = "INSERT INTO pacjenci (login, haslo, pin) VALUES (@Login, @Haslo, @Pin)";
                        cmd = new MySqlCommand(insertQuery, connection);
                        cmd.Parameters.AddWithValue("@Login", newLogin);
                        cmd.Parameters.AddWithValue("@Haslo", newPassword);
                        cmd.Parameters.AddWithValue("@Pin", newPin);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            Console.Clear();
                            Console.WriteLine("Konto zostało pomyślnie utworzone!");
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("Nie udało się utworzyć konta.");
                        }
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Konto o takiej nazwie już istnieje!");
                    }

                    Console.ReadKey();
                    RunLoginScreen();
                connection.Close();   
            }
            else
            {
                Console.Clear();
                if (newPin.Length != 4)
                {
                    Console.WriteLine("Podany PIN musi zawierać 4 cyfry!");
                }
                if (newPassword != newPasswordRepeat)
                {
                    Console.WriteLine("Podane hasła nie są takie same!");
                }
                Console.ReadKey();
                RunMainMenu();
            }
        }


        private void DisplayInfo()
        {
            Console.Clear();
            Console.WriteLine("Aplikacja konsoli do wyświetlania bazy danych szpitali oraz rezerwowania wizyt lekarskich.");
            Console.WriteLine("\nNaciśnij klawisz, żeby wrócić do głównego Menu.");
            Console.ReadKey(true);
            RunMainMenu();
        }


        private void ExitProgram()
        {
            Console.Clear();
            Environment.Exit(0);
        }


        // Menu właściwe V

        private void BrowserForLogged()
        {
            Console.Clear();
            string Prompt = "Wybierz opcję klikając klawisz ENTER:";
            string[] Options = { "Wyszukaj szpital", "Wyszukaj odział", "Zapisz się", "Wyloguj" };
            Menu LoginMenu = new Menu(Prompt, Options);
            LoginMenu.ConsoleRefresh();
            int selectedIndex = LoginMenu.Run();

            switch (selectedIndex)
            {
                case 0: // szpitale
                    HospitalBaze();
                    break;
                case 1: // oddziały
                    WardBaze();
                    break;
                case 2: // zapisy
                    SignUp();
                    break;
                case 3: // wyloguj
                    Console.Clear();
                    RunMainMenu();
                    break;
            }
        }


        private void HospitalBaze()
        {
            Console.Clear();
            string Prompt = "Wybierz opcję klikając klawisz ENTER:";
            string[] Options = { "Gdańsk", "Gdynia", "Wrocław", "Poznań", "Warszawa", "Kraków", "Wróc" };
            Menu LoginMenu = new Menu(Prompt, Options);
            LoginMenu.ConsoleRefresh();
            int selectedIndex = LoginMenu.Run();

            switch (selectedIndex)
            {
                case 0:
                    break;
                case 1:
                    // ZAPYTANIE WYŚWIETLAJĄCE ODZIAŁY W KONKRETNYM SZPITALU W KONRETNY MIESCIE 
                    break;
                case 2:
                    // ZAPYTANIE WYŚWIETLAJĄCE ODZIAŁY W KONKRETNYM SZPITALU W KONRETNY MIESCIE 
                    break;
                case 3:
                    // ZAPYTANIE WYŚWIETLAJĄCE ODZIAŁY W KONKRETNYM SZPITALU W KONRETNY MIESCIE 
                    break;
                case 4:
                    // ZAPYTANIE WYŚWIETLAJĄCE ODZIAŁY W KONKRETNYM SZPITALU W KONRETNY MIESCIE 
                    break;
                case 5:
                    // ZAPYTANIE WYŚWIETLAJĄCE ODZIAŁY W KONKRETNYM SZPITALU W KONRETNY MIESCIE 
                    break;
                case 6: // wróć
                    Console.Clear();
                    BrowserForLogged();
                    break;
            }
        }


        private void WardBaze()
        {
            Console.Clear();
            string Prompt = "Wybierz opcję klikając klawisz ENTER:";
            string[] Options = { "Chirurgia", "Pediatria", "Ginekologia", "Ortopedia", "Psychiatria", "Wróc" };
            Menu LoginMenu = new Menu(Prompt, Options);
            LoginMenu.ConsoleRefresh();
            int selectedIndex = LoginMenu.Run();

            switch (selectedIndex)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5: //wróc
                    Console.Clear();
                    BrowserForLogged();
                    break;
            }
        }


        private void SignUp()
        {
            Console.Clear();
        }


        private void BrowserForQuest()
        {
            Console.Clear();
            Console.WriteLine("questaccess placeholder\n");
            Console.WriteLine("Naciśnij klawisz, żeby wrócić do głównego Menu.");
            Console.ReadKey();
            RunMainMenu();
        }
    }
}
