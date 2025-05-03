using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndoorMappingApp.Scripts.DTOs
{
    public class ResetPasswordRequestDTO
    {
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
}
