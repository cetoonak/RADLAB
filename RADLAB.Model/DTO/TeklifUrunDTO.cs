using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.DTO
{
    public class TeklifUrunDTO
    {
        public int Id { get; set; }
        public string Urun { get; set; } = string.Empty;
        public int Miktar { get; set; }
        public decimal Fiyat { get; set; }
        public decimal KdvOrani { get; set; }
    }
}