using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RADLAB.Business.Abstract;
using RADLAB.Model.DTO;
using RADLAB.Model.ResponseModels;

namespace RADLAB.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TanimSayiliController : ControllerBase
    {
        private readonly ITanimSayiliManager TanimSayiliManager;

        public TanimSayiliController(ITanimSayiliManager _TanimSayiliManager)
        {
            TanimSayiliManager = _TanimSayiliManager;
        }

        [HttpGet("[action]/{tanim}/{id:int}")]
        public async Task<TanimSayiliDTO> GetTanimSayili(string Tanim, int Id)
        {
            return await TanimSayiliManager.GetTanimSayili(Tanim, Id);
        }

        [HttpGet("[action]/{tanim}")]
        public async Task<List<TanimSayiliDTO>> GetTanimSayiliList(string Tanim)
        {
            return await TanimSayiliManager.GetTanimSayiliList(Tanim);
        }

        [HttpPost("[action]/{tanim}")]
        public async Task<ServiceResponse<string>> InsertOrUpdateTanimSayili(string Tanim, [FromBody] TanimSayiliDTO dto)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await TanimSayiliManager.InsertOrUpdateTanimSayili(Tanim, dto, KisiId);
        }

        [HttpPost("[action]/{tanim}")]
        public async Task<ServiceResponse<string>> DeleteTanimSayili(string Tanim, [FromBody] List<TanimSayiliDTO> dtos)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await TanimSayiliManager.DeleteTanimSayili(Tanim, dtos, KisiId);
        }
    }
}