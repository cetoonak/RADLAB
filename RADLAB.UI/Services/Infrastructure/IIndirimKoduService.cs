using RADLAB.Model.DTO;
using RADLAB.Model.FilterDTO;
using RADLAB.Model.ResponseModels;

namespace RADLAB.UI.Services.Infrastructure
{
    public interface IIndirimKoduService
    {
        public Task<ServiceResponse<List<IndirimKoduDTO>>> GetIndirimKoduList(IndirimKoduFilterDTO filterDTO);
        public Task<ServiceResponse<string>> InsertOrUpdateIndirimKodu(IndirimKoduDTO dto);
        public Task<ServiceResponse<string>> DeleteIndirimKodu(List<IndirimKoduDTO> dtos);
    }
}