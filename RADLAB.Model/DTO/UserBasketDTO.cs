using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.DTO
{
    public class UserBasketDTO
    {
        public string Urun { get; set; } = string.Empty;
        public string Fiyat { get; set; } = string.Empty;
        public int Miktar { get; set; }
    }
}