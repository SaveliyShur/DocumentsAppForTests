using System.ComponentModel.DataAnnotations;

namespace ShopAppForTest.Models.Api
{
    public class CreateUserModel
    {
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public string? Patronymic { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Range(14,1000)]
        public int Age { get; set; }

        public UserPersonalInfo? UserPersonalInfo { get; set; }
    }

    public class UserPersonalInfo
    {
        public string? UserNumber { get; set; }

        public string? UserEmail { get; set; }

        public string? Description { get; set; }

        public Passport? Passport { get; set; }
    }

    public class Passport
    {
        public string? Address { get; set; }

        public string? City { get; set; }

        public int? Series { get; set; }

        public int? Number { get; set; }
    }
}
