using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Utilities;
using Microsoft.EntityFrameworkCore;
using ProjectApi.Dtos;
using ProjectApi.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System;

namespace ProjectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly DataContext _dataContext;
        public ReservationsController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet("GetAllReservations")]
        public async Task<IActionResult> GetAllReservationsAsync()
        {
            var Reservations = await _dataContext.Reservations.ToListAsync();

            return Ok(Reservations);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReservationById(int id)
        {
            var Reservation = await _dataContext.Reservations.FindAsync(id);
            if (Reservation == null)
            {
                return NotFound(new { Error = "Client does not exist!" });
            }
            return Ok(Reservation);
        }

        private bool ValidateEgyptianPhoneNumber(string phoneNumber)
        {
            // Check if phoneNumber is null or empty
            if (string.IsNullOrEmpty(phoneNumber))
            {
                return false;
            }

            // Perform validation using regular expression
            string pattern = @"^(01)[0-9]{9}$";
            return Regex.IsMatch(phoneNumber, pattern);
        }

        
        [HttpPost("{id}")]
        public async Task<IActionResult> CreateReservationAsync(int id, [FromBody] ReservationDto dto)
        {
            var Stadium = await _dataContext.Stadiums.FindAsync(id);
            if (Stadium == null)
            {
                return NotFound($"Invalid Stadium ID : {id}");
            }

            if(!ValidateEgyptianPhoneNumber(dto.MobileNumber))
            {
                return BadRequest("Invalid Mobile Number!");
            }
           
                
            var reservation = new Reservation()
            {
                Name = dto.Name,
                MobileNumber = dto.MobileNumber,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                StadiumId = id
            };

            await _dataContext.Reservations.AddAsync(reservation);
            _dataContext.SaveChanges();
            return Ok(reservation);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReservationAsync(int id, [FromBody] ReservationDto dto)
        {
            var Reservation = await _dataContext.Reservations.FindAsync(id);
            if (Reservation == null)
            {
                return NotFound($"Invalid Reservation ID : {id}");
            }

            if (!ValidateEgyptianPhoneNumber(dto.MobileNumber))
            {
                return BadRequest("Invalid Mobile Number!");
            }

            //ValidateDateTimeAttributes(dto.StartTime, dto.EndTime);

            Reservation.Name = dto.Name;
            Reservation.MobileNumber = dto.MobileNumber;
            Reservation.StartTime = dto.StartTime;
            Reservation.EndTime = dto.EndTime;

            _dataContext.SaveChanges();
            return Ok(Reservation);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReservationAsync(int id)
        {
            var Reservation = await _dataContext.Reservations.FindAsync(id);
            if (Reservation == null)
            {
                return NotFound($"Invalid Reservation ID : {id}");
            }

            _dataContext.Remove(Reservation);
            _dataContext.SaveChanges();
            return Ok(Reservation);
        }

    }
}
