using RADLAB.Model.DTO;
using RADLAB.Model.FilterDTO;
using RADLAB.Model.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Business.Abstract
{
    public interface IDashboardManager
    {
        public Task<ServiceResponse<DashboardSayiDTO>> GetDashboardSayi(DashboardFilterDTO filterDTO, int KisiId);
        public Task<ServiceResponse<List<DashboardGrafikDTO>>> GetDashboardGrafik(DashboardFilterDTO filterDTO, int KisiId);
        public Task<ServiceResponse<List<DuyuruDTO>>> GetDashboardDuyuru(int KisiId);
    }
}