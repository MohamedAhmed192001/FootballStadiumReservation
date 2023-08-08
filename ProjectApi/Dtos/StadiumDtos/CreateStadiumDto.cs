using Microsoft.AspNetCore.Http;

namespace ProjectApi.Dtos.StadiumDtos
{
    public class CreateStadiumDto : BaseStadiumDto
    {
        public IFormFile Image { get; set; }
    }
}
