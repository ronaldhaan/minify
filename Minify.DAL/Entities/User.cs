using System.ComponentModel.DataAnnotations;

namespace Minify.DAL.Entities
{
    public class User : BaseEntity
    {
        [Required]
        public string UserName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [MinLength(8), RegularExpression("^(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$")]
        public string PassWord { get; set; }

        public User()
        {
        }

        public User(string userName, string email, string firstName, string lastName, string passWord)
        {
            UserName = userName;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            PassWord = passWord;
        }
    }
}