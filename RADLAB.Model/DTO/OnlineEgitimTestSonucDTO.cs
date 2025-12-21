using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.DTO
{
    public class OnlineEgitimTestSonucDTO
    {
        public int Id { get; set; }
        public string OnlineEgitim { get; set; } = string.Empty;
        public DateTime Tarih1 { get; set; }
        public DateTime Tarih2 { get; set; }
        public int Grup { get; set; }
        public string Test { get; set; } = string.Empty;
        public decimal GecmeOrani { get; set; }
        public string AdSoyad { get; set; } = string.Empty;
        public string Soru { get; set; } = string.Empty;
        public int SoruSira { get; set; }
        public string DogruCevap { get; set; } = string.Empty;
        public string OgrenciCevap { get; set; } = string.Empty;
        public string SoruSonucu { get; set; } = string.Empty;
    }
}