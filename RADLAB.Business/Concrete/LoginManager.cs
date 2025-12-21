using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RADLAB.Business.Abstract;
using RADLAB.Business.Utils;
using RADLAB.Data.Context;
using RADLAB.Model.DTO;
using RADLAB.Model.Models;
using RADLAB.Model.ResponseModels;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;

namespace RADLAB.Business.Concrete
{
    public class LoginManager : ILoginManager
    {
        private readonly RADLABDBContext context;
        private readonly IConfiguration configuration;
        private readonly string CS;

        public LoginManager(RADLABDBContext _context, IConfiguration _configuration)
        {            
            bool debug = false;
#if DEBUG
            debug = true;
#endif

            context = _context;
            configuration = _configuration;

            CS = configuration.GetConnectionString(debug ? "Debug" : "Release");
        }

        public async Task<ServiceResponse<AuthenticatedUserDTO>> Login(LoginDTO loginDTO)
        {
            var R = new ServiceResponse<AuthenticatedUserDTO>();

            try
            {
                using var connection = new SqlConnection(CS);

                string Q = "";

                if (loginDTO.LoginTipi == 1) // sistem kullanicisi
                {
                    Q = $"SELECT Id, Tip, Ad, Soyad, EMail, Password, TelefonCep, Aktif FROM Kisi WHERE Tip = 1 AND EMail = '{loginDTO.Mail}'";
                }
                else if (loginDTO.LoginTipi == 2) // online egitim ogrencisi
                {
                    Q = $"SELECT Id, Tip, Ad, Soyad, EMail, Password, TelefonCep, Aktif FROM Kisi WHERE Tip = 2 AND TelefonCep = '{loginDTO.TelefonCep}'";
                }

                var kisi = await connection.QuerySingleOrDefaultAsync<KisiModel>(Q);

                if (kisi == null || !kisi.Aktif)
                {
                    R.Success = false;
                    R.Message = "Kullanıcı giriş bilgileri hatalı";
                }
                else
                {
                    bool verified = false;

                    if (loginDTO.LoginTipi == 1) // sistem kullanicisi
                    {
                        var encryptedPassword = PasswordEncrypter.Encrypt(loginDTO.Password);

                        if (kisi.Password == encryptedPassword) verified = true;
                    }
                    else if (loginDTO.LoginTipi == 2) // online egitim ogrencisi
                    {
                        if (kisi.Id > 0) verified = true;
                    }

                    if (verified)
                    {
                        var result = new AuthenticatedUserDTO();

                        result.Mail = loginDTO.Mail;
                        result.AdSoyad = kisi.Ad + " " + kisi.Soyad;
                        result.Unvan = "...";
                        result.TelefonCep = kisi.TelefonCep;

                        if (loginDTO.TokenAl)
                        {
                            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSecurityKey"]));
                            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                            var expiry = DateTime.Now.AddDays(int.Parse(configuration["JwtExpiryInDays"].ToString()));

                            var claims = new[]
                            {
                                new Claim(ClaimTypes.Email, loginDTO.Mail),
                                new Claim(ClaimTypes.Name, kisi.Ad + " " + kisi.Soyad),
                                new Claim(ClaimTypes.UserData, kisi.Id.ToString())
                            };

                            var token = new JwtSecurityToken(configuration["JwtIssuer"], configuration["JwtAudience"], claims, null, expiry, creds);

                            result.Token = new JwtSecurityTokenHandler().WriteToken(token);
                        }

                        R.Value = result;
                    }
                    else
                    {
                        R.Success = false;
                        R.Message = "Kullanıcı giriş bilgileri hatalı";
                    }
                }
            }
            catch (Exception ex)
            {
                R.Success = false;
                R.Message = ex.Message;
            }

            return R;
        }

        public async Task<ServiceResponse<AuthenticatedUserDTO>> GetKisiMenu(int Id)
        {
            var R = new ServiceResponse<AuthenticatedUserDTO>();

            try
            {
                using var connection = new SqlConnection(CS);

                var kisi = await connection.QuerySingleOrDefaultAsync<KisiModel>($"SELECT Id, Tip, Ad, Soyad, EMail, Password, Aktif FROM Kisi WHERE Id = {Id}");

                string QMenuBasliklar = "";
                string QMenuLinkler = "";

                if (kisi.Tip == 1)
                {
                    QMenuBasliklar = @$"SELECT DISTINCT
	                                        YetkiUst.Id,
	                                        YetkiUst.Yetki AS Ad,
                                            YetkiUst.Icon
                                        FROM
	                                        KisiRol
	                                        LEFT JOIN Rol ON KisiRol.RolId = Rol.Id
	                                        LEFT JOIN RolYetki ON Rol.Id = RolYetki.RolId
	                                        LEFT JOIN Yetki ON RolYetki.YetkiId = Yetki.Id
	                                        LEFT JOIN Yetki YetkiOrta ON Yetki.ParentId = YetkiOrta.Id
	                                        LEFT JOIN Yetki YetkiUst ON YetkiOrta.ParentId = YetkiUst.Id
                                        WHERE
	                                        KisiRol.KisiId = {Id}
	                                        AND YetkiUst.Yetki != 'Dashboard'";

                    QMenuLinkler = @$"SELECT DISTINCT
	                                    YetkiOrta.Id,
	                                    YetkiOrta.ParentId,
	                                    YetkiOrta.Yetki AS Ad,
	                                    YetkiOrta.Link,
                                        YetkiOrta.Icon,
                                        YetkiOrta.Favori
                                    FROM
	                                    KisiRol
	                                    LEFT JOIN Rol ON KisiRol.RolId = Rol.Id
	                                    LEFT JOIN RolYetki ON Rol.Id = RolYetki.RolId
	                                    LEFT JOIN Yetki ON RolYetki.YetkiId = Yetki.Id
	                                    LEFT JOIN Yetki YetkiOrta ON Yetki.ParentId = YetkiOrta.Id
                                    WHERE
	                                    KisiRol.KisiId = {Id}";
                }
                else
                {
                    QMenuBasliklar = $@"SELECT
	                                        1 AS Id,
	                                        'Eğitimler' AS Ad,
                                            'bi bi-play-circle' AS Icon";

                    QMenuLinkler = $@"SELECT
	                                        0 - OnlineEgitimOgrenci.Id AS Id,
	                                        1 AS ParentId,
	                                        OnlineEgitim.OnlineEgitim AS Ad,
	                                        'Index/' + CONVERT(VARCHAR(10), OnlineEgitimOgrenci.Id) AS Link,
	                                        '' AS Icon,
	                                        CONVERT(BIT, 0) AS Favori
                                        FROM
	                                        OnlineEgitimOgrenci
	                                        LEFT JOIN Kisi ON OnlineEgitimOgrenci.KisiId = Kisi.Id
	                                        LEFT JOIN OnlineEgitim ON OnlineEgitimOgrenci.OnlineEgitimId = OnlineEgitim.Id
                                        WHERE
	                                        Kisi.Tip = 2
	                                        AND OnlineEgitimOgrenci.KisiId = {Id}
                                            AND CONVERT(DATE, GETDATE()) BETWEEN OnlineEgitimOgrenci.Tarih1 AND OnlineEgitimOgrenci.Tarih2";

                    /*string rol = "Online Eğitim Öğrenci";

                    QMenuBasliklar = @$"SELECT DISTINCT
	                                        YetkiUst.Id,
	                                        YetkiUst.Yetki AS Ad,
                                            YetkiUst.Icon
                                        FROM
	                                        RolYetki
											LEFT JOIN Rol ON RolYetki.RolId = Rol.Id
	                                        LEFT JOIN Yetki ON RolYetki.YetkiId = Yetki.Id
	                                        LEFT JOIN Yetki YetkiOrta ON Yetki.ParentId = YetkiOrta.Id
	                                        LEFT JOIN Yetki YetkiUst ON YetkiOrta.ParentId = YetkiUst.Id
                                        WHERE
	                                        Rol.Rol = '{rol}'
	                                        AND YetkiUst.Yetki != 'Dashboard'";

                    QMenuLinkler = @$"  SELECT DISTINCT
	                                        YetkiOrta.Id,
	                                        YetkiOrta.ParentId,
	                                        YetkiOrta.Yetki AS Ad,
	                                        YetkiOrta.Link,
                                            YetkiOrta.Icon,
                                            YetkiOrta.Favori
                                        FROM
	                                        RolYetki
										    LEFT JOIN Rol ON RolYetki.RolId = Rol.Id
	                                        LEFT JOIN Yetki ON RolYetki.YetkiId = Yetki.Id
	                                        LEFT JOIN Yetki YetkiOrta ON Yetki.ParentId = YetkiOrta.Id
                                        WHERE
	                                        Rol.Rol = '{rol}'";*/
                }

                var MenuBasliklar = await connection.QueryAsync<MenuBaslikDTO>(QMenuBasliklar);

                var MenuLinkler = await connection.QueryAsync<MenuLinkDTO>(QMenuLinkler);

                var result = new AuthenticatedUserDTO();

                if (kisi != null)
                {
                    result.Mail = kisi.EMail;
                    result.AdSoyad = kisi.Ad + " " + kisi.Soyad;
                    result.Unvan = "Müdür (geçici)";
                    result.MenuBasliklar = MenuBasliklar.ToList();
                    result.MenuLinkler = MenuLinkler.ToList();
                }

                R.Value = result;
            }
            catch (Exception ex)
            {
                R.Success = false;
                R.Message = ex.Message;
            }

            return R;
        }

        public async Task<ServiceResponse<string>> GetAktivasyonKoduByMail(string mail)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                string Q = $@"  IF EXISTS(SELECT Id FROM Kisi WHERE EMail = '{mail}')
                                BEGIN
	                                DECLARE @AktivasyonKodu VARCHAR(50)

	                                SELECT @AktivasyonKodu = NEWID()

	                                UPDATE Kisi SET
		                                AktivasyonKodu = @AktivasyonKodu
	                                WHERE
		                                EMail = '{mail}'

	                                SELECT @AktivasyonKodu
                                END ELSE
                                BEGIN
	                                SELECT 'KayitYok'
                                END";

                var result = await connection.ExecuteScalarAsync<string>(Q);

                R.Value = result;
            }
            catch (Exception ex)
            {
                R.Success = false;
                R.Message = ex.Message;
            }

            if (!R.Success)
            {
                var logReturn = await IslemLog.Logla(configuration, 0, MethodBase.GetCurrentMethod().DeclaringType, R.Message, mail, 0);
                R.Message = logReturn;
            }

            return R;
        }

        public async Task<ServiceResponse<string>> UpdatePasswordByAktivasyonKodu(LoginDTO dto)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                string encryptedPassword = PasswordEncrypter.Encrypt(dto.Password);

                string Q = $@"  IF EXISTS(SELECT Id FROM Kisi WHERE EMail = @Mail AND AktivasyonKodu = @AktivasyonKodu)
                                BEGIN
	                                UPDATE Kisi SET
		                                Password = '{encryptedPassword}'
	                                WHERE
                                        EMail = @Mail
		                                AND AktivasyonKodu = @AktivasyonKodu

	                                SELECT ''
                                END ELSE
                                BEGIN
	                                SELECT 'KayitYok'
                                END";

                var result = await connection.ExecuteScalarAsync<string>(Q, dto);

                R.Value = result;
            }
            catch (Exception ex)
            {
                R.Success = false;
                R.Message = ex.Message;
            }

            if (!R.Success)
            {
                var logReturn = await IslemLog.Logla(configuration, 0, MethodBase.GetCurrentMethod().DeclaringType, R.Message, dto, 0);
                R.Message = logReturn;
            }

            return R;
        }
    }
}