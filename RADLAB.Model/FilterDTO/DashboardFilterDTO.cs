using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.FilterDTO
{
    public class DashboardFilterDTO
    {
        public string Tip { get; set; } = string.Empty;
        public int Yil { get; set; }
        public int KisiId { get; set; }
    }
}