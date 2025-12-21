using RADLAB.Model.DTO;
using RADLAB.Model.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Business.Abstract
{
    public interface IVakifBankManager
    {
        public Task<ServiceResponse<VakifBankMPIDTO>> Enrollment(SiparisDTO dto, int KisiId);
    }
}