using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.FilterDTO
{
    public class DuyuruFilterDTO : FilterDTO
    {
        public string Konu { get; set; } = string.Empty;
        public int SistemKullanicilarinaGoster { get; set; }
        public int WebSitesindeGoster { get; set; }
        public int Aktif { get; set; }
    }
}