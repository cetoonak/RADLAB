using Dapper;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit.Text;
using MimeKit;
using RADLAB.Business.Abstract;
using RADLAB.Business.Utils;
using RADLAB.Model.DTO;
using RADLAB.Model.ResponseModels;
using System.Data.SqlClient;
using System.Reflection;
using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore.Storage;

namespace RADLAB.Business.Concrete
{
	public class SMSMailManager : ISMSMailManager
    {
        private readonly IConfiguration configuration;
        private readonly string CS;
        public string SMSUrl { get; set; }

        public SMSMailManager(IConfiguration _configuration)
        {
            bool debug = false;
#if DEBUG
            debug = true;
#endif

            configuration = _configuration;

            SMSUrl = configuration.GetSection("URL:SMS").Value;

            CS = configuration.GetConnectionString(debug ? "Debug" : "Release");
        }

        private class MailSettingsDTO
        {
            public string KurumAdi { get; set; } = string.Empty;
            public string EMail { get; set; } = string.Empty;
            public string WebAdresi { get; set; } = string.Empty;
            public string EMailSunucusu { get; set; } = string.Empty;
            public string EMailGonderenAdres { get; set; } = string.Empty;
            public string EMailGonderenSifre { get; set; } = string.Empty;
            public int EMailPort { get; set; }
            public bool EMailSSL { get; set; }
            public string MailSablonu { get; set; } = string.Empty;
        }

        public async Task<ServiceResponse<string>> SendMail(MailDTO dto)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                string QMailSettings = $@"  SELECT TOP 1
                                                KurumAdi, EMail, WebAdresi, EMailSunucusu, EMailGonderenAdres, EMailGonderenSifre, EMailPort, EMailSSL, MailSablonu
                                            FROM
                                                Ayar";

                var mailSettingsDTO = await connection.QuerySingleOrDefaultAsync<MailSettingsDTO>(QMailSettings);

                var email = new MimeMessage();

                email.From.Add(MailboxAddress.Parse(mailSettingsDTO.EMailGonderenAdres));

                foreach(var alici in dto.ToList)
                {
                    email.To.Add(MailboxAddress.Parse(alici));
                }

                if (dto.KendisineDeGonder)
                {
                    email.To.Add(MailboxAddress.Parse(mailSettingsDTO.EMailGonderenAdres));
                }

                string body = mailSettingsDTO.MailSablonu.Replace("[Body]", dto.Body);

                email.Subject = dto.Subject;
                email.Body = new TextPart(TextFormat.Html) { Text = body };

                using var smtp = new SmtpClient();

                smtp.Connect(mailSettingsDTO.EMailSunucusu, mailSettingsDTO.EMailPort, SecureSocketOptions.Auto);
                smtp.Authenticate(mailSettingsDTO.EMailGonderenAdres, mailSettingsDTO.EMailGonderenSifre);
                smtp.Send(email);
                smtp.Disconnect(true);
            }
            catch (Exception ex)
            {
                R.Success = false;
                R.Message = ex.Message;
            }

            var logReturn = await IslemLog.Logla(configuration, 0, MethodBase.GetCurrentMethod().DeclaringType, R.Message, "", 0);

            if (!R.Success) R.Message = logReturn;

            return R;
        }

        public async Task<ServiceResponse<string>> SendSMS(string GSM, string Mesaj)
        {
            var R = new ServiceResponse<string>();

            try
            {
                Mesaj = Mesaj.Replace("[BCKSPC]", "/");

                if (GSM.StartsWith("SMS"))
                {
                    using var connection = new SqlConnection(CS);

                    string Q = $@"  DECLARE @GSM VARCHAR(1000) = ''

                                    SELECT
	                                    @GSM = @GSM + TelefonCep + ','
                                    FROM
	                                    Kisi
                                    WHERE
	                                    Aktif = 1
	                                    AND {GSM} = 1

                                    IF LEN(@GSM) > 0 SET @GSM = LEFT(@GSM, LEN(@GSM) - 1)

                                    SELECT @GSM";

                    GSM = await connection.QuerySingleOrDefaultAsync<string>(Q);
                }

                if (string.IsNullOrEmpty(GSM))
                {
                    R.Success = false;
                    R.Message = "GSM belirlenmemis";
                }
                else
                {
                    string url = SMSUrl.Replace("[GSM]", GSM).Replace("[Mesaj]", Mesaj);

                    var client = new HttpClient();

                    var request = new HttpRequestMessage(HttpMethod.Get, url);

                    var response = await client.SendAsync(request);

                    response.EnsureSuccessStatusCode();

                    R.Value = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                R.Success = false;
                R.Message = ex.Message;
            }

            var logReturn = await IslemLog.Logla(configuration, 0, MethodBase.GetCurrentMethod().DeclaringType, R.Message, "", 0);

            if (!R.Success) R.Message = logReturn;

            return R;
        }

        public async Task<ServiceResponse<string>> SendSMSAndMail(MailDTO dto)
        {
            var RSMS = new ServiceResponse<string>();
            var RMail = new ServiceResponse<string>();

            if (string.IsNullOrEmpty(dto.GSM) == false)
            {
                RSMS = await SendSMS(dto.GSM, (string.IsNullOrEmpty(dto.Mesaj) ? dto.Body : dto.Mesaj));
            }

            if (string.IsNullOrEmpty(dto.ToList[0]) == false)
            {
                RMail = await SendMail(dto);
            }

            return RSMS.Success ? RSMS : RMail;
        }
    }
}