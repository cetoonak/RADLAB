using RADLAB.Business.Abstract;
using RADLAB.Model.DTO;
using RADLAB.Model.ResponseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RADLAB.Model.FilterDTO;

namespace RADLAB.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class KisiController : ControllerBase
    {
        private readonly IKisiManager KisiManager;

        public KisiController(IKisiManager _KisiManager)
        {
            KisiManager = _KisiManager;
        }

        [HttpGet("[action]/{id:int}")]
        public async Task<KisiDTO> GetKisi(int Id)
        {
            return await KisiManager.GetKisi(Id);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> UpdateKisiProfil([FromBody] KullaniciForLoginDTO dto)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await KisiManager.UpdateKisiProfil(dto, KisiId);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> UpdateKisiOnayAcikRiza([FromBody] List<KisiOnayDTO> dtos)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await KisiManager.UpdateKisiOnayAcikRiza(dtos, KisiId);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> UpdateKisiCookiePolitikasiniGordu([FromBody] KullaniciForLoginDTO dto)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await KisiManager.UpdateKisiCookiePolitikasiniGordu(dto, KisiId);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> UpdateKisiAcikRiza([FromBody] KullaniciForLoginDTO dto)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await KisiManager.UpdateKisiAcikRiza(dto, KisiId);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> UpdateKisiGizlilikOnaylandi([FromBody] KullaniciForLoginDTO dto)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await KisiManager.UpdateKisiGizlilikOnaylandi(dto, KisiId);
        }
    }
}