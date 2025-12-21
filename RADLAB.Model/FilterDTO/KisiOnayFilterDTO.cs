using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.FilterDTO
{
    public class KisiOnayFilterDTO : FilterDTO
    {
        public int KurulusId { get; set; }
        public int BirimId { get; set; }
        public string AdSoyad { get; set; } = string.Empty;
        public string TCKimlikNo { get; set; } = string.Empty;
        public string EMail { get; set; } = string.Empty;
        public int @GizlilikOnaylandi { get; set; }
        public int AcikRiza { get; set; }
        public int CookiePolitikasiniGordu { get; set; }
        public int AnketDoldurdu { get; set; }
        public int AnketYili { get; set; }
        public int AnketPuan1 { get; set; }
        public int AnketPuan2 { get; set; }
        public int Aktif { get; set; }
    }
}