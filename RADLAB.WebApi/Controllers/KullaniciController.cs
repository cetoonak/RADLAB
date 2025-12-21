using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RADLAB.Business.Abstract;
using RADLAB.Model.DTO;
using RADLAB.Model.ResponseModels;

namespace RADLAB.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class KullaniciController : ControllerBase
    {
        private readonly IKullaniciManager kullaniciManager;

        public KullaniciController(IKullaniciManager _kullaniciManager)
        {
            kullaniciManager = _kullaniciManager;
        }

        [HttpGet("[action]/{id:int}")]
        public async Task<KullaniciDTO> GetKullanici(int Id)
        {
            return await kullaniciManager.GetKullanici(Id);
        }

        [HttpGet("[action]")]
        public async Task<List<KullaniciDTO>> GetKullaniciList()
        {
            return await kullaniciManager.GetKullaniciList();
        }

        [HttpGet("[action]/{KullaniciId:int}")]
        public async Task<List<RolDTO>> GetRolListByKullaniciId(int KullaniciId)
        {
            return await kullaniciManager.GetRolListByKullaniciId(KullaniciId);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> InsertOrUpdateKullanici([FromBody] KullaniciDTO dto)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await kullaniciManager.InsertOrUpdateKullanici(dto, KisiId);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> DeleteKullanici([FromBody] List<KullaniciDTO> dtos)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await kullaniciManager.DeleteKullanici(dtos, KisiId);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> UpdateKisiProfil([FromBody] KullaniciForLoginDTO dto)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await kullaniciManager.UpdateKisiProfil(dto, KisiId);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> UpdateKisiCookiePolitikasiniGordu([FromBody] KullaniciForLoginDTO dto)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await kullaniciManager.UpdateKisiCookiePolitikasiniGordu(dto, KisiId);
        }
    }
}