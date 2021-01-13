using System.ComponentModel.DataAnnotations;


namespace Minify.DAL.Entities
{
    public class Person : BaseEntity
    {
        [Required]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        public Person() { }

        public Person(string firstName, string lastName, string email)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }

    }
}
