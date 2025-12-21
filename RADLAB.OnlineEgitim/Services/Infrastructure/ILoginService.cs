using Microsoft.AspNetCore.Mvc;
using RADLAB.Model.DTO;
using RADLAB.Model.ResponseModels;

namespace RADLAB.OnlineEgitim.Services.Infrastructure
{
    public interface ILoginService
    {
        public Task<ServiceResponse<AuthenticatedUserDTO>> Login(LoginDTO loginDTO);
        public Task<ServiceResponse<AuthenticatedUserDTO>> GetKisiMenu(int Id);
        public Task<ServiceResponse<string>> GetAktivasyonKoduByMail(string mail);
        public Task<ServiceResponse<string>> UpdatePasswordByAktivasyonKodu(LoginDTO dto);
    }
}
