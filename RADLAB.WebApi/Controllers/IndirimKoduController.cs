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
    public class IndirimKoduController : ControllerBase
    {
        private readonly IIndirimKoduManager IndirimKoduManager;

        public IndirimKoduController(IIndirimKoduManager _IndirimKoduManager)
        {
            IndirimKoduManager = _IndirimKoduManager;
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<List<IndirimKoduDTO>>> GetIndirimKoduList([FromBody] IndirimKoduFilterDTO filterDTO)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await IndirimKoduManager.GetIndirimKoduList(filterDTO, KisiId);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> InsertOrUpdateIndirimKodu([FromBody] IndirimKoduDTO dto)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await IndirimKoduManager.InsertOrUpdateIndirimKodu(dto, KisiId);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> DeleteIndirimKodu([FromBody] List<IndirimKoduDTO> dtos)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await IndirimKoduManager.DeleteIndirimKodu(dtos, KisiId);
        }
    }
}