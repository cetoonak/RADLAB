using RADLAB.Model.DTO;
using RADLAB.Model.ResponseModels;

namespace RADLAB.UI.Services.Infrastructure
{
    public interface ITanimSayiliService
    {
        public Task<TanimSayiliDTO> GetTanimSayili(string Tanim, int Id);
        public Task<List<TanimSayiliDTO>> GetTanimSayiliList(string Tanim);
        public Task<ServiceResponse<string>> InsertOrUpdateTanimSayili(string Tanim, TanimSayiliDTO dto);
        public Task<ServiceResponse<string>> DeleteTanimSayili(string Tanim, List<TanimSayiliDTO> dtos);
    }
}