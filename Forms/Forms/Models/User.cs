using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace Forms.Models
{
    [Table("user")]
    class User
    {
        public User() { }
        public User(string username, string password)
        {
            Username = username;
            Password = password;
        }

        [PrimaryKey,Column("username")]
        public string Username { get; set; }

        [Column("email_id")]
        public string EmailId { get; set; }

        [Column("password")]
        public string Password { get; set; }

        public override string ToString()
        {
            return $"Username: {Username} --- EmailId: {EmailId} || Password: {Password}";
        }
    }
}
