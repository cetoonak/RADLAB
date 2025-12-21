using RADLAB.Model.DTO;
using RADLAB.Model.FilterDTO;
using RADLAB.Model.ResponseModels;
using RADLAB.UI.Services.Infrastructure;

namespace RADLAB.UI.Services.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly HttpClient httpClient;

        public DashboardService(HttpClient _httpClient)
        {
            httpClient = _httpClient;
        }

        public async Task<ServiceResponse<DashboardSayiDTO>> GetDashboardSayi(DashboardFilterDTO filterDTO)
        {
            var result = await httpClient.PostAsJsonAsync("Dashboard/GetDashboardSayi", filterDTO);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<DashboardSayiDTO>>();
        }

        public async Task<ServiceResponse<List<DashboardGrafikDTO>>> GetDashboardGrafik(DashboardFilterDTO filterDTO)
        {
            var result = await httpClient.PostAsJsonAsync("Dashboard/GetDashboardGrafik", filterDTO);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<List<DashboardGrafikDTO>>>();
        }

        public async Task<ServiceResponse<List<DuyuruDTO>>> GetDashboardDuyuru()
        {
            return await httpClient.GetFromJsonAsync<ServiceResponse<List<DuyuruDTO>>>("Dashboard/GetDashboardDuyuru");
        }
    }
}