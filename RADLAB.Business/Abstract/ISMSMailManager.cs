using RADLAB.Model.DTO;
using RADLAB.Model.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Business.Abstract
{
    public interface ISMSMailManager
    {
        public Task<ServiceResponse<string>> SendMail(MailDTO dto);
        public Task<ServiceResponse<string>> SendSMS(string GSM, string Mesaj);
        public Task<ServiceResponse<string>> SendSMSAndMail(MailDTO dto);
    }
}