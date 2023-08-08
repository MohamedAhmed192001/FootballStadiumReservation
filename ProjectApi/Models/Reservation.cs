using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectApi.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public string Name { get; set; }    
        public string MobileNumber { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public Stadium Stadium { get; set; }
        
        public int StadiumId { get; set; }

        
        

    }
}
