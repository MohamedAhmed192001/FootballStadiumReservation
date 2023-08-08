using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ProjectApi.Dtos.OwnerDtos
{
    public class CreateOwnerDto : BaseOwnerDto
    {
        [Required]
        public IFormFile Photo { get; set; }
    }
}
