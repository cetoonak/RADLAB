using RADLAB.Model.DTO;
using RADLAB.Model.FilterDTO;
using RADLAB.Model.ResponseModels;

namespace RADLAB.UI.Services.Infrastructure
{
    public interface IEgitimService
    {
        public Task<EgitimDTO> GetEgitim(int Id);
        public Task<List<EgitimDTO>> GetEgitimList();
        public Task<ServiceResponse<string>> InsertOrUpdateEgitim(EgitimDTO dto);
        public Task<ServiceResponse<string>> DeleteEgitim(List<EgitimDTO> dtos);
    }
}