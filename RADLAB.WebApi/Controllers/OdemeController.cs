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
    public class OdemeController : ControllerBase
    {
        private readonly IOdemeManager odemeManager;

        public OdemeController(IOdemeManager _odemeManager)
        {
            odemeManager = _odemeManager;
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<ServiceResponse<List<OdemeDTO>>> GetOdeme([FromBody] MusteriTakipFilterDTO filterDTO)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await odemeManager.GetOdeme(filterDTO, KisiId);
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<ServiceResponse<List<OdemeDTO>>> GetOdemeByVerifyEnrollmentRequestId([FromBody] string VerifyEnrollmentRequestId)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await odemeManager.GetOdemeByVerifyEnrollmentRequestId(VerifyEnrollmentRequestId, KisiId);
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> UpdateOdeme([FromBody] List<OdemeDTO> dto)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await odemeManager.UpdateOdeme(dto, KisiId);
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> UpdateOdemeVerifyEnrollmentRequestId([FromBody] List<OdemeDTO> dto)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await odemeManager.UpdateOdemeVerifyEnrollmentRequestId(dto, KisiId);
        }
    }
}
