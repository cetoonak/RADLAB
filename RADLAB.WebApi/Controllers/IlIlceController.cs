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
    public class IlIlceController : ControllerBase
    {
        private readonly IIlIlceManager IlIlceManager;

        public IlIlceController(IIlIlceManager _IlIlceManager)
        {
            IlIlceManager = _IlIlceManager;
        }

        [AllowAnonymous]
        [HttpGet("[action]/{id:int}")]
        public async Task<IlIlceDTO> GetIlIlce(int Id)
        {
            return await IlIlceManager.GetIlIlce(Id);
        }

        [HttpGet("[action]/{acilanlar}")]
        public async Task<List<IlIlceDTO>> GetIlIlceList(string Acilanlar)
        {
            return await IlIlceManager.GetIlIlceList(Acilanlar);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> InsertOrUpdateIlIlce([FromBody] IlIlceDTO dto)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await IlIlceManager.InsertOrUpdateIlIlce(dto, KisiId);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> DeleteIlIlce([FromBody] List<IlIlceDTO> dtos)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await IlIlceManager.DeleteIlIlce(dtos, KisiId);
        }
    }
}