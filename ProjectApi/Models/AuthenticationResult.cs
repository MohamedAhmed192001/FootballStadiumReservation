using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectApi.Models
{
    public class AuthenticationResult
    {
        public string Token { get; set; }        
        public bool Result { get; set; }
        public List<string> Errors { get; set; }
        public List <string> Roles { get; set; }
        
    }
}
