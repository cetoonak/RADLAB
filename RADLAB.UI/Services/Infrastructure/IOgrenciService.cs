using RADLAB.Model.DTO;
using RADLAB.Model.FilterDTO;
using RADLAB.Model.ResponseModels;

namespace RADLAB.UI.Services.Infrastructure
{
    public interface IOgrenciService
    {
        public Task<OgrenciDTO> GetOgrenci(int Id);
        public Task<ServiceResponse<List<OgrenciDTO>>> GetOgrenciList(OgrenciFilterDTO filterDTO);
        public Task<ServiceResponse<string>> InsertOrUpdateOgrenci(OgrenciDTO dto);
        public Task<ServiceResponse<string>> UpdateOgrenciAktif(List<OgrenciDTO> dtos);
        public Task<ServiceResponse<string>> DeleteOgrenci(List<OgrenciDTO> dtos);
    }
}