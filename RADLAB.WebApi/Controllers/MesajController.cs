using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RADLAB.Business.Abstract;
using RADLAB.Model.DTO;
using RADLAB.Model.FilterDTO;
using RADLAB.Model.ResponseModels;

namespace RADLAB.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MesajController : ControllerBase
    {
        private readonly IMesajManager MesajManager;

        public MesajController(IMesajManager _MesajManager)
        {
            MesajManager = _MesajManager;
        }

        [HttpGet("[action]/{gelengiden:int}/{id:int}")]
        public async Task<ServiceResponse<MesajDTO>> GetMesaj(int GelenGiden, int Id)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await MesajManager.GetMesaj(GelenGiden, Id, KisiId);
        }

        [HttpGet("[action]")]
        public async Task<ServiceResponse<int>> GetOkunmamisMesajSayisi()
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await MesajManager.GetOkunmamisMesajSayisi(KisiId);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<List<MesajDTO>>> GetMesajKutusu(MesajKutusuFilterDTO filterDTO)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await MesajManager.GetMesajKutusu(filterDTO, KisiId);
        }

        [HttpGet("[action]")]
        public async Task<ServiceResponse<List<MesajDTO>>> GetOkunmamisMesajlar()
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await MesajManager.GetOkunmamisMesajlar(KisiId);
        }

        [HttpGet("[action]/{id}")]
        public async Task<ServiceResponse<MesajGrubuMasterDTO>> GetMesajGrubuMaster(int Id)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await MesajManager.GetMesajGrubuMaster(Id, KisiId);
        }

        [HttpGet("[action]")]
        public async Task<ServiceResponse<List<MesajGrubuMasterDTO>>> GetMesajGrubuMasterList()
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await MesajManager.GetMesajGrubuMasterList(KisiId);
        }

        [HttpGet("[action]/{SearchText}")]
        public async Task<ServiceResponse<List<MesajKisiDTO>>> GetMesajKisiList(string SearchText)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await MesajManager.GetMesajKisiList(SearchText, KisiId);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> InsertMesaj([FromBody] MesajYazDTO dto)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await MesajManager.InsertMesaj(dto, KisiId);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> InsertOrUpdateMesajGrubu([FromBody] MesajGrubuMasterDTO dto)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await MesajManager.InsertOrUpdateMesajGrubu(dto, KisiId);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> UpdateMesajGonderilenKisiOkundu([FromBody] MesajDTO dto)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await MesajManager.UpdateMesajGonderilenKisiOkundu(dto, KisiId);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> UpdateMesajGonderilenKisiVeyaMesajSilVeyaGeriAl([FromBody] MesajDTO dto)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await MesajManager.UpdateMesajGonderilenKisiVeyaMesajSilVeyaGeriAl(dto, KisiId);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> DeleteMesajGrubuMaster([FromBody] MesajGrubuMasterDTO dto)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await MesajManager.DeleteMesajGrubuMaster(dto, KisiId);
        }
    }
}