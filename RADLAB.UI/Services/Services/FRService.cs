using RADLAB.Model.DTO;
using RADLAB.Model.ResponseModels;
using RADLAB.UI.Services.Infrastructure;
using System.Net.Http;

namespace RADLAB.UI.Services.Services
{
    public class FRService : IFRService
    {
        private readonly IConfiguration configuration;

        public FRService(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        public async Task<ServiceResponse<byte[]>> GetReport(FRDTO dto)
        {
            bool debug = false;
#if DEBUG
            debug = true;
#endif

            string WebApiUrlFR = configuration[debug ? "WebApiUrlFRDebug" : "WebApiUrlFRRelease"];

            HttpClient httpClient = new HttpClient();

            dto.ConnectionString = configuration.GetConnectionString(debug ? "Debug" : "Release");

            var response = await httpClient.PostAsJsonAsync(WebApiUrlFR + "/GetReport", dto);

            var result = await response.Content.ReadFromJsonAsync<ServiceResponse<byte[]>>();

            return result;
        }
    }
}