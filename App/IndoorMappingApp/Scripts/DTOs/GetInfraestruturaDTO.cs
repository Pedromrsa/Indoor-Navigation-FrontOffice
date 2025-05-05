using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndoorMappingApp.Scripts.DTOs
{
    public class GetInfraestruturaDTO
    {
        public long Id { get; set; }
        public string? Descricao { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int Piso { get; set; }
        public bool Acessivel { get; set; }
        public string TipoInfraestrutura { get; set; } = string.Empty;
    }
}
