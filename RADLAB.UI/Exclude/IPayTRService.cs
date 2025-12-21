using RADLAB.Model.DTO;
using RADLAB.Model.Exclude;
using RADLAB.Model.FilterDTO;
using RADLAB.Model.ResponseModels;

namespace RADLAB.UI.Exclude
{
    public interface IPayTRService
    {
        public Task<ServiceResponse<string>> PayTRAdim1(PayTRDTO dto);
        public Task<ServiceResponse<string>> PayTRAdim2(PayTRDTO dto);
        public Task<ServiceResponse<string>> UpdateSiparisPayTRToken(SiparisDTO dto);
        public Task<ServiceResponse<string>> UpdateSiparisPayTROdemeTamam(string MerchantOID);
        public Task<string> GetSiparisPayTRToken(int id);
    }
}