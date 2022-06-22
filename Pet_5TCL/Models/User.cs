using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pet_5TCL.Models
{
    public class User
    {

        public string Username { get; set; }
        public string Password { get; set; }

        public User(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}