using RADLAB.Model.DTO;
using RADLAB.Model.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Business.Exclude
{
    public interface IPayTRManager
    {
        public Task<ServiceResponse<string>> PayTRAdim1(PayTRDTO dto, int KisiId);
        public Task<ServiceResponse<string>> PayTRAdim2(PayTRDTO dto, int KisiId);
        public Task<ServiceResponse<string>> UpdateSiparisPayTRToken(SiparisDTO dto, int KisiId);
        public Task<ServiceResponse<string>> UpdateSiparisPayTROdemeTamam(string MerchantOID, int KisiId);
        public Task<string> GetSiparisPayTRToken(int Id, int KisiId);
    }
}