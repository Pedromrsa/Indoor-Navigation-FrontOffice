using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndoorMappingApp.Scripts.DTOs
{
    public class GetMelhorCaminhoDTO
    {
        public List<long> InfraestruturasIds { get; set; }
        public bool UsouEntradaAlternativa { get; set; }
        public string Mensagem { get; set; }
    }
}
