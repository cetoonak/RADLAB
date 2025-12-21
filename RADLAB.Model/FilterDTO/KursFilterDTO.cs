using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.FilterDTO
{
    public class KursFilterDTO : FilterDTO
    {
        public int EgitimId { get; set; }
        public int KursOrtamiId { get; set; }
        public int Yil { get; set; }
        public int Durum { get; set; }
        public int Id { get; set; }
    }
}