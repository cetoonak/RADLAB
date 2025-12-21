using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.DTO
{
    public class OnlineEgitimBolumDTO
    {
        public int Id { get; set; }
        public int OnlineEgitimId { get; set; }
        public int Sira { get; set; }
        public string Baslik { get; set; } = string.Empty;
        public int TestId { get; set; }
        public int VideoId { get; set; }
        public string TestVideo { get; set; } = string.Empty;
        public bool Checked { get; set; }
    }
}