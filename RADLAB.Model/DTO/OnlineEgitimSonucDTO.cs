using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.DTO
{
    public class OnlineEgitimSonucDTO
    {
        public int Id { get; set; }
        public int OnlineEgitimBolumId { get; set; }
        public string Baslik { get; set; } = string.Empty;
        public int SoruSayisi { get; set; }
        public int DogruSayisi { get; set; }
        public int YanlisSayisi { get; set; }
        public int BosSayisi { get; set; }
        public decimal GecmeOrani { get; set; }
        public decimal DogruOrani { get; set; }
    }
}