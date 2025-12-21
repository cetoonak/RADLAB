using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RADLAB.Business.Abstract;
using RADLAB.Business.Concrete;
using RADLAB.Model.DTO;
using RADLAB.Model.ResponseModels;
using System.Linq.Expressions;
using System.Text;

namespace RADLAB.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VakifBankController : ControllerBase
    {
        private readonly IVakifBankManager vakifBankManager;

        public VakifBankController(IVakifBankManager _vakifBankManager)
        {
            vakifBankManager = _vakifBankManager;
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<ServiceResponse<VakifBankMPIDTO>> Enrollment([FromBody] SiparisDTO dto)
        {
            return await vakifBankManager.Enrollment(dto, 0);
        }
    }
}