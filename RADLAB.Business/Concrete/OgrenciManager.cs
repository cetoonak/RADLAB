using Dapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RADLAB.Business.Abstract;
using RADLAB.Business.Utils;
using RADLAB.Model.DTO;
using RADLAB.Model.FilterDTO;
using RADLAB.Model.ParamDTO;
using RADLAB.Model.ResponseModels;
using System.Data.SqlClient;
using System.Reflection;

namespace RADLAB.Business.Concrete
{
    public class OgrenciManager : IOgrenciManager
    {
        private readonly IConfiguration configuration;
        private readonly string CS;

        public OgrenciManager(IConfiguration _configuration)
        {
            bool debug = false;
#if DEBUG
            debug = true;
#endif

            configuration = _configuration;

            CS = configuration.GetConnectionString(debug ? "Debug" : "Release");
        }

        public async Task<OgrenciDTO> GetOgrenci(int Id)
        {
            using var connection = new SqlConnection(CS);

            var ogrenci = await connection.QuerySingleOrDefaultAsync<OgrenciDTO>($@"SELECT * FROM Kisi WHERE Id = {Id}");

            ogrenci.OnlineEgitimOgrenciler = GetOnlineEgitimOgrenciListByOgrenciId(Id).Result;

            return ogrenci;
        }

        public async Task<List<OnlineEgitimOgrenciDTO>> GetOnlineEgitimOgrenciListByOgrenciId(int OgrenciId)
        {
            using var connection = new SqlConnection(CS);

            string Q = $@"  SELECT
	                            OnlineEgitimOgrenci.Id,
	                            OnlineEgitimOgrenci.OnlineEgitimId,
	                            OnlineEgitimOgrenci.KisiId,
                                OnlineEgitimOgrenci.Tarih1,
                                OnlineEgitimOgrenci.Tarih2,
                                OnlineEgitimOgrenci.Grup
                            FROM
	                            OnlineEgitimOgrenci
	                            LEFT JOIN OnlineEgitim ON OnlineEgitimOgrenci.OnlineEgitimId = OnlineEgitim.Id
                            WHERE
                                OnlineEgitimOgrenci.KisiId = {OgrenciId.ToString()}
                            ORDER BY
                                OnlineEgitim.OnlineEgitim";

            var result = await connection.QueryAsync<OnlineEgitimOgrenciDTO>(Q);

            return result.ToList();
        }

        public async Task<ServiceResponse<List<OgrenciDTO>>> GetOgrenciList(OgrenciFilterDTO filterDTO, int KisiId)
        {
            var R = new ServiceResponse<List<OgrenciDTO>>();

            try
            {
                using var connection = new SqlConnection(CS);

                string Q = $@"SPSelectOgrenci
	                                @AdSoyad,
	                                @TelefonCep,
	                                @EMail,
                                    @IlkEgitimAcilisTarihi,
                                    @OnlineEgitimId,
                                    @Durum,
	                                @Id,
									@PageNo,
									@PageSize,
									@DonusTipi";

                var result = await connection.QueryAsync<OgrenciDTO>(Q, filterDTO);

                if (filterDTO.DonusTipi == 0)
                {
                    filterDTO.DonusTipi = 1;

                    var RC = await connection.QuerySingleOrDefaultAsync<int>(Q, filterDTO);

                    R.RowCount = RC;
                }

                R.Value = result.ToList();
            }
            catch (Exception ex)
            {
                R.Success = false;
                R.Message = ex.Message;
            }

            if (!R.Success)
            {
                var logReturn = await IslemLog.Logla(configuration, KisiId, MethodBase.GetCurrentMethod().DeclaringType, R.Message, filterDTO, 0);
                R.Message = logReturn;
            }

            return R;
        }

        public async Task<ServiceResponse<string>> InsertOrUpdateOgrenci(OgrenciDTO dto, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                // Öğrenci ekle, değiştir

                string Q = $@"  IF @Id = 0
                                BEGIN
                                    INSERT INTO Kisi
                                        (Ad,
                                        Soyad,
		                                TelefonCep,
                                        EMail,
                                        Aktif,
                                        Tip)
                                    VALUES
                                        (@Ad,
                                        @Soyad,
		                                @TelefonCep,
                                        @EMail,
                                        @Aktif,
                                        2)

                                    SELECT @Id = @@IDENTITY
                                END ELSE
                                BEGIN
                                    UPDATE Kisi SET
                                        Ad = @Ad,
                                        Soyad = @Soyad,
		                                TelefonCep = @TelefonCep,
                                        EMail = @EMail,
                                        Aktif = @Aktif
                                    WHERE
                                        Id = @Id
                                END

                                SELECT @Id";

                var result = await connection.ExecuteScalarAsync<int>(Q, dto);

                dto.Id = result;

                var resultOnlineEgitimOgrenciList = new List<int>();

                // OnlineEgitimOgrenciler ekle, değiştir

                foreach (var onlineEgitimOgrenciDTO in dto.OnlineEgitimOgrenciler)
                {
                    onlineEgitimOgrenciDTO.KisiId = dto.Id;

                    string QOnlineEgitimOgrenci = $@"   IF @Id = 0
                                                        BEGIN
	                                                        INSERT INTO OnlineEgitimOgrenci
		                                                        (OnlineEgitimId, KisiId, Tarih1, Tarih2, Grup)
	                                                        VALUES
		                                                        (@OnlineEgitimId, @KisiId, @Tarih1, @Tarih2, @Grup)

	                                                        SELECT @@IDENTITY
                                                        END ELSE
                                                        BEGIN
	                                                        UPDATE OnlineEgitimOgrenci SET
		                                                        OnlineEgitimId = @OnlineEgitimId,
		                                                        KisiId = @KisiId,
		                                                        Tarih1 = @Tarih1,
		                                                        Tarih2 = @Tarih2,
		                                                        Grup = @Grup
	                                                        WHERE
		                                                        Id = @Id

	                                                        SELECT @Id
                                                        END";

                    int resultOnlineEgitimOgrenci = await connection.ExecuteScalarAsync<int>(QOnlineEgitimOgrenci, onlineEgitimOgrenciDTO);

                    resultOnlineEgitimOgrenciList.Add(resultOnlineEgitimOgrenci);
                }

                // OnlineEgitimOgrenciler sil

                string SilinmeyecekOnlineEgitimOgrenciIdList = string.Join("_", resultOnlineEgitimOgrenciList.Select(x => x).ToList());
                
                string QSilinecekOnlineEgitimOgrenci = $@"  DELETE
                                                            FROM
	                                                            OnlineEgitimOgrenci
                                                            WHERE
	                                                            KisiId = {dto.Id}
	                                                            AND Id NOT IN (SELECT Id FROM dbo.FIdList('{SilinmeyecekOnlineEgitimOgrenciIdList}'))";

                await connection.ExecuteScalarAsync(QSilinecekOnlineEgitimOgrenci);

                R.Value = result.ToString();
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

        public async Task<ServiceResponse<string>> UpdateOgrenciAktif(List<OgrenciDTO> dtos, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                foreach (var dto in dtos)
                {
                    await connection.ExecuteAsync($"UPDATE Kisi SET Aktif = {(dto.Aktif ? "1" : "0")} WHERE Id = {dto.Id}");
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

        public async Task<ServiceResponse<string>> DeleteOgrenci(List<OgrenciDTO> dtos, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                foreach (var dto in dtos)
                {
                    await connection.ExecuteAsync($"DELETE FROM Kisi WHERE Id = {dto.Id}");
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
    }
}