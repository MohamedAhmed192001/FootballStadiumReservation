using System;

namespace ProjectApi.Dtos
{
    public class ReservationDto
    {
        public string Name { get; set; }
        public string MobileNumber { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
