using RADLAB.Model.DTO;
using RADLAB.Model.ResponseModels;

namespace RADLAB.OnlineEgitim.Services.Infrastructure
{
    public interface IKullaniciService
    {
        public Task<KullaniciDTO> GetKullanici(int Id);
        public Task<List<KullaniciDTO>> GetKullaniciList();
        public Task<List<RolDTO>> GetRolListVerilenByKullaniciId(int KullaniciId);
        public Task<ServiceResponse<string>> InsertOrUpdateKullanici(KullaniciDTO dto);
        public Task<ServiceResponse<string>> DeleteKullanici(List<KullaniciDTO> dtos);
        public Task<ServiceResponse<string>> UpdateKisiProfil(KullaniciForLoginDTO dto);
        public Task<ServiceResponse<string>> UpdateKisiCookiePolitikasiniGordu(KullaniciForLoginDTO dto);
    }
}