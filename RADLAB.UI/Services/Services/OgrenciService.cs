using DocumentFormat.OpenXml.Office2010.Excel;
using RADLAB.Model.DTO;
using RADLAB.Model.FilterDTO;
using RADLAB.Model.ResponseModels;
using RADLAB.UI.Services.Infrastructure;

namespace RADLAB.UI.Services.Services
{
    public class OgrenciService : IOgrenciService
    {
        private readonly HttpClient httpClient;

        public OgrenciService(HttpClient _httpClient)
        {
            httpClient = _httpClient;
        }

        public async Task<OgrenciDTO> GetOgrenci(int Id)
        {
            return await httpClient.GetFromJsonAsync<OgrenciDTO>("Ogrenci/GetOgrenci/" + Id.ToString());
        }

        public async Task<ServiceResponse<List<OgrenciDTO>>> GetOgrenciList(OgrenciFilterDTO filterDTO)
        {
            var result = await httpClient.PostAsJsonAsync("Ogrenci/GetOgrenciList", filterDTO);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<List<OgrenciDTO>>>();
        }

        public async Task<ServiceResponse<string>> InsertOrUpdateOgrenci(OgrenciDTO dto)
        {
            var result = await httpClient.PostAsJsonAsync("Ogrenci/InsertOrUpdateOgrenci", dto);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<string>> UpdateOgrenciAktif(List<OgrenciDTO> dtos)
        {
            var result = await httpClient.PostAsJsonAsync("Ogrenci/UpdateOgrenciAktif", dtos);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<string>> DeleteOgrenci(List<OgrenciDTO> dtos)
        {
            var result = await httpClient.PostAsJsonAsync("Ogrenci/DeleteOgrenci", dtos);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }
    }
}