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
    public class MesajManager : IMesajManager
    {
        private readonly IConfiguration configuration;
        private readonly string CS;

        public MesajManager(IConfiguration _configuration)
        {
            bool debug = false;
#if DEBUG
            debug = true;
#endif

            configuration = _configuration;

            CS = configuration.GetConnectionString(debug ? "Debug" : "Release");
        }

        //public async Task<MesajDTO> GetMesaj(int GelenGiden, int Id)
        //{
        //    using var connection = new SqlConnection(CS);

        //    string Q = $@"	SELECT
		      //                  Mesaj.*,
		      //                  CASE {GelenGiden}
			     //                   WHEN 1 THEN KisiGonderen.Ad + '_' + KisiGonderen.Soyad
			     //                   WHEN 2 THEN dbo.FKimeList(Mesaj.Id)
		      //                  END AS Kim
	       //                 FROM
		      //                  Mesaj
		      //                  LEFT JOIN Kisi KisiGonderen ON Mesaj.GonderenKisiId = KisiGonderen.Id
	       //                 WHERE
		      //                  Mesaj.Id = {Id}";

        //    return await connection.QuerySingleOrDefaultAsync<MesajDTO>(Q);
        //}

        public async Task<ServiceResponse<MesajDTO>> GetMesaj(int GelenGiden, int Id, int KisiId)
        {
            var R = new ServiceResponse<MesajDTO>();

            try
            {
                using var connection = new SqlConnection(CS);

                string Q = $@"	SELECT
		                            Mesaj.*,
		                            CASE {GelenGiden}
			                            WHEN 1 THEN KisiGonderen.Ad + ' ' + KisiGonderen.Soyad
			                            WHEN 2 THEN dbo.FKimeList(Mesaj.Id)
		                            END AS Kim
	                            FROM
		                            Mesaj
		                            LEFT JOIN Kisi KisiGonderen ON Mesaj.GonderenKisiId = KisiGonderen.Id
	                            WHERE
		                            Mesaj.Id = {Id}";

                var result = await connection.QuerySingleOrDefaultAsync<MesajDTO>(Q);

                R.Value = result;
            }
            catch (Exception ex)
            {
                R.Success = false;
                R.Message = ex.Message;
            }

            if (!R.Success)
            {
                var logReturn = await IslemLog.Logla(configuration, KisiId, MethodBase.GetCurrentMethod().DeclaringType, R.Message, "", KisiId);
                R.Message = logReturn;
            }

            return R;
        }

        public async Task<ServiceResponse<int>> GetOkunmamisMesajSayisi(int KisiId)
        {
            var R = new ServiceResponse<int>();

            try
            {
                using var connection = new SqlConnection(CS);

                string Q = $@"	SELECT
		                            COUNT(Id) AS Sayi
	                            FROM
		                            MesajGonderilenKisi
	                            WHERE
		                            Silindi = 0 AND
		                            Okundu = 0 AND
		                            KisiId = {KisiId}";

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
                var logReturn = await IslemLog.Logla(configuration, KisiId, MethodBase.GetCurrentMethod().DeclaringType, R.Message, "", KisiId);
                R.Message = logReturn;
            }

            return R;
        }

        public async Task<ServiceResponse<List<MesajDTO>>> GetOkunmamisMesajlar(int KisiId)
        {
            var R = new ServiceResponse<List<MesajDTO>>();

            try
            {
                using var connection = new SqlConnection(CS);

                string Q = $@"  SELECT TOP 4
			                        Mesaj.Id,
			                        Mesaj.Zaman,
			                        KisiGonderen.Ad + ' ' + KisiGonderen.Soyad AS Kim,
			                        Mesaj.Konu
		                        FROM
			                        MesajGonderilenKisi
			                        LEFT JOIN Mesaj ON MesajGonderilenKisi.MesajId = Mesaj.Id
			                        LEFT JOIN Kisi KisiGonderen ON Mesaj.GonderenKisiId = KisiGonderen.Id
		                        WHERE
                                    MesajGonderilenKisi.Okundu = 0 AND
			                        MesajGonderilenKisi.Silindi = 0 AND
			                        MesajGonderilenKisi.KisiId = {KisiId}
		                        ORDER BY
			                        Mesaj.Zaman DESC";

                var result = await connection.QueryAsync<MesajDTO>(Q);

                R.Value = result.ToList();
            }
            catch (Exception ex)
            {
                R.Success = false;
                R.Message = ex.Message;
            }

            if (!R.Success)
            {
                var logReturn = await IslemLog.Logla(configuration, KisiId, MethodBase.GetCurrentMethod().DeclaringType, R.Message, "", KisiId);
                R.Message = logReturn;
            }

            return R;
        }

        public async Task<ServiceResponse<List<MesajDTO>>> GetMesajKutusu(MesajKutusuFilterDTO filterDTO, int KisiId)
        {
            var R = new ServiceResponse<List<MesajDTO>>();

            try
            {
                using var connection = new SqlConnection(CS);

                string Q = $@"  SELECT
                                    Mesaj.Id,
                                    MesajGonderilenKisi.Id AS MesajGonderilenKisiId,
			                        Mesaj.Zaman,
			                        KisiGonderen.Ad + ' ' + KisiGonderen.Soyad AS Kim,
			                        Mesaj.Konu,
			                        CONVERT(BIT, (CASE WHEN MesajGonderilenKisi.Okundu = 1 THEN 0 ELSE 1 END)) AS Bold
		                        FROM
			                        MesajGonderilenKisi
			                        LEFT JOIN Mesaj ON MesajGonderilenKisi.MesajId = Mesaj.Id
			                        LEFT JOIN Kisi KisiGonderen ON Mesaj.GonderenKisiId = KisiGonderen.Id
		                        WHERE
			                        CONVERT(INT, MesajGonderilenKisi.Silindi) = (CASE {filterDTO.GelenGidenArsiv} WHEN 1 THEN 0 WHEN 3 THEN 1 ELSE -1 END)
			                        AND MesajGonderilenKisi.KisiId = {KisiId}
                                    AND (KisiGonderen.Ad + ' ' + KisiGonderen.Soyad LIKE '%{filterDTO.SearchText}%' OR Mesaj.Konu LIKE '%{filterDTO.SearchText}%')

                                UNION ALL

		                        SELECT
			                        Id,
                                    0 AS MesajGonderilenKisiId,
			                        Zaman,
			                        dbo.FKimeList(Id) AS Kim,
			                        Konu,
			                        CONVERT(BIT, 0) AS Bold
		                        FROM
			                        Mesaj
		                        WHERE
                                    CONVERT(INT, Silindi) = (CASE {filterDTO.GelenGidenArsiv} WHEN 2 THEN 0 WHEN 3 THEN 1 ELSE -1 END)
			                        AND GonderenKisiId = {KisiId}
                                    AND (dbo.FKimeList(Id) LIKE '%{filterDTO.SearchText}%' OR Konu LIKE '%{filterDTO.SearchText}%')

		                        ORDER BY
			                        Zaman DESC";

                var result = await connection.QueryAsync<MesajDTO>(Q);

                R.Value = result.ToList();
            }
            catch (Exception ex)
            {
                R.Success = false;
                R.Message = ex.Message;
            }

            if (!R.Success)
            {
                var logReturn = await IslemLog.Logla(configuration, KisiId, MethodBase.GetCurrentMethod().DeclaringType, R.Message, filterDTO, KisiId);
                R.Message = logReturn;
            }

            return R;
        }

        public async Task<ServiceResponse<MesajGrubuMasterDTO>> GetMesajGrubuMaster(int Id, int KisiId)
        {
            var R = new ServiceResponse<MesajGrubuMasterDTO>();

            try
            {
                using var connection = new SqlConnection(CS);

                var resultMaster = await connection.QuerySingleOrDefaultAsync<MesajGrubuMasterDTO>($"SELECT * FROM MesajGrubuMaster WHERE Id = {Id}");

                string QDetail = $@"SELECT
                                        MesajGrubuDetail.*,
                                        Kisi.Ad + ' ' + Kisi.Soyad AS UyeKisi
                                    FROM
                                        MesajGrubuDetail
                                        LEFT JOIN Kisi ON MesajGrubuDetail.UyeKisiId = Kisi.Id
                                    WHERE
                                        MesajGrubuDetail.MesajGrubuMasterId = {Id}";

                var resultDetail = await connection.QueryAsync<MesajGrubuDetailDTO>(QDetail);

                resultMaster.MesajGrubuDetailList = new List<MesajGrubuDetailDTO>();
                resultMaster.MesajGrubuDetailList.AddRange(resultDetail);

                R.Value = resultMaster;
            }
            catch (Exception ex)
            {
                R.Success = false;
                R.Message = ex.Message;
            }

            if (!R.Success)
            {
                var logReturn = await IslemLog.Logla(configuration, KisiId, MethodBase.GetCurrentMethod().DeclaringType, R.Message, Id, KisiId);
                R.Message = logReturn;
            }

            return R;
        }

        public async Task<ServiceResponse<List<MesajGrubuMasterDTO>>> GetMesajGrubuMasterList(int KisiId)
        {
            var R = new ServiceResponse<List<MesajGrubuMasterDTO>>();

            try
            {
                using var connection = new SqlConnection(CS);

                string Q = $@"  SELECT
	                                Id,
	                                GrupAdi,
	                                dbo.FKisilerByMesajGrubuMasterId(Id) AS Kisiler
                                FROM
	                                MesajGrubuMaster
                                WHERE
	                                OlusturanKisiId = {KisiId}
                                ORDER BY
	                                GrupAdi";

                var result = await connection.QueryAsync<MesajGrubuMasterDTO>(Q);

                R.Value = result.ToList();
            }
            catch (Exception ex)
            {
                R.Success = false;
                R.Message = ex.Message;
            }

            if (!R.Success)
            {
                var logReturn = await IslemLog.Logla(configuration, KisiId, MethodBase.GetCurrentMethod().DeclaringType, R.Message, "", KisiId);
                R.Message = logReturn;
            }

            return R;
        }

        public async Task<ServiceResponse<List<MesajKisiDTO>>> GetMesajKisiList(string SearchText, int KisiId)
        {
            var R = new ServiceResponse<List<MesajKisiDTO>>();

            try
            {
                using var connection = new SqlConnection(CS);

                string Q = $@"  DECLARE @KisiId INT = {KisiId}

                                DECLARE @KurulusId INT

                                SELECT
	                                @KurulusId = Hastane.KurulusId
                                FROM
	                                Kisi
	                                LEFT JOIN Birim ON Kisi.BirimId = Birim.Id
	                                LEFT JOIN Hastane ON Birim.HastaneId = Hastane.Id
                                WHERE
	                                Kisi.Id = @KisiId

                                SELECT
	                                0 - MesajGrubuMaster.Id AS Id,
	                                MesajGrubuMaster.GrupAdi AS AdSoyad,
	                                '' AS Kurulus,
	                                '' AS Birim,
	                                0 AS Tip
                                FROM
	                                MesajGrubuMaster
                                WHERE
	                                OlusturanKisiId = @KisiId
                                    AND LEN('{SearchText}') > 2
                                    AND MesajGrubuMaster.GrupAdi LIKE '%{SearchText}%' 

                                UNION ALL

                                SELECT
	                                Kisi.Id,
	                                Kisi.Ad + ' ' + Kisi.Soyad AS AdSoyad,
	                                Kurulus.Kurulus,
	                                Birim.Birim,
	                                1 AS Tip
                                FROM
	                                Kisi
	                                LEFT JOIN Birim ON Kisi.BirimId = Birim.Id
	                                LEFT JOIN Hastane ON Birim.HastaneId = Hastane.Id
	                                LEFT JOIN Kurulus ON Hastane.KurulusId = Kurulus.Id
                                WHERE
	                                Kisi.Ad NOT LIKE '%TRANS%'
	                                AND Kisi.Soyad NOT LIKE '%TRANS%'
                                    AND LEN('{SearchText}') > 2
                                    AND (Kisi.Ad + ' ' + Kisi.Soyad LIKE '%{SearchText}%' OR Kurulus.Kurulus LIKE '%{SearchText}%' OR Birim.Birim LIKE '%{SearchText}%')
	                                AND ISNULL(Hastane.KurulusId, 0) = (CASE WHEN ISNULL(@KurulusId, 0) = 0 THEN ISNULL(Hastane.KurulusId, 0) ELSE ISNULL(@KurulusId, 0) END)

                                ORDER BY
	                                Tip,
	                                AdSoyad";

                var result = await connection.QueryAsync<MesajKisiDTO>(Q);

                R.Value = result.ToList();
            }
            catch (Exception ex)
            {
                R.Success = false;
                R.Message = ex.Message;
            }

            if (!R.Success)
            {
                var logReturn = await IslemLog.Logla(configuration, KisiId, MethodBase.GetCurrentMethod().DeclaringType, R.Message, "", KisiId);
                R.Message = logReturn;
            }

            return R;
        }

        public async Task<ServiceResponse<string>> InsertMesaj(MesajYazDTO dto, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                dto.Mesaj.GonderenKisiId = KisiId;

                string QMesaj = $@" INSERT INTO Mesaj
		                                (GonderenKisiId, Zaman, Konu, Icerik)
	                                VALUES
		                                (@GonderenKisiId, GETDATE(), @Konu, @Icerik)

	                                SELECT @@IDENTITY";

                var resultMesaj = await connection.ExecuteScalarAsync<string>(QMesaj, dto.Mesaj);

                R.Value = resultMesaj;

                dto.Mesaj.Id = Convert.ToInt32(R.Value);

                foreach (var mesajGonderilenKisi in dto.MesajGonderilenKisiler)
                {
                    mesajGonderilenKisi.MesajId = dto.Mesaj.Id; // Log için

                    if (mesajGonderilenKisi.KisiId < 0) // mesajGonderilenKisi.KisiId sıfırdan küçükse MesajGrubuMasterId demek
                    {
                        int MesajGrubuMasterId = 0 - mesajGonderilenKisi.KisiId;

                        var resultMesajGrubuDetailList = await connection.QueryAsync<int>($"SELECT UyeKisiId FROM MesajGrubuDetail WHERE MesajGrubuMasterId = {MesajGrubuMasterId}");

                        foreach (var mesajGrubuDetailUyeKisiId in resultMesajGrubuDetailList)
                        {
                            await connection.ExecuteAsync($"INSERT INTO MesajGonderilenKisi (MesajId, KisiId) VALUES ({dto.Mesaj.Id}, {mesajGrubuDetailUyeKisiId})");
                        }
                    }
                    else
                    {
                        await connection.ExecuteAsync($"INSERT INTO MesajGonderilenKisi (MesajId, KisiId) VALUES ({dto.Mesaj.Id}, {mesajGonderilenKisi.KisiId})");
                    }
                }
            }
            catch (Exception ex)
            {
                R.Success = false;
                R.Message = ex.Message;
            }

            var LogReturn = await IslemLog.Logla(configuration, KisiId, MethodBase.GetCurrentMethod().DeclaringType, R.Message, dto, dto.Mesaj.Id);

            if (!R.Success) R.Message = LogReturn;

            return R;
        }

        public async Task<ServiceResponse<string>> InsertOrUpdateMesajGrubu(MesajGrubuMasterDTO dto, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                string QMesajGrubuMaster = $@"  IF @Id = 0
                                                BEGIN
                                                    INSERT INTO MesajGrubuMaster
		                                                (OlusturanKisiId, GrupAdi)
	                                                VALUES
		                                                ({KisiId}, @GrupAdi)

	                                                SELECT @@IDENTITY
                                                END ELSE
                                                BEGIN
                                                    UPDATE MesajGrubuMaster SET
                                                        GrupAdi = @GrupAdi
                                                    WHERE
                                                        Id = @Id

                                                    SELECT @Id
                                                END";

                var resultMesajGrubuMaster = await connection.ExecuteScalarAsync<string>(QMesajGrubuMaster, dto);

                R.Value = resultMesajGrubuMaster;

                // Kayıt değiştiriyorsa eski MesajGrubuDetail kayıtlarını sil

                if (dto.Id != 0)
                {
                    await connection.ExecuteAsync($"DELETE FROM MesajGrubuDetail WHERE MesajGrubuMasterId = {dto.Id}");
                }

                // Son MesajGrubuDetail kayıtlarını ekle

                dto.Id = Convert.ToInt32(resultMesajGrubuMaster);

                foreach (var mesajGrubuDetail in dto.MesajGrubuDetailList)
                {
                    mesajGrubuDetail.MesajGrubuMasterId = dto.Id;

                    string QMesajGrubuDetail = $@"	INSERT INTO MesajGrubuDetail
		                                                (MesajGrubuMasterId, UyeKisiId)
	                                                VALUES
		                                                (@MesajGrubuMasterId, @UyeKisiId)";

                    await connection.ExecuteAsync(QMesajGrubuDetail, mesajGrubuDetail);
                }
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

        public async Task<ServiceResponse<string>> UpdateMesajGonderilenKisiOkundu(MesajDTO dto, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                await connection.ExecuteAsync($"UPDATE MesajGonderilenKisi SET Okundu = 1 WHERE MesajId = {dto.Id} AND KisiId = {KisiId}");
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

        public async Task<ServiceResponse<string>> UpdateMesajGonderilenKisiVeyaMesajSilVeyaGeriAl(MesajDTO dto, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                string Q = "";

                if (dto.MesajGonderilenKisiId == 0)
                {
                    Q = $"UPDATE Mesaj SET Silindi = (CASE WHEN Silindi = 0 THEN 1 ELSE 0 END) WHERE Id = {dto.Id}";
                }
                else
                {
                    Q = $"UPDATE MesajGonderilenKisi SET Silindi = (CASE WHEN Silindi = 0 THEN 1 ELSE 0 END) WHERE MesajId = {dto.Id} AND KisiId = {KisiId}";
                }

                await connection.ExecuteAsync(Q);
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

        public async Task<ServiceResponse<string>> DeleteMesajGrubuMaster(MesajGrubuMasterDTO dto, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                await connection.ExecuteAsync($@"   DELETE FROM MesajGrubuDetail WHERE MesajGrubuMasterId = @Id
                                                    DELETE FROM MesajGrubuMaster WHERE Id = @Id", dto);
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