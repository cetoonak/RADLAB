using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.DTO
{
    public class StokHareketleriDTO
    {
        public int Id { get; set; }
        public string Marka { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int MiktarGiren { get; set; }
        public int MiktarCikan { get; set; }
        public string StokHareketTuru { get; set; } = string.Empty;
        public DateTime? HareketTarihi { get; set; }
        public string Aciklama { get; set; } = string.Empty;
    }
}