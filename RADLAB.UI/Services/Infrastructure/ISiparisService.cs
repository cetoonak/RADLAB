using RADLAB.Model.DTO;
using RADLAB.Model.FilterDTO;
using RADLAB.Model.ResponseModels;

namespace RADLAB.UI.Services.Infrastructure
{
    public interface ISiparisService
    {
        public Task<ServiceResponse<List<SiparisDTO>>> GetSiparisList(SiparisFilterDTO filterDTO);
        public Task<ServiceResponse<List<SiparisDTO>>> GetSiparisListByVerifyEnrollmentRequestId(string VerifyEnrollmentRequestId);
        public Task<ServiceResponse<List<SiparisCihazTakipDTO>>> GetMusteriTakip(MusteriTakipFilterDTO filterDTO);
        public Task<ServiceResponse<string>> InsertSiparis(SiparisDTO dto);
        public Task<ServiceResponse<string>> InsertOrUpdateSiparis(SiparisDTO dto);
        public Task<ServiceResponse<string>> DeleteSiparis(List<SiparisDTO> dtos);
        public Task<ServiceResponse<string>> InsertOrUpdateSiparisCihaz(SiparisCihazDTO dto);
        public Task<ServiceResponse<string>> DeleteSiparisCihaz(List<SiparisCihazDTO> dtos);
        public Task<ServiceResponse<List<SiparisCihazHareketDTO>>> GetSiparisCihazHareket(int SiparisCihazId);
        public Task<ServiceResponse<string>> InsertSiparisCihazHareket(SiparisCihazHareketDTO dto);
    }
}