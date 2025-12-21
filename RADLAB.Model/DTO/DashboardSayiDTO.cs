using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.DTO
{
    public class DashboardSayiDTO
    {
        public int BasvuruSayisi { get; set; }
        public int TeslimEdilen { get; set; }
        public int IslemiDevamEden { get; set; }
        public int OlumluSonuclanan { get; set; }
        public int OlumsuzSonuclanan { get; set; }
    }
}