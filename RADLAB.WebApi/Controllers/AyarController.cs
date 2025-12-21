using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RADLAB.Business.Abstract;
using RADLAB.Model.DTO;
using RADLAB.Model.FilterDTO;
using RADLAB.Model.ResponseModels;

namespace RADLAB.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AyarController : ControllerBase
    {
        private readonly IAyarManager ayarManager;

        public AyarController(IAyarManager _ayarManager)
        {
            ayarManager = _ayarManager;
        }

        [HttpGet("[action]")]
        public async Task<AyarDTO> GetAyar()
        {
            return await ayarManager.GetAyar();
        }

        [HttpGet("[action]")]
        public async Task<AyarDTO> GetAyarForOlcum()
        {
            return await ayarManager.GetAyarForOlcum();
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<AyarKargoDTO> GetAyarForKargo([FromBody] KargoAyarFilterDTO filterDTO)
        {
            return await ayarManager.GetAyarForKargo(filterDTO);
        }

        [AllowAnonymous]
        [HttpGet("[action]/{field}")]
        public async Task<string> GetMetin(string field)
        {
            return await ayarManager.GetMetin(field);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> InsertOrUpdateAyar([FromBody] AyarDTO dto)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await ayarManager.InsertOrUpdateAyar(dto, KisiId);
        }
    }
}