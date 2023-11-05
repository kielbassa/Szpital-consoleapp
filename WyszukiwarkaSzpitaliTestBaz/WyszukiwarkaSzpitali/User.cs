using System;

namespace Program
{
    public class User
    {
        public int UserId;
        public string UserType;
        public string Imie;
        public string Nazwisko;

        public User(int userId = -1, string userType = "", string imie = "", string nazwisko = "")
        {
            this.UserId = userId;
            this.UserType = userType;
            this.Imie = imie;
            this.Nazwisko = nazwisko;
        }
    }

}