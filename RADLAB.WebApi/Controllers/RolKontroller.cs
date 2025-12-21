using RADLAB.Business.Abstract;
using RADLAB.Model.DTO;
using RADLAB.Model.ResponseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RADLAB.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RolController : ControllerBase
    {
        private readonly IRolManager rolManager;

        public RolController(IRolManager _rolManager)
        {
            rolManager = _rolManager;
        }

        [HttpGet("[action]/{id:int}")]
        public async Task<RolDTO> GetRol(int Id)
        {
            return await rolManager.GetRol(Id);
        }

        [HttpGet("[action]")]
        public async Task<List<RolDTO>> GetRolList()
        {
            return await rolManager.GetRolList();
        }

        [HttpGet("[action]")]
        public async Task<List<YetkiDTO>> GetYetkiList()
        {
            return await rolManager.GetYetkiList();
        }

        [HttpGet("[action]/{RolId:int}")]
        public async Task<List<YetkiDTO>> GetYetkiListVerilmeyenByRolId(int RolId)
        {
            return await rolManager.GetYetkiListVerilmeyenByRolId(RolId);
        }

        [HttpGet("[action]/{RolId:int}")]
        public async Task<List<YetkiDTO>> GetYetkiListVerilenByRolId(int RolId)
        {
            return await rolManager.GetYetkiListVerilenByRolId(RolId);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> InsertOrUpdateRol([FromBody] RolDTO dto)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await rolManager.InsertOrUpdateRol(dto, KisiId);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> DeleteRol([FromBody] List<RolDTO> dtos)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await rolManager.DeleteRol(dtos, KisiId);
        }

        [HttpGet("[action]")]
        [AllowAnonymous]
        public async Task<ServiceResponse<List<string>>> GetSayfaList()
        {
            return await rolManager.GetSayfaList();
        }

        [HttpGet("[action]/{Sayfa}")]
        [AllowAnonymous]
        public async Task<ServiceResponse<bool>> YetkiKontrolBySayfa(string Sayfa)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await rolManager.YetkiKontrolBySayfa(KisiId, Sayfa);
        }

        [HttpGet("[action]/{Yetki}")]
        public async Task<bool> YetkiKontrolByYetki(string Yetki)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await rolManager.YetkiKontrolByYetki(KisiId, Yetki);
        }

        [HttpGet("[action]")]
        public async Task<KullaniciForLoginDTO> GetKullaniciForLogin()
        {
            var Id = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await rolManager.GetKullaniciForLogin(Id);
        }

        [HttpGet("[action]/{rapor}")]
        public async Task<YetkiForRaporDTO> GetYetkiForRaporByRapor(string Rapor)
        {
            return await rolManager.GetYetkiForRaporByRapor(Rapor);
        }

        [HttpGet("[action]/{link}")]
        public async Task<ServiceResponse<string>> GetYetkiBaseByLink(string link)
        {
            return await rolManager.GetYetkiBaseByLink(link);
        }
    }
}