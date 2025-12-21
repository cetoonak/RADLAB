using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.DTO
{
    public class OnlineEgitimKisiDTO
    {
        public int Id { get; set; }
        public int OnlineEgitimId { get; set; }
        public string AdSoyad { get; set; } = string.Empty;
        public string TelefonCep { get; set; } = string.Empty;
        public string EMail { get; set; } = string.Empty;
    }
}