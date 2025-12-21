using RADLAB.Model.DTO;
using RADLAB.Model.FilterDTO;
using RADLAB.Model.ResponseModels;

namespace RADLAB.UI.Services.Infrastructure
{
    public interface IOdemeService
    {
        public Task<ServiceResponse<List<OdemeDTO>>> GetOdeme(MusteriTakipFilterDTO filterDTO);
        public Task<ServiceResponse<List<OdemeDTO>>> GetOdemeByVerifyEnrollmentRequestId(string VerifyEnrollmentRequestId);
        public Task<ServiceResponse<string>> UpdateOdeme(List<OdemeDTO> dto);
        public Task<ServiceResponse<string>> UpdateOdemeVerifyEnrollmentRequestId(List<OdemeDTO> dto);
    }
}