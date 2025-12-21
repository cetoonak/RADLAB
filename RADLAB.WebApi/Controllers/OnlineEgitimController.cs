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
    public class OnlineEgitimController : ControllerBase
    {
        private readonly IOnlineEgitimManager OnlineEgitimManager;

        public OnlineEgitimController(IOnlineEgitimManager _OnlineEgitimManager)
        {
            OnlineEgitimManager = _OnlineEgitimManager;
        }

        [HttpGet("[action]/{id:int}")]
        public async Task<ServiceResponse<OnlineEgitimDTO>> GetOnlineEgitim(int Id)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await OnlineEgitimManager.GetOnlineEgitim(Id, KisiId);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<List<OnlineEgitimDTO>>> GetOnlineEgitimList([FromBody] OnlineEgitimFilterDTO filterDTO)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await OnlineEgitimManager.GetOnlineEgitimList(filterDTO, KisiId);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> InsertOrUpdateOnlineEgitim([FromBody] OnlineEgitimDTO dto)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await OnlineEgitimManager.InsertOrUpdateOnlineEgitim(dto, KisiId);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> DeleteOnlineEgitim([FromBody] List<OnlineEgitimDTO> dtos)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await OnlineEgitimManager.DeleteOnlineEgitim(dtos, KisiId);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> InsertOnlineEgitimOgrenci([FromBody] List<OnlineEgitimKisiDTO> dtos)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await OnlineEgitimManager.InsertOnlineEgitimOgrenci(dtos, KisiId);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> DeleteOnlineEgitimOgrenci([FromBody] List<OnlineEgitimKisiDTO> dtos)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await OnlineEgitimManager.DeleteOnlineEgitimOgrenci(dtos, KisiId);
        }

        [HttpGet("[action]/{OnlineEgitimOgrenciId:int}")]
        public async Task<ServiceResponse<List<OnlineEgitimTreeDTO>>> GetOnlineEgitimTreeList(int OnlineEgitimOgrenciId)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await OnlineEgitimManager.GetOnlineEgitimTreeList(OnlineEgitimOgrenciId, KisiId);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> InsertUpdateOnlineEgitimBolumTamamlanan([FromBody] OnlineEgitimBolumTamamlananDTO dto)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await OnlineEgitimManager.InsertUpdateOnlineEgitimBolumTamamlanan(dto, KisiId);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> UpdateOnlineEgitimBolumTamamlananGecenSure([FromBody] OnlineEgitimTreeDTO dto)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await OnlineEgitimManager.UpdateOnlineEgitimBolumTamamlananGecenSure(dto, KisiId);
        }

        [HttpGet("[action]/{OnlineEgitimBolumId:int}")]
        public async Task<ServiceResponse<int>> GetOnlineEgitimBolumTamamlananGecenSure(int OnlineEgitimBolumId)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await OnlineEgitimManager.GetOnlineEgitimBolumTamamlananGecenSure(OnlineEgitimBolumId, KisiId);
        }

        [HttpGet("[action]/{OnlineEgitimOgrenciId:int}")]
        public async Task<ServiceResponse<List<OnlineEgitimSonucDTO>>> GetOnlineEgitimSonucList(int OnlineEgitimOgrenciId)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await OnlineEgitimManager.GetOnlineEgitimSonucList(OnlineEgitimOgrenciId, KisiId);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<List<OnlineEgitimTestSonucDTO>>> GetOnlineEgitimTestSonuc([FromBody] OnlineEgitimTestSonucFilterDTO filterDTO)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await OnlineEgitimManager.GetOnlineEgitimTestSonuc(filterDTO, KisiId);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> UpdateOnlineEgitimBolumTamamlananVideoTime([FromBody] OnlineEgitimBolumTamamlananDTO dto)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await OnlineEgitimManager.UpdateOnlineEgitimBolumTamamlananVideoTime(dto, KisiId);
        }
    }
}