using Microsoft.AspNetCore.Authorization;
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
    public class DuyuruController : ControllerBase
    {
        private readonly IDuyuruManager DuyuruManager;

        public DuyuruController(IDuyuruManager _DuyuruManager)
        {
            DuyuruManager = _DuyuruManager;
        }

        [HttpGet("[action]/{id:int}")]
        public async Task<DuyuruDTO> GetDuyuru(int Id)
        {
            return await DuyuruManager.GetDuyuru(Id);
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<ServiceResponse<List<DuyuruDTO>>> GetDuyuruList([FromBody] DuyuruFilterDTO filterDTO)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await DuyuruManager.GetDuyuruList(filterDTO, KisiId);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> InsertOrUpdateDuyuru([FromBody] DuyuruDTO dto)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await DuyuruManager.InsertOrUpdateDuyuru(dto, KisiId);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> DeleteDuyuru([FromBody] List<DuyuruDTO> dtos)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await DuyuruManager.DeleteDuyuru(dtos, KisiId);
        }
    }
}