using RADLAB.Model.DTO;
using RADLAB.Model.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Business.Abstract
{
    public interface IKullaniciManager
    {
        public Task<KullaniciDTO> GetKullanici(int Id);
        public Task<List<KullaniciDTO>> GetKullaniciList();
        public Task<List<RolDTO>> GetRolListByKullaniciId(int KullaniciId);
        public Task<ServiceResponse<string>> InsertOrUpdateKullanici(KullaniciDTO dto, int KullaniciId);
        public Task<ServiceResponse<string>> DeleteKullanici(List<KullaniciDTO> dtos, int KullaniciId);
        public Task<ServiceResponse<string>> UpdateKisiProfil(KullaniciForLoginDTO dto, int KisiId);
        public Task<ServiceResponse<string>> UpdateKisiCookiePolitikasiniGordu(KullaniciForLoginDTO dto, int KisiId);
    }
}