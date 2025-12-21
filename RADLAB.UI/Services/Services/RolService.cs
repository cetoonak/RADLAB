using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RADLAB.Model.DTO;
using RADLAB.Model.ResponseModels;
using RADLAB.UI.Services.Infrastructure;

namespace RADLAB.UI.Services.Services
{
    public class RolService : IRolService
    {
        private readonly HttpClient httpClient;

        public RolService(HttpClient _httpClient)
        {
            httpClient = _httpClient;
        }

        public async Task<RolDTO> GetRol(int Id)
        {
            return await httpClient.GetFromJsonAsync<RolDTO>("Rol/GetRol/" + Id.ToString());
        }

        public async Task<List<RolDTO>> GetRolList()
        {
            return await httpClient.GetFromJsonAsync<List<RolDTO>>("Rol/GetRolList");
        }

        public async Task<List<YetkiDTO>> GetYetkiList()
        {
            return await httpClient.GetFromJsonAsync<List<YetkiDTO>>("Rol/GetYetkiList");
        }

        public async Task<List<YetkiDTO>> GetYetkiListVerilmeyenByRolId(int RolId)
        {
            return await httpClient.GetFromJsonAsync<List<YetkiDTO>>("Rol/GetYetkiListVerilmeyenByRolId/" + RolId.ToString());
        }

        public async Task<List<YetkiDTO>> GetYetkiListVerilenByRolId(int RolId)
        {
            return await httpClient.GetFromJsonAsync<List<YetkiDTO>>("Rol/GetYetkiListVerilenByRolId/" + RolId.ToString());
        }

        public async Task<ServiceResponse<string>> InsertOrUpdateRol(RolDTO dto)
        {
            var result = await httpClient.PostAsJsonAsync("Rol/InsertOrUpdateRol", dto);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<string>> DeleteRol(List<RolDTO> dtos)
        {
            var result = await httpClient.PostAsJsonAsync("Rol/DeleteRol", dtos);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<List<string>>> GetSayfaList()
        {
            return await httpClient.GetFromJsonAsync<ServiceResponse<List<string>>>("Rol/GetSayfaList");
        }

        public async Task<ServiceResponse<bool>> YetkiKontrolBySayfa(string Sayfa)
        {
            return await httpClient.GetFromJsonAsync<ServiceResponse<bool>>("Rol/YetkiKontrolBySayfa/" + Sayfa);
        }

        public async Task<bool> YetkiKontrolByYetki(string Yetki)
        {
            return await httpClient.GetFromJsonAsync<bool>("Rol/YetkiKontrolByYetki/" + Yetki);
        }

        public async Task<KullaniciForLoginDTO> GetKullaniciForLogin()
        {
            return await httpClient.GetFromJsonAsync<KullaniciForLoginDTO>("Rol/GetKullaniciForLogin");
        }

        public async Task<YetkiForRaporDTO> GetYetkiForRaporByRapor(string Rapor)
        {
            return await httpClient.GetFromJsonAsync<YetkiForRaporDTO>("Rol/GetYetkiForRaporByRapor/" + Rapor);
        }

        public async Task<ServiceResponse<string>> GetYetkiBaseByLink(string link)
        {
            return await httpClient.GetFromJsonAsync<ServiceResponse<string>>($"Rol/GetYetkiBaseByLink/{link}");
        }
    }
}