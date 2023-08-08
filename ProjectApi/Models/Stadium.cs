using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectApi.Models
{
    public class Stadium
    {
        public int StadiumId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(100)]
        public string Address { get; set; }

        [Required]
        public byte[] Image { get; set; }

        [Required]
        public int AvaliableHours { get; set; }

        [Required]
        public int ReservedHours { get; set; }

        public string Offer { get; set; }

        [Required]
        public int HourPrice { get; set; }

        [Required]
        public double StadiumArea { get; set; }

        [Required]
        [MaxLength(100)]
        public string BallType { get; set; }

        public int Rating { get; set; }

        public int MyProperty { get; set; }

        public int OwnerId { get; set; }

        public string OwnerName { get; set; }
        public Owner Owner { get; set; }

        public List<Reservation> Reservations { get; set; }


    }
}
