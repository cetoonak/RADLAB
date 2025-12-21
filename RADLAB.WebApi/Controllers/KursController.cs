using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RADLAB.Business.Abstract;
using RADLAB.Business.Concrete;
using RADLAB.Model.DTO;
using RADLAB.Model.FilterDTO;
using RADLAB.Model.ResponseModels;

namespace RADLAB.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class KursController : ControllerBase
    {
        private readonly IKursManager KursManager;

        public KursController(IKursManager _KursManager)
        {
            KursManager = _KursManager;
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<List<KursDTO>>> GetKursList([FromBody] KursFilterDTO filterDTO)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await KursManager.GetKursList(filterDTO, KisiId);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> InsertOrUpdateKurs([FromBody] KursDTO dto)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await KursManager.InsertOrUpdateKurs(dto, KisiId);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> DeleteKurs([FromBody] List<KursDTO> dtos)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await KursManager.DeleteKurs(dtos, KisiId);
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<ServiceResponse<List<KursYayinDTO>>> GetKursYayinList([FromBody] int Id)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await KursManager.GetKursYayinList(Id, KisiId);
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<ServiceResponse<List<KursiyerTakipDTO>>> GetMusteriTakip([FromBody] MusteriTakipFilterDTO filterDTO)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await KursManager.GetMusteriTakip(filterDTO, KisiId);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<KursiyerDTO>> GetKursiyer([FromBody] int Id)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await KursManager.GetKursiyer(Id, KisiId);
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> InsertOrUpdateKursiyer([FromBody] KursiyerDTO dto)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await KursManager.InsertOrUpdateKursiyer(dto, KisiId);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> DeleteKursiyer([FromBody] List<KursiyerDTO> dtos)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await KursManager.DeleteKursiyer(dtos, KisiId);
        }
    }
}