using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ProjectApi.Dtos.ClientDtos
{
    public class BaseClientDto
    {
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }


        [Required]
        [MaxLength(50)]
        public string Address { get; set; }


        [Required]
        [MaxLength(50)]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required]
        [MaxLength(30)]
        public string PhoneNumber { get; set; }

        [Required]
        public int ReservedHours { get; set; }

        [MaxLength(100)]
        public string WayOfPay { get; set; }
    }
}
