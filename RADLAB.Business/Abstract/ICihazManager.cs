using RADLAB.Model.DTO;
using RADLAB.Model.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Business.Abstract
{
    public interface ICihazManager
    {
        public Task<CihazDTO> GetCihaz(int Id);
        public Task<List<CihazDTO>> GetCihazList(string Acilanlar);
        public Task<List<CihazDTO>> GetCihazSatisList();
        public Task<ServiceResponse<string>> InsertOrUpdateCihaz(CihazDTO dto, int KisiId);
        public Task<ServiceResponse<string>> DeleteCihaz(List<CihazDTO> dtos, int KisiId);
    }
}