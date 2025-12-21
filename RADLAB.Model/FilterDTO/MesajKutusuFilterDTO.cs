using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.FilterDTO
{
    public class MesajKutusuFilterDTO : FilterDTO
    {
        public int GelenGidenArsiv { get; set; }
        public string SearchText { get; set; } = string.Empty;
    }
}