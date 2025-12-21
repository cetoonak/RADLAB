using RADLAB.Model.DTO;
using RADLAB.Model.FilterDTO;
using RADLAB.Model.ResponseModels;

namespace RADLAB.OnlineEgitim.Services.Infrastructure
{
    public interface ISoruService
    {
        public Task<ServiceResponse<List<SoruDTO>>> GetSoruList(SoruFilterDTO filterDTO);
        public Task<ServiceResponse<string>> InsertOrUpdateSoru(SoruDTO dto);
        public Task<ServiceResponse<string>> DeleteSoru(List<SoruDTO> dtos);
    }
}