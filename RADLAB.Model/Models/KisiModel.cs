using System;
using System.Collections.Generic;
using System.Text;

namespace RADLAB.Model.Models
{
    public class KisiModel
    {
        public int Id { get; set; }
        public int Tip { get; set; }
        public string Ad { get; set; } = string.Empty;
        public string Soyad { get; set; } = string.Empty;
        public string EMail { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string TelefonCep { get; set; } = string.Empty;
        public bool Aktif { get; set; }
    }
}