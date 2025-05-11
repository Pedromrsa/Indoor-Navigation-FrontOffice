using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndoorMappingApp.Scripts.DTOs
{
    class UpdateAccountRequestDTO
    {
        public int usuarioId { get; set; }
        public string nome { get; set; }
        public string email { get; set; }
        public int tipoUsuarioId { get; set; }
        public int mobilidadeId { get; set; }
    }
}
