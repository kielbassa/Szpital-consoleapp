// Baza danych postawiona jest lokalnie za pomocą aplikacji XAMPP
// Dane bazowe: localhost, szpitaltest, root, (bez hasła)
// W bazie znajdują się następujące tabele:
// - uzytkownicy (user_id (AI), login (varchar 30), haslo (varchar 100), pin (varchar 4), imie(varchar30), nazwisko(varchar30), user_type(enum)

using System.Data;
using System.Globalization;
using System.Reflection.Metadata;

using MySql.Data.MySqlClient;

namespace Program
{
    class Szpital
    {

        public User LoggedUser = new User();

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


            while (true)
            {
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
                        Szukaj();
                        break;
                    case 3: // info
                        DisplayInfo();
                        break;
                    case 4: // wyjście
                        ExitProgram();
                        break;
                }
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
                    break;
            }
        }

        private MySqlConnection GetMySqlConnection()
        {
            string connectionString = "Server=localhost;Database=szpitaltest;Uid=root;Pwd=;";   //dane bazy

            MySqlConnection connection = new MySqlConnection(connectionString);                //połączenie z bazą

            return connection;
        }

        private void GiveLoginDate()
        {
            Console.Clear();
            string userLogin = askUser("Podaj Login: ");
            string userPassword = askUser("Podaj Hasło: ");

            MySqlConnection connection = GetMySqlConnection();                                  //połączenie z bazą
            MySqlCommand cmd = new MySqlCommand();                                              //połączenie z bazą
            bool userExists = false;                                                            //wartość do sprawdzania czy user istnieje

            connection.Open();                                                                  //otwarcie pola bazy
                string query = "SELECT COUNT(*), user_id, user_type, imie, nazwisko FROM uzytkownicy WHERE login = @Login AND haslo = @Haslo";
                cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Login", userLogin);
                cmd.Parameters.AddWithValue("@Haslo", userPassword);

                int userCount = Convert.ToInt32(cmd.ExecuteScalar());           //przypisanie wartości liczbowej sprawdzającej istneinie usera
                if (userCount > 0)
                {
                    userExists = true;
                    CreateUser(cmd.ExecuteReader());
                }
            connection.Close();                                             //zamknięcie połączenia

            if (userExists)
            {
                Console.Clear();
                Console.WriteLine("Witaj {0} {1}!", LoggedUser.Imie, LoggedUser.Nazwisko);
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
            string userLogin = askUser("Podaj Login: ");
            string userPin = askUser("Podaj PIN: ");

            MySqlConnection connection = GetMySqlConnection();
            MySqlCommand cmd = new MySqlCommand();
            bool isPinCorrect = false;


            connection.Open();
            string query = "SELECT COUNT(*) FROM uzytkownicy WHERE login = @Login AND pin = @Pin";
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
            string newPassword = askUser("Podaj nowe hasło:");

            string connectionString = "Server=localhost;Database=szpitaltest;Uid=root;Pwd=;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand cmd = new MySqlCommand();

            connection.Open();
                
                string updateQuery = "UPDATE uzytkownicy SET haslo = @NoweHaslo WHERE login = @Login";
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

            string newLogin = askUser("Podaj nowy login: ");
            string newPassword = askUser("Podaj nowe hasło: ");
            string newPasswordRepeat = askUser("Powtórz nowe hasło: ");
            string newPin = askUser("Podaj nowy PIN (musi zawierać dokładnie 4 cyfry): ");
            string newName = askUser("Podaj swoje imię: ");
            string newSurname = askUser("Podaj swoje nazwisko: ");

            if (newPin.Length == 4 && newPassword == newPasswordRepeat)
            {
                string connectionString = "Server=localhost;Database=szpitaltest;Uid=root;Pwd=;";
                MySqlConnection connection = new MySqlConnection(connectionString);
                MySqlCommand cmd = new MySqlCommand();

                connection.Open();
                    string checkQuery = "SELECT COUNT(*) FROM uzytkownicy WHERE login = @Login";
                    cmd = new MySqlCommand(checkQuery, connection);
                    cmd.Parameters.AddWithValue("@Login", newLogin);
                    int userCount = Convert.ToInt32(cmd.ExecuteScalar());

                    if (userCount == 0)
                    {
                        // Konto o podanym loginie nie istnieje, więc można je dodać
                        string insertQuery = "INSERT INTO uzytkownicy (login, haslo, pin, imie, nazwisko) VALUES (@Login, @Haslo, @Pin, @Imie, @Nawisko)";
                        cmd = new MySqlCommand(insertQuery, connection);
                        cmd.Parameters.AddWithValue("@Login", newLogin);
                        cmd.Parameters.AddWithValue("@Haslo", newPassword);
                        cmd.Parameters.AddWithValue("@Imie", newName);
                        cmd.Parameters.AddWithValue("@Nazwisko", newSurname);
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
            }
        }




        private void DisplayInfo()
        {
            Console.Clear();
            Console.WriteLine("Aplikacja konsoli do wyświetlania bazy danych szpitali oraz rezerwowania wizyt lekarskich.");
            Console.WriteLine("\nNaciśnij klawisz, żeby wrócić do głównego Menu.");
            Console.ReadKey(true);
        }


        private void ExitProgram()
        {
            Console.Clear();
            Environment.Exit(0);
        }


        // Menu właściwe

        private void BrowserForLogged()
        {
            Console.Clear();
            string Prompt = "Wybierz opcję klikając klawisz ENTER:";
            string[] Options = { "Szukaj", "Moje Wizyty", "Nowa wizyta", "Wyloguj" };
            Menu LoginMenu = new Menu(Prompt, Options);
            LoginMenu.ConsoleRefresh();
            int selectedIndex = LoginMenu.Run();
            bool keepLoop = true;

            while(keepLoop)
            {
                switch (selectedIndex)
                {
                    case 0: // wyszukiwarka
                        Szukaj();
                        break;
                    case 1: // moje wizyty
                        //MojeWizyty();
                        break;
                    case 2: // nowa wizyta
                        SignUp();
                        break;
                    case 3: // wyloguj
                        keepLoop = false;
                        Console.Clear();
                        break;
                }
            }
        }

        // Wyszukiwarka
        private void Szukaj()
        {
            Filters filters = new Filters();
            Menu wyszukiwarka;
            string prompt = "Wyszukiwarka terminów wizyt";
            string[] options = { "Wybierz miasto", "Wybierz szpital", "Wybierz oddział", "Wybierz lekarza", "Szukaj", "Wróć" };
            wyszukiwarka = new Menu(prompt, options);
            bool shouldRun = true;
            int selectedIndex = 0;

            while (shouldRun)
            {
                selectedIndex = wyszukiwarka.Run();
                MenuEntry row;

                switch (selectedIndex)
                {
                    case 0: // Wybierz miasto
                        filters.SetLocation(WybierzMiasto(filters)); // Dodajemy wybrane miasto jako filtr
                        if (filters.LocationEmpty())
                        {
                            row = new MenuEntry("Wybierz miasto");
                        }
                        else
                        {
                            row = new MenuEntry("Miasto: " + filters.Location);
                        }
                        wyszukiwarka.ReplaceRow(0, row);
                        break;
                    case 1: // Wybierz szpital
                        WybierzSzpital(filters);
                        if (filters.SzpitalEmpty())
                        {
                            row = new MenuEntry("Wybierz szpital");
                        }
                        else
                        {
                            row = new MenuEntry("Szpital: " + filters.SzpitalName, filters.SzpitalId);
                        }
                        wyszukiwarka.ReplaceRow(1, row);
                        break;
                    case 2: // Wybierz Oddział
                        WybierzOddzial(filters);
                        if (filters.OddzialEmpty())
                        {
                            row = new MenuEntry("Wybierz oddział");
                        }
                        else
                        {
                            row = new MenuEntry("Oddział: " + filters.OddzialType);
                        }
                        wyszukiwarka.ReplaceRow(2, row);
                        break;
                    case 3: // Wybierz lekarza
                        /*WybierzLekarza(filters);
                        if (filters.LekarzEmpty())
                        {
                            row = new MenuEntry("Wybierz lekarza");
                        }
                        else
                        {
                            row = new MenuEntry("Lekarz: " + filters.LekarzName);
                        }
                        wyszukiwarka.ReplaceRow(3, row);*/
                        break;

                    case 4:
                        GetSearchResults(filters);
                        break;

                    default:
                        Console.Clear();
                        shouldRun = false;
                        break;
                }
            }
        }

        private string WybierzMiasto(Filters filters)
        {
            Console.Clear();
            string Prompt = "Wybierz opcję klikając klawisz ENTER:";
            string[] Options = GetLocationByFilters(filters);
            Menu LoginMenu = new Menu(Prompt, Options);
            int clearIndex = LoginMenu.AddRow("Wyczyść");
            int returnIndex = LoginMenu.AddRow("Wróć");
            LoginMenu.ConsoleRefresh();
            int selectedIndex = LoginMenu.Run();

            if (selectedIndex == clearIndex)
            {
                return String.Empty;
            }
            else if (selectedIndex == returnIndex)
            {
                return "_";
            }
            else
            {
                return Options[selectedIndex];
            }
        }

        private void WybierzSzpital(Filters filters)
        {
            Console.Clear();
            string Prompt = "Wybierz opcję klikając klawisz ENTER:";
            var Options = GetSzpitaleByFilters(filters);
            Menu LoginMenu = new Menu(Prompt, Options);
            int clearIndex = LoginMenu.AddRow("Wyczyść");
            int returnIndex = LoginMenu.AddRow("Wróć");
            LoginMenu.ConsoleRefresh();
            int selectedIndex = LoginMenu.Run();

            if (selectedIndex == clearIndex)
            {
                filters.SzpitalName = String.Empty;
                filters.SzpitalId = -1;

            }
            else if (selectedIndex != returnIndex)
            {
                filters.SzpitalName = Options[selectedIndex].Item1;
                filters.SzpitalId = Options[selectedIndex].Item2;
            }
            return;


        }

        private void WybierzOddzial(Filters filters)
        {
            Console.Clear();
            string Prompt = "Wybierz opcję klikając klawisz ENTER:";
            var Options = GetOddzialyByFilters(filters);
            Menu LoginMenu = new Menu(Prompt, Options);
            int clearIndex = LoginMenu.AddRow("Wyczyść");
            int returnIndex = LoginMenu.AddRow("Wróć");
            LoginMenu.ConsoleRefresh();
            int selectedIndex = LoginMenu.Run();

            if (selectedIndex == clearIndex)
            {
                filters.SetOddzialType(String.Empty);

            }
            else if (selectedIndex != returnIndex)
            {
                filters.SetOddzialType(Options[selectedIndex].Item1);
            }
            return;

        }
        
        /* TODO
        private void WybierzLekarza(Filters filters)
        {
            Console.Clear();
            string Prompt = "Wybierz opcję klikając klawisz ENTER:";
            var Options = GetOddzialyByFilters(filters);
            Menu LoginMenu = new Menu(Prompt, Options);
            int clearIndex = LoginMenu.AddRow("Wyczyść");
            int returnIndex = LoginMenu.AddRow("Wróć");
            LoginMenu.ConsoleRefresh();
            int selectedIndex = LoginMenu.Run();

            if (selectedIndex == clearIndex)
            {
                filters.SetOddzialType(String.Empty);

            }
            else if (selectedIndex != returnIndex)
            {
                filters.SetOddzialType(Options[selectedIndex].Item1);
            }
            return;

        }*/

        private void SignUp()
        {
            Console.Clear();
        }

        private string[] GetLocationByFilters(Filters filters)
        {
            MySqlConnection db = GetMySqlConnection();
            List<string> list = new List<string>();
            MySqlCommand cmd;

            string location = filters.Location;

            string prompt = @"SELECT DISTINCT location FROM szpitale
                                CROSS JOIN oddzialy ON oddzialy.szpital_id = szpitale.szpital_id";

            if (!filters.LocationEmpty()) { prompt = addSqlParamaterString(prompt, "szpitale.location", filters.Location); }
            if (!filters.SzpitalEmpty()) { prompt = addSqlParamaterInt(prompt, "szpitale.szpital_id", filters.SzpitalId); }
            if (!filters.OddzialEmpty()) { prompt = addSqlParamaterString(prompt, "oddzialy.oddzial_type", filters.OddzialType); }

            cmd = new MySqlCommand(prompt, db);
            db.Open();
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(reader.GetString("location"));
            }
            db.Close();

            list.Sort();

            return list.ToArray(); // zamieniamy dynamiczną listę na listę statyczną
        }

        private Tuple<string, int>[] GetSzpitaleByFilters(Filters filters)
        {
            MySqlConnection db = GetMySqlConnection();
            MySqlCommand cmd;

            List<Tuple<string, int>> list = new List<Tuple<string, int>>();
            string prompt = @"SELECT DISTINCT szpitale.szpital_id, szpitale.name FROM oddzialy
                                CROSS JOIN szpitale ON szpitale.szpital_id = oddzialy.szpital_id";

            if (!filters.LocationEmpty()) { prompt = addSqlParamaterString(prompt, "szpitale.location", filters.Location); }
            if (!filters.SzpitalEmpty()) { prompt = addSqlParamaterInt(prompt, "szpitale.szpital_id", filters.SzpitalId); }
            if (!filters.OddzialEmpty()) { prompt = addSqlParamaterString(prompt, "oddzialy.oddzial_type", filters.OddzialType); }

            cmd = new MySqlCommand(prompt, db);
            db.Open();

            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int szpitalId = reader.GetInt32("szpital_id");
                string szpitalName = reader.GetString("name");

                var t = new Tuple<string, int>(szpitalName, szpitalId);             // Tworzymy tuple
                list.Add(t);                                                        // Dodajemy szpital do dynamicznej listy
            }
            db.Close();

            return list.ToArray(); // zamieniamy dynamiczną listę na listę statyczną
        }

        private Tuple<string, int>[] GetOddzialyByFilters(Filters filters)
        {

            MySqlConnection db = GetMySqlConnection();
            List<Tuple<string, int>> list = new List<Tuple<string, int>>();
            string prompt = @"SELECT oddzialy.oddzial_id, szpitale.szpital_id, szpitale.location, oddzialy.oddzial_type FROM oddzialy
                                CROSS JOIN szpitale ON szpitale.szpital_id = oddzialy.szpital_id";

            if (!filters.LocationEmpty()) { prompt = addSqlParamaterString(prompt, "szpitale.location", filters.Location); }
            if (!filters.SzpitalEmpty()) { prompt = addSqlParamaterInt(prompt, "szpitale.szpital_id", filters.SzpitalId); }
            if (!filters.OddzialEmpty()) { prompt = addSqlParamaterString(prompt, "oddzialy.oddzial_type", filters.OddzialType); }

            MySqlCommand cmd = new MySqlCommand(prompt, db);
            db.Open();
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int oddzialId = reader.GetInt32("oddzial_id");
                string oddzialType = reader.GetString("oddzial_type");

                var t = new Tuple<string, int>(oddzialType, oddzialId);

                list.Add(t);
            }
            db.Close();

            return list.ToArray();

        }

        private Tuple<string, int>[] GetLekarzeByFilters(Filters filters)
        {
            MySqlConnection db = GetMySqlConnection();
            List<Tuple<string, int>> list = new List<Tuple<string, int>>();

            string prompt = @"SELECT uzytkownicy.imie, uzytkownicy.nazwisko, pracownicy.lekarz_id FROM uzytkownicy
                                CROSS JOIN pracownicy ON uzytkownicy.user_id = pracownicy.lekarz_id 
                                CROSS JOIN oddzialy ON oddzialy.oddzial_id = pracownicy.oddzial_id
                                CROSS JOIN szpitale ON oddzialy.szpital_id = szpitale.szpital_id";

            if (!filters.LocationEmpty()) prompt = addSqlParamaterString(prompt, "szpitale.location", filters.Location);
            if (!filters.SzpitalEmpty()) prompt = addSqlParamaterInt(prompt, "szpitale.szpital_id", filters.SzpitalId);
            if (!filters.OddzialEmpty()) prompt = addSqlParamaterString(prompt, "oddzialy.oddzial_type", filters.OddzialType);
            if (!filters.LekarzEmpty()) prompt = addSqlParamaterInt(prompt, "pracownicy.lekarz_id", filters.LekarzId);

            MySqlCommand cmd = new MySqlCommand(prompt, db);
            db.Open();
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string imie = reader.GetString("imie");
                string nazwisko = reader.GetString("nazwisko");
                int doctorId = reader.GetInt32("lekarz_id");

                var t = new Tuple<string, int>(String.Format("{0} {1}", imie, nazwisko), doctorId);

                list.Add(t);
            }
            db.Close();

            return list.ToArray();

        }

        private void GetSearchResults(Filters filters)
        {
            var today = DateTime.Today;
            var dayOfWeek = (int)today.DayOfWeek;

            // liczba 5 reprezentuje piątek
            if (dayOfWeek > 5) {
                /*  szpitale nie działają w soboty i niedziele... a przynajmniej w naszym programie.
                    W przypadku weekendu dodajemy dni by uzyskać poniedziałek.
                */
                today = today.AddDays(8 - dayOfWeek);
            }
            
            string date = today.ToString("yyyy-MM-dd");  // Data w formacie który przyda nam się do tworzenia zapytań do bany danych

            // FIXME: Zmienić nazwy pól tabeli pracownicy tak aby odopwiadały wszystkim możliwym wartościom zmiennej dayShortName
            string dayShortName = today.ToString("ddd", new CultureInfo("pl-PL"));
            Console.WriteLine(dayShortName);
            Console.ReadKey();

            // Zamienia datę na format rok, miesiąc, dzień
            string prompt = String.Format("Data: {0}", today.ToString(" dddd, dd.MM.yyyy", new CultureInfo("pl-PL")));
            string[] Options = { "Poprzedni", "Następny" };

            var menu = new Menu(prompt);

            foreach (Tuple<string, int> d in GetLekarzeByFilters(filters))
            {
                                                            // dayShortName może wywoływać crashe
                Tuple<int, int> godziny = GetGodzinyPrzyjmowania("pon", d.Item2);
                List<int> wizyty = GetTodaysAppointmentsByDoctor("pon", d.Item2);
                for (int i = godziny.Item1; i < godziny.Item2; i++)
                {
                        if (wizyty.Exists(x => x == i)) {
                            wizyty.Remove(i);
                        } else {
                            // Reprezentacja wizyty w postaci tekstowej "Imie nazwisko godzina-godzina"
                            menu.AddRow(string.Format("{0} {1}-{2}", d.Item1, i, i + 1));
                        }

                }
            }

            menu.Run();
            Console.ReadKey();

        }

        // zakładamy, że każda wizyta trwa jedną godzinę zegarową, w przypadku zmiany założeń należy zmienić int[] nad Tuple<int, int>[]
        private List<int> GetTodaysAppointmentsByDoctor(string date, int doctorId) 
        {
            var db  = GetMySqlConnection();
            var list = new List<int>();
                                                // begin oznacza godzinę wizyty
            string prompt = String.Format("SELECT begin FROM `kolejka` WHERE date = \"{0}\" AND lekarz_id = {1}", date, doctorId);
            var cmd = new MySqlCommand(prompt, db);
            db.Open();
                var reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    string hour;
                    try
                    {
                        hour = reader.GetString(0);      // 5-znakowy łańcuch "godzina:minuta"

                    } catch (System.Data.SqlTypes.SqlNullValueException)
                    {
                        hour = "00:00"; // Domyślna wartość
                    }
                    
                    // Zakładamy, że wizyta trwa godzinę zegarową więc nie potrzebujemy końca wizyty
                    list.Add(Convert.ToInt32(hour[0..2]));
                }

                return list;
        }
        

        private Tuple<int, int> GetGodzinyPrzyjmowania(string dzien, int doctorId)
        {
            var db = GetMySqlConnection();
            string prompt = string.Format("SELECT pracownicy.{0} from pracownicy CROSS JOIN uzytkownicy ON pracownicy.lekarz_id = uzytkownicy.user_id", dzien);
            prompt = addSqlParamaterInt(prompt, "uzytkownicy.user_id", doctorId);
            var cmd = new MySqlCommand(prompt, db);
            var workingHours = new Tuple<int, int>(-1, -1);
            db.Open();
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                string work;
                try
                {
                    work = reader.GetString(0);      // 5-znakowy łańcuch opisujący godziny przyjmowania lekarzy w formacie "XX-YY"
                }
                catch (System.Data.SqlTypes.SqlNullValueException)
                {
                    work = "00-00";
                }

                int begin = Convert.ToInt32(work[0..2]); // godzina rozpoczęcia pracy
                int end = Convert.ToInt32(work[3..5]); // godzina zakończenia pracy
                workingHours = new Tuple<int, int>(begin, end);
            }

            db.Close();

            return workingHours;
        }
        private void CreateUser(MySqlDataReader reader)
        {
            reader.Read();
            int userId = reader.GetInt32("user_id");
            string userType = reader.GetString("user_type");
            string imie = reader.GetString("imie");
            string nazwisko = reader.GetString("nazwisko");

            LoggedUser = new User(userId, userType, imie, nazwisko);

        }

        // Funkcje niezależne

        // Podpatrzyłem to z innego projektu, +1000 do czytelności kodu
        private string askUser(string prompt)
        {
            Console.Write(prompt);

            return Console.ReadLine(); // input użytkownika
        }

        // Dodaje dynamicznie klauzuly "WHERE" i "AND"

        private string addSqlParamaterString(string prompt, string field, string value)
        {
            if (prompt.Contains("WHERE"))
            {
                prompt += string.Format(" AND {0} = \"{1}\"", field, value);
            }
            else
            {
                prompt += string.Format(" WHERE {0} = \"{1}\"", field, value);
            }

            return prompt;
        }
        private string addSqlParamaterInt(string prompt, string field, int value)
        {
            if (prompt.Contains("WHERE"))
            {
                prompt += string.Format(" AND {0} = {1}", field, value);
            }
            else
            {
                prompt += string.Format(" WHERE {0} = {1}", field, value);
            }

            return prompt;
        }

    }
}