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
    public class RaporController : ControllerBase
    {
        private readonly IRaporManager RaporManager;

        public RaporController(IRaporManager _RaporManager)
        {
            RaporManager = _RaporManager;
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> GetRaporBarkod([FromBody] FRDTO dto)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await RaporManager.GetRaporBarkod(dto, KisiId);
        }
    }
}