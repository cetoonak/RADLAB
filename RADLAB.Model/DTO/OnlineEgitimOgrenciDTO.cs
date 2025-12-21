using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.DTO
{
    public class OnlineEgitimOgrenciDTO
    {
        public int Id { get; set; }
        public int OnlineEgitimId { get; set; }
        public int KisiId { get; set; }
        public DateTime? Tarih1 { get; set; }
        public DateTime? Tarih2 { get; set; }
        public int Grup { get; set; }
    }
}