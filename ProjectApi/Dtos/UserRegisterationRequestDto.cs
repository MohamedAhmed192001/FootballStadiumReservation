using System.ComponentModel.DataAnnotations;

namespace ProjectApi.Dtos
{
    public class UserRegisterationRequestDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
