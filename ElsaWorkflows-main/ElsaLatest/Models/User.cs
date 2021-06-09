using System;

namespace Elsa.Samples.UserRegistration.Web.Models
{
    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public string Country { get; set; }
        public string Gender { get; set; }
        public string AboutMe { get; set; }
        public bool IsActive { get; set; }
        public DateTime Date { get; set; }
    }
}