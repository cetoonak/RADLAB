using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.DTO
{
    public class KalibrasyonCihazHareketDTO
    {
        public int Id { get; set; }
        public int KalibrasyonCihazId { get; set; }
        public KalibrasyonCihazDTO KalibrasyonCihaz { get; set; }
        public int HareketTuruId { get; set; }
        public string HareketTuruString { get; set; } = string.Empty;
        public HareketTuruDTO HareketTuru { get; set; }
        public DateTime Zaman { get; set; }
        public string Aciklama { get; set; } = string.Empty;
        public int DosyaSayisi { get; set; }
        public List<KalibrasyonCihazHareketDosyaDTO> KalibrasyonCihazHareketDosyaList { get; set; }
    }
}