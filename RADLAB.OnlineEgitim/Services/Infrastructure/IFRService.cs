using RADLAB.Model.DTO;
using RADLAB.Model.ResponseModels;

namespace RADLAB.OnlineEgitim.Services.Infrastructure
{
    public interface IFRService
    {
        public Task<ServiceResponse<byte[]>> GetReport(FRDTO dto);
    }
}