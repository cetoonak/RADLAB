using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace RADLAB.WF.VB
{
    public partial class WFVBSuccess : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack && Request.Form.Count <= 0)
            {
                return;
            }

            // Forma Vakıfbanktan post ile gelen değerleri al

            Status.Value = Request.Form["Status"];
            MerchantId.Value = Request.Form["MerchantId"];
            VerifyEnrollmentRequestId.Value = Request.Form["VerifyEnrollmentRequestId"];
            Xid.Value = Request.Form["Xid"];
            PurchAmount.Value = Request.Form["PurchAmount"];
            Xid.Value = Request.Form["Xid"];
            SessionInfo.Value = Request.Form["SessionInfo"];
            PurchCurrency.Value = Request.Form["PurchCurrency"];
            Pan.Value = Request.Form["Pan"];
            ExpiryDate.Value = Request.Form["Expiry"];
            Eci.Value = Request.Form["Eci"];
            Cavv.Value = Request.Form["Cavv"];
            InstallmentCount.Value = Request.Form["InstallmentCount"];

            bool debug = false;

#if DEBUG
            debug = true;
#endif

            string CS = ConfigurationManager.ConnectionStrings["RadlabConnection"].ConnectionString;
            string WebSiteUrl = ConfigurationManager.ConnectionStrings[(debug ? "WebSiteUrlDebug" : "WebSiteUrlCanli")].ConnectionString;

            using (var connection = new SqlConnection(CS))
            {
                // SiparisAyarDTO değerlerini dbden al

                string QSiparisAyar = $@"SELECT
                                            Ayar.VakifBankMerchantPassword,
                                            Ayar.VakifBankTerminalNo,
	                                        S.*
                                        FROM
	                                        (SELECT
		                                        'Siparis' AS TableName,
		                                        CVV
	                                        FROM
		                                        Siparis
	                                        WHERE
		                                        VerifyEnrollmentRequestId = '{VerifyEnrollmentRequestId.Value}'

	                                        UNION ALL

	                                        SELECT
		                                        'Kalibrasyon' AS TableName,
		                                        CVV
	                                        FROM
		                                        Kalibrasyon
	                                        WHERE
		                                        VerifyEnrollmentRequestId = '{VerifyEnrollmentRequestId.Value}'

	                                        UNION ALL

	                                        SELECT
		                                        'KalibrasyonCihazOdeme' AS TableName,
		                                        CVV
	                                        FROM
		                                        KalibrasyonCihazOdeme
	                                        WHERE
		                                        VerifyEnrollmentRequestId = '{VerifyEnrollmentRequestId.Value}'

	                                        UNION ALL

	                                        SELECT
		                                        'Kursiyer' AS TableName,
		                                        CVV
	                                        FROM
		                                        Kursiyer
	                                        WHERE
		                                        VerifyEnrollmentRequestId = '{VerifyEnrollmentRequestId.Value}') AS S

	                                        LEFT JOIN Ayar ON 0 = 0";

                var dtoList = connection.Query<SiparisAyarDTO>(QSiparisAyar).ToList();

                XmlDocument xmlDoc = new XmlDocument();

                XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);

                XmlElement rootNode = xmlDoc.CreateElement("VposRequest");
                xmlDoc.InsertBefore(xmlDeclaration, xmlDoc.DocumentElement);
                xmlDoc.AppendChild(rootNode);

                //eklemek istediğiniz diğer elementleride bu şekilde ekleyebilirsiniz.
                XmlElement merchantNode = xmlDoc.CreateElement("MerchantId");
                XmlElement passwordNode = xmlDoc.CreateElement("Password");
                XmlElement terminalNode = xmlDoc.CreateElement("TerminalNo");
                XmlElement transactionTypeNode = xmlDoc.CreateElement("TransactionType");
                //XmlElement transactionIdNode = xmlDoc.CreateElement("TransactionId");
                XmlElement currencyAmountNode = xmlDoc.CreateElement("CurrencyAmount");
                XmlElement currencyCodeNode = xmlDoc.CreateElement("CurrencyCode");
                XmlElement panNode = xmlDoc.CreateElement("Pan");
                XmlElement cvvNode = xmlDoc.CreateElement("Cvv");
                XmlElement expiryNode = xmlDoc.CreateElement("Expiry");
                XmlElement eciNode = xmlDoc.CreateElement("ECI");
                XmlElement cavvNode = xmlDoc.CreateElement("CAVV");
                XmlElement mpiTransactionIdNode = xmlDoc.CreateElement("MpiTransactionId");
                XmlElement ClientIpNode = xmlDoc.CreateElement("ClientIp");
                XmlElement transactionDeviceSourceNode = xmlDoc.CreateElement("TransactionDeviceSource");

                //yukarıda eklediğimiz node lar için değerleri ekliyoruz.
                XmlText merchantText = xmlDoc.CreateTextNode(MerchantId.Value);
                XmlText passwordtext = xmlDoc.CreateTextNode(dtoList[0].VakifBankMerchantPassword);
                XmlText terminalNoText = xmlDoc.CreateTextNode(dtoList[0].VakifBankTerminalNo);
                XmlText transactionTypeText = xmlDoc.CreateTextNode("Sale");
                //XmlText transactionIdText = xmlDoc.CreateTextNode(VerifyEnrollmentRequestId.Value); //uniqe olacak şekilde düzenleyebilirsiniz.
                XmlText currencyAmountText = xmlDoc.CreateTextNode((Convert.ToDecimal(PurchAmount.Value) / 100).ToString("##0.#0").Replace(",", ".")); //tutarı nokta ile gönderdiğinizden emin olunuz.
                XmlText currencyCodeText = xmlDoc.CreateTextNode("949");
                XmlText panText = xmlDoc.CreateTextNode(Pan.Value);
                XmlText cvvText = xmlDoc.CreateTextNode(dtoList[0].CVV);
                XmlText expiryText = xmlDoc.CreateTextNode("20" + ExpiryDate.Value);
                XmlText eciText = xmlDoc.CreateTextNode(Eci.Value);
                XmlText cavvText = xmlDoc.CreateTextNode(Cavv.Value);
                XmlText mpiTransactionIdText = xmlDoc.CreateTextNode(VerifyEnrollmentRequestId.Value);
                XmlText ClientIpText = xmlDoc.CreateTextNode(GetIPAddress());
                XmlText transactionDeviceSourceText = xmlDoc.CreateTextNode("0");

                //nodeları root elementin altına ekliyoruz.
                rootNode.AppendChild(merchantNode);
                rootNode.AppendChild(passwordNode);
                rootNode.AppendChild(terminalNode);
                rootNode.AppendChild(transactionTypeNode);
                //rootNode.AppendChild(transactionIdNode);
                rootNode.AppendChild(currencyAmountNode);
                rootNode.AppendChild(currencyCodeNode);
                rootNode.AppendChild(panNode);
                rootNode.AppendChild(cvvNode);
                rootNode.AppendChild(expiryNode);
                rootNode.AppendChild(eciNode);
                rootNode.AppendChild(cavvNode);
                rootNode.AppendChild(mpiTransactionIdNode);
                rootNode.AppendChild(ClientIpNode);
                rootNode.AppendChild(transactionDeviceSourceNode);

                //nodelar için oluşturduğumuz textleri node lara ekliyoruz.
                merchantNode.AppendChild(merchantText);
                passwordNode.AppendChild(passwordtext);
                terminalNode.AppendChild(terminalNoText);
                transactionTypeNode.AppendChild(transactionTypeText);
                //transactionIdNode.AppendChild(transactionIdText);
                currencyAmountNode.AppendChild(currencyAmountText);
                currencyCodeNode.AppendChild(currencyCodeText);
                panNode.AppendChild(panText);
                cvvNode.AppendChild(cvvText);
                expiryNode.AppendChild(expiryText);
                eciNode.AppendChild(eciText);
                cavvNode.AppendChild(cavvText);
                mpiTransactionIdNode.AppendChild(mpiTransactionIdText);
                ClientIpNode.AppendChild(ClientIpText);
                transactionDeviceSourceNode.AppendChild(transactionDeviceSourceText);

                string xmlMessage = xmlDoc.OuterXml;

                //oluşturduğumuz xml i vposa gönderiyoruz.
                byte[] dataStream = Encoding.UTF8.GetBytes("prmstr=" + xmlMessage);
                HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create("https://onlineodeme.vakifbank.com.tr:4443/VposService/v3/Vposreq.aspx");//Vpos adresi
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

                if (string.IsNullOrEmpty(responseFromServer))
                {
                    return;
                }
                var xmlResponse = new XmlDocument();
                xmlResponse.LoadXml(responseFromServer);
                var resultCodeNode = xmlResponse.SelectSingleNode("VposResponse/ResultCode");
                var resultDescriptionNode = xmlResponse.SelectSingleNode("VposResponse/ResultDescription");
                string resultCode = "";
                string resultDescription = "";

                if (resultCodeNode != null)
                {
                    resultCode = resultCodeNode.InnerText;
                }
                if (resultDescriptionNode != null)
                {
                    resultDescription = resultDescriptionNode.InnerText;
                }

                // Servis sonucunu dbye kaydet ve sonuç sayfasına yönlendir

                string PosIslemSonucu = resultCode == "0000" ? "Ok" : resultCode + " - " + resultDescription;

                string QTahsilat = $@" UPDATE Siparis SET
	                                        TahsilatSekliId = 1,
	                                        PosIslemZamani = GETDATE(),
                                            PosIslemSonucu = '{PosIslemSonucu}'
                                        WHERE
	                                        VerifyEnrollmentRequestId = '{VerifyEnrollmentRequestId.Value}'

                                        UPDATE Kalibrasyon SET
	                                        TahsilatSekliId = 1,
	                                        PosIslemZamani = GETDATE(),
                                            PosIslemSonucu = '{PosIslemSonucu}'
                                        WHERE
	                                        VerifyEnrollmentRequestId = '{VerifyEnrollmentRequestId.Value}'

                                        UPDATE KalibrasyonCihazOdeme SET
	                                        TahsilatSekliId = 1,
	                                        PosIslemZamani = GETDATE(),
                                            PosIslemSonucu = '{PosIslemSonucu}'
                                        WHERE
	                                        VerifyEnrollmentRequestId = '{VerifyEnrollmentRequestId.Value}'

                                        UPDATE Kursiyer SET
	                                        TahsilatSekliId = 1,
	                                        PosIslemZamani = GETDATE(),
                                            PosIslemSonucu = '{PosIslemSonucu}'
                                        WHERE
	                                        VerifyEnrollmentRequestId = '{VerifyEnrollmentRequestId.Value}'";

                connection.ExecuteScalar(QTahsilat);

                Response.Redirect(WebSiteUrl + "/VBReturn/" + VerifyEnrollmentRequestId.Value);

                /*

                // Vakifbank servisine bazısı formdan, bazısı dbden gelen değerleri setle

                var client = new SRVakifBank.TransactionWebServicesSoapClient();

                var vposRequest = new VposRequest();

                vposRequest.MerchantId = MerchantId.Value;
                vposRequest.Password = dto.VakifBankMerchantPassword;
                vposRequest.TerminalNo = dto.VakifBankTerminalNo;
                vposRequest.Pan = Pan.Value;
                vposRequest.Expiry = "20" + ExpiryDate.Value;
                vposRequest.CurrencyAmount = Convert.ToDecimal(PurchAmount.Value) / 100;
                vposRequest.CurrencyCode = Convert.ToInt32(PurchCurrency.Value);
                vposRequest.TransactionType = "Sale";
                vposRequest.Cvv = dto.CVV;
                vposRequest.ECI = Eci.Value;
                vposRequest.CAVV = Cavv.Value;
                vposRequest.MpiTransactionId = VerifyEnrollmentRequestId.Value;
                vposRequest.ClientIp = GetIPAddress();
                vposRequest.TransactionDeviceSource = 0;

                var resultVPos = client.ExecuteVposRequest(vposRequest);

                // Servis sonucunu dbye kaydet ve sonuç sayfasına yönlendir

                string QTahsilat = "";

                if (resultVPos.ResultCode == "0000")
                {
                    QTahsilat = $@" UPDATE Siparis SET
	                                    TahsilatSekliId = 1,
	                                    PosIslemZamani = GETDATE(),
                                        PosIslemSonucu = 'OK'
                                    WHERE
	                                    VerifyEnrollmentRequestId = '{VerifyEnrollmentRequestId.Value}'";
                }
                else
                {
                    QTahsilat = $@" UPDATE Siparis SET
	                                    TahsilatSekliId = NULL,
	                                    PosIslemZamani = GETDATE(),
                                        PosIslemSonucu = '{resultVPos.ResultCode + " - " + resultVPos.ResultDetail}'
                                    WHERE
	                                    VerifyEnrollmentRequestId = '{VerifyEnrollmentRequestId.Value}'";
                }

                var resultTahsilat = connection.ExecuteScalar<string>(QTahsilat, dto);

                Response.Redirect(WebSiteUrl + "/VBReturn/" + VerifyEnrollmentRequestId.Value);*/
            };
        }

        public class SiparisAyarDTO
        {
            public string VakifBankMerchantPassword { get; set; } = string.Empty;
            public string VakifBankTerminalNo { get; set; } = string.Empty;
            public string TableName { get; set; } = string.Empty;
            public string CVV { get; set; } = string.Empty;
        }

        public string GetIPAddress()
        {
            string R = "";

            HttpContext context = HttpContext.Current;

            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');

                if (addresses.Length != 0)
                {
                    R = addresses[0];
                }
            }

            R = context.Request.ServerVariables["REMOTE_ADDR"];

            if (!string.IsNullOrEmpty(R) || R.Length <= 7)
            {
                R = "176.53.40.162";
            }

            return R;
        }
    }
}