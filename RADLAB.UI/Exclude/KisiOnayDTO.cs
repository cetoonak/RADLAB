using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.DTO
{
    public class KisiOnayDTO
    {
        public int Id { get; set; }
        public string AdSoyad { get; set; } = string.Empty;
        public string TCKimlikNo { get; set; } = string.Empty;
        public string Kurulus { get; set; } = string.Empty;
        public string Birim { get; set; } = string.Empty;
        public string EMail { get; set; } = string.Empty;
        public bool GizlilikOnaylandi { get; set; }
        public string GizlilikOnaylandiString { get; set; } = string.Empty;
        public int AcikRiza { get; set; }
        public string AcikRizaString { get; set; } = string.Empty;
        public bool CookiePolitikasiniGordu { get; set; }
        public string CookiePolitikasiniGorduString { get; set; } = string.Empty;
        public int Puan { get; set; }
        public int AnketId { get; set; }
        public string AnketDoldurduString { get; set; } = string.Empty;
        public DateTime? AnketDoldurulmaTarihi { get; set; }
        public bool Aktif { get; set; }
        public string AktifString { get; set; } = string.Empty;
        public bool Checked { get; set; }
    }
}