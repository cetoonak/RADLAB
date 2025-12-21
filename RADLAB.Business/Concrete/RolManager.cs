using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RADLAB.Model.DTO;
using RADLAB.Business.Abstract;
using Microsoft.Extensions.Configuration;
using RADLAB.Model.ResponseModels;
using RADLAB.Business.Utils;
using System.Reflection;
using Newtonsoft.Json;
using RADLAB.Model.ParamDTO;
using System.Text;

namespace RADLAB.Business.Concrete
{
    public class RolManager : IRolManager
    {
        //private readonly RADLABDBContext context;
        private readonly IConfiguration configuration;
        private readonly string CS;

        public RolManager(/*RADLABDBContext _context,*/ IConfiguration _configuration)
        {
            bool debug = false;
#if DEBUG
            debug = true;
#endif

            //context = _context;
            configuration = _configuration;

            CS = configuration.GetConnectionString(debug ? "Debug" : "Release");
        }

        public async Task<RolDTO> GetRol(int Id)
        {
            using var connection = new SqlConnection(CS);

            var rol = await connection.QuerySingleOrDefaultAsync<RolDTO>($"SELECT * FROM Rol WHERE Id = {Id}");

            rol.Yetkiler = GetYetkiListVerilenByRolId(Id).Result;

            return rol;
        }

        public async Task<List<RolDTO>> GetRolList()
        {
            using var connection = new SqlConnection(CS);

            var result = await connection.QueryAsync<RolDTO>("SELECT * FROM Rol ORDER BY Rol");

            return result.ToList();
        }

        public async Task<List<YetkiDTO>> GetYetkiList()
        {
            using var connection = new SqlConnection(CS);

            var result = await connection.QueryAsync<YetkiDTO>("SELECT Id, Yetki FROM Yetki WHERE Id > 10000 ORDER BY Id");

            return result.ToList();
        }

        public async Task<List<YetkiDTO>> GetYetkiListVerilmeyenByRolId(int RolId)
        {
            using var connection = new SqlConnection(CS);

            string Q = $@"SELECT
	                            *
                            FROM
	                            Yetki
                            WHERE
	                            ISNULL(ParentId, 0) > 100
	                            AND Id NOT IN (SELECT
						                            YetkiId
					                            FROM
						                            RolYetki
					                            WHERE
						                            RolId = {RolId})
                            ORDER BY
	                            Id";

            var result = await connection.QueryAsync<YetkiDTO>(Q);

            return result.ToList();
        }

        public async Task<List<YetkiDTO>> GetYetkiListVerilenByRolId(int RolId)
        {
            using var connection = new SqlConnection(CS);

            string Q = $@"SELECT
	                            Yetki.Id,
	                            Yetki.Yetki
                            FROM
	                            RolYetki
	                            LEFT JOIN Yetki ON RolYetki.YetkiId = Yetki.Id
                            WHERE
                                RolYetki.RolId = {RolId.ToString()}
                            ORDER BY
                                Yetki.Id";

            var result = await connection.QueryAsync<YetkiDTO>(Q);

            return result.ToList();
        }

        public async Task<ServiceResponse<string>> InsertOrUpdateRol(RolDTO dto, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                var dtoParam = JsonConvert.DeserializeObject<RolParamDTO>(JsonConvert.SerializeObject(dto));
                dtoParam.YetkiIdler = string.Join("_", dtoParam.Yetkiler.Select(x => x.Id).ToList());

                string Q = $@"  IF @Id = 0
                                BEGIN
                                    INSERT INTO Rol
                                        (Rol)
                                    VALUES
                                        (@Rol)

                                    SELECT @Id = @@IDENTITY
                                END ELSE
                                BEGIN
                                    UPDATE Rol SET
                                        Rol = @Rol
                                    WHERE
                                        Id = @Id

                                    DELETE FROM RolYetki WHERE RolId = @Id
                                END

                                IF @YetkiIdler != ''
                                BEGIN
                                    INSERT INTO RolYetki
                                        (RolId, YetkiId)
                                    SELECT
                                        @Id AS RolId,
                                        Id AS YetkiId
                                    FROM
                                        dbo.FIdList(@YetkiIdler)
                                END

                                SELECT @Id";

                var result = await connection.ExecuteScalarAsync<string>(Q, dtoParam);

                R.Value = result;
            }
            catch (Exception ex)
            {
                R.Success = false;
                R.Message = ex.Message;
            }

            var LogReturn = await IslemLog.Logla(configuration, KisiId, MethodBase.GetCurrentMethod().DeclaringType, R.Message, dto, dto.Id);

            if (!R.Success) R.Message = LogReturn;

            return R;
        }

        public async Task<ServiceResponse<string>> DeleteRol(List<RolDTO> dtos, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                foreach (var dto in dtos)
                {
                    await connection.ExecuteAsync($"DELETE FROM RolYetki WHERE RolId = {dto.Id} DELETE FROM Rol WHERE Id = {dto.Id}");
                }
            }
            catch (Exception ex)
            {
                R.Success = false;
                R.Message = ex.Message;
            }

            var LogReturn = await IslemLog.Logla(configuration, KisiId, MethodBase.GetCurrentMethod().DeclaringType, R.Message, dtos, dtos[0].Id);

            if (!R.Success) R.Message = LogReturn;

            return R;
        }

        public async Task<ServiceResponse<List<string>>> GetSayfaList()
        {
            var R = new ServiceResponse<List<string>>();

            try
            {
                using var connection = new SqlConnection(CS);

                var result = await connection.QueryAsync<string>("SELECT Link FROM Yetki WHERE ISNULL(Link, '') != ''");

                R.Value = result.ToList();
            }
            catch (Exception ex)
            {
                R.Success = false;
                R.Message = ex.Message;
            }

            if (!R.Success)
            {
                var logReturn = await IslemLog.Logla(configuration, 0, MethodBase.GetCurrentMethod().DeclaringType, R.Message, "", 0);

                R.Message = logReturn;
            }

            return R;
        }

        public async Task<ServiceResponse<bool>> YetkiKontrolBySayfa(int KisiId, string Sayfa)
        {
            var R = new ServiceResponse<bool>();

            try
            {
                using var connection = new SqlConnection(CS);

                Sayfa = Sayfa.Replace("~", "/");

                string Q = $@"  DECLARE @KisiId INT = {KisiId}
                                DECLARE @Link VARCHAR(100) = '{Sayfa}'

                                DECLARE @Rol VARCHAR(200)
                                DECLARE @Tip INT
                                DECLARE @YetkiId INT

                                SELECT
	                                @Tip = Tip
                                FROM
	                                Kisi
                                WHERE
	                                Id = @KisiId

                                SELECT
	                                @YetkiId = Id * 100 + 1
                                FROM
	                                Yetki
                                WHERE
	                                Link = @Link

                                SELECT
	                                KisiRol.Id
                                FROM
	                                KisiRol
	                                LEFT JOIN RolYetki ON KisiRol.RolId = RolYetki.RolId
                                WHERE
	                                KisiRol.KisiId = @KisiId
	                                AND RolYetki.YetkiId = @YetkiId";

                var result = await connection.QuerySingleOrDefaultAsync<int>(Q);

                R.Value = result > 1;
            }
            catch (Exception ex)
            {
                R.Success = false;
                R.Message = ex.Message;
            }

            if (!R.Success)
            {
                var logReturn = await IslemLog.Logla(configuration, KisiId, MethodBase.GetCurrentMethod().DeclaringType, R.Message, Sayfa, 0);
                
                R.Message = logReturn;
            }

            return R;
        }

        public async Task<bool> YetkiKontrolByYetki(int KisiId, string Yetki)
        {
            using var connection = new SqlConnection(CS);

            string Q = $@"  DECLARE @KisiId INT = {KisiId}
                            DECLARE @Yetki VARCHAR(400) = '{Yetki}'

                            DECLARE @Rol VARCHAR(200)
                            DECLARE @Tip INT

                            SELECT
	                            @Tip = Tip
                            FROM
	                            Kisi
                            WHERE
	                            Id = @KisiId

	                        SELECT TOP 1
		                        RolYetki.Id
	                        FROM
		                        KisiRol
		                        LEFT JOIN Rol ON KisiRol.RolId = Rol.Id
		                        LEFT JOIN RolYetki ON Rol.Id = RolYetki.RolId
		                        LEFT JOIN Yetki ON RolYetki.YetkiId = Yetki.Id
	                        WHERE
		                        KisiRol.KisiId = @KisiId
		                        AND Yetki.Yetki = @Yetki";

            var result = await connection.QuerySingleOrDefaultAsync<int>(Q);

            return result > 0;
        }

        public async Task<KullaniciForLoginDTO> GetKullaniciForLogin(int Id)
        {
            using var connection = new SqlConnection(CS);

            string Q = $@"SELECT
	                            Id,
                                Tip,
                                Ad + ' ' + Soyad AS AdSoyad,
                                TelefonCep,
                                EMail,
                                GizlilikOnaylandi,
                                AcikRiza,
                                CookiePolitikasiniGordu
                            FROM
	                            Kisi
                            WHERE
	                            Id = {Id}";

            var result = await connection.QuerySingleOrDefaultAsync<KullaniciForLoginDTO>(Q);

            return result;
        }

        public async Task<YetkiForRaporDTO> GetYetkiForRaporByRapor(string Rapor)
        {
            using var connection = new SqlConnection(CS);

            string RaporAdi = await connection.QuerySingleOrDefaultAsync<string>($"SELECT Yetki FROM Yetki WHERE Link = 'Rapor/{Rapor}'");

            string Q = $@"  SELECT
                                Yetki.Yetki AS RaporAdi,
	                            RaporAlani.RaporAlani,
	                            YetkiRaporAlani.Zorunlu
                            FROM
	                            YetkiRaporAlani
	                            LEFT JOIN RaporAlani ON YetkiRaporAlani.RaporAlaniId = RaporAlani.Id
	                            LEFT JOIN Yetki ON YetkiRaporAlani.YetkiId = Yetki.Id
                            WHERE
	                            Yetki.Link = 'Rapor/{Rapor}'";

            var YetkiRaporAlanlari = await connection.QueryAsync<YetkiRaporAlani>(Q);

            var result = new YetkiForRaporDTO();

            result.RaporAdi = RaporAdi;

            result.YetkiRaporAlanlari = new List<YetkiRaporAlani>();

            result.YetkiRaporAlanlari.AddRange(YetkiRaporAlanlari);

            return result;
        }

        public async Task<ServiceResponse<string>> GetYetkiBaseByLink(string link)
        {
            var R = new ServiceResponse<string>();

            try
            {
                link = link.Replace("_", "/");

                using var connection = new SqlConnection(CS);

                string Q = $@"SELECT
	                            YetkiUst.Yetki + ' - ' + Yetki.Yetki AS Yetki
                            FROM
	                            Yetki
	                            LEFT JOIN Yetki YetkiUst ON Yetki.ParentId = YetkiUst.Id
                            WHERE
	                            Yetki.Link = '{link}'";

                R.Value = await connection.QuerySingleOrDefaultAsync<string>(Q);
            }
            catch (Exception ex)
            {
                R.Success = false;
                R.Message = ex.Message;
            }

            return R;
        }
    }
}