using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Framework.Enums;
using static TaskManagement.Framework.Statics.UserStatics;

namespace TaskManagement.DataAccessLayer.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        public User(string fullName, string username, string password, string role)
        {
            Id = Guid.NewGuid();
            FullName = fullName;
            Username = username;
            Password = password;
            Role = role;
        }
    }
}
