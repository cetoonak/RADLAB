using RADLAB.Model.DTO;
using RADLAB.Model.ResponseModels;

namespace RADLAB.UI.Services.Infrastructure
{
    public interface ISMSMailService
    {
        public Task<ServiceResponse<string>> SendMail(MailDTO dto);
        public Task<ServiceResponse<string>> SendSMS(string GSM, string Mesaj);
        public Task<ServiceResponse<string>> SendSMSAndMail(MailDTO dto);
    }
}