using RADLAB.Model.DTO;
using RADLAB.Model.FilterDTO;
using RADLAB.Model.ResponseModels;

namespace RADLAB.UI.Services.Infrastructure
{
    public interface IDashboardService
    {
        public Task<ServiceResponse<DashboardSayiDTO>> GetDashboardSayi(DashboardFilterDTO filterDTO);
        public Task<ServiceResponse<List<DashboardGrafikDTO>>> GetDashboardGrafik(DashboardFilterDTO filterDTO);
        public Task<ServiceResponse<List<DuyuruDTO>>> GetDashboardDuyuru();
    }
}