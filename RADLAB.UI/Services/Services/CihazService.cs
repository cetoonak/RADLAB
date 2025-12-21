using RADLAB.Model.DTO;
using RADLAB.Model.ResponseModels;
using RADLAB.UI.Services.Infrastructure;

namespace RADLAB.UI.Services.Services
{
    public class CihazService : ICihazService
    {
        private readonly HttpClient httpClient;

        public CihazService(HttpClient _httpClient)
        {
            httpClient = _httpClient;
        }

        public async Task<CihazDTO> GetCihaz(int Id)
        {
            return await httpClient.GetFromJsonAsync<CihazDTO>("Cihaz/GetCihaz/" + Id.ToString());
        }

        public async Task<List<CihazDTO>> GetCihazList(string Acilanlar)
        {
            return await httpClient.GetFromJsonAsync<List<CihazDTO>>("Cihaz/GetCihazList/" + Acilanlar);
        }

        public async Task<List<CihazDTO>> GetCihazSatisList()
        {
            return await httpClient.GetFromJsonAsync<List<CihazDTO>>("Cihaz/GetCihazSatisList");
        }

        public async Task<ServiceResponse<string>> InsertOrUpdateCihaz(CihazDTO dto)
        {
            var result = await httpClient.PostAsJsonAsync("Cihaz/InsertOrUpdateCihaz", dto);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<string>> DeleteCihaz(List<CihazDTO> dtos)
        {
            var result = await httpClient.PostAsJsonAsync("Cihaz/DeleteCihaz", dtos);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }
    }
}