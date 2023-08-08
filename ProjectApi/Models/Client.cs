using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectApi.Models
{
    public class Client
    {
        public int ClientId { get; set; }
        
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
        public byte[] Photo { get; set; }

        public int ReservedHours { get; set; }

        [MaxLength(100)]
        public string WayOfPay { get; set; }

        public List<Reservation> Reservations { get; set; }


    }
}
