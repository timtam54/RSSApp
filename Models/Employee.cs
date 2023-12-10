using System;
using SQLite;
namespace RssMob.Models
{
	public class Employee
	{
        public Employee()
        {
        }

        public Employee(int id, string given, string surname, bool? inspector, string email, string password)
        {
            this.id = id;
            Given = given;
            Surname = surname;
            Inspector = inspector;
            Email = email;
            Password = password;
        }
        [PrimaryKey]
        public int id { get; set; }
        //        [Display(Name = "Given Name")]
        public string? Given { get; set; }
        public string? Surname { get; set; }
        public bool? Inspector { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}

