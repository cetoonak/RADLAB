using DocumentFormat.OpenXml.Office2010.Excel;
using RADLAB.Model.DTO;
using RADLAB.Model.FilterDTO;
using RADLAB.Model.ResponseModels;
using RADLAB.UI.Services.Infrastructure;

namespace RADLAB.UI.Services.Services
{
    public class StokHareketService : IStokHareketService
    {
        private readonly HttpClient httpClient;

        public StokHareketService(HttpClient _httpClient)
        {
            httpClient = _httpClient;
        }

        public async Task<ServiceResponse<List<StokHareketDTO>>> GetStokHareketList(StokHareketFilterDTO filterDTO)
        {
            var result = await httpClient.PostAsJsonAsync("StokHareket/GetStokHareketList", filterDTO);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<List<StokHareketDTO>>>();
        }

        public async Task<ServiceResponse<string>> InsertOrUpdateStokHareket(StokHareketDTO dto)
        {
            var result = await httpClient.PostAsJsonAsync("StokHareket/InsertOrUpdateStokHareket", dto);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<string>> DeleteStokHareket(List<StokHareketDTO> dtos)
        {
            var result = await httpClient.PostAsJsonAsync("StokHareket/DeleteStokHareket", dtos);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<string>> InsertOrUpdateStokHareketCihaz(StokHareketCihazDTO dto)
        {
            var result = await httpClient.PostAsJsonAsync("StokHareket/InsertOrUpdateStokHareketCihaz", dto);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<string>> DeleteStokHareketCihaz(List<StokHareketCihazDTO> dtos)
        {
            var result = await httpClient.PostAsJsonAsync("StokHareket/DeleteStokHareketCihaz", dtos);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<List<StokHareketleriDTO>>> GetStokHareketleriList(StokHareketleriFilterDTO filterDTO)
        {
            var result = await httpClient.PostAsJsonAsync("StokHareket/GetStokHareketleriList", filterDTO);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<List<StokHareketleriDTO>>>();
        }

        public async Task<ServiceResponse<List<StokMiktarlariDTO>>> GetStokMiktarlariList(StokMiktarlariFilterDTO filterDTO)
        {
            var result = await httpClient.PostAsJsonAsync("StokHareket/GetStokMiktarlariList", filterDTO);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<List<StokMiktarlariDTO>>>();
        }
    }
}