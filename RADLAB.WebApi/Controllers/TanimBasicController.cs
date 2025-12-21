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
    public class TanimBasicController : ControllerBase
    {
        private readonly ITanimBasicManager TanimBasicManager;

        public TanimBasicController(ITanimBasicManager _TanimBasicManager)
        {
            TanimBasicManager = _TanimBasicManager;
        }

        [HttpGet("[action]/{tanim}/{id:int}")]
        public async Task<TanimBasicDTO> GetTanimBasic(string Tanim, int Id)
        {
            return await TanimBasicManager.GetTanimBasic(Tanim, Id);
        }

        [HttpGet("[action]/{tanim}")]
        public async Task<List<TanimBasicDTO>> GetTanimBasicList(string Tanim)
        {
            return await TanimBasicManager.GetTanimBasicList(Tanim);
        }

        [HttpPost("[action]/{tanim}")]
        public async Task<ServiceResponse<string>> InsertOrUpdateTanimBasic(string Tanim, [FromBody] TanimBasicDTO dto)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await TanimBasicManager.InsertOrUpdateTanimBasic(Tanim, dto, KisiId);
        }

        [HttpPost("[action]/{tanim}")]
        public async Task<ServiceResponse<string>> DeleteTanimBasic(string Tanim, [FromBody] List<TanimBasicDTO> dtos)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await TanimBasicManager.DeleteTanimBasic(Tanim, dtos, KisiId);
        }
    }
}