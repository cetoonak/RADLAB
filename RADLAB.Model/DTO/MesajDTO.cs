using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.DTO
{
    public class MesajDTO
    {
        public int Id { get; set; }
        public int MesajGonderilenKisiId { get; set; }
        public int GonderenKisiId { get; set; }
        public string Kim { get; set; } = string.Empty;
        public DateTime? Zaman { get; set; }
        public string Konu { get; set; } = string.Empty;
        public string Icerik { get; set; } = string.Empty;
        public bool Bold { get; set; }
        public bool Silindi { get; set; }
        public bool Checked { get; set; }
    }
}