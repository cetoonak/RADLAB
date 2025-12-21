using RADLAB.Model.DTO;
using RADLAB.Model.ResponseModels;

namespace RADLAB.UI.Services.Infrastructure
{
    public interface IFRService
    {
        public Task<ServiceResponse<byte[]>> GetReport(FRDTO dto);
    }
}