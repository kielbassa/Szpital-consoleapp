// Baza danych postawiona jest lokalnie za pomocą aplikacji XAMPP
// Dane bazowe: localhost, test3, root, (bez hasła)
// W bazie znajdują się następujące tabele:

//Patients:
//-ID(AI)
//- login(varchar 100)
//- password(varchar 100)
//- PIN(varchar 4)
//- firstName(varchat 100)
//- lastName(varchar 100)

//Doctors:
//-ID(AI)
//- login(varchar 100)
//- password(varchar 100)
//- firstName(varchat 100)
//- lastName(varchar 100)

//Admins:
//-ID(AI)
//- login(varchar 100)
//- password(varchar 100)

//Wards:
//-ID(AI)
//- name(varchar 100)

//Hospitals:
//-ID(AI)
//- name(varchar 100)
//- location(varchar 100)

//Information:
//-ID(AI)
//- HospitalID(int)
//- WardID(int)
//- DoctorID(int)

//Visits:
//-ID(AI)
//- InformationID(int)
//- VisitDate(date)

using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Reflection.Metadata;

using MySql.Data.MySqlClient;

namespace Program
{
    class Szpital
    {
        // Nadanie tutułu konsoli

        public User LoggedUser = new User();

        public void Start()
        {
            Console.Title = "Aplikacja Szpital";
            RunMainMenu();
        }



        // Menu główne służące do logowania, rejestracji, dostepu gościa oraz opuszczenia aplikacji

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



        // wszystkie opcje dot. logowania, rejestracji etc.

        private void RunLoginScreen() // panel logowania
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

        private void GiveLoginDate() // logowanie
        {
            Console.Clear();
            string userLogin = askUser("Podaj Login: ");
            string userPassword = askUser("Podaj Hasło: ");

            MySqlConnection connection = GetMySqlConnection();                                  //połączenie z bazą
            MySqlCommand cmd = new MySqlCommand();                                              //połączenie z bazą
            bool userExists = false;                                                            //wartość do sprawdzania czy user istnieje

            connection.Open();                                                                  //otwarcie pola bazy
            string query = "SELECT * FROM patients WHERE login = @Login AND password = @Haslo";
            cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Login", userLogin);
            cmd.Parameters.AddWithValue("@Haslo", userPassword);

            int userCount = Convert.ToInt32(cmd.ExecuteScalar());     //przypisanie wartości liczbowej sprawdzającej istneinie usera

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



        private void PasswordForgot() // przypomnij hasło
        {
            Console.Clear();
            string userLogin = askUser("Podaj Login: ");
            string userPin = askUser("Podaj PIN: ");

            MySqlConnection connection = GetMySqlConnection();
            MySqlCommand cmd = new MySqlCommand();
            bool isPinCorrect = false;

            connection.Open();

            string query = "SELECT * FROM patients WHERE login = @Login AND PIN = @Pin";
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


        private void ChangeForgottenPassword(string userLogin) // zmień hasło
        {
            string newPassword = askUser("Podaj nowe hasło:");

            MySqlConnection connection = GetMySqlConnection();                                  
            MySqlCommand cmd = new MySqlCommand();

            connection.Open();

            string updateQuery = "UPDATE patients SET password = @NoweHaslo WHERE login = @Login";
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



        private void RegisterScreen()  // rejestracja
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
                string connectionString = "Server=localhost;Database=test3;Uid=root;Pwd=;";
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Sprawdzenie, czy istnieje już użytkownik o podanym loginie
                    string checkQuery = "SELECT * FROM patients WHERE login = @Login";
                    using (MySqlCommand cmdCheck = new MySqlCommand(checkQuery, connection))
                    {
                        cmdCheck.Parameters.AddWithValue("@Login", newLogin);
                        int userCount = Convert.ToInt32(cmdCheck.ExecuteScalar());

                        if (userCount == 0)
                        {
                            // Konto o podanym loginie nie istnieje, więc można je dodać
                            string insertQuery = "INSERT INTO patients (login, password, pin, firstname, lastname) VALUES (@Login, @Haslo, @Pin, @Imie, @Nazwisko)";
                            using (MySqlCommand cmdInsert = new MySqlCommand(insertQuery, connection))
                            {
                                cmdInsert.Parameters.AddWithValue("@Login", newLogin);
                                cmdInsert.Parameters.AddWithValue("@Haslo", newPassword);
                                cmdInsert.Parameters.AddWithValue("@Pin", newPin);
                                cmdInsert.Parameters.AddWithValue("@Imie", newName);
                                cmdInsert.Parameters.AddWithValue("@Nazwisko", newSurname);

                                int rowsAffected = cmdInsert.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    Console.Clear();
                                    Console.WriteLine("Konto zostało pomyślnie utworzone!");
                                    Console.ReadKey();
                                    RunLoginScreen();
                                }
                                else
                                {
                                    Console.Clear();
                                    Console.WriteLine("Nie udało się utworzyć konta.");
                                }
                            }
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("Konto o takiej nazwie już istnieje!");
                        }
                    }
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Nieprawidłowy PIN lub hasła nie są identyczne.");
            }

            Console.ReadKey();
        }



        private void DisplayInfo() // informacje o aplikacji
        {
            Console.Clear();
            Console.WriteLine("Aplikacja konsoli do wyświetlania bazy danych szpitali oraz rezerwowania wizyt lekarskich.");
            Console.WriteLine("Autorstwa: Karol Lademan, Dawid Laskowski, Karol Narel, Tomasz Papierowski");
            Console.WriteLine("\nNaciśnij klawisz, żeby wrócić do głównego Menu.");
            Console.ReadKey(true);
        }



        private void ExitProgram() // wyjście z aplikacji
        {
            Console.Clear();
            Environment.Exit(0);
        }



        // Menu właściwe:

        private void BrowserForLogged() // przeglądarka dla zalogowanych użytkowników
        {
            Console.Clear();
            string Prompt = "Wybierz opcję klikając klawisz ENTER:";
            string[] Options = { "Szukaj", "Moje Wizyty", "Nowa wizyta", "Wyloguj" };
            Menu LoginMenu = new Menu(Prompt, Options);
            LoginMenu.ConsoleRefresh();
            int selectedIndex = LoginMenu.Run();
            bool keepLoop = true;

            while (keepLoop)
            {
                switch (selectedIndex)
                {
                    case 0: // wyszukiwarka
                        Szukaj();
                        break;
                    case 1: // sprawdz moje wizyty
                        MojeWizyty();
                        break;
                    case 2: // utwórz nową wizyte
                        SignUp();
                        break;
                    case 3: // wróć do ekranu głównego
                        LoggedUser.Logged = "no";
                        RunMainMenu();
                        break;
                }
            }
        }



        private void Szukaj() // opcja szukaj dostępna zarówno dla użytkowników zalogowanych jak i niezalogowanych
        {

            Console.Clear();

            string Prompt = "Jakie miasto Cie interesuje? Wybierz opcję klikając klawisz ENTER:";
            string[] Options = { "Kraków", "Łódź", "Poznań", "Warszawa", "Wrocław", "Wróć" };

            Menu mainMenu = new Menu(Prompt, Options);

            int selectedIndex = mainMenu.Run();

            switch (selectedIndex)
            {
                case 0:
                    SzukajPoMiescie("Kraków");
                    break;
                case 1:
                    SzukajPoMiescie("Łódź");
                    break;
                case 2:
                    SzukajPoMiescie("Poznań");
                    break;
                case 3:
                    SzukajPoMiescie("Warszawa");
                    break;
                case 4:
                    SzukajPoMiescie("Wrocław");
                    break;
                case 5:
                    if (LoggedUser.Logged == "yes")
                    {
                        BrowserForLogged();
                    }
                    else
                    {
                        RunMainMenu();
                    }
                    break;
            }

        }



        private void SzukajPoMiescie(string cityName) // funkcja wykonawsza dla opcji Szukaj()
        {
            Console.Clear();

            using (MySqlConnection connection = GetMySqlConnection())
            {
                connection.Open();

                string query = "SELECT h.ID AS HospitalID, h.name AS HospitalName, h.location AS HospitalLocation, w.name AS WardName, CONCAT(d.firstName, ' ', d.lastName) AS DoctorName FROM Hospitals h LEFT JOIN Information i ON h.ID = i.HospitalID LEFT JOIN Wards w ON i.WardID = w.ID LEFT JOIN Doctors d ON i.DoctorID = d.ID WHERE h.location = @CityName;";

                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@CityName", cityName);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            int currentHospitalID = -1;  // Zmienna do śledzenia bieżącego szpitala

                            Console.WriteLine("Oto lista szpitali w wybranym przez Ciebie mieście:");

                            while (reader.Read())
                            {
                                int hospitalID = Convert.ToInt32(reader["HospitalID"]);

                                if (hospitalID != currentHospitalID)
                                {
                                    Console.WriteLine();
                                    Console.WriteLine(reader["HospitalName"]);
                                    Console.WriteLine($"- Lokalizacja: {reader["HospitalLocation"]}");
                                    currentHospitalID = hospitalID;
                                }

                                Console.Write($"  - Oddział: {reader["WardName"]}");
                                Console.WriteLine($", lekarz: {reader["DoctorName"]}");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Brak szpitali w mieście {cityName}.");
                        }
                    }
                }
            }
            Console.ReadKey();
            Szukaj();
        }



        private void MojeWizyty() // wyświetlenie wizyt dla zalogowanego użytkownika
        {
            Console.Clear();

            using (MySqlConnection connection = GetMySqlConnection())
            {
                connection.Open();

                string query = "SELECT h.name AS HospitalName, h.location AS HospitalLocation, CONCAT(d.firstName, ' ', d.lastName) AS DoctorName, v.VisitDate FROM Visits v JOIN Information i ON v.InformationID = i.ID JOIN Hospitals h ON i.HospitalID = h.ID JOIN Doctors d ON i.DoctorID = d.ID WHERE v.PatientID = @UserId;";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@UserId", LoggedUser.UserId);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            int i = 1;

                            Console.WriteLine("Oto lista Twoich wizyt:");
                            Console.WriteLine();
                            while (reader.Read())
                            {
                                DateTime visitDate = reader.GetDateTime("VisitDate");
                                string tekstowaData = visitDate.ToString("yyyy-MM-dd");

                                Console.WriteLine("   Wizyta nr. " + i);
                                Console.WriteLine($"- Szpital:      |  {reader["HospitalName"]}");
                                Console.WriteLine($"- Lokalizacja:  |  {reader["HospitalLocation"]}");
                                Console.WriteLine($"- Lekarz:       |  {reader["DoctorName"]}");
                                Console.WriteLine($"- Data wizyty:  |  {tekstowaData}");

                                Console.WriteLine();
                                i++;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Nie masz zaplanowanych żadnych wizyt!");
                        }
                    }
                }
            }
            Console.ReadKey();
            BrowserForLogged();
        }



        private void SignUp() // zapisane się na wizytę
        {
            Console.Clear();

            string city = " ";

            string Prompt = "Jakie miasto Cie interesuje? Wybierz opcję klikając klawisz ENTER:";
            string[] Options = { "Kraków", "Łódź", "Poznań", "Warszawa", "Wrocław", "Wróć" };

            Menu mainMenu = new Menu(Prompt, Options);

            int selectedIndex = mainMenu.Run();

            switch (selectedIndex)
            {
                case 0:
                    city = "Kraków";
                    break;
                case 1:
                    city = "Łódź";
                    break;
                case 2:
                    city = "Poznań";
                    break;
                case 3:
                    city = "Warszawa";
                    break;
                case 4:
                    city = "Wrocław";
                    break;
                case 5:
                    BrowserForLogged();
                    break;
            }
            SignUpCity(city);
        }



        private void SignUpCity(string city) // funkcja dodająca miasto (czytelność kodu)
        {
            Console.Clear();

            MySqlConnection connection = GetMySqlConnection();
            MySqlCommand cmd = new MySqlCommand();

            string[] hospital = new string[5];

            for (int j = 0; j < hospital.Length; j++)
            {
                hospital[j] = "---";
            }

            connection.Open();

            int i = 0;

            string query = "SELECT * FROM hospitals WHERE location = '" + city + "';";
            cmd = new MySqlCommand(query, connection);

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    hospital[i] = reader["name"].ToString();
                    i++;
                }
            }

            connection.Close();

            string name = " ";

            string Prompt = "Jakie szpital Cie interesuje? Wybierz opcję klikając klawisz ENTER:";
            string[] Options = { hospital[0], hospital[1], hospital[2], hospital[3], hospital[4], "Wróć" };

            Menu mainMenu = new Menu(Prompt, Options);

            int selectedIndex = mainMenu.Run();

            switch (selectedIndex)
            {
                case 0:
                    name = hospital[0];
                    break;
                case 1:
                    name = hospital[1];
                    break;
                case 2:
                    name = hospital[2];
                    break;
                case 3:
                    name = hospital[3];
                    break;
                case 4:
                    name = hospital[4];
                    break;
                case 5:
                    BrowserForLogged();
                    break;
            }

            SignUpWard(city, name);
        }



        private void SignUpWard(string city, string name) // funkcja dodająca oodział (czytelność kodu)
        {
            Console.Clear();

            MySqlConnection connection = GetMySqlConnection();
            MySqlCommand cmd = new MySqlCommand();

            string[] wards = new string[5];

            for (int j = 0; j < wards.Length; j++)
            {
                wards[j] = "---";
            }

            connection.Open();

            int i = 0;

            string ward = "";

            string query = "SELECT i.ID, w.name AS WardName FROM information i JOIN Hospitals h ON i.HospitalID = h.ID JOIN Wards w ON i.WardID = w.ID WHERE h.name = '" + name + "' AND h.location = '" + city + "';";
            cmd = new MySqlCommand(query, connection);

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    wards[i] = reader["WardName"].ToString();
                    i++;
                }
            }

            connection.Close();

            string Prompt = "Jakie oddział Cie interesuje? Wybierz opcję klikając klawisz ENTER:";
            string[] Options = { wards[0], wards[1], wards[2], wards[3], wards[4], "Wróć" };

            Menu mainMenu = new Menu(Prompt, Options);

            int selectedIndex = mainMenu.Run();

            switch (selectedIndex)
            {
                case 0:
                    ward = wards[0];
                    break;
                case 1:
                    ward = wards[1];
                    break;
                case 2:
                    ward = wards[2];
                    break;
                case 3:
                    ward = wards[3];
                    break;
                case 4:
                    ward = wards[4];
                    break;
                case 5:
                    BrowserForLogged();
                    break;
            }
            SignUpTime(city, name, ward);
        }



        private void SignUpTime(string city, string name, string ward) // funkcja dodająca czas (czytelność kodu) oraz wykonująca polecenie INSERT
        {
            Console.Clear();

            MySqlConnection connection = GetMySqlConnection();
            MySqlCommand cmd = new MySqlCommand();

            string[] wards = new string[5];

            for (int j = 0; j < wards.Length; j++)
            {
                wards[j] = "---";
            }

            connection.Open();

            int i = 0;

            string query = "SELECT H.name AS HospitalName, H.location AS HospitalLocation, W.name AS WardName, V.* FROM Hospitals H JOIN Information I ON H.ID = I.HospitalID JOIN Wards W ON I.WardID = W.ID JOIN Visits V ON I.ID = V.InformationID WHERE H.name = '"+name+"' AND W.name = '"+ward+"';";
            cmd = new MySqlCommand(query, connection);

            Console.WriteLine("Oto lista terminów, które są zajęte:");

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                    DateTime visitDate = reader.GetDateTime("VisitDate");

                    Console.WriteLine(visitDate);
                    }
                    Console.ReadKey();
                }
            
            connection.Close();

            Console.WriteLine();
            Console.WriteLine("Wpisz termin, który Cię interesuje:");
            Console.WriteLine("(Termin należy wpisać w konstrukcji Amerykańskiej, tj. YYYY-MM-DD)");
            string userDate = askUser("Podaj datę: ");

            Console.Clear();

            connection = GetMySqlConnection();
            cmd = new MySqlCommand();

            connection.Open();

            string checkDateQuery = "SELECT COUNT(*) FROM Hospitals H JOIN Information I ON H.ID = I.HospitalID JOIN Wards W ON I.WardID = W.ID JOIN Visits V ON I.ID = V.InformationID WHERE H.name = '" + name + "' AND W.name = '" + ward + "' AND V.VisitDate = @UserDate;";

            cmd = new MySqlCommand(checkDateQuery, connection);
            cmd.Parameters.AddWithValue("@UserDate", userDate);

            int count = Convert.ToInt32(cmd.ExecuteScalar());

            if (count > 0)
            {
                Console.WriteLine("Podana data jest zajęta!");
                Console.ReadKey();
            }
            else
            {
                string expectedFormat = "yyyy-MM-dd";

                if (DateTime.TryParseExact(userDate, expectedFormat, null, System.Globalization.DateTimeStyles.None, out DateTime parsedDate))
                {
                    string getInformationIDQuery = "SELECT I.ID FROM Hospitals H JOIN Information I ON H.ID = I.HospitalID JOIN Wards W ON I.WardID = W.ID WHERE H.name = '" + name + "' AND W.name = '" + ward + "';";
                    cmd.Parameters.Clear();
                    cmd.CommandText = getInformationIDQuery;

                    int informationID = Convert.ToInt32(cmd.ExecuteScalar());

                    string insertVisitQuery = "INSERT INTO Visits (PatientID, InformationID, VisitDate) VALUES (" + LoggedUser.UserId + ", " + informationID + ", '" + userDate + "');";
                    cmd = new MySqlCommand(insertVisitQuery, connection);

                    cmd.ExecuteNonQuery();

                    Console.WriteLine("Wizyta została dodana!");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("Podana data nie jest w prawidłowym formacie!");
                    Console.ReadKey();
                }
            }
            connection.Close();
        }



        private void CreateUser(MySqlDataReader reader) // tworzenie użytkownika z zaspisem do pliku User.cs, skąd wykorzystywane są jego dane
        {
            reader.Read();
            int userId = reader.GetInt32("ID");
            string logged = "yes";
            string imie = reader.GetString("firstName");
            string nazwisko = reader.GetString("lastName");
            LoggedUser = new User(userId, logged, imie, nazwisko);
        }



        private string askUser(string prompt) // funkcja służąca do pobierania danych od użytkownika (ograniczenie kodu)
        {
            Console.Write(prompt);
            return Console.ReadLine();
        }



        private MySqlConnection GetMySqlConnection() // funkcja służąca do ponownego łączenia się z bazą danych (ograniczenie kodu)
        {
            string connectionString = "Server=localhost;Database=test3;Uid=root;Pwd=;";   
            MySqlConnection connection = new MySqlConnection(connectionString);                
            return connection;
        }
    }
}