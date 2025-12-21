using Microsoft.AspNetCore.Authorization;
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
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardManager dashboardManager;

        public DashboardController(IDashboardManager _dashboardManager)
        {
            dashboardManager = _dashboardManager;
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<DashboardSayiDTO>> GetDashboardSayi([FromBody] DashboardFilterDTO filterDTO)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await dashboardManager.GetDashboardSayi(filterDTO, KisiId);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<List<DashboardGrafikDTO>>> GetDashboardGrafik([FromBody] DashboardFilterDTO filterDTO)
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await dashboardManager.GetDashboardGrafik(filterDTO, KisiId);
        }

        [HttpGet("[action]")]
        public async Task<ServiceResponse<List<DuyuruDTO>>> GetDashboardDuyuru()
        {
            var KisiId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("userdata"))?.Value);

            return await dashboardManager.GetDashboardDuyuru(KisiId);
        }
    }
}