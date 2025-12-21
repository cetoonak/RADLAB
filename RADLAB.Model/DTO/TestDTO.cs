using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.DTO
{
    public class TestDTO
    {
        public int Id { get; set; }
        public string Test { get; set; } = string.Empty;
        public string Aciklama { get; set; } = string.Empty;
        public int Sure { get; set; }
        public int GecmeOrani { get; set; }
        public List<SoruDTO> SoruList { get; set; }
    }
}