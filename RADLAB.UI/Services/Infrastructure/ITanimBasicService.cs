using RADLAB.Model.DTO;
using RADLAB.Model.ResponseModels;

namespace RADLAB.UI.Services.Infrastructure
{
    public interface ITanimBasicService
    {
        public Task<TanimBasicDTO> GetTanimBasic(string Tanim, int Id);
        public Task<List<TanimBasicDTO>> GetTanimBasicList(string Tanim);
        public Task<ServiceResponse<string>> InsertOrUpdateTanimBasic(string Tanim, TanimBasicDTO dto);
        public Task<ServiceResponse<string>> DeleteTanimBasic(string Tanim, List<TanimBasicDTO> dtos);
    }
}
