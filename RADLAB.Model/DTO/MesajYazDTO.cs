using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.DTO
{
    public class MesajYazDTO
    {
        public MesajDTO Mesaj { get; set; }
        public List<MesajGonderilenKisiDTO> MesajGonderilenKisiler { get; set; }
    }
}