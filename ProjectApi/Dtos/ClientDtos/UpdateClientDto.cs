using Microsoft.AspNetCore.Http;
using Microsoft.Build.Framework;

namespace ProjectApi.Dtos.ClientDtos
{
    public class UpdateClientDto : BaseClientDto
    {
        public IFormFile? Photo { get; set; }
    }
}
