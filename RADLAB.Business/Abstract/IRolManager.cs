using RADLAB.Model.DTO;
using RADLAB.Model.ResponseModels;

namespace RADLAB.Business.Abstract
{
    public interface IRolManager
    {
        public Task<RolDTO> GetRol(int Id);
        public Task<List<RolDTO>> GetRolList();
        public Task<List<YetkiDTO>> GetYetkiList();
        public Task<List<YetkiDTO>> GetYetkiListVerilmeyenByRolId(int RolId);
        public Task<List<YetkiDTO>> GetYetkiListVerilenByRolId(int RolId);
        public Task<ServiceResponse<string>> InsertOrUpdateRol(RolDTO dto, int KisiId);
        public Task<ServiceResponse<string>> DeleteRol(List<RolDTO> dtos, int KisiId);
        public Task<ServiceResponse<List<string>>> GetSayfaList();
        public Task<ServiceResponse<bool>> YetkiKontrolBySayfa(int KisiId, string Sayfa);
        public Task<bool> YetkiKontrolByYetki(int KisiId, string Yetki);
        public Task<KullaniciForLoginDTO> GetKullaniciForLogin(int Id);
        public Task<YetkiForRaporDTO> GetYetkiForRaporByRapor(string Rapor);
        public Task<ServiceResponse<string>> GetYetkiBaseByLink(string link);
    }
}