using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectApi.Models
{
    public class Owner
    {

        public int OwnerId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(150)]
        public string Address { get; set; }

        [Required]
        [MaxLength(150)]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required]
        public byte[]  Photo { get; set; }

        [Required]
        [MaxLength(50)]
        public string PhoneNumber { get; set; }

        public List<Stadium> Stadiums  { get; set; }

    }
}
