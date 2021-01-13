using Minify.DAL.Models;

using System;
using System.ComponentModel.DataAnnotations;

namespace Minify.DAL.Entities
{
    public class User : BaseEntity
    {
        [Required]
        public string UserName { get; set; }

        [MinLength(8), RegularExpression("^(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$")]
        public string PassWord { get; set; }

        [Required]
        public Guid PersonId { get; set; }
        public DefaultTheme DefaultTheme { get; set; }


        public Person Person { get; set; }


        public User()
        {
            DefaultTheme = DefaultTheme.Light;
        }

        public User(string userName, string email, string firstName, string lastName, string passWord) : this()
        {
            UserName = userName;
            PassWord = passWord;
            Person = new Person(firstName, lastName, email);
        }
    }
}