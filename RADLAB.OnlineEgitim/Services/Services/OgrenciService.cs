using DocumentFormat.OpenXml.Office2010.Excel;
using RADLAB.Model.DTO;
using RADLAB.Model.FilterDTO;
using RADLAB.Model.ResponseModels;
using RADLAB.OnlineEgitim.Services.Infrastructure;

namespace RADLAB.OnlineEgitim.Services.Services
{
    public class OgrenciService : IOgrenciService
    {
        private readonly HttpClient httpClient;

        public OgrenciService(HttpClient _httpClient)
        {
            httpClient = _httpClient;
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

        public async Task<ServiceResponse<string>> DeleteOgrenci(List<OgrenciDTO> dtos)
        {
            var result = await httpClient.PostAsJsonAsync("Ogrenci/DeleteOgrenci", dtos);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }
    }
}