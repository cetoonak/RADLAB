using Microsoft.AspNetCore.Mvc;
using RADLAB.Model.DTO;
using RADLAB.Model.ResponseModels;

namespace RADLAB.OnlineEgitim.Services.Infrastructure
{
    public interface IRolService
    {
        public Task<RolDTO> GetRol(int Id);
        public Task<List<RolDTO>> GetRolList();
        public Task<List<YetkiDTO>> GetYetkiList();
        public Task<List<YetkiDTO>> GetYetkiListVerilmeyenByRolId(int RolId);
        public Task<List<YetkiDTO>> GetYetkiListVerilenByRolId(int RolId);
        public Task<ServiceResponse<string>> InsertOrUpdateRol(RolDTO dto);
        public Task<ServiceResponse<string>> DeleteRol(List<RolDTO> dtos);
        public Task<ServiceResponse<List<string>>> GetSayfaList();
        public Task<ServiceResponse<bool>> YetkiKontrolBySayfa(string Sayfa);
        public Task<bool> YetkiKontrolByYetki(string Yetki);
        public Task<KullaniciForLoginDTO> GetKullaniciForLogin();
        public Task<YetkiForRaporDTO> GetYetkiForRaporByRapor(string Rapor);
        public Task<ServiceResponse<string>> GetYetkiBaseByLink(string link);
    }
}