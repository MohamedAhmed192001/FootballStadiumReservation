using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ProjectApi.Dtos.ClientDtos
{
    public class CreateClientDto : BaseClientDto
    {
        [Required]
        public IFormFile Photo { get; set; }
    }
}
