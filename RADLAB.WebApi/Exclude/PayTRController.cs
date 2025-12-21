using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RADLAB.Business.Concrete;
using RADLAB.Business.Exclude;
using RADLAB.Model.DTO;
using RADLAB.Model.Exclude;
using RADLAB.Model.FilterDTO;
using RADLAB.Model.ResponseModels;

namespace RADLAB.WebApi.Exclude
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PayTRController : ControllerBase
    {
        private readonly IPayTRManager paytrManager;

        public PayTRController(IPayTRManager _paytrManager)
        {
            paytrManager = _paytrManager;
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> PayTRAdim1([FromBody] PayTRDTO dto)
        {
            return await paytrManager.PayTRAdim1(dto, 0);
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> PayTRAdim2([FromBody] PayTRDTO dto)
        {
            return await paytrManager.PayTRAdim2(dto, 0);
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> UpdateSiparisPayTRToken([FromBody] SiparisDTO dto)
        {
            return await paytrManager.UpdateSiparisPayTRToken(dto, 0);
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> UpdateSiparisPayTROdemeTamam([FromBody] string MerchantOID)
        {
            return await paytrManager.UpdateSiparisPayTROdemeTamam(MerchantOID, 0);
        }

        [AllowAnonymous]
        [HttpGet("[action]/{id:int}")]
        public async Task<string> GetSiparisPayTRToken(int Id)
        {
            return await paytrManager.GetSiparisPayTRToken(Id, 0);
        }
    }
}