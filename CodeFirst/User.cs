using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirst
{
    internal class User
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PatronomicName { get; set; }
        public int Age { get; set; }
        public string? Role { get; set; }
        public string Account { get; set; }
        public decimal Balance { get; set; }

        public User(int id, string? firstName, string? lastName, string? patronomicName, int age, string? role, string account, decimal balance)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            PatronomicName = patronomicName;
            Age = age;
            Role = role;
            Account = account;
            Balance = balance;
        }
    }
}
