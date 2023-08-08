using Microsoft.AspNetCore.Http;

namespace ProjectApi.Dtos.OwnerDtos
{
    public class UpdateOwnerDto : BaseOwnerDto
    {
        public IFormFile? Photo { get; set; }
    }
}
