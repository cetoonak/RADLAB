using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.FilterDTO
{
    public class OnlineEgitimFilterDTO : FilterDTO
    {
        public string OnlineEgitim { get; set; } = string.Empty;
        public int Zorluk { get; set; }
        public int Id { get; set; }
    }
}