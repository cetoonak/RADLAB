using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.FilterDTO
{
    public class TestFilterDTO : FilterDTO
    {
        public string Test { get; set; } = string.Empty;
        public int Sure { get; set; }
        public int GecmeOrani { get; set; }
        public int Id { get; set; }
    }
}