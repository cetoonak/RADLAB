using RADLAB.Model.DTO;
using RADLAB.Model.FilterDTO;
using RADLAB.Model.ResponseModels;

namespace RADLAB.UI.Exclude
{
    public interface IKisiService
    {
        public Task<KisiDTO> GetKisi(int Id);
        public Task<ServiceResponse<List<KisiDTO>>> GetKisiList(KisiFilterDTO filterDTO);
        public Task<List<LookupBasicDTO>> GetBirimListByKisiId(int KisiId);
        public Task<ServiceResponse<string>> InsertOrUpdateKisi(KisiDTO dto);
        public Task<ServiceResponse<string>> DeleteKisi(List<KisiDTO> dtos);
        public Task<ServiceResponse<string>> UpdateKisiProfil(KullaniciForLoginDTO dto);
        public Task<ServiceResponse<List<KisiOnayDTO>>> GetKisiOnayList(KisiOnayFilterDTO filterDTO);
        public Task<ServiceResponse<string>> UpdateKisiOnayAcikRiza(List<KisiOnayDTO> dtos);
        public Task<ServiceResponse<string>> UpdateKisiCookiePolitikasiniGordu(KullaniciForLoginDTO dto);
        public Task<ServiceResponse<string>> UpdateKisiAcikRiza(KullaniciForLoginDTO dto);
        public Task<ServiceResponse<string>> UpdateKisiGizlilikOnaylandi(KullaniciForLoginDTO dto);
    }
}