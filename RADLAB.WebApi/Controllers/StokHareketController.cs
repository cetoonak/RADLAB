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
    public class StokHareketController : ControllerBase
    {
        private readonly IStokHareketManager StokHareketManager;

        public StokHareketController(IStokHareketManager _StokHareketManager)
        {
            StokHareketManager = _StokHareketManager;
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<List<StokHareketDTO>>> GetStokHareketList([FromBody] StokHareketFilterDTO filterDTO)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await StokHareketManager.GetStokHareketList(filterDTO, KisiId);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> InsertOrUpdateStokHareket([FromBody] StokHareketDTO dto)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await StokHareketManager.InsertOrUpdateStokHareket(dto, KisiId);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> DeleteStokHareket([FromBody] List<StokHareketDTO> dtos)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await StokHareketManager.DeleteStokHareket(dtos, KisiId);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> InsertOrUpdateStokHareketCihaz([FromBody] StokHareketCihazDTO dto)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await StokHareketManager.InsertOrUpdateStokHareketCihaz(dto, KisiId);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> DeleteStokHareketCihaz([FromBody] List<StokHareketCihazDTO> dtos)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await StokHareketManager.DeleteStokHareketCihaz(dtos, KisiId);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<List<StokHareketleriDTO>>> GetStokHareketleriList([FromBody] StokHareketleriFilterDTO filterDTO)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await StokHareketManager.GetStokHareketleriList(filterDTO, KisiId);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<List<StokMiktarlariDTO>>> GetStokMiktarlariList([FromBody] StokMiktarlariFilterDTO filterDTO)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await StokHareketManager.GetStokMiktarlariList(filterDTO, KisiId);
        }
    }
}