using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndoorMappingApp.Scripts.DTOs
{
    class UpdateAccountRequestDTO
    {
        //public string NewPassword { get; set; }
        public int usuarioId { get; set; }
        public string nome { get; set; }
        public string email { get; set; }
        public int tipoUsuarioId { get; set; }
        public int mobilidadeId { get; set; }

        //public int Language { get; set; }
    }
}
