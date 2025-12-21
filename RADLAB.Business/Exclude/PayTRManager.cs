using Dapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using RADLAB.Business.Utils;
using RADLAB.Model.DTO;
using RADLAB.Model.ResponseModels;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace RADLAB.Business.Exclude
{
    public class PayTRManager : IPayTRManager
    {
        private readonly IConfiguration configuration;
        private readonly string CS;
        private readonly bool debug = false;

        public PayTRManager(IConfiguration _configuration)
        {
#if DEBUG
            debug = true;
#endif

            configuration = _configuration;

            CS = configuration.GetConnectionString(debug ? "Debug" : "Release");
        }

        public async Task<ServiceResponse<string>> PayTRAdim1(PayTRDTO dto, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                string Q = $@"  SELECT TOP 1
                                    PayTRMerchantId,
	                                PayTRMerchantKey,
	                                PayTRMerchantSalt
                                FROM
                                    Ayar";

                var ayarPayTR = await connection.QuerySingleOrDefaultAsync<AyarPayTRDTO>(Q);

                // !!! Siparişi onaylayacağız sayfa "Bildirim URL" sayfasıdır (Bakınız: 2.ADIM Klasörü).
                // !!! Siparişi iptal edeceğiniz sayfa "Bildirim URL" sayfasıdır (Bakınız: 2.ADIM Klasörü).

                string merchant_id = ayarPayTR.PayTRMerchantId;
                string merchant_key = ayarPayTR.PayTRMerchantKey;
                string merchant_salt = ayarPayTR.PayTRMerchantSalt;

                string emailstr = dto.emailstr; // Müşterinizin sitenizde kayıtlı veya form vasıtasıyla aldığınız eposta adresi

                int payment_amountstr = dto.payment_amountstr; // Tahsil edilecek tutar. 9.99 için 9.99 * 100 = 999 gönderilmelidir.

                string merchant_oid = dto.merchant_oid; // Sipariş numarası: Her işlemde benzersiz olmalıdır!! Bu bilgi bildirim sayfanıza yapılacak bildirimde geri gönderilir.

                string user_ip = debug ? "88.240.90.155" : "176.53.40.162"; // !!! Eğer bu örnek kodu sunucuda değil local makinanızda çalıştırıyorsanız buraya dış ip adresinizi (https://www.whatismyip.com/) yazmalısınız. Aksi halde geçersiz paytr_token hatası alırsınız.

                var user_basket = new List<object>();

                foreach (var item in dto.UserBasketList)
                {
                    user_basket.Add(new object[] { item.Urun, item.Fiyat, item.Miktar });
                }

                var xx = user_basket.ToArray();

                /*object[][] user_basket = {                    
                    new object[] {"TRKD LEY11", "30", 1},
                    new object[] {"TRKD LEY20", "20", 2},
                    new object[] {"TRKD LEY25", "30", 1},
                };*/

                string test_mode = "1"; // Mağaza canlı modda iken test işlem yapmak için 1 olarak gönderilebilir.

                string no_installment = "0"; // Taksit yapılmasını istemiyorsanız, sadece tek çekim sunacaksanız 1 yapın

                string max_installment = "0"; // Sayfada görüntülenecek taksit adedini sınırlamak istiyorsanız uygun şekilde değiştirin. Sıfır (0) gönderilmesi durumunda yürürlükteki en fazla izin verilen taksit geçerli olur.

                string currency = "TL"; // Para birimi olarak TL, EUR, USD gönderilebilir. USD ve EUR kullanmak için kurumsal@paytr.com üzerinden bilgi almanız gerekmektedir. Boş gönderilirse TL geçerli olur.

                // Gönderilecek veriler oluşturuluyor

                NameValueCollection data = new NameValueCollection();
                data["merchant_id"] = merchant_id;
                data["user_ip"] = user_ip;
                data["merchant_oid"] = merchant_oid;
                data["email"] = emailstr;
                data["payment_amount"] = payment_amountstr.ToString();

                // Sepet içerği oluşturma fonksiyonu, değiştirilmeden kullanılabilir.
                string user_basket_json = JsonConvert.SerializeObject(user_basket);
                string user_basketstr = Convert.ToBase64String(Encoding.UTF8.GetBytes(user_basket_json));
                data["user_basket"] = user_basketstr;

                // Token oluşturma fonksiyonu, değiştirilmeden kullanılmalıdır.
                string Birlestir = string.Concat(merchant_id, user_ip, merchant_oid, emailstr, payment_amountstr.ToString(), user_basketstr, no_installment, max_installment, currency, test_mode, merchant_salt);
                HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(merchant_key));
                byte[] b = hmac.ComputeHash(Encoding.UTF8.GetBytes(Birlestir));
                data["paytr_token"] = Convert.ToBase64String(b);

                data["debug_on"] = "1"; // Hata mesajlarının ekrana basılması için entegrasyon ve test sürecinde 1 olarak bırakın. Daha sonra 0 yapabilirsiniz.
                data["test_mode"] = test_mode;
                data["no_installment"] = no_installment;
                data["max_installment"] = max_installment;
                data["user_name"] = dto.user_name; // Müşterinizin sitenizde kayıtlı veya form aracılığıyla aldığınız ad ve soyad bilgisi
                data["user_address"] = dto.user_address; // Müşterinizin sitenizde kayıtlı veya form aracılığıyla aldığınız adres bilgisi
                data["user_phone"] = dto.user_phone; // Müşterinizin sitenizde kayıtlı veya form aracılığıyla aldığınız telefon bilgisi
                data["merchant_ok_url"] = (debug ? "http://localhost:14445/" : "https://test.radlab.com.tr/") + "SepetOk/" + dto.Id.ToString(); // Başarılı ödeme sonrası müşterinizin yönlendirileceği sayfa !!! Bu sayfa siparişi onaylayacağınız sayfa değildir! Yalnızca müşterinizi bilgilendireceğiniz sayfadır!
                data["merchant_fail_url"] = (debug ? "http://localhost:14445/" : "https://test.radlab.com.tr/") + "SepetFail/" + dto.Id.ToString(); // Ödeme sürecinde beklenmedik bir hata oluşması durumunda müşterinizin yönlendirileceği sayfa !!! Bu sayfa siparişi iptal edeceğiniz sayfa değildir! Yalnızca müşterinizi bilgilendireceğiniz sayfadır!
                data["timeout_limit"] = "30"; // İşlem zaman aşımı süresi - dakika cinsinden
                data["currency"] = currency;
                data["lang"] = "";

                using (WebClient client = new WebClient())
                {
                    client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                    byte[] result = client.UploadValues("https://www.paytr.com/odeme/api/get-token", "POST", data);
                    string ResultAuthTicket = Encoding.UTF8.GetString(result);
                    dynamic json = JToken.Parse(ResultAuthTicket);

                    if (json.status == "success")
                    {
                        R.Value = json.token;
                    }
                    else
                    {
                        R.Success = false;
                        R.Message = json.reason;
                    }
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

        public async Task<ServiceResponse<string>> PayTRAdim2(PayTRDTO dto, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                string Q = $@"  SELECT TOP 1
                                    PayTRMerchantId,
	                                PayTRMerchantKey,
	                                PayTRMerchantSalt
                                FROM
                                    Ayar";

                var ayarPayTR = await connection.QuerySingleOrDefaultAsync<AyarPayTRDTO>(Q);

                string merchant_id = ayarPayTR.PayTRMerchantId;
                string merchant_key = ayarPayTR.PayTRMerchantKey;
                string merchant_salt = ayarPayTR.PayTRMerchantSalt;

                string emailstr = dto.emailstr; // Müşterinizin sitenizde kayıtlı veya form vasıtasıyla aldığınız eposta adresi

                int payment_amountstr = dto.payment_amountstr; // Tahsil edilecek tutar. 9.99 için 9.99 * 100 = 999 gönderilmelidir.

                string merchant_oid = dto.merchant_oid; // Sipariş numarası: Her işlemde benzersiz olmalıdır!! Bu bilgi bildirim sayfanıza yapılacak bildirimde geri gönderilir.

                string user_ip = debug ? "88.240.90.155" : "176.53.40.162"; // !!! Eğer bu örnek kodu sunucuda değil local makinanızda çalıştırıyorsanız buraya dış ip adresinizi (https://www.whatismyip.com/) yazmalısınız. Aksi halde geçersiz paytr_token hatası alırsınız.

                var user_basket = new List<object>();

                foreach (var item in dto.UserBasketList)
                {
                    user_basket.Add(new object[] { item.Urun, item.Fiyat, item.Miktar });
                }

                var xx = user_basket.ToArray();

                /*object[][] user_basket = {                    
                    new object[] {"TRKD LEY11", "30", 1},
                    new object[] {"TRKD LEY20", "20", 2},
                    new object[] {"TRKD LEY25", "30", 1},
                };*/

                string test_mode = "1"; // Mağaza canlı modda iken test işlem yapmak için 1 olarak gönderilebilir.

                string no_installment = "0"; // Taksit yapılmasını istemiyorsanız, sadece tek çekim sunacaksanız 1 yapın

                string max_installment = "0"; // Sayfada görüntülenecek taksit adedini sınırlamak istiyorsanız uygun şekilde değiştirin. Sıfır (0) gönderilmesi durumunda yürürlükteki en fazla izin verilen taksit geçerli olur.

                string currency = "TL"; // Para birimi olarak TL, EUR, USD gönderilebilir. USD ve EUR kullanmak için kurumsal@paytr.com üzerinden bilgi almanız gerekmektedir. Boş gönderilirse TL geçerli olur.

                // Gönderilecek veriler oluşturuluyor

                NameValueCollection data = new NameValueCollection();
                data["merchant_id"] = merchant_id;
                data["user_ip"] = user_ip;
                data["merchant_oid"] = merchant_oid;
                data["email"] = emailstr;
                data["payment_amount"] = payment_amountstr.ToString();

                // Sepet içerği oluşturma fonksiyonu, değiştirilmeden kullanılabilir.
                string user_basket_json = JsonConvert.SerializeObject(user_basket);
                string user_basketstr = Convert.ToBase64String(Encoding.UTF8.GetBytes(user_basket_json));
                data["user_basket"] = user_basketstr;

                // Token oluşturma fonksiyonu, değiştirilmeden kullanılmalıdır.
                string Birlestir = string.Concat(merchant_id, user_ip, merchant_oid, emailstr, payment_amountstr.ToString(), user_basketstr, no_installment, max_installment, currency, test_mode, merchant_salt);
                HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(merchant_key));
                byte[] b = hmac.ComputeHash(Encoding.UTF8.GetBytes(Birlestir));
                data["paytr_token"] = Convert.ToBase64String(b);

                data["debug_on"] = "1"; // Hata mesajlarının ekrana basılması için entegrasyon ve test sürecinde 1 olarak bırakın. Daha sonra 0 yapabilirsiniz.
                data["test_mode"] = test_mode;
                data["no_installment"] = no_installment;
                data["max_installment"] = max_installment;
                data["user_name"] = dto.user_name; // Müşterinizin sitenizde kayıtlı veya form aracılığıyla aldığınız ad ve soyad bilgisi
                data["user_address"] = dto.user_address; // Müşterinizin sitenizde kayıtlı veya form aracılığıyla aldığınız adres bilgisi
                data["user_phone"] = dto.user_phone; // Müşterinizin sitenizde kayıtlı veya form aracılığıyla aldığınız telefon bilgisi
                data["merchant_ok_url"] = (debug ? "http://localhost:14445/" : "https://test.radlab.com.tr/") + "SepetOk/" + dto.Id.ToString(); // Başarılı ödeme sonrası müşterinizin yönlendirileceği sayfa !!! Bu sayfa siparişi onaylayacağınız sayfa değildir! Yalnızca müşterinizi bilgilendireceğiniz sayfadır!
                data["merchant_fail_url"] = (debug ? "http://localhost:14445/" : "https://test.radlab.com.tr/") + "SepetFail/" + dto.Id.ToString(); // Ödeme sürecinde beklenmedik bir hata oluşması durumunda müşterinizin yönlendirileceği sayfa !!! Bu sayfa siparişi iptal edeceğiniz sayfa değildir! Yalnızca müşterinizi bilgilendireceğiniz sayfadır!
                data["timeout_limit"] = "30"; // İşlem zaman aşımı süresi - dakika cinsinden
                data["currency"] = currency;
                data["lang"] = "";

                using (WebClient client = new WebClient())
                {
                    client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                    byte[] result = client.UploadValues("https://www.paytr.com/odeme/api/get-token", "POST", data);
                    string ResultAuthTicket = Encoding.UTF8.GetString(result);
                    dynamic json = JToken.Parse(ResultAuthTicket);

                    if (json.status == "success")
                    {
                        R.Value = json.token;
                    }
                    else
                    {
                        R.Success = false;
                        R.Message = json.reason;
                    }
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

        public async Task<ServiceResponse<string>> UpdateSiparisPayTRToken(SiparisDTO dto, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                var result = await connection.ExecuteScalarAsync<string>($"UPDATE Siparis SET PayTRToken = '{dto.PayTRToken}', MerchantOID = '{dto.MerchantOID}' WHERE Id = {dto.Id} SELECT {dto.Id}", dto);

                R.Value = result;
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

        public async Task<ServiceResponse<string>> UpdateSiparisPayTROdemeTamam(string MerchantOID, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                var result = await connection.ExecuteScalarAsync<string>($"UPDATE Siparis SET PayTROdemeTamam = 1 WHERE MerchantOID = {MerchantOID} SELECT {MerchantOID}");

                R.Value = result;
            }
            catch (Exception ex)
            {
                R.Success = false;
                R.Message = ex.Message;
            }

            var LogReturn = await IslemLog.Logla(configuration, KisiId, MethodBase.GetCurrentMethod().DeclaringType, R.Message, MerchantOID, 0);

            if (!R.Success) R.Message = LogReturn;

            return R;
        }

        public async Task<string> GetSiparisPayTRToken(int Id, int KisiId)
        {
            using var connection = new SqlConnection(CS);

            return await connection.QuerySingleOrDefaultAsync<string>($"SELECT PayTRToken FROM Siparis WHERE Id = {Id}");
        }
    }
}