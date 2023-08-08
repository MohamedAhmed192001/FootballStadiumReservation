using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectApi.Dtos;
using ProjectApi.Dtos.ClientDtos;
using ProjectApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProjectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly DataContext _dataContext;

        private List<string> _allowedExtenstions = new List<string>() { ".png",".jpg", ".jpeg" };
        private long _maxAllowedPictureSize = 1048576;

        public ClientsController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }


        [HttpGet("GetAllClients")]
        public async Task <IActionResult> GetAllClients()
        {
            var clients = await _dataContext.Clients.ToListAsync();

            return Ok(clients);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClientById(int id)
        {
            var Client = await _dataContext.Clients.FindAsync(id);
            if (Client == null)
            {
                return NotFound(new { Error = "Client does not exist!" });
            }
            return Ok(Client);
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

        [HttpPost]
        public async Task<IActionResult> CreateClientAsync([FromForm] CreateClientDto dto)
        {
            
            if (!ValidateEgyptianPhoneNumber(dto.PhoneNumber))
            {
                return BadRequest("Invalid Mobile Number!");
            }

            if (dto.Photo == null)
            {
                return BadRequest("The Photo is required!");
            }

           if(!_allowedExtenstions.Contains(Path.GetExtension(dto.Photo.FileName).ToLower()))
           {
                return BadRequest("Only .png, .jpg and .jpeg images are allowed!");
           }

           if(dto.Photo.Length > _maxAllowedPictureSize)
           {
                return BadRequest("Max allowed size for photo is 1MB! ");
           }

            using var dataStream = new MemoryStream();
            await dto.Photo.CopyToAsync(dataStream);

            var client = new Client
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Address = dto.Address,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Photo = dataStream.ToArray(),
                ReservedHours = dto.ReservedHours,
                WayOfPay = dto.WayOfPay
             
            };
             await _dataContext.AddAsync(client);
            _dataContext.SaveChanges();

            return Ok(client);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClientAsync(int id, [FromForm] UpdateClientDto dto)
        {
            if (!ValidateEgyptianPhoneNumber(dto.PhoneNumber))
            {
                return BadRequest("Invalid Mobile Number!");
            }

            var client = await _dataContext.Clients.FindAsync(id);

            if (client == null)
            {
                return NotFound($"The client is not found in Id : {id}");
            }

            if (dto.Photo != null)
            {
                if (!_allowedExtenstions.Contains(Path.GetExtension(dto.Photo.FileName).ToLower()))
                {
                    return BadRequest("Only .png and .jpg images are allowed!");
                }

                if (dto.Photo.Length > _maxAllowedPictureSize)
                {
                    return BadRequest("Max allowed size for photo is 1MB! ");
                }

                using var dataStream = new MemoryStream();
                await dto.Photo.CopyToAsync(dataStream);

                client.Photo = dataStream.ToArray();

            }
            
            client.FirstName = dto.FirstName;   
            client.LastName = dto.LastName; 
            client.Address = dto.Address;
            client.Email = dto.Email;
            client.PhoneNumber = dto.PhoneNumber;
            client.ReservedHours = dto.ReservedHours;
            client.WayOfPay = dto.WayOfPay;

            _dataContext.SaveChanges();

            return Ok(client);

        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStadiumAsync(int id)
        {
            var stadium = await _dataContext.Clients.FindAsync(id);
            if (stadium == null) 
            {
                return NotFound($"The client is not found in Id : {id}!");
            }

            _dataContext.Remove(stadium);  
            _dataContext.SaveChanges();
            return Ok(stadium);
        }
       

    }
}
