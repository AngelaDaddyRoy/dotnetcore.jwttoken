using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotnetCore.JwtToken.Models
{
    public class TokenDTO
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
