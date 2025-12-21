using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.DTO
{
    public class OnlineEgitimBolumTamamlananDTO
    {
        public int Id { get; set; }
        //public int KisiId { get; set; }
        public int OnlineEgitimId { get; set; }
        public int OnlineEgitimOgrenciId { get; set; }
        public int OnlineEgitimBolumId { get; set; }
        public int GecenSure { get; set; }
        public int SoruId { get; set; }
        public int Cevap { get; set; }
        public bool BuradaKaldi { get; set; }
        public int VideoTime { get; set; }
    }
}