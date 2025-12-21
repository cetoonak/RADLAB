using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.DTO
{
    public class OnlineEgitimDTO
    {
        public int Id { get; set; }
        public string OnlineEgitim { get; set; } = string.Empty;
        public int Zorluk { get; set; }
        public string GerekliBelgeler { get; set; } = string.Empty;
        public string SertifikaBilgi { get; set; } = string.Empty;
        public string Aciklama { get; set; } = string.Empty;
        public string OnlineEgitimBolumler { get; set; } = string.Empty;
        public List<OnlineEgitimBolumDTO> OnlineEgitimBolumList { get; set; }
        public List<OgrenciDTO> OgrenciList { get; set; }
    }
}