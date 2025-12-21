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
    public class CihazController : ControllerBase
    {
        private readonly ICihazManager CihazManager;

        public CihazController(ICihazManager _CihazManager)
        {
            CihazManager = _CihazManager;
        }

        [AllowAnonymous]
        [HttpGet("[action]/{id:int}")]
        public async Task<CihazDTO> GetCihaz(int Id)
        {
            return await CihazManager.GetCihaz(Id);
        }

        [HttpGet("[action]/{acilanlar}")]
        public async Task<List<CihazDTO>> GetCihazList(string Acilanlar)
        {
            return await CihazManager.GetCihazList(Acilanlar);
        }

        [AllowAnonymous]
        [HttpGet("[action]")]
        public async Task<List<CihazDTO>> GetCihazSatisList()
        {
            return await CihazManager.GetCihazSatisList();
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> InsertOrUpdateCihaz([FromBody] CihazDTO dto)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await CihazManager.InsertOrUpdateCihaz(dto, KisiId);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> DeleteCihaz([FromBody] List<CihazDTO> dtos)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await CihazManager.DeleteCihaz(dtos, KisiId);
        }
    }
}