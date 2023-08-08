using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;

namespace ProjectApi.Dtos
{
    public class UserLoginRequestDto
    {

        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        public string Password { get; set; }

    }
}
