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
    public class SiparisController : ControllerBase
    {
        private readonly ISiparisManager SiparisManager;

        public SiparisController(ISiparisManager _SiparisManager)
        {
            SiparisManager = _SiparisManager;
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<List<SiparisDTO>>> GetSiparisList([FromBody] SiparisFilterDTO filterDTO)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await SiparisManager.GetSiparisList(filterDTO, KisiId);
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<ServiceResponse<List<SiparisDTO>>> GetSiparisListByVerifyEnrollmentRequestId([FromBody] string VerifyEnrollmentRequestId)
        {
            var filterDTO = new SiparisFilterDTO()
            {
                VerifyEnrollmentRequestId = VerifyEnrollmentRequestId,
                PageNo = 1,
                PageSize = 1,
                DonusTipi = 0
            };

            return await SiparisManager.GetSiparisList(filterDTO, 0);
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<ServiceResponse<List<SiparisCihazTakipDTO>>> GetMusteriTakip([FromBody] MusteriTakipFilterDTO filterDTO)
        {
            return await SiparisManager.GetMusteriTakip(filterDTO, 0);
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> InsertSiparis([FromBody] SiparisDTO dto)
        {
            return await SiparisManager.InsertSiparis(dto, 0);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> InsertOrUpdateSiparis([FromBody] SiparisDTO dto)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await SiparisManager.InsertOrUpdateSiparis(dto, KisiId);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> DeleteSiparis([FromBody] List<SiparisDTO> dtos)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await SiparisManager.DeleteSiparis(dtos, KisiId);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> InsertOrUpdateSiparisCihaz([FromBody] SiparisCihazDTO dto)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await SiparisManager.InsertOrUpdateSiparisCihaz(dto, KisiId);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> DeleteSiparisCihaz([FromBody] List<SiparisCihazDTO> dtos)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await SiparisManager.DeleteSiparisCihaz(dtos, KisiId);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<List<SiparisCihazHareketDTO>>> GetSiparisCihazHareket([FromBody] int SiparisCihazId)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await SiparisManager.GetSiparisCihazHareket(SiparisCihazId, KisiId);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> InsertSiparisCihazHareket([FromBody] SiparisCihazHareketDTO dto)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await SiparisManager.InsertSiparisCihazHareket(dto, KisiId);
        }
    }
}