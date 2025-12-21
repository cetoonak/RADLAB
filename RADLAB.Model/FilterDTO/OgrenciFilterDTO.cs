using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.FilterDTO
{
    public class OgrenciFilterDTO : FilterDTO
    {
        public string AdSoyad { get; set; } = string.Empty;
        public string TelefonCep { get; set; } = string.Empty;
        public string EMail { get; set; } = string.Empty;
        public string IlkEgitimAcilisTarihi { get; set; } = string.Empty;
        public int OnlineEgitimId { get; set; }
        public int Durum { get; set; }
        public int Id { get; set; }
    }
}