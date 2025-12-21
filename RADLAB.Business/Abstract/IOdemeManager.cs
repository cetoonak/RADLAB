using RADLAB.Model.DTO;
using RADLAB.Model.FilterDTO;
using RADLAB.Model.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Business.Abstract
{
    public interface IOdemeManager
    {
        public Task<ServiceResponse<List<OdemeDTO>>> GetOdeme(MusteriTakipFilterDTO filterDTO, int KisiId);
        public Task<ServiceResponse<List<OdemeDTO>>> GetOdemeByVerifyEnrollmentRequestId(string VerifyEnrollmentRequestId, int KisiId);
        public Task<ServiceResponse<string>> UpdateOdeme(List<OdemeDTO> dto, int KisiId);
        public Task<ServiceResponse<string>> UpdateOdemeVerifyEnrollmentRequestId(List<OdemeDTO> dto, int KisiId);
    }
}