using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.DTO
{
    public class YetkiForRaporDTO
    {
        public string RaporAdi { get; set; } = string.Empty;
        public List<YetkiRaporAlani> YetkiRaporAlanlari { get; set; }
    }

    public class YetkiRaporAlani
    {
        public string RaporAlani { get; set; } = string.Empty;
        public bool Zorunlu { get; set; }
    }
}