using RADLAB.Model.DTO;
using RADLAB.Model.FilterDTO;
using RADLAB.Model.ResponseModels;

namespace RADLAB.UI.Services.Infrastructure
{
    public interface IVakifBankService
    {
        public Task<ServiceResponse<VakifBankMPIDTO>> Enrollment(SiparisDTO dto);
        public Task<ServiceResponse<string>> CreateHtmlFile(string apiBasePath, string icerik);
    }
}