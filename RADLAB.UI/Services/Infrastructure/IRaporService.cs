using RADLAB.Model.DTO;
using RADLAB.Model.ResponseModels;

namespace RADLAB.UI.Services.Infrastructure
{
    public interface IRaporService
    {
        public Task<ServiceResponse<string>> GetRaporBarkod(FRDTO dto);
    }
}