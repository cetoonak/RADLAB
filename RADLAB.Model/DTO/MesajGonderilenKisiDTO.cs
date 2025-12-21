using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.DTO
{
    public class MesajGonderilenKisiDTO
    {
        public int Id { get; set; }
        public int MesajId { get; set; }
        public int KisiId { get; set; }
        public string Kurulus { get; set; } = string.Empty;
        public string Birim { get; set; } = string.Empty;
        public string AdSoyad { get; set; } = string.Empty;
        public bool Okundu { get; set; }
        public bool Silindi { get; set; }
    }
}