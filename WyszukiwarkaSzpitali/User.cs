using System;

namespace Program
{
    public class User
    {
        public int UserId;
        public string Logged;
        public string Imie;
        public string Nazwisko;

        public User(int userId = -1, string Logged = "No", string imie = "", string nazwisko = "")
        {
            this.UserId = userId;
            this.Logged = Logged;
            this.Imie = imie;
            this.Nazwisko = nazwisko;
        }
    }

}