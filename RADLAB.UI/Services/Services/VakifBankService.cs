using RADLAB.Model.DTO;
using RADLAB.Model.FilterDTO;
using RADLAB.Model.ResponseModels;
using RADLAB.UI.Services.Infrastructure;
using System.Net.Http.Json;

namespace RADLAB.UI.Services.Services
{
    public class VakifBankService : IVakifBankService
    {
        private readonly HttpClient httpClient;

        public VakifBankService(HttpClient _httpClient)
        {
            httpClient = _httpClient;
        }

        public async Task<ServiceResponse<VakifBankMPIDTO>> Enrollment(SiparisDTO dto)
        {
            var result = await httpClient.PostAsJsonAsync("VakifBank/Enrollment", dto);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<VakifBankMPIDTO>>();
        }

        public async Task<ServiceResponse<string>> CreateHtmlFile(string apiBasePath, string icerik)
        {
            var result = await httpClient.PostAsJsonAsync(apiBasePath + "/Upload/CreateHtmlFile", icerik);

            //var result = await httpClient.PostAsJsonAsync("Upload/CreateHtmlFile", icerik);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }
    }
}