using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using RADLAB.Business.Utils;
using RADLAB.Model.DTO;
using RADLAB.Model.ResponseModels;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using RADLAB.Business.Abstract;
using System.Data.SqlClient;
using Dapper;
using System.Xml;

namespace RADLAB.Business.Concrete
{
    public class VakifBankManager : IVakifBankManager
    {
        private readonly IConfiguration configuration;
        private readonly string CS;
        public string EnrollmentSuccessUrlBase { get; set; }
        public string EnrollmentFailureUrlBase { get; set; }
        private readonly bool debug = false;

        public VakifBankManager(IConfiguration _configuration)
        {
#if DEBUG
            debug = true;
#endif

            configuration = _configuration;

            CS = configuration.GetConnectionString(debug ? "Debug" : "Release");

            EnrollmentSuccessUrlBase = configuration.GetSection($"VakifBank:EnrollmentSuccessUrl{(debug ? "Debug" : "Release")}").Value;
            EnrollmentFailureUrlBase = configuration.GetSection($"VakifBank:EnrollmentFailureUrl{(debug ? "Debug" : "Release")}").Value;
        }

        public async Task<ServiceResponse<VakifBankMPIDTO>> Enrollment(SiparisDTO dto, int KisiId)
        {
            var R = new ServiceResponse<VakifBankMPIDTO>();

            var vakifBankMPIDTO = new VakifBankMPIDTO();

            try
            {
                using var connection = new SqlConnection(CS);

                string Q = $@"  SELECT TOP 1
                                    VakifBankMerchantId,
                                    VakifBankMerchantPassword,
                                    VakifBankTerminalNo,
                                    VakifBankEnrollmentUrl
                                FROM
                                    Ayar";

                var ayarVakifBankDTO = await connection.QuerySingleOrDefaultAsync<AyarVakifBankDTO>(Q);

                string EnrollmentSuccessUrl = EnrollmentSuccessUrlBase + "/VB/WFVBSuccess";
                string EnrollmentFailureUrl = EnrollmentFailureUrlBase  + "/VBEnrollmentFailure";

                string Tutar = dto.Tutar.ToString("##0.#0").Replace(",", ".");

                string data = $"Pan={dto.KrediKartiNo.Replace(" ", "")}&ExpiryDate={dto.KrediKartiSonKullanimYil.PadLeft(2, '0')}{dto.KrediKartiSonKullanimAy.PadLeft(2, '0')}&PurchaseAmount={Tutar}&Currency=949&BrandName={dto.KrediKartiBrandName}&VerifyEnrollmentRequestId={dto.VerifyEnrollmentRequestId}&SessionInfo=&MerchantID={ayarVakifBankDTO.VakifBankMerchantId}&MerchantPassword={ayarVakifBankDTO.VakifBankMerchantPassword}&SuccessUrl={EnrollmentSuccessUrl}&FailureUrl={EnrollmentFailureUrl}&InstallmentCount=";
                byte[] dataStream = Encoding.UTF8.GetBytes(data);
                HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(ayarVakifBankDTO.VakifBankEnrollmentUrl);
                webRequest.Method = "POST";
                webRequest.ContentType = "application/x-www-form-urlencoded";
                webRequest.ContentLength = dataStream.Length;
                webRequest.KeepAlive = false;
                string responseFromServer = "";

                using (Stream newStream = webRequest.GetRequestStream())
                {
                    newStream.Write(dataStream, 0, dataStream.Length);
                    newStream.Close();
                }

                using (WebResponse webResponse = webRequest.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(webResponse.GetResponseStream()))
                    {
                        responseFromServer = reader.ReadToEnd();
                        reader.Close();
                    }

                    webResponse.Close();
                }

                if (string.IsNullOrEmpty(responseFromServer) == false)
                {
                    var xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(responseFromServer);

                    var statusNode = xmlDocument.SelectSingleNode("IPaySecure/Message/VERes/Status");
                    var pareqNode = xmlDocument.SelectSingleNode("IPaySecure/Message/VERes/PaReq");
                    var acsUrlNode = xmlDocument.SelectSingleNode("IPaySecure/Message/VERes/ACSUrl");
                    var termUrlNode = xmlDocument.SelectSingleNode("IPaySecure/Message/VERes/TermUrl");
                    var mdNode = xmlDocument.SelectSingleNode("IPaySecure/Message/VERes/MD");
                    var messageErrorCodeNode = xmlDocument.SelectSingleNode("IPaySecure/MessageErrorCode");

                    string statusText = "";

                    if (statusNode != null)
                    {
                        statusText = statusNode.InnerText;
                    }

                    //3d secure programına dahil
                    if (statusText == "Y")
                    {
                        string postBackForm = @"<html>
                                                      <head>
                                                        <meta name='viewport' content='width=device-width' />
                                                        <title>MpiForm</title>
                                                        <script>
                                                            function postPage() {
                                                                document.forms['frmMpiForm'].submit();
                                                            }
                                                        </script>
                                                      </head>
                                                      <body onload='javascript:postPage();'>
                                                        <form action='@ACSUrl' method='post' id='frmMpiForm' name='frmMpiForm'>
                                                          <input type='hidden' name='PaReq' value='@PAReq' />
                                                          <input type='hidden' name='TermUrl' value='@TermUrl' />
                                                          <input type='hidden' name='MD' value='@MD' />
                                                          <noscript>
                                                            <input type='submit' id='btnSubmit' value='Gönder' />
                                                          </noscript>
                                                        </form>
                                                      </body>
                                                    </html>";

                        postBackForm = postBackForm.Replace("@ACSUrl", acsUrlNode.InnerText);
                        postBackForm = postBackForm.Replace("@PAReq", pareqNode.InnerText);
                        postBackForm = postBackForm.Replace("@TermUrl", termUrlNode.InnerText);
                        postBackForm = postBackForm.Replace("@MD", mdNode.InnerText);

                        vakifBankMPIDTO.PostBackForm = postBackForm;

                        vakifBankMPIDTO.Status = statusText;
                        vakifBankMPIDTO.PAReq = pareqNode.InnerText;
                        vakifBankMPIDTO.ACSUrl = acsUrlNode.InnerText;
                        vakifBankMPIDTO.TermUrl = termUrlNode.InnerText;
                        vakifBankMPIDTO.MD = mdNode.InnerText;
                        vakifBankMPIDTO.ErrorCode = messageErrorCodeNode.InnerText;

                        R.Value = vakifBankMPIDTO;
                    }
                    else if (statusText == "E")
                    {
                        R.Success = false;
                        R.Message = messageErrorCodeNode.InnerText;
                    }
                }
                else
                {
                    R.Success = false;
                    R.Message = "responseFromServer bos";
                }
            }
            catch (Exception ex)
            {
                R.Success = false;
                R.Message = ex.Message;
            }

            var LogReturn = await IslemLog.Logla(configuration, KisiId, MethodBase.GetCurrentMethod().DeclaringType, R.Message, dto, 0);

            if (!R.Success) R.Message = LogReturn;

            return R;
        }
    }
}