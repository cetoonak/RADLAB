using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.DTO
{
    public class StokMiktarlariDTO
    {
        public int Id { get; set; }
        public string Marka { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Miktar { get; set; }
    }
}