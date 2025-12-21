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
    public class DosyaController : ControllerBase
    {
        private readonly IDosyaManager DosyaManager;

        public DosyaController(IDosyaManager _DosyaManager)
        {
            DosyaManager = _DosyaManager;
        }

        [AllowAnonymous]
        [HttpGet("[action]/{id:int}")]
        public async Task<ServiceResponse<DosyaDTO>> GetDosya(int Id)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await DosyaManager.GetDosya(Id, KisiId);
        }

        [AllowAnonymous]
        [HttpGet("[action]")]
        public async Task<ServiceResponse<List<DosyaDTO>>> GetDosyaList()
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await DosyaManager.GetDosyaList(KisiId);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> InsertOrUpdateDosya([FromBody] DosyaDTO dto)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await DosyaManager.InsertOrUpdateDosya(dto, KisiId);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> DeleteDosya([FromBody] List<DosyaDTO> dtos)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await DosyaManager.DeleteDosya(dtos, KisiId);
        }
    }
}