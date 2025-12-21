using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.DTO
{
    public class StokHareketDTO
    {
        public int Id { get; set; }
        public int StokHareketTuruId { get; set; }
        public string StokHareketTuru { get; set; } = string.Empty;
        public DateTime? HareketTarihi { get; set; }
        public string EvrakNo { get; set; } = string.Empty;
        public DateTime? EvrakTarihi { get; set; }
        public string Aciklama { get; set; } = string.Empty;
        public List<StokHareketCihazDTO> StokHareketCihazList { get; set; }
        public bool Expanded { get; set; }
        public int DetailCount { get; set; }
        public bool Checked { get; set; }
    }
}