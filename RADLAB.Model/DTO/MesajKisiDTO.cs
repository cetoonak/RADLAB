using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.DTO
{
    public class MesajKisiDTO
    {
        public int Id { get; set; }
        public string AdSoyad { get; set; } = string.Empty;
        public string Kurulus { get; set; } = string.Empty;
        public string Birim { get; set; } = string.Empty;
    }
}