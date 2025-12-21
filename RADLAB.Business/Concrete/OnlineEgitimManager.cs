using Dapper;
using Microsoft.Extensions.Configuration;
using RADLAB.Business.Abstract;
using RADLAB.Business.Utils;
using RADLAB.Model.DTO;
using RADLAB.Model.FilterDTO;
using RADLAB.Model.ResponseModels;
using System.Data.SqlClient;
using System.Reflection;

namespace RADLAB.Business.Concrete
{
    public class OnlineEgitimManager : IOnlineEgitimManager
    {
        private readonly IConfiguration configuration;
        private readonly string CS;

        public OnlineEgitimManager(IConfiguration _configuration)
        {
            bool debug = false;
#if DEBUG
            debug = true;
#endif

            configuration = _configuration;

            CS = configuration.GetConnectionString(debug ? "Debug" : "Release");
        }

        public async Task<ServiceResponse<OnlineEgitimDTO>> GetOnlineEgitim(int Id, int KisiId)
        {
            var R = new ServiceResponse<OnlineEgitimDTO>();

            try
            {
                using var connection = new SqlConnection(CS);

                if (Id < 0)
                {
                    Id = await connection.QuerySingleOrDefaultAsync<int>($"SELECT OnlineEgitimId FROM OnlineEgitimOgrenci WHERE Id = {0 - Id}");
                }

                var data = await connection.QuerySingleOrDefaultAsync<OnlineEgitimDTO>($"SELECT * FROM OnlineEgitim WHERE Id = {Id}");

                string QOgrenciList = $@"  SELECT
	                                            Kisi.Id,
                                                Kisi.Ad,
                                                Kisi.Soyad,
	                                            Kisi.Ad + ' ' + Kisi.Soyad AS AdSoyad,
                                                Kisi.TelefonCep,
                                                Kisi.EMail
                                            FROM
	                                            OnlineEgitimOgrenci
	                                            LEFT JOIN Kisi ON OnlineEgitimOgrenci.KisiId = Kisi.Id
                                            WHERE
                                                OnlineEgitimOgrenci.OnlineEgitimId = {Id.ToString()}
                                            ORDER BY
                                                Kisi.Ad,
                                                Kisi.Soyad";

                var resultOgrenciList = await connection.QueryAsync<OgrenciDTO>(QOgrenciList);

                data.OgrenciList = resultOgrenciList.ToList();

                string QOnlineEgitimBolumList = $@" SELECT
                                                        (CASE WHEN OnlineEgitimBolum.TestId > 0 THEN OnlineEgitimBolum.TestId ELSE 0 - OnlineEgitimBolum.VideoId END) AS Id,
                                                        OnlineEgitimBolum.OnlineEgitimId,
                                                        OnlineEgitimBolum.Sira,
                                                        OnlineEgitimBolum.Baslik,
                                                        OnlineEgitimBolum.TestId,
                                                        OnlineEgitimBolum.VideoId,
	                                                    ISNULL(Test.Test, Video.DosyaAdi) AS TestVideo
                                                    FROM
	                                                    OnlineEgitimBolum
	                                                    LEFT JOIN Test ON OnlineEgitimBolum.TestId = Test.Id
	                                                    LEFT JOIN Video ON OnlineEgitimBolum.VideoId = Video.Id
                                                    WHERE
                                                        OnlineEgitimBolum.OnlineEgitimId = {Id.ToString()}
                                                    ORDER BY
	                                                    OnlineEgitimBolum.Sira";

                var resultOnlineEgitimBolumList = await connection.QueryAsync<OnlineEgitimBolumDTO>(QOnlineEgitimBolumList);

                data.OnlineEgitimBolumList = resultOnlineEgitimBolumList.ToList();

                R.Value = data;
            }
            catch (Exception ex)
            {
                R.Success = false;
                R.Message = ex.Message;
            }

            if (!R.Success)
            {
                var logReturn = await IslemLog.Logla(configuration, KisiId, MethodBase.GetCurrentMethod().DeclaringType, R.Message, Id, 0);
                R.Message = logReturn;
            }

            return R;
        }

        public async Task<ServiceResponse<List<OnlineEgitimDTO>>> GetOnlineEgitimList(OnlineEgitimFilterDTO filterDTO, int KisiId)
        {
            var R = new ServiceResponse<List<OnlineEgitimDTO>>();

            try
            {
                using var connection = new SqlConnection(CS);

                string Q = $@"SPSelectOnlineEgitim
	                                @OnlineEgitim,
	                                @Zorluk,
	                                @Id,
									@PageNo,
									@PageSize,
									@DonusTipi";

                var result = await connection.QueryAsync<OnlineEgitimDTO>(Q, filterDTO);

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

        public async Task<ServiceResponse<string>> InsertOrUpdateOnlineEgitim(OnlineEgitimDTO dto, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                string Q = $@"  IF @Id = 0
                                BEGIN
                                    INSERT INTO OnlineEgitim
                                        (OnlineEgitim,
                                        Zorluk,
                                        GerekliBelgeler,
                                        SertifikaBilgi,
                                        Aciklama)
                                    VALUES
                                        (@OnlineEgitim,
                                        @Zorluk,
                                        @GerekliBelgeler,
                                        @SertifikaBilgi,
                                        @Aciklama)

                                    SELECT @@IDENTITY
                                END ELSE
                                BEGIN
                                    UPDATE OnlineEgitim SET
                                        OnlineEgitim = @OnlineEgitim,
                                        Zorluk = @Zorluk,
                                        GerekliBelgeler = @GerekliBelgeler,
                                        SertifikaBilgi = @SertifikaBilgi,
                                        Aciklama = @Aciklama
                                    WHERE
                                        Id = @Id

                                    /*DELETE
                                    FROM
                                        OnlineEgitimOgrenci
                                    WHERE
                                        OnlineEgitimId = @Id*/

                                    DELETE
                                    FROM
                                        OnlineEgitimBolum
                                    WHERE
                                        OnlineEgitimId = @Id

                                    SELECT @Id
                                END";

                var result = await connection.ExecuteScalarAsync<string>(Q, dto);

                /*foreach (var ogrenciDTO in dto.OgrenciList)
                {
                    string QOgrenci = $"INSERT INTO OnlineEgitimOgrenci (OnlineEgitimId, KisiId) VALUES ({result}, {ogrenciDTO.Id})";

                    await connection.ExecuteScalarAsync<string>(QOgrenci);
                }*/

                int sira = 0;

                foreach (var onlineEgitimBolumDTO in dto.OnlineEgitimBolumList)
                {
                    sira++;

                    string QOnlineEgitimBolum = @$" INSERT INTO OnlineEgitimBolum
                                                        (OnlineEgitimId,
                                                        Sira,
                                                        Baslik,
                                                        TestId,
                                                        VideoId)
                                                    VALUES
                                                        ({result},
                                                        {sira},
                                                        @Baslik,
                                                        (CASE WHEN @Id > 0 THEN @Id ELSE NULL END),
                                                        (CASE WHEN @Id < 0 THEN 0 - @Id ELSE NULL END))";

                    await connection.ExecuteScalarAsync<string>(QOnlineEgitimBolum, onlineEgitimBolumDTO);
                }

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

        public async Task<ServiceResponse<string>> InsertOnlineEgitimOgrenci(List<OnlineEgitimKisiDTO> dtos, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                string Q = $@"  IF NOT EXISTS(SELECT Id FROM OnlineEgitimOgrenci WHERE OnlineEgitimId = @OnlineEgitimId AND KisiId = @Id)
                                BEGIN
	                                INSERT INTO OnlineEgitimOgrenci
		                                (OnlineEgitimId, KisiId, Tarih1, Tarih2, Grup)
	                                VALUES
		                                (@OnlineEgitimId, @Id, CONVERT(DATE, GETDATE()), DATEADD(DAY, 7, CONVERT(DATE, GETDATE())), 1)
                                END

                                SELECT 0";

                foreach (var dto in dtos)
                {
                    await connection.ExecuteAsync(Q, dto);
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

        public async Task<ServiceResponse<string>> DeleteOnlineEgitimOgrenci(List<OnlineEgitimKisiDTO> dtos, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                string Q = $@"  DELETE FROM
                                    OnlineEgitimOgrenci
                                WHERE
                                    OnlineEgitimId = @OnlineEgitimId
                                    AND KisiId = @Id

                                SELECT 0";

                foreach (var dto in dtos)
                {
                    await connection.ExecuteAsync(Q, dto);
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

        public async Task<ServiceResponse<string>> DeleteOnlineEgitim(List<OnlineEgitimDTO> dtos, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                foreach (var dto in dtos)
                {
                    await connection.ExecuteAsync(@$"   DELETE FROM OnlineEgitimBolumTamamlanan WHERE OnlineEgitimBolumId IN (SELECT Id FROM OnlineEgitimBolum WHERE OnlineEgitimId = {dto.Id})
                                                        DELETE FROM OnlineEgitimOgrenci WHERE OnlineEgitimId = {dto.Id}
                                                        DELETE FROM OnlineEgitimBolum WHERE OnlineEgitimId = {dto.Id}
                                                        DELETE FROM OnlineEgitim WHERE Id = {dto.Id}");
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

        public async Task<ServiceResponse<List<OnlineEgitimTreeDTO>>> GetOnlineEgitimTreeList(int OnlineEgitimOgrenciId, int KisiId)
        {
            var R = new ServiceResponse<List<OnlineEgitimTreeDTO>>();

            try
            {
                using var connection = new SqlConnection(CS);

                var result = await connection.QueryAsync<OnlineEgitimTreeDTO>($"SPSelectOnlineEgitimTree {OnlineEgitimOgrenciId}");

                R.Value = result.ToList();
            }
            catch (Exception ex)
            {
                R.Success = false;
                R.Message = ex.Message;
            }

            if (!R.Success)
            {
                var logReturn = await IslemLog.Logla(configuration, KisiId, MethodBase.GetCurrentMethod().DeclaringType, R.Message, OnlineEgitimOgrenciId, 0);
                R.Message = logReturn;
            }

            return R;
        }

        public async Task<ServiceResponse<string>> InsertUpdateOnlineEgitimBolumTamamlanan(OnlineEgitimBolumTamamlananDTO dto, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                string Q = $@"  DECLARE @CurrentId INT

                                SELECT
	                                @CurrentId = Id
                                FROM
	                                OnlineEgitimBolumTamamlanan
                                WHERE
	                                OnlineEgitimOgrenciId = @OnlineEgitimOgrenciId
	                                AND OnlineEgitimBolumId = @OnlineEgitimBolumId
	                                AND (SoruId = @SoruId OR SoruId IS NULL)

                                UPDATE OnlineEgitimBolumTamamlanan SET
	                                BuradaKaldi = 0
                                FROM
	                                OnlineEgitimBolumTamamlanan
	                                LEFT JOIN OnlineEgitimBolum ON OnlineEgitimBolumTamamlanan.OnlineEgitimBolumId = OnlineEgitimBolum.Id
                                WHERE
	                                OnlineEgitimBolumTamamlanan.OnlineEgitimOgrenciId = @OnlineEgitimOgrenciId
	                                AND OnlineEgitimBolum.OnlineEgitimId = @OnlineEgitimId

                                IF ISNULL(@CurrentId, 0) = 0
                                BEGIN
	                                INSERT INTO OnlineEgitimBolumTamamlanan
		                                (OnlineEgitimOgrenciId,
		                                OnlineEgitimBolumId,
		                                SoruId,
		                                Cevap,
                                        BuradaKaldi)
	                                VALUES
		                                (@OnlineEgitimOgrenciId,
		                                @OnlineEgitimBolumId,
		                                (CASE WHEN @SoruId = 0 THEN NULL ELSE @SoruId END),
		                                @Cevap,
                                        1)

	                                SELECT @@IDENTITY
                                END ELSE
                                BEGIN
	                                UPDATE OnlineEgitimBolumTamamlanan SET
		                                Cevap = @Cevap,
                                        BuradaKaldi = 1
	                                WHERE
		                                Id = @CurrentId

	                                SELECT @CurrentId
                                END";

                var result = await connection.ExecuteScalarAsync<string>(Q, dto);

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

        public async Task<ServiceResponse<string>> UpdateOnlineEgitimBolumTamamlananGecenSure(OnlineEgitimTreeDTO dto, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                string Q = $@"  DECLARE @CurrentId INT

                                SELECT TOP 1
	                                @CurrentId = Id
                                FROM
	                                OnlineEgitimBolumTamamlanan
                                WHERE
	                                KisiId = {KisiId}
	                                AND OnlineEgitimBolumId = @ParentId

                                IF ISNULL(@CurrentId, 0) = 0
                                BEGIN
	                                INSERT INTO OnlineEgitimBolumTamamlanan
		                                (KisiId,
		                                OnlineEgitimBolumId,
		                                GecenSure,
		                                SoruId,
		                                Cevap,
		                                BuradaKaldi)
	                                VALUES
		                                ({KisiId},
		                                @ParentId,
		                                @GecenSure,
		                                @VideoIdSoruId,
		                                0,
		                                0)		
                                END ELSE
                                BEGIN
	                                UPDATE OnlineEgitimBolumTamamlanan SET
		                                GecenSure = @GecenSure
	                                WHERE
	                                    KisiId = {KisiId}
	                                    AND OnlineEgitimBolumId = @ParentId
                                END";

                var result = await connection.ExecuteScalarAsync<string>(Q, dto);

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

        public async Task<ServiceResponse<int>> GetOnlineEgitimBolumTamamlananGecenSure(int OnlineEgitimBolumId, int KisiId)
        {
            var R = new ServiceResponse<int>();

            try
            {
                using var connection = new SqlConnection(CS);

                string Q = $@"  SELECT TOP 1
	                                GecenSure
                                FROM
	                                OnlineEgitimBolumTamamlanan
                                WHERE
	                                OnlineEgitimBolumId = {OnlineEgitimBolumId}
	                                AND KisiId = {KisiId}";

                var result = await connection.QuerySingleOrDefaultAsync<int>(Q);

                R.Value = result;
            }
            catch (Exception ex)
            {
                R.Success = false;
                R.Message = ex.Message;
            }

            if (!R.Success)
            {
                var logReturn = await IslemLog.Logla(configuration, KisiId, MethodBase.GetCurrentMethod().DeclaringType, R.Message, OnlineEgitimBolumId, 0);
                R.Message = logReturn;
            }

            return R;
        }

        public async Task<ServiceResponse<List<OnlineEgitimSonucDTO>>> GetOnlineEgitimSonucList(int OnlineEgitimOgrenciId, int KisiId)
        {
            var R = new ServiceResponse<List<OnlineEgitimSonucDTO>>();

            try
            {
                using var connection = new SqlConnection(CS);

                var result = await connection.QueryAsync<OnlineEgitimSonucDTO>($"SPSelectOnlineEgitimSonuc {OnlineEgitimOgrenciId}");

                R.Value = result.ToList();
            }
            catch (Exception ex)
            {
                R.Success = false;
                R.Message = ex.Message;
            }

            if (!R.Success)
            {
                var logReturn = await IslemLog.Logla(configuration, KisiId, MethodBase.GetCurrentMethod().DeclaringType, R.Message, OnlineEgitimOgrenciId, 0);
                R.Message = logReturn;
            }

            return R;
        }

        public async Task<ServiceResponse<List<OnlineEgitimTestSonucDTO>>> GetOnlineEgitimTestSonuc(OnlineEgitimTestSonucFilterDTO filterDTO, int KisiId)
        {
            var R = new ServiceResponse<List<OnlineEgitimTestSonucDTO>>();

            try
            {
                using var connection = new SqlConnection(CS);

                string Q = $@"SPSelectOnlineEgitimTestSonuc
	                                @OnlineEgitimId,
	                                @Tarih1,
	                                @Tarih2,
	                                @Grup,
	                                @TestId,
	                                @SoruId,
	                                @AdSoyad,
									@PageNo,
									@PageSize,
									@DonusTipi";

                var result = await connection.QueryAsync<OnlineEgitimTestSonucDTO>(Q, filterDTO);

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

        public async Task<ServiceResponse<string>> UpdateOnlineEgitimBolumTamamlananVideoTime(OnlineEgitimBolumTamamlananDTO dto, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                string Q = $@"  UPDATE OnlineEgitimBolumTamamlanan SET
		                            VideoTime = {dto.VideoTime}
	                            WHERE
	                                OnlineEgitimOgrenciId = {dto.OnlineEgitimOgrenciId}
	                                AND OnlineEgitimBolumId = {dto.OnlineEgitimBolumId}";

                var result = await connection.ExecuteScalarAsync<string>(Q);

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
    }
}