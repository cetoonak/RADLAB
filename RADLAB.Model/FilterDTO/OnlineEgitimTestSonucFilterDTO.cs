using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.FilterDTO
{
    public class OnlineEgitimTestSonucFilterDTO : FilterDTO
    {
        public int OnlineEgitimId { get; set; }
        public DateTime? Tarih1 { get; set; }
        public DateTime? Tarih2 { get; set; }
        public int Grup { get; set; }
        public int TestId { get; set; }
        public int SoruId { get; set; }
        public string AdSoyad { get; set; } = string.Empty;
    }
}