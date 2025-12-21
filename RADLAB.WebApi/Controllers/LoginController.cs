using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RADLAB.Business.Abstract;
using RADLAB.Business.Concrete;
using RADLAB.Model.DTO;
using RADLAB.Model.ResponseModels;

namespace RADLAB.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LoginController : ControllerBase
    {
        private readonly ILoginManager loginManager;

        public LoginController(ILoginManager _loginManager)
        {
            loginManager = _loginManager;
        }

        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<ServiceResponse<AuthenticatedUserDTO>> Login([FromBody] LoginDTO loginDTO)
        {
            return await loginManager.Login(loginDTO);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<AuthenticatedUserDTO>> GetKisiMenu([FromBody] int Id)
        {
            return await loginManager.GetKisiMenu(Id);
        }

        //[HttpGet("[action]/{mail}")]
        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<ServiceResponse<string>> GetAktivasyonKoduByMail([FromBody] string mail)
        {
            return await loginManager.GetAktivasyonKoduByMail(mail);
        }

        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<ServiceResponse<string>> UpdatePasswordByAktivasyonKodu([FromBody] LoginDTO dto)
        {
            return await loginManager.UpdatePasswordByAktivasyonKodu(dto);
        }
    }
}
