using RADLAB.Model.DTO;
using RADLAB.Model.FilterDTO;
using RADLAB.Model.ResponseModels;

namespace RADLAB.UI.Services.Infrastructure
{
    public interface IDuyuruService
    {
        public Task<DuyuruDTO> GetDuyuru(int Id);
        public Task<ServiceResponse<List<DuyuruDTO>>> GetDuyuruList(DuyuruFilterDTO filterDTO);
        public Task<ServiceResponse<string>> InsertOrUpdateDuyuru(DuyuruDTO dto);
        public Task<ServiceResponse<string>> DeleteDuyuru(List<DuyuruDTO> dtos);
    }
}