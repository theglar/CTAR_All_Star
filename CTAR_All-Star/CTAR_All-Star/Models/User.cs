using System;
using CTAR_All_Star.Database;
using SQLite;

namespace CTAR_All_Star.Models
{
    public class User
    {
        [PrimaryKey, AutoIncrement, Unique]
        public int Id { get; set; }
        
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPass { get; set; }
        public string userType { get; set;  }
        public bool IsLoggedIn { get; set; }

        public User()
        {
        }

        public User(string Username, string Password, string userType)
        {
            this.Username = Username;
            this.Password = Password;
            this.userType = userType;
        }

        public User(string Username, string Password)
        {
            this.Username = Username;
            this.Password = Password;
        }


        public bool CheckInformation()
        {
            DatabaseHelper dbHelper = new DatabaseHelper();
            if (!this.Username.Equals("") && !this.Password.Equals("") && dbHelper.verifyUser(this.Username, this.Password))
                return true;
            else
                return false;
        }

        public bool VerifySignUp()
        {

            if (!this.Username.Equals("") && !this.Password.Equals(""))
                return true;
            else
                return false;
        }
    }
}
