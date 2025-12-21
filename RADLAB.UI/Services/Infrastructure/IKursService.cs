using RADLAB.Model.DTO;
using RADLAB.Model.FilterDTO;
using RADLAB.Model.ResponseModels;

namespace RADLAB.UI.Services.Infrastructure
{
    public interface IKursService
    {
        public Task<ServiceResponse<List<KursDTO>>> GetKursList(KursFilterDTO filterDTO);
        public Task<ServiceResponse<string>> InsertOrUpdateKurs(KursDTO dto);
        public Task<ServiceResponse<string>> DeleteKurs(List<KursDTO> dtos);
        public Task<ServiceResponse<List<KursYayinDTO>>> GetKursYayinList(int Id);
        public Task<ServiceResponse<List<KursiyerTakipDTO>>> GetMusteriTakip(MusteriTakipFilterDTO filterDTO);
        public Task<ServiceResponse<KursiyerDTO>> GetKursiyer(int Id);
        public Task<ServiceResponse<string>> InsertOrUpdateKursiyer(KursiyerDTO dto);
        public Task<ServiceResponse<string>> DeleteKursiyer(List<KursiyerDTO> dtos);
    }
}