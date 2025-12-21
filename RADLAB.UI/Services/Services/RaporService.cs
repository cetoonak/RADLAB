using RADLAB.Model.DTO;
using RADLAB.Model.ResponseModels;
using RADLAB.UI.Services.Infrastructure;

namespace RADLAB.UI.Services.Services
{
    public class RaporService : IRaporService
    {
        private readonly HttpClient httpClient;

        public RaporService(HttpClient _httpClient)
        {
            httpClient = _httpClient;
        }

        public async Task<ServiceResponse<string>> GetRaporBarkod(FRDTO dto)
        {
            var result = await httpClient.PostAsJsonAsync("Rapor/GetRaporBarkod", dto);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }
    }
}