using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.DTO
{
    public class DosyaDTO
    {
        public int Id { get; set; }
        public string GorunenAd { get; set; } = string.Empty;
        public string DosyaAdi { get; set; } = string.Empty;
        public string Tip { get; set; } = string.Empty;
        public string Uzanti { get; set; } = string.Empty;
        public string Dosya { get; set; } = string.Empty;
        public Int64 Boyut { get; set; }
    }
}
