using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.DTO
{
    public class OgrenciDTO
    {
        public int Id { get; set; }
        public string Ad { get; set; } = string.Empty;
        public string Soyad { get; set; } = string.Empty;
        public string AdSoyad { get; set; } = string.Empty;
        public string TelefonCep { get; set; } = string.Empty;
        public string EMail { get; set; } = string.Empty;
        public DateTime IlkEgitimAcilisTarihi { get; set; }
        public string Durum { get; set; } = string.Empty;
        public string OgrenciOnlineEgitimler { get; set; } = string.Empty;
        //public List<OnlineEgitimDTO> OnlineEgitimler { get; set; } = new List<OnlineEgitimDTO>();
        public List<OnlineEgitimOgrenciDTO> OnlineEgitimOgrenciler { get; set; } = new List<OnlineEgitimOgrenciDTO>();
        public bool Aktif { get; set; }
        public bool Checked { get; set; }
    }
}