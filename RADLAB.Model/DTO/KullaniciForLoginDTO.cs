using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.DTO
{
    public class KullaniciForLoginDTO
    {
        public int Id { get; set; }
        public int Tip { get; set; }
        public string AdSoyad { get; set; } = string.Empty;
        public string TelefonCep { get; set; } = string.Empty;
        public string EMail { get; set; } = string.Empty;
        public bool GizlilikOnaylandi { get; set; }
        public bool AcikRiza { get; set; }
        public bool CookiePolitikasiniGordu { get; set; }
    }
}