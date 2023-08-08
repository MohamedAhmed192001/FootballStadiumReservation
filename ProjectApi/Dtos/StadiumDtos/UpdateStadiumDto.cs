using Microsoft.AspNetCore.Http;

namespace ProjectApi.Dtos.StadiumDtos
{
    public class UpdateStadiumDto :BaseStadiumDto
    {
        public IFormFile? Image { get; set; }
    }
}
