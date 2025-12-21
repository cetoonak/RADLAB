using RADLAB.Model.DTO;
using RADLAB.Model.ResponseModels;
using RADLAB.UI.Services.Infrastructure;

namespace RADLAB.UI.Services.Services
{
    public class SMSMailService : ISMSMailService
    {
        private readonly HttpClient httpClient;

        public SMSMailService(HttpClient _httpClient)
        {
            httpClient = _httpClient;
        }

        public async Task<ServiceResponse<string>> SendMail(MailDTO dto)
        {
            var result = await httpClient.PostAsJsonAsync("SMSMail/SendMail", dto);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<string>> SendSMS(string GSM, string Mesaj)
        {
            return await httpClient.GetFromJsonAsync<ServiceResponse<string>>($"SMSMail/SendSMS/{GSM}/{Mesaj}");
        }

        public async Task<ServiceResponse<string>> SendSMSAndMail(MailDTO dto)
        {
            var result = await httpClient.PostAsJsonAsync("SMSMail/SendSMSAndMail", dto);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }
    }
}