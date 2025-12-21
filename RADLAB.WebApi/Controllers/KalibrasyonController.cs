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
    public class KalibrasyonController : ControllerBase
    {
        private readonly IKalibrasyonManager KalibrasyonManager;

        public KalibrasyonController(IKalibrasyonManager _KalibrasyonManager)
        {
            KalibrasyonManager = _KalibrasyonManager;
        }

        //////////////////////////////////////////////// Kalibrasyon ////////////////////////////////////////////

        [AllowAnonymous]
        [HttpGet("[action]/{id:int}")]
        public async Task<ServiceResponse<KalibrasyonDTO>> GetKalibrasyon(int Id)
        {
            return await KalibrasyonManager.GetKalibrasyon(Id, 0);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<List<KalibrasyonDTO>>> GetKalibrasyonList([FromBody] KalibrasyonFilterDTO filterDTO)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await KalibrasyonManager.GetKalibrasyonList(filterDTO, KisiId);
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> InsertUpdateKalibrasyon([FromBody] KalibrasyonDTO dto)
        {
            int KisiId = 0;
            
            try
            {
                KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);
            }
            catch
            {

            }

            return await KalibrasyonManager.InsertUpdateKalibrasyon(dto, KisiId);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> DeleteKalibrasyon([FromBody] List<int> Idler)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await KalibrasyonManager.DeleteKalibrasyon(Idler, KisiId);
        }

        //////////////////////////////////////////////// KalibrasyonCihaz ////////////////////////////////////////////

        [HttpGet("[action]/{id:int}")]
        public async Task<ServiceResponse<KalibrasyonCihazDTO>> GetKalibrasyonCihaz(int Id)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await KalibrasyonManager.GetKalibrasyonCihaz(Id, KisiId);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<List<KalibrasyonCihazDTO>>> GetKalibrasyonCihazList([FromBody] KalibrasyonCihazFilterDTO filterDTO)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await KalibrasyonManager.GetKalibrasyonCihazList(filterDTO, KisiId);
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<ServiceResponse<List<KalibrasyonCihazTakipDTO>>> GetMusteriTakip([FromBody] MusteriTakipFilterDTO filterDTO)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await KalibrasyonManager.GetMusteriTakip(filterDTO, KisiId);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> InsertUpdateKalibrasyonCihaz([FromBody] KalibrasyonCihazDTO dto)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await KalibrasyonManager.InsertUpdateKalibrasyonCihaz(dto, KisiId);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> DeleteKalibrasyonCihaz([FromBody] List<int> Idler)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await KalibrasyonManager.DeleteKalibrasyonCihaz(Idler, KisiId);
        }

        //////////////////////////////////////////// KalibrasyonCihazHareket ////////////////////////////////////////////

        [HttpPost("[action]")]
        public async Task<ServiceResponse<List<KalibrasyonCihazHareketDTO>>> GetKalibrasyonCihazHareketList([FromBody] KalibrasyonCihazHareketFilterDTO filterDTO)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await KalibrasyonManager.GetKalibrasyonCihazHareketList(filterDTO, KisiId);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> InsertKalibrasyonCihazHareket([FromBody] KalibrasyonCihazHareketDTO dto)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await KalibrasyonManager.InsertKalibrasyonCihazHareket(dto, KisiId);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> InsertUpdateKalibrasyonCihazHareket([FromBody] KalibrasyonCihazHareketDTO dto)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await KalibrasyonManager.InsertUpdateKalibrasyonCihazHareket(dto, KisiId);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> DeleteKalibrasyonCihazHareket([FromBody] List<int> Idler)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await KalibrasyonManager.DeleteKalibrasyonCihazHareket(Idler, KisiId);
        }

        //////////////////////////////////////////// KalibrasyonCihazOdeme ////////////////////////////////////////////

        [HttpPost("[action]")]
        public async Task<ServiceResponse<List<KalibrasyonCihazOdemeDTO>>> GetKalibrasyonCihazOdemeList([FromBody] KalibrasyonCihazOdemeFilterDTO filterDTO)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await KalibrasyonManager.GetKalibrasyonCihazOdemeList(filterDTO, KisiId);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> InsertUpdateKalibrasyonCihazOdeme([FromBody] KalibrasyonCihazOdemeDTO dto)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await KalibrasyonManager.InsertUpdateKalibrasyonCihazOdeme(dto, KisiId);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> DeleteKalibrasyonCihazOdeme([FromBody] List<int> Idler)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await KalibrasyonManager.DeleteKalibrasyonCihazOdeme(Idler, KisiId);
        }

        //////////////////////////////////////////// KalibrasyonCihazHareketDosya ////////////////////////////////////////////

        [HttpPost("[action]")]
        public async Task<ServiceResponse<List<KalibrasyonCihazHareketDosyaDTO>>> GetKalibrasyonCihazHareketDosyaList([FromBody] int Id)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await KalibrasyonManager.GetKalibrasyonCihazHareketDosyaList(Id, KisiId);
        }

        //////////////////////////////////////////// KalibrasyonCihazOlcum ////////////////////////////////////////////

        [HttpPost("[action]")]
        public async Task<ServiceResponse<List<KalibrasyonCihazOlcumDTO>>> GetKalibrasyonCihazOlcumList([FromBody] int Id)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await KalibrasyonManager.GetKalibrasyonCihazOlcumList(Id, KisiId);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> UpdateKalibrasyonCihazOlcum([FromBody] KalibrasyonCihazOlcumForKayitDTO dto)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await KalibrasyonManager.UpdateKalibrasyonCihazOlcum(dto, KisiId);
        }

        //////////////////////////////////////////// Turkak ////////////////////////////////////////////

        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> TurkakaGonder([FromBody] KalibrasyonCihazDTO dto)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await KalibrasyonManager.TurkakaGonder(dto, KisiId);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> TurkakQRKoduAl([FromBody] KalibrasyonCihazDTO dto)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await KalibrasyonManager.TurkakQRKoduAl(dto, KisiId);
        }
    }
}