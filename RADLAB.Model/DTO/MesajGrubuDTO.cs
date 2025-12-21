using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.DTO
{
    public class MesajGrubuDTO
    {
        public MesajGrubuMasterDTO MesajGrubuMaster { get; set; }
        public List<MesajGrubuDetailDTO> MesajGrubuDetailler { get; set; }
    }
}