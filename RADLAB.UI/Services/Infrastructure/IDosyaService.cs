using RADLAB.Model.DTO;
using RADLAB.Model.FilterDTO;
using RADLAB.Model.ResponseModels;

namespace RADLAB.UI.Services.Infrastructure
{
    public interface IDosyaService
    {
        public Task<ServiceResponse<DosyaDTO>> GetDosya(int Id);
        public Task<ServiceResponse<List<DosyaDTO>>> GetDosyaList();
        public Task<ServiceResponse<string>> InsertOrUpdateDosya(DosyaDTO dto);
        public Task<ServiceResponse<string>> DeleteDosya(List<DosyaDTO> dtos);
    }
}