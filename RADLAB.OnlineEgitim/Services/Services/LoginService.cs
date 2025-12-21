using DocumentFormat.OpenXml.Office2010.Excel;
using RADLAB.Model.DTO;
using RADLAB.Model.ResponseModels;
using RADLAB.OnlineEgitim.Services.Infrastructure;

namespace RADLAB.OnlineEgitim.Services.Services
{
    public class LoginService : ILoginService
    {
        private readonly HttpClient httpClient;

        public LoginService(HttpClient _httpClient)
        {
            httpClient = _httpClient;
        }

        public async Task<ServiceResponse<AuthenticatedUserDTO>> Login(LoginDTO loginDTO)
        {
            var result = await httpClient.PostAsJsonAsync("Login/Login", loginDTO);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<AuthenticatedUserDTO>>();
        }

        public async Task<ServiceResponse<AuthenticatedUserDTO>> GetKisiMenu(int Id)
        {
            var result = await httpClient.PostAsJsonAsync("Login/GetKisiMenu", Id);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<AuthenticatedUserDTO>>();
        }

        public async Task<ServiceResponse<string>> GetAktivasyonKoduByMail(string mail)
        {
            var result = await httpClient.PostAsJsonAsync("Login/GetAktivasyonKoduByMail", mail);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();

            //return await httpClient.GetFromJsonAsync<ServiceResponse<string>>("Login/GetAktivasyonKoduByMail/" + mail);
        }

        public async Task<ServiceResponse<string>> UpdatePasswordByAktivasyonKodu(LoginDTO dto)
        {
            var result = await httpClient.PostAsJsonAsync("Login/UpdatePasswordByAktivasyonKodu", dto);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }
    }
}