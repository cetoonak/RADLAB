using Dapper;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Configuration;
using RADLAB.Business.Abstract;
using RADLAB.Business.Utils;
using RADLAB.Model.DTO;
using RADLAB.Model.ResponseModels;
using System.Data.SqlClient;
using System.Reflection;

namespace RADLAB.Business.Concrete
{
    public class CihazManager : ICihazManager
    {
        private readonly IConfiguration configuration;
        private readonly string CS;

        public CihazManager(IConfiguration _configuration)
        {
            bool debug = false;
#if DEBUG
            debug = true;
#endif

            configuration = _configuration;

            CS = configuration.GetConnectionString(debug ? "Debug" : "Release");
        }

        public async Task<CihazDTO> GetCihaz(int Id)
        {
            using var connection = new SqlConnection(CS);

            string QCihaz = $@"SELECT
                                    Model.*,
                                    CihazTuru.CihazTuru,
	                                Marka.Cihaz AS Marka,
	                                Model.Cihaz AS Model,
							        dbo.FCihazStokMiktari(Model.Id) AS StokMiktari,
	                                CihazResim.ResimDosyaAdi,
	                                CihazResim.ResimDosya,
	                                CihazResim.ResimUzanti
                                FROM
                                    Cihaz Model
                                    LEFT JOIN Cihaz Marka ON Model.ParentId = Marka.Id
                                    LEFT JOIN CihazTuru ON Model.CihazTuruId = CihazTuru.Id
			                        OUTER APPLY (SELECT TOP 1
                                                    DosyaAdi AS ResimDosyaAdi,
                                                    Uzanti AS ResimUzanti,
                                                    DosyaKucuk AS ResimDosya
						                        FROM
							                        CihazResim
						                        WHERE
							                        CihazId = Model.Id
                                                ORDER BY
                                                    Id) AS CihazResim
                                WHERE
                                    Model.Id = {Id}";

            var cihaz = await connection.QuerySingleOrDefaultAsync<CihazDTO>(QCihaz);

            var cihazResimleri = await connection.QueryAsync<CihazResimDTO>($"SELECT * FROM CihazResim WHERE CihazId = {Id}");

            cihaz.CihazResimleri = new List<CihazResimDTO>();

            cihaz.CihazResimleri.AddRange(cihazResimleri.ToList());

            return cihaz;
        }

        public async Task<List<CihazDTO>> GetCihazList(string Acilanlar)
        {
            using var connection = new SqlConnection(CS);

            string Q = $@"SELECT
	                            *,
	                            CASE
		                            WHEN ChildCount = 0 THEN ''
		                            WHEN CONVERT(VARCHAR(50), Id) IN (SELECT Id FROM dbo.FIdList(@Acilanlar)) THEN 'bi bi-chevron-down'
		                            ELSE 'bi bi-chevron-right'
	                            END AS Icon
                            FROM
	                            (SELECT
	                                V.*,
		                            (SELECT
			                            COUNT(CihazIc.Id)
		                            FROM
			                            Cihaz CihazIc
		                            WHERE
			                            CihazIc.ParentId = V.Id) AS ChildCount
                                FROM
	                                ViewCihaz V
	                            WHERE
		                            (V.Seviye = 0 OR
		                            CONVERT(VARCHAR(50), V.Id) IN (SELECT Id FROM dbo.FIdList(@Acilanlar)) OR
                                    CONVERT(VARCHAR(50), V.ParentId) IN (SELECT Id FROM dbo.FIdList(@Acilanlar)))) AS S
                            ORDER BY
	                            AdAcilimi";

            var result = await connection.QueryAsync<CihazDTO>(Q, new { Acilanlar = Acilanlar });

            return result.ToList();
        }

        public async Task<List<CihazDTO>> GetCihazSatisList()
        {
            using var connection = new SqlConnection(CS);

            string Q = $@"SELECT
	                        Model.Id,
	                        Marka.Cihaz AS Marka,
	                        Model.Cihaz AS Model,
	                        CihazTuru.CihazTuru,
	                        Model.Ozellik,
	                        Model.Fiyat,
	                        Model.KalibrasyonUcreti,
	                        Model.KdvOrani,
							Model.KritikStok,
							Model.Desi,
							dbo.FCihazStokMiktari(Model.Id) AS StokMiktari,
	                        CihazResim.ResimDosyaAdi,
	                        CihazResim.ResimDosya,
	                        CihazResim.ResimUzanti
                        FROM
	                        Cihaz Model
	                        LEFT JOIN Cihaz Marka ON Model.ParentId = Marka.Id
	                        LEFT JOIN CihazTuru ON Model.CihazTuruId = CihazTuru.Id
			                OUTER APPLY (SELECT TOP 1
                                            DosyaAdi AS ResimDosyaAdi,
                                            Uzanti AS ResimUzanti,
                                            DosyaKucuk AS ResimDosya
						                FROM
							                CihazResim
						                WHERE
							                CihazId = Model.Id
                                        ORDER BY
                                            Id) AS CihazResim
                        WHERE
	                        Model.Fiyat > 0";

            var result = await connection.QueryAsync<CihazDTO>(Q);

            return result.ToList();
        }

        public async Task<ServiceResponse<string>> InsertOrUpdateCihaz(CihazDTO dto, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                string Q = $@"IF @Id = 0
                                BEGIN
                                    INSERT INTO Cihaz
                                        (ParentId,
                                        Seviye,
                                        Cihaz,
                                        CihazTuruId,
		                                Fiyat,
                                        KalibrasyonUcreti,
                                        KdvOrani,
                                        KritikStok,
                                        Desi,
		                                Ozellik,
		                                KullanmaKlavuzuDosyaAdi,
		                                KullanmaKlavuzuUzanti,
		                                KullanmaKlavuzuIcerik)
                                    VALUES
                                        ((CASE WHEN @ParentId = 0 THEN NULL ELSE @ParentId END),
                                        @Seviye,
                                        @Cihaz,
                                        (CASE WHEN @CihazTuruId = 0 THEN NULL ELSE @CihazTuruId END),
                                        @Fiyat,
		                                @KalibrasyonUcreti,
                                        @KdvOrani,
		                                @KritikStok,
		                                @Desi,
                                        @Ozellik,
		                                @KullanmaKlavuzuDosyaAdi,
		                                @KullanmaKlavuzuUzanti,
		                                @KullanmaKlavuzuIcerik)

                                    SELECT @@IDENTITY
                                END ELSE
                                BEGIN
                                    UPDATE Cihaz SET
                                        Cihaz = @Cihaz,
                                        CihazTuruId = (CASE WHEN @CihazTuruId = 0 THEN NULL ELSE @CihazTuruId END),
                                        Fiyat = @Fiyat,
		                                KalibrasyonUcreti = @KalibrasyonUcreti,
                                        KdvOrani = @KdvOrani,
                                        KritikStok = @KritikStok,
                                        Desi = @Desi,
		                                Ozellik = @Ozellik,
		                                KullanmaKlavuzuDosyaAdi = @KullanmaKlavuzuDosyaAdi,
		                                KullanmaKlavuzuUzanti = @KullanmaKlavuzuUzanti,
		                                KullanmaKlavuzuIcerik = @KullanmaKlavuzuIcerik
                                    WHERE
                                        Id = @Id

                                    SELECT @Id
                                END";

                var resultQ = await connection.ExecuteScalarAsync<string>(Q, dto);

                if (dto.Id != 0)
                {
                    string QSil = $@"   DELETE
                                        FROM
                                            CihazResim
                                        WHERE
                                            CihazId = {dto.Id}

                                        SELECT {dto.Id}";

                    await connection.ExecuteScalarAsync<string>(QSil);
                }

                foreach (var item in dto.CihazResimleri)
                {
                    string QDosya = $@" INSERT INTO CihazResim
	                                        (CihazId, DosyaBuyuk, DosyaKucuk, DosyaAdi, Uzanti)
                                        VALUES
	                                        ({resultQ}, @DosyaBuyuk, @DosyaKucuk, @DosyaAdi, @Uzanti)

                                        SELECT @@IDENTITY";

                    await connection.ExecuteScalarAsync<string>(QDosya, item);
                }

                R.Value = resultQ;
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

        public async Task<ServiceResponse<string>> DeleteCihaz(List<CihazDTO> dtos, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                foreach (var dto in dtos)
                {
                    await connection.ExecuteAsync($@"   DELETE FROM Cihaz WHERE ParentId = {dto.Id}
                                                        DELETE FROM Cihaz WHERE Id = {dto.Id}");
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