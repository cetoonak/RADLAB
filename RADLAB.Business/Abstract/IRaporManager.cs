using RADLAB.Model.DTO;
using RADLAB.Model.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Business.Abstract
{
    public interface IRaporManager
    {
        public Task<ServiceResponse<string>> GetRaporBarkod(FRDTO dto, int KisiId);
    }
}