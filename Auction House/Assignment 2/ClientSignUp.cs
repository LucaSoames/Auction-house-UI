using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2
{
    class ClientSignUp
    {
        private string name;
        public string Name { get; set; }

        private string email;

        public string Email { get; set; }

        private string password;

        public string Password { get; set; }


        public ClientSignUp(string userName, string userEmailaddress, string userPassword)
        {
            //constructor
            Name = userName;
            Email = userEmailaddress;
            Password = userPassword;
        }
        
        
    }
}
