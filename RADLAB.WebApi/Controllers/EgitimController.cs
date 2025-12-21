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
    public class EgitimController : ControllerBase
    {
        private readonly IEgitimManager EgitimManager;

        public EgitimController(IEgitimManager _EgitimManager)
        {
            EgitimManager = _EgitimManager;
        }

        [HttpGet("[action]/{id:int}")]
        public async Task<EgitimDTO> GetEgitim(int Id)
        {
            return await EgitimManager.GetEgitim(Id);
        }

        [HttpGet("[action]")]
        public async Task<List<EgitimDTO>> GetEgitimList()
        {
            return await EgitimManager.GetEgitimList();
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> InsertOrUpdateEgitim([FromBody] EgitimDTO dto)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await EgitimManager.InsertOrUpdateEgitim(dto, KisiId);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> DeleteEgitim([FromBody] List<EgitimDTO> dtos)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await EgitimManager.DeleteEgitim(dtos, KisiId);
        }
    }
}