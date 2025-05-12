using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndoorMappingApp.Scripts.DTOs
{
    public class AllRoutesResponseDTO
    {
        public int origemId { get; set; }
        public int destinoId { get; set; }
        public List<List<PathDetail>> caminhos { get; set; }
    }
}
