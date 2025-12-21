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
    public class TanimKodluController : ControllerBase
    {
        private readonly ITanimKodluManager TanimKodluManager;

        public TanimKodluController(ITanimKodluManager _TanimKodluManager)
        {
            TanimKodluManager = _TanimKodluManager;
        }

        [HttpGet("[action]/{tanim}/{id:int}")]
        public async Task<TanimKodluDTO> GetTanimKodlu(string Tanim, int Id)
        {
            return await TanimKodluManager.GetTanimKodlu(Tanim, Id);
        }

        [HttpGet("[action]/{tanim}")]
        public async Task<List<TanimKodluDTO>> GetTanimKodluList(string Tanim)
        {
            return await TanimKodluManager.GetTanimKodluList(Tanim);
        }

        [HttpPost("[action]/{tanim}")]
        public async Task<ServiceResponse<string>> InsertOrUpdateTanimKodlu(string Tanim, [FromBody] TanimKodluDTO dto)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await TanimKodluManager.InsertOrUpdateTanimKodlu(Tanim, dto, KisiId);
        }

        [HttpPost("[action]/{tanim}")]
        public async Task<ServiceResponse<string>> DeleteTanimKodlu(string Tanim, [FromBody] List<TanimKodluDTO> dtos)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await TanimKodluManager.DeleteTanimKodlu(Tanim, dtos, KisiId);
        }
    }
}