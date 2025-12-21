using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.DTO
{
    public class OnlineEgitimTreeDTO
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public int VideoIdSoruId { get; set; }
        public string Baslik { get; set; } = string.Empty;
        public int Cevap { get; set; }
        public string Sira { get; set; } = string.Empty;
        public bool Tamamlandi { get; set; }
        public bool BuradaKaldi { get; set; }
        public int ToplamSure { get; set; }
        public int GecenSure { get; set; }
        public int VideoTime { get; set; }
    }
}