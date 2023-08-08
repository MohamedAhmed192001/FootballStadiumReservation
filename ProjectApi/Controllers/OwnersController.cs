using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Utilities;
using ProjectApi.Models;
using ProjectApi.Dtos;
using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjectApi.Dtos.OwnerDtos;
using System.Collections.Generic;
using ProjectApi.Dtos.ClientDtos;
using System.Text.RegularExpressions;

namespace ProjectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnersController : ControllerBase
    {
        private readonly DataContext _dataContext;

        private List<string> _allowedExtenstions = new List<string>() { ".png", ".jpg", ".jpeg"};
        private long _maxAllowedPictureSize = 1048576;

        public OwnersController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet("GetAllOwners")]
        public async Task<IActionResult> GetAllOwnersAsync()
        {
            var Owners = await _dataContext.Owners.ToListAsync();
            return Ok(Owners);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOwnerById(int id)
        {
            var Owner = await _dataContext.Owners.FindAsync(id);
            if (Owner == null)
            {
                return NotFound(new { Error = "Client does not exist!" });
            }
            return Ok(Owner);
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
        public async Task<IActionResult> CreateOwnerAsync([FromForm] CreateOwnerDto dto)
        {

            if (!ValidateEgyptianPhoneNumber(dto.PhoneNumber))
            {
                return BadRequest("Invalid Phone Number!");
            }

            if (dto.Photo == null)
            {
                return BadRequest("the Image is required!");
            }

            if(!_allowedExtenstions.Contains(Path.GetExtension(dto.Photo.FileName).ToLower()))
            {
                return BadRequest("Only .png, .jpg and .jpeg images are allowed!");
            }

            if(dto.Photo.Length > _maxAllowedPictureSize)
            {
                return BadRequest("Max allowed size for photo is 1MB!");
            }

            using var dataStream = new MemoryStream();
            {
               await dto.Photo.CopyToAsync(dataStream);
            }
            var Owner = new Owner()
            {
                Address = dto.Address,
                Email = dto.Email,
                Name = dto.Name,    
                PhoneNumber = dto.PhoneNumber,
                Photo = dataStream.ToArray()
            };

            await _dataContext.AddAsync(Owner);
            _dataContext.SaveChanges();  

            return Ok(Owner);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOwnerAsync(int id, [FromForm] UpdateOwnerDto dto)
        {
            if (!ValidateEgyptianPhoneNumber(dto.PhoneNumber))
            {
                return BadRequest("Invalid Phone Number!");
            }

            var owner = await _dataContext.Owners.FindAsync(id); 
            if (owner == null)
            {
                return NotFound($"The owner is not exist in Id : {id}");
            }


            if(dto.Photo != null)
            {
                if (!_allowedExtenstions.Contains(Path.GetExtension(dto.Photo.FileName).ToLower()))
                {
                    return BadRequest("Only .png and .jpg images are allowed!");
                }

                if (dto.Photo.Length > _maxAllowedPictureSize)
                {
                    return BadRequest("Max allowed size for photo is 1MB!");
                }

                using var dataStream = new MemoryStream();
                await dto.Photo.CopyToAsync(dataStream);    
                owner.Photo = dataStream.ToArray();
            }


            owner.Name = dto.Name;
            owner.PhoneNumber = dto.PhoneNumber;
            owner.Address = dto.Address;
            owner.Email = dto.Email;


            _dataContext.SaveChanges();

            return Ok(owner);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOwnerAsync(int id)
        {
            var owner = await _dataContext.Owners.FindAsync(id);
            if (owner == null)
            {
                return NotFound($"The owner is not found in Id : {id}!");
            }

            _dataContext.Remove(owner);
            _dataContext.SaveChanges();
            return Ok(owner);
        }
    }
}
