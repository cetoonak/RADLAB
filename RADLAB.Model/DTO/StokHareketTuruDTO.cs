using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.DTO
{
    public class StokHareketTuruDTO
    {
        public int Id { get; set; }
        public string StokHareketTuru { get; set; } = string.Empty;
        public bool GirisCikis { get; set; }
    }
}