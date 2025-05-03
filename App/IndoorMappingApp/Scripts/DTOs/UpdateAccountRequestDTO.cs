using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndoorMappingApp.Scripts.DTOs
{
    class UpdateAccountRequestDTO
    {
        public string NewPassword { get; set; }
        public int Limitation { get; set; }  // Use enum index
        public int Language { get; set; }    // Use enum index
    }
}
