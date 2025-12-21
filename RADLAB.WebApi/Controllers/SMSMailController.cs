using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;
using RADLAB.Business.Abstract;
using RADLAB.Business.Concrete;
using RADLAB.Model.DTO;
using RADLAB.Model.ResponseModels;

namespace RADLAB.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SMSMailController : ControllerBase
    {
        private readonly ISMSMailManager mailManager;

        public SMSMailController(ISMSMailManager _mailManager)
        {
            mailManager = _mailManager;
        }

        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<ServiceResponse<string>> SendMail([FromBody] MailDTO dto)
        {
            return await mailManager.SendMail(dto);
        }

        [AllowAnonymous]
        [HttpGet("[action]/{GSM}/{Mesaj}")]
        public async Task<ServiceResponse<string>> SendSMS(string GSM, string Mesaj)
        {
            return await mailManager.SendSMS(GSM, Mesaj);
        }

        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<ServiceResponse<string>> SendSMSAndMail([FromBody] MailDTO dto)
        {
            return await mailManager.SendSMSAndMail(dto);
        }
    }
}