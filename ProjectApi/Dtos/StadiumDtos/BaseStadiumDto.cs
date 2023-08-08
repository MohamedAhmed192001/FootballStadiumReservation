using Microsoft.AspNetCore.Http;
using ProjectApi.Models;
using System.ComponentModel.DataAnnotations;

namespace ProjectApi.Dtos.StadiumDtos
{
    public class BaseStadiumDto
    {

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(100)]
        public string Address { get; set; }

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

        public int OwnerId { get; set; }
        
    } 
}

