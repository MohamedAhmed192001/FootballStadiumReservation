using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Utilities;
using Microsoft.EntityFrameworkCore;
using ProjectApi.Dtos;
using ProjectApi.Dtos.OwnerDtos;
using ProjectApi.Dtos.StadiumDtos;
using ProjectApi.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ProjectApi.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class StadiumsController : ControllerBase
    {
        private readonly DataContext _dataContext;

        private List<string> _allowedExtenstions = new List<string>() { ".png", ".jpg", ".jpeg" };
        private long _maxAllowedPictureSize = 1048576;
        public StadiumsController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet("GetAllStadiums")]
        public async Task<IActionResult> GetAllStadiumssAsync()
        {
            var Stadiums = await _dataContext.Stadiums.ToListAsync();
            return Ok(Stadiums);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStadiumByIdAsync(int id)
        {
            var stadium = await _dataContext.Stadiums.FindAsync(id);
            if (stadium == null)
            {
                return NotFound(new {Error = "Stadium does not exist!"});
            }
            return Ok(stadium);
        }



        [HttpPost]
        public async Task<IActionResult> CreateStadiumAsync([FromForm] CreateStadiumDto dto)
        {

            if (dto.Image == null)
            {
                return BadRequest("The Photo is required!");
            }

            if (!_allowedExtenstions.Contains(Path.GetExtension(dto.Image.FileName).ToLower()))
            {
                return BadRequest("Only .png, .jpg and .jpeg images are allowed!");
            }

            if (dto.Image.Length > _maxAllowedPictureSize)
            {
                return BadRequest("Max allowed size for photo is 1MB! ");
            }

            var isValidOwnerId = await _dataContext.Owners.AnyAsync(s => s.OwnerId == dto.OwnerId);
            if (!isValidOwnerId)
            {
                return BadRequest("Invalid Owner Id!!");
            }

            using var dataStream = new MemoryStream();
            {
                await dto.Image.CopyToAsync(dataStream);
            }
            var ownerObjectToGetMoreDetailsAboutIt = await _dataContext.Owners.FindAsync(dto.OwnerId);
            var Stadium = new Stadium()
            {
                Name = dto.Name,
                Address = dto.Address,
                AvaliableHours = dto.AvaliableHours,
                BallType = dto.BallType,
                HourPrice = dto.HourPrice,
                Image = dataStream.ToArray(),
                Offer = dto.Offer,  
                Rating = dto.Rating,    
                ReservedHours = dto.ReservedHours,
                StadiumArea = dto.StadiumArea,
                OwnerId = dto.OwnerId,
                OwnerName = ownerObjectToGetMoreDetailsAboutIt.Name

            };

            await _dataContext.AddAsync(Stadium);
            _dataContext.SaveChanges();

            

            return Ok(Stadium);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOwnerAsync(int id, [FromForm] UpdateStadiumDto dto)
        {
            var stadium = await _dataContext.Stadiums.FindAsync(id);
            if (stadium == null)
            {
                return NotFound($"The stadium is not exist in Id : {id}");
            }

            var isValidOwnerId = await _dataContext.Stadiums.AnyAsync(s => s.OwnerId == dto.OwnerId);

            if (!isValidOwnerId)
            {
                return BadRequest("Invalid Owner Id!!");
            }

            if (dto.Image != null)
            {
                if (!_allowedExtenstions.Contains(Path.GetExtension(dto.Image.FileName).ToLower()))
                {
                    return BadRequest("Only .png and .jpg images are allowed!");
                }

                if (dto.Image.Length > _maxAllowedPictureSize)
                {
                    return BadRequest("Max allowed size for photo is 1MB!");
                }

                using var dataStream = new MemoryStream();
                await dto.Image.CopyToAsync(dataStream);
                stadium.Image = dataStream.ToArray();
            }

            stadium.Name = dto.Name;
            stadium.Address = dto.Address;
            stadium.AvaliableHours = dto.AvaliableHours;
            stadium.ReservedHours = dto.ReservedHours;
            stadium.HourPrice = dto.HourPrice;  
            stadium.Rating = dto.Rating;    
            stadium.BallType = dto.BallType;    
            stadium.OwnerId = dto.OwnerId;
            stadium.StadiumArea = dto.StadiumArea;
            stadium.Offer = dto.Offer;



            _dataContext.SaveChanges();

            return Ok(stadium);
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStadiumAsync(int id)
        {
            var stadium = await _dataContext.Stadiums.FindAsync(id);
            if (stadium == null)
            {
                return NotFound($"The stadium is not found in Id : {id}!");
            }

            _dataContext.Remove(stadium);
            _dataContext.SaveChanges();
            return Ok(stadium);
        }
    }
}
