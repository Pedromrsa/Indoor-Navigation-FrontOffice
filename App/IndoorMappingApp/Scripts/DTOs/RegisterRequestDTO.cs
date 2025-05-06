using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndoorMappingApp.Scripts.DTOs
{
    public class RegisterRequestDTO
    {
        public string nome { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public int tipoId { get; set; } // 4 User
        public int mobilidadeId { get; set; } 
    }


}
