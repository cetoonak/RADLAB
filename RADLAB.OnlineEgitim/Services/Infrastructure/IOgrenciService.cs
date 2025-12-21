using RADLAB.Model.DTO;
using RADLAB.Model.FilterDTO;
using RADLAB.Model.ResponseModels;

namespace RADLAB.OnlineEgitim.Services.Infrastructure
{
    public interface IOgrenciService
    {
        public Task<ServiceResponse<List<OgrenciDTO>>> GetOgrenciList(OgrenciFilterDTO filterDTO);
        public Task<ServiceResponse<string>> InsertOrUpdateOgrenci(OgrenciDTO dto);
        public Task<ServiceResponse<string>> DeleteOgrenci(List<OgrenciDTO> dtos);
    }
}