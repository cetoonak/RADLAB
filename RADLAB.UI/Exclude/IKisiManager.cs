using RADLAB.Model.DTO;
using RADLAB.Model.FilterDTO;
using RADLAB.Model.ResponseModels;

namespace RADLAB.Business.Abstract
{
    public interface IKisiManager
    {
        public Task<KisiDTO> GetKisi(int Id);
        public Task<ServiceResponse<string>> UpdateKisiProfil(KullaniciForLoginDTO dto, int KisiId);
        public Task<ServiceResponse<string>> UpdateKisiOnayAcikRiza(List<KisiOnayDTO> dtos, int KisiId);
        public Task<ServiceResponse<string>> UpdateKisiCookiePolitikasiniGordu(KullaniciForLoginDTO dto, int KisiId);
        public Task<ServiceResponse<string>> UpdateKisiAcikRiza(KullaniciForLoginDTO dto, int KisiId);
        public Task<ServiceResponse<string>> UpdateKisiGizlilikOnaylandi(KullaniciForLoginDTO dto, int KisiId);
    }
}