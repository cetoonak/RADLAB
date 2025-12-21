using RADLAB.Model.DTO;
using RADLAB.Model.ResponseModels;

namespace RADLAB.UI.Services.Infrastructure
{
    public interface ITanimKodluService
    {
        public Task<TanimKodluDTO> GetTanimKodlu(string Tanim, int Id);
        public Task<List<TanimKodluDTO>> GetTanimKodluList(string Tanim);
        public Task<ServiceResponse<string>> InsertOrUpdateTanimKodlu(string Tanim, TanimKodluDTO dto);
        public Task<ServiceResponse<string>> DeleteTanimKodlu(string Tanim, List<TanimKodluDTO> dtos);
    }
}