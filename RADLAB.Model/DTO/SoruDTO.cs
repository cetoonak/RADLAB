using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.DTO
{
    public class SoruDTO
    {
        public int Id { get; set; }
        public string Soru { get; set; } = string.Empty;
        public string DosyaAdi { get; set; } = string.Empty;
        public string Uzanti { get; set; } = string.Empty;
        public string Dosya { get; set; } = string.Empty;
        public string CevapA { get; set; } = string.Empty;
        public string CevapB { get; set; } = string.Empty;
        public string CevapC { get; set; } = string.Empty;
        public string CevapD { get; set; } = string.Empty;
        public string CevapE { get; set; } = string.Empty;
        public int Cevap { get; set; }
        public int Sira { get; set; }
        public int Zorluk { get; set; }
        public string Etiket { get; set; } = string.Empty;
        public bool Checked { get; set; }
    }
}