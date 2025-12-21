using Dapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RADLAB.Business.Abstract;
using RADLAB.Business.Utils;
using RADLAB.Model.DTO;
using RADLAB.Model.ParamDTO;
using RADLAB.Model.ResponseModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Business.Concrete
{
    public class KullaniciManager : IKullaniciManager
    {
        private readonly IConfiguration configuration;
        private readonly string CS;

        public KullaniciManager(IConfiguration _configuration)
        {
            bool debug = false;
#if DEBUG
            debug = true;
#endif

            configuration = _configuration;

            CS = configuration.GetConnectionString(debug ? "Debug" : "Release");
        }

        public async Task<KullaniciDTO> GetKullanici(int Id)
        {
            using var connection = new SqlConnection(CS);

            var Kullanici = await connection.QuerySingleOrDefaultAsync<KullaniciDTO>($@"SELECT * FROM Kisi WHERE Id = {Id}");

            Kullanici.Roller = GetRolListByKullaniciId(Id).Result;

            return Kullanici;
        }

        public async Task<List<KullaniciDTO>> GetKullaniciList()
        {
            using var connection = new SqlConnection(CS);

            string Q = $@"  SELECT
	                            *
                            FROM
	                            Kisi
                            WHERE
	                            Tip = 1
                            ORDER BY
                                Aktif DESC,
	                            Ad,
	                            Soyad";

            var result = await connection.QueryAsync<KullaniciDTO>(Q);

            return result.ToList();
        }

        public async Task<List<RolDTO>> GetRolListByKullaniciId(int KullaniciId)
        {
            using var connection = new SqlConnection(CS);

            string Q = $@"  SELECT
	                            Rol.Id,
	                            Rol.Rol
                            FROM
	                            KisiRol
	                            LEFT JOIN Rol ON KisiRol.RolId = Rol.Id
                            WHERE
                                KisiRol.KisiId = {KullaniciId.ToString()}
                            ORDER BY
                                Rol.Rol";

            var result = await connection.QueryAsync<RolDTO>(Q);

            return result.ToList();
        }

        public async Task<ServiceResponse<string>> InsertOrUpdateKullanici(KullaniciDTO dto, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                var dtoParam = JsonConvert.DeserializeObject<KullaniciParamDTO>(JsonConvert.SerializeObject(dto));
                dtoParam.RolIdler = string.Join("_", dtoParam.Roller.Select(x => x.Id).ToList());

                string Q = $@"  IF @Id = 0
                                BEGIN
                                    INSERT INTO Kisi
                                        (Tip,
                                        Ad,
                                        Soyad,
                                        TelefonCep,
                                        EMail,
                                        SMSKalibrasyon,
                                        SMSEgitim,
                                        SMSSiparis,
                                        Aktif)
                                    VALUES
                                        (1,
                                        @Ad,
                                        @Soyad,
                                        @TelefonCep,
                                        @EMail,
                                        @SMSKalibrasyon,
                                        @SMSEgitim,
                                        @SMSSiparis,
                                        @Aktif)

                                    SELECT @Id = @@IDENTITY
                                END ELSE
                                BEGIN
                                    UPDATE Kisi SET
                                        Ad = @Ad,
                                        Soyad = @Soyad,
                                        TelefonCep = @TelefonCep,
                                        EMail = @EMail,
                                        SMSKalibrasyon = @SMSKalibrasyon,
                                        SMSEgitim = @SMSEgitim,
                                        SMSSiparis = @SMSSiparis,
                                        Aktif = @Aktif
                                    WHERE
                                        Id = @Id

                                    DELETE FROM KisiRol WHERE KisiId = @Id
                                END

                                IF @RolIdler != ''
                                BEGIN
                                    INSERT INTO KisiRol
                                        (KisiId, RolId)
                                    SELECT
                                        @Id AS KisiId,
                                        Id AS RolId
                                    FROM
                                        dbo.FIdList(@RolIdler)
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

        public async Task<ServiceResponse<string>> DeleteKullanici(List<KullaniciDTO> dtos, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                foreach (var dto in dtos)
                {
                    await connection.ExecuteAsync($"DELETE FROM KisiRol WHERE KisiId = {dto.Id} DELETE FROM Kisi WHERE Id = {dto.Id}");
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

        public async Task<ServiceResponse<string>> UpdateKisiProfil(KullaniciForLoginDTO dto, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                await connection.ExecuteAsync($@"   UPDATE Kisi SET
                                                        TelefonCep = @TelefonCep,
                                                        EMail = @EMail
                                                    WHERE
                                                        Id = @Id", dto);
            }
            catch (Exception ex)
            {
                R.Success = false;
                R.Message = ex.Message;
            }

            var logReturn = await IslemLog.Logla(configuration, KisiId, MethodBase.GetCurrentMethod().DeclaringType, R.Message, dto, KisiId);

            if (!R.Success) R.Message = logReturn;

            return R;
        }

        public async Task<ServiceResponse<string>> UpdateKisiCookiePolitikasiniGordu(KullaniciForLoginDTO dto, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                int value = dto.CookiePolitikasiniGordu ? 1 : 0;

                await connection.ExecuteAsync($"UPDATE Kisi SET CookiePolitikasiniGordu = {value} WHERE Id = {dto.Id}");
            }
            catch (Exception ex)
            {
                R.Success = false;
                R.Message = ex.Message;
            }

            var logReturn = await IslemLog.Logla(configuration, KisiId, MethodBase.GetCurrentMethod().DeclaringType, R.Message, dto, dto.Id);

            if (!R.Success) R.Message = logReturn;

            return R;
        }
    }
}