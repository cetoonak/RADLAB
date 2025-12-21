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
    public class OgrenciController : ControllerBase
    {
        private readonly IOgrenciManager OgrenciManager;

        public OgrenciController(IOgrenciManager _OgrenciManager)
        {
            OgrenciManager = _OgrenciManager;
        }

        [HttpGet("[action]/{id:int}")]
        public async Task<OgrenciDTO> GetOgrenci(int Id)
        {
            return await OgrenciManager.GetOgrenci(Id);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<List<OgrenciDTO>>> GetOgrenciList([FromBody] OgrenciFilterDTO filterDTO)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await OgrenciManager.GetOgrenciList(filterDTO, KisiId);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> InsertOrUpdateOgrenci([FromBody] OgrenciDTO dto)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await OgrenciManager.InsertOrUpdateOgrenci(dto, KisiId);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> UpdateOgrenciAktif([FromBody] List<OgrenciDTO> dtos)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await OgrenciManager.UpdateOgrenciAktif(dtos, KisiId);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> DeleteOgrenci([FromBody] List<OgrenciDTO> dtos)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await OgrenciManager.DeleteOgrenci(dtos, KisiId);
        }
    }
}