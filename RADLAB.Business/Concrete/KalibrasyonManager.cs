using Dapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RADLAB.Business.Abstract;
using RADLAB.Business.Utils;
using RADLAB.Model.DTO;
using RADLAB.Model.DTO.Turkak;
using RADLAB.Model.FilterDTO;
using RADLAB.Model.ResponseModels;
using System.Data.SqlClient;
using System.Net.Http.Json;
using System.Reflection;

namespace RADLAB.Business.Concrete
{
    public class KalibrasyonManager: IKalibrasyonManager
    {
        private readonly IConfiguration configuration;
        private readonly string CS;

        public KalibrasyonManager(IConfiguration _configuration)
        {
            bool debug = false;
#if DEBUG
            debug = true;
#endif

            configuration = _configuration;

            CS = configuration.GetConnectionString(debug ? "Debug" : "Release");
        }

        //////////////////////////////////////////////// Kalibrasyon ////////////////////////////////////////////

        public async Task<ServiceResponse<KalibrasyonDTO>> GetKalibrasyon(int Id, int KisiId)
        {
            var R = new ServiceResponse<KalibrasyonDTO>();
            
            try
            {
                using var connection = new SqlConnection(CS);

                string Q = $@"SELECT
	                                Kalibrasyon.*,
	                                CASE
		                                WHEN ISNULL(Kalibrasyon.AdSoyadBasvuru, '') != '' THEN 1
		                                ELSE 2
	                                END AS BasvuruTipi,
	                                CASE
		                                WHEN ISNULL(Kalibrasyon.AdSoyadFatura, '') != '' THEN 2
                                        WHEN ISNULL(Kalibrasyon.UnvanFatura, '') != '' THEN 3
		                                ELSE 1
	                                END AS FaturaTipi,
	                                CASE
		                                WHEN ISNULL(Kalibrasyon.IlIlceIdTeslim, 0) = 0 THEN 1
		                                ELSE 2
	                                END AS TeslimAdresiTipi,
									IlceBasvuru.ParentId AS IlIdBasvuru,
									ISNULL(IlceFatura.ParentId, 0) AS IlIdFatura,
									ISNULL(IlceTeslim.ParentId, 0) AS IlIdTeslim,
									Kalibrasyon.IlIlceIdBasvuru AS IlceIdBasvuru,
                                    ISNULL(Kalibrasyon.IlIlceIdFatura, 0) AS IlceIdFatura,
									ISNULL(Kalibrasyon.IlIlceIdTeslim, 0) AS IlceIdTeslim,
                                    IndirimKodu.IndirimKodu,
                                    IndirimKodu.Tutar AS IndirimTutari,
                                    dbo.FKalibrasyonOdeme(Kalibrasyon.Id) AS KalibrasyonOdeme
                                FROM
	                                Kalibrasyon
									LEFT JOIN IlIlce IlceBasvuru ON Kalibrasyon.IlIlceIdBasvuru = IlceBasvuru.Id
                                    LEFT JOIN IlIlce IlceFatura ON Kalibrasyon.IlIlceIdFatura = IlceFatura.Id
									LEFT JOIN IlIlce IlceTeslim ON Kalibrasyon.IlIlceIdTeslim = IlceTeslim.Id
                                    LEFT JOIN IndirimKodu ON Kalibrasyon.IndirimKoduId = IndirimKodu.Id
                                WHERE
	                                Kalibrasyon.Id = {Id}";

                var result = await connection.QuerySingleOrDefaultAsync<KalibrasyonDTO>(Q);

                string QCihaz = $@"SELECT
	                                    KalibrasyonCihaz.*,
                                        dbo.FCihazOdemeYapildi(KalibrasyonCihaz.Id) AS OdemeYapildi,
                                        Model.ParentId AS MarkaId,
                                        KalibrasyonCihaz.CihazId AS ModelId,
	                                    Marka.Cihaz AS Marka,
	                                    Model.Cihaz AS Model,
                                        KalibrasyonCihazAciklama.AciklamaSayisi
                                    FROM
	                                    KalibrasyonCihaz
	                                    LEFT JOIN Cihaz Model ON KalibrasyonCihaz.CihazId = Model.Id
	                                    LEFT JOIN Cihaz Marka ON Model.ParentId = Marka.Id
                                        OUTER APPLY (SELECT
                                                        COUNT(*) AS AciklamaSayisi
                                                    FROM
                                                        KalibrasyonCihazHareket
                                                    WHERE
                                                        KalibrasyonCihazId = KalibrasyonCihaz.Id
                                                        AND ISNULL(Aciklama, '') != '') AS KalibrasyonCihazAciklama
                                    WHERE
	                                    KalibrasyonCihaz.KalibrasyonId = {Id}";

                var resultCihaz = await connection.QueryAsync<KalibrasyonCihazDTO>(QCihaz);

                result.KalibrasyonCihazList = new List<KalibrasyonCihazDTO>();

                if (resultCihaz.Count() > 0)
                {
                    var cihazlar = resultCihaz.ToList();

                    for (int i = 0; i < cihazlar.Count; i++)
                    {
                        cihazlar[i].DataSourceModel = new List<LookupBasicDTO>();
                        result.KalibrasyonCihazList.Add(cihazlar[i]);
                    }
                }

                result.DetailCount = result.KalibrasyonCihazList.Count();

                //if (resultCihaz.Count() > 0) result.KalibrasyonCihazList.AddRange(resultCihaz);

                R.Value = result;
            }
            catch (Exception ex)
            {
                R.Success = false;
                R.Message = ex.Message;
            }

            if (!R.Success)
            {
                var logReturn = await IslemLog.Logla(configuration, KisiId, MethodBase.GetCurrentMethod().DeclaringType, R.Message, new object(), Id);
                R.Message = logReturn;
            }

            return R;
        }

        public async Task<ServiceResponse<List<KalibrasyonDTO>>> GetKalibrasyonList(KalibrasyonFilterDTO filterDTO, int KisiId)
        {
            try
            {
                string tarih1 = filterDTO.BasvuruTarihiAraligi.Split('-')[0].Trim();
                string tarih2 = filterDTO.BasvuruTarihiAraligi.Split('-')[1].Trim();

                int tarih1Gun = Convert.ToInt16(tarih1.Split('.')[0]);
                int tarih1Ay = Convert.ToInt16(tarih1.Split('.')[1]);
                int tarih1Yil = Convert.ToInt16(tarih1.Split('.')[2]);

                int tarih2Gun = Convert.ToInt16(tarih2.Split('.')[0]);
                int tarih2Ay = Convert.ToInt16(tarih2.Split('.')[1]);
                int tarih2Yil = Convert.ToInt16(tarih2.Split('.')[2]);

                filterDTO.BasvuruTarihi1 = Convert.ToInt32((new DateTime(tarih1Yil, tarih1Ay, tarih1Gun)).ToOADate());
                filterDTO.BasvuruTarihi2 = Convert.ToInt32((new DateTime(tarih2Yil, tarih2Ay, tarih2Gun)).ToOADate());
            }
            catch
            {
                filterDTO.BasvuruTarihi1 = 0;
                filterDTO.BasvuruTarihi2 = 0;
            }

            var R = new ServiceResponse<List<KalibrasyonDTO>>();

            try
            {
                using var connection = new SqlConnection(CS);

                string QKalibrasyon = $@"SPSelectKalibrasyon
                                            @AdSoyadUnvan,
							                @BasvuruTakipNo,
							                @BasvuruTarihi1,
							                @BasvuruTarihi2,
							                @Cihaz,
                                            @GelisSekli,
                                            @KalibrasyonOdeme,
							                @Sayfa,
							                @Order,
							                @Id,
							                @PageNo,
							                @PageSize,
							                @DonusTipi";

                var resultKalibrasyon = await connection.QueryAsync<KalibrasyonDTO>(QKalibrasyon, filterDTO);

                foreach (var kalibrasyon in resultKalibrasyon)
                {
                    string QCihaz = $@"SELECT
	                                        KalibrasyonCihaz.*,
	                                        dbo.FCihazOdemeYapildi(KalibrasyonCihaz.Id) AS OdemeYapildi,
                                            Model.ParentId AS MarkaId,
                                            KalibrasyonCihaz.CihazId AS ModelId,
	                                        Marka.Cihaz AS Marka,
	                                        Model.Cihaz AS Model,
                                            dbo.FCihazDurumuHareketTuru(KalibrasyonCihaz.Id) CihazDurumu,
                                            KalibrasyonCihazAciklama.AciklamaSayisi
                                        FROM
	                                        KalibrasyonCihaz
	                                        LEFT JOIN Cihaz Model ON KalibrasyonCihaz.CihazId = Model.Id
	                                        LEFT JOIN Cihaz Marka ON Model.ParentId = Marka.Id
                                            OUTER APPLY (SELECT
                                                            COUNT(*) AS AciklamaSayisi
                                                        FROM
                                                            KalibrasyonCihazHareket
                                                        WHERE
                                                            KalibrasyonCihazId = KalibrasyonCihaz.Id
                                                            AND ISNULL(Aciklama, '') != '') AS KalibrasyonCihazAciklama
                                        WHERE
	                                        KalibrasyonCihaz.KalibrasyonId = {kalibrasyon.Id.ToString()}";

                    var resultCihaz = await connection.QueryAsync<KalibrasyonCihazDTO>(QCihaz);

                    kalibrasyon.KalibrasyonCihazList = new List<KalibrasyonCihazDTO>();

                    if (resultCihaz.Count() > 0)
                    {
                        var cihazlar = resultCihaz.ToList();

                        for (int i = 0; i < cihazlar.Count; i++)
                        {
                            cihazlar[i].DataSourceModel = new List<LookupBasicDTO>();
                            kalibrasyon.KalibrasyonCihazList.Add(cihazlar[i]);
                        }
                    }

                    kalibrasyon.DetailCount = kalibrasyon.KalibrasyonCihazList.Count();

                    // if (resultCihaz.Count() > 0) kalibrasyon.KalibrasyonCihazList.AddRange(resultCihaz);
                }

                if (filterDTO.DonusTipi == 0)
                {
                    filterDTO.DonusTipi = 1;

                    var RC = await connection.QuerySingleOrDefaultAsync<int>(QKalibrasyon, filterDTO);

                    R.RowCount = RC;
                }

                R.Value = resultKalibrasyon.ToList();
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

        public async Task<ServiceResponse<string>> InsertUpdateKalibrasyon(KalibrasyonDTO dto, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                string Q = $@"SPInsertUpdateKalibrasyon
	                                @Mail,
	                                @TelefonCep,
	                                @AdSoyadBasvuru,
	                                @AdSoyadFatura,
	                                @TCKimlikNoBasvuru,
                                    @TCKimlikNoFatura,
	                                @UnvanBasvuru,
	                                @UnvanFatura,
	                                @VergiDairesiBasvuru,
	                                @VergiDairesiFatura,
	                                @VergiNoBasvuru,
	                                @VergiNoFatura,
	                                @IlceIdBasvuru,
	                                @IlceIdFatura,
	                                @IlceIdTeslim,
	                                @PostaKoduBasvuru,
	                                @PostaKoduFatura,
	                                @PostaKoduTeslim,
	                                @AdresBasvuru,
	                                @AdresFatura,
	                                @AdresTeslim,
	                                @GelisSekli,
	                                @IndirimKodu,
                                    @Id";

                dto.AdSoyadBasvuru = dto.BasvuruTipi == 2 ? "" : dto.AdSoyadBasvuru;
                dto.AdSoyadFatura = dto.FaturaTipi != 2 ? "" : dto.AdSoyadFatura;
                dto.TCKimlikNoBasvuru = dto.BasvuruTipi == 2 ? "" : dto.TCKimlikNoBasvuru;
                dto.TCKimlikNoFatura = dto.FaturaTipi != 2 ? "" : dto.TCKimlikNoFatura;
                dto.UnvanBasvuru = dto.BasvuruTipi == 1 ? "" : dto.UnvanBasvuru;
                dto.UnvanFatura = dto.FaturaTipi != 3 ? "" : dto.UnvanFatura;
                dto.VergiDairesiBasvuru = dto.BasvuruTipi == 1 ? "" : dto.VergiDairesiBasvuru;
                dto.VergiDairesiFatura = dto.FaturaTipi != 3 ? "" : dto.VergiDairesiFatura;
                dto.VergiNoBasvuru = dto.BasvuruTipi == 1 ? "" : dto.VergiNoBasvuru;
                dto.VergiNoFatura = dto.FaturaTipi != 3 ? "" : dto.VergiNoFatura;
                dto.IlIdFatura = dto.FaturaTipi == 1 ? 0 : dto.IlIdFatura;
                dto.IlIdTeslim = dto.TeslimAdresiTipi == 1 ? 0 : dto.IlIdTeslim;
                dto.IlceIdFatura = dto.FaturaTipi == 1 ? 0 : dto.IlceIdFatura;
                dto.IlceIdTeslim = dto.TeslimAdresiTipi == 1 ? 0 : dto.IlceIdTeslim;
                dto.AdresFatura = dto.FaturaTipi == 1 ? "" : dto.AdresFatura;
                dto.AdresTeslim = dto.TeslimAdresiTipi == 1 ? "" : dto.AdresTeslim;
                dto.PostaKoduFatura = dto.FaturaTipi == 1 ? "" : dto.PostaKoduFatura;
                dto.PostaKoduTeslim = dto.TeslimAdresiTipi == 1 ? "" : dto.PostaKoduTeslim;

                var result = await connection.ExecuteScalarAsync<string>(Q, dto);

                if (dto.Id == 0) // Kalibrasyon ekleniyorsa başvurudan geliyor demektir. Bu durumda cihazlar da eklenecek. Yoksa sadece kalibrasyon tablosu update edilir.
                {
                    foreach (var cihaz in dto.KalibrasyonCihazList)
                    {
                        cihaz.KalibrasyonId = Convert.ToInt32(result);

                        await connection.ExecuteScalarAsync<string>("SPInsertUpdateKalibrasyonCihaz @KalibrasyonId, @ModelId, @SeriNo, @TeslimTarihi, @GonderilisTarihi, @Id", cihaz);
                    }
                }

                R.Value = result;
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

        public async Task<ServiceResponse<string>> DeleteKalibrasyon(List<int> Idler, int KisiId)
        {
            var R = new ServiceResponse<string>();

            var dtos = new List<KalibrasyonDTO>();

            try
            {
                using var connection = new SqlConnection(CS);

                foreach (int Id in Idler)
                {
                    var dto = await GetKalibrasyon(Id, KisiId);
                    
                    dtos.Add(dto.Value);
                }

                foreach (int Id in Idler)
                {
                    await connection.ExecuteAsync($@"   DELETE FROM KalibrasyonCihazHareketDosya WHERE KalibrasyonCihazHareketId IN (SELECT Id FROM KalibrasyonCihazHareket WHERE KalibrasyonCihazId IN (SELECT Id FROM KalibrasyonCihaz WHERE KalibrasyonId = {Id}))
                                                        DELETE FROM KalibrasyonCihazHareket WHERE KalibrasyonCihazId IN (SELECT Id FROM KalibrasyonCihaz WHERE KalibrasyonId = {Id})
                                                        DELETE FROM KalibrasyonCihazOdeme WHERE KalibrasyonCihazId IN (SELECT Id FROM KalibrasyonCihaz WHERE KalibrasyonId = {Id})
                                                        DELETE FROM KalibrasyonCihazOlcum WHERE KalibrasyonCihazId IN (SELECT Id FROM KalibrasyonCihaz WHERE KalibrasyonId = {Id})
                                                        DELETE FROM KalibrasyonCihaz WHERE KalibrasyonId = {Id}
                                                        DELETE FROM Kalibrasyon WHERE Id = {Id}");
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

        //////////////////////////////////////////////// KalibrasyonCihaz ////////////////////////////////////////////

        public async Task<ServiceResponse<KalibrasyonCihazDTO>> GetKalibrasyonCihaz(int Id, int KisiId)
        {
            var R = new ServiceResponse<KalibrasyonCihazDTO>();

            try
            {
                using var connection = new SqlConnection(CS);

                string Q = $@"	SELECT
									KalibrasyonCihaz.Id,
									KalibrasyonCihaz.KalibrasyonId,
									KalibrasyonCihaz.CihazId,
									KalibrasyonCihaz.SeriNo,
									KalibrasyonCihaz.KalibrasyonUcreti,
									KalibrasyonCihaz.KdvOrani,
									KalibrasyonCihaz.TeslimTarihi,
									KalibrasyonCihaz.OdemeTarihi,
									KalibrasyonCihaz.ReferansTarihi,
									KalibrasyonCihaz.KalibrasyonTarihi,
									KalibrasyonCihaz.GonderilisTarihi,
									KalibrasyonCihaz.YayinlandigiTarih,
									KalibrasyonCihaz.DozHiziTuruId,
									KalibrasyonCihaz.SonucTuruId,
									KalibrasyonCihaz.SonucAciklama,
                                    KalibrasyonCihaz.ToplamDozTuruId,
									CONVERT(FLOAT, KalibrasyonCihaz.Sicaklik1) AS Sicaklik1,
									CONVERT(FLOAT, KalibrasyonCihaz.Sicaklik2) AS Sicaklik2,
									CONVERT(FLOAT, KalibrasyonCihaz.Basinc1) AS Basinc1,
									CONVERT(FLOAT, KalibrasyonCihaz.Basinc2) AS Basinc2,
									CONVERT(FLOAT, KalibrasyonCihaz.Nem1) AS Nem1,
									CONVERT(FLOAT, KalibrasyonCihaz.Nem2) AS Nem2,
									CONVERT(FLOAT, KalibrasyonCihaz.Bozunma) AS Bozunma,
									CONVERT(FLOAT, KalibrasyonCihaz.ReferansDoz) AS ReferansDoz,
									CONVERT(FLOAT, KalibrasyonCihaz.DDozCarpimDozHizi1) AS DDozCarpimDozHizi1,
									CONVERT(FLOAT, KalibrasyonCihaz.DDozCarpimDozHizi2) AS DDozCarpimDozHizi2,
									CONVERT(FLOAT, KalibrasyonCihaz.DDozCarpimDozHizi3) AS DDozCarpimDozHizi3,
									CONVERT(FLOAT, KalibrasyonCihaz.DDozCarpimDozHizi4) AS DDozCarpimDozHizi4,
									CONVERT(FLOAT, KalibrasyonCihaz.DDozCarpimDozHizi5) AS DDozCarpimDozHizi5,
									CONVERT(FLOAT, KalibrasyonCihaz.DDozCarpimToplamDoz) AS DDozCarpimToplamDoz,
									CONVERT(FLOAT, KalibrasyonCihaz.DDozCikarilanDozHizi1) AS DDozCikarilanDozHizi1,
									CONVERT(FLOAT, KalibrasyonCihaz.DDozCikarilanDozHizi2) AS DDozCikarilanDozHizi2,
									CONVERT(FLOAT, KalibrasyonCihaz.DDozCikarilanDozHizi3) AS DDozCikarilanDozHizi3,
									CONVERT(FLOAT, KalibrasyonCihaz.DDozCikarilanDozHizi4) AS DDozCikarilanDozHizi4,
									CONVERT(FLOAT, KalibrasyonCihaz.DDozCikarilanDozHizi5) AS DDozCikarilanDozHizi5,
									CONVERT(FLOAT, KalibrasyonCihaz.DDozCikarilanToplamDoz) AS DDozCikarilanToplamDoz,
			                        Kalibrasyon.BasvuruTakipNo,
			                        Marka.Cihaz AS Marka,
			                        Model.Cihaz AS Model,
			                        dbo.FCihazDurumuHareketTuru(KalibrasyonCihaz.Id) AS CihazDurumu,
			                        DozHiziTuru.DozHiziTuru,
			                        ToplamDozTuru.ToplamDozTuru,
                                    SonucTuru.SonucTuru,
                                    CihazTuru.Sayi AS ToplamBelirsizlikSiniri,
                                    KalibrasyonCihaz.TurkakCustomerGuid,
                                    KalibrasyonCihaz.TurkakCertificateGuid,
                                    KalibrasyonCihaz.TurkakaGonderilmeZamani,
                                    KalibrasyonCihaz.TurkakTBDSNumber,
                                    KalibrasyonCihaz.TurkakCertificationBodyDocumentNumber,
                                    KalibrasyonCihaz.TurkaktaAktiflestirmeZamani
		                        FROM
			                        KalibrasyonCihaz
			                        LEFT JOIN Kalibrasyon ON KalibrasyonCihaz.KalibrasyonId = Kalibrasyon.Id
			                        LEFT JOIN Cihaz Model ON KalibrasyonCihaz.CihazId = Model.Id
			                        LEFT JOIN Cihaz Marka ON Model.ParentId = Marka.Id
			                        LEFT JOIN DozHiziTuru ON KalibrasyonCihaz.DozHiziTuruId = DozHiziTuru.Id
			                        LEFT JOIN ToplamDozTuru ON KalibrasyonCihaz.ToplamDozTuruId = ToplamDozTuru.Id
                                    LEFT JOIN CihazTuru ON Model.CihazTuruId = CihazTuru.Id
                                    LEFT JOIN SonucTuru ON KalibrasyonCihaz.SonucTuruId = SonucTuru.Id
		                        WHERE
			                        KalibrasyonCihaz.Id = {Id}";

                var result = await connection.QuerySingleOrDefaultAsync<KalibrasyonCihazDTO>(Q);

                R.Value = result;
            }
            catch (Exception ex)
            {
                R.Success = false;
                R.Message = ex.Message;
            }

            if (!R.Success)
            {
                var logReturn = await IslemLog.Logla(configuration, KisiId, MethodBase.GetCurrentMethod().DeclaringType, R.Message, new object(), Id);
                R.Message = logReturn;
            }

            return R;
        }

        public async Task<ServiceResponse<List<KalibrasyonCihazDTO>>> GetKalibrasyonCihazList(KalibrasyonCihazFilterDTO filterDTO, int KisiId)
        {
            var R = new ServiceResponse<List<KalibrasyonCihazDTO>>();

            try
            {
                using var connection = new SqlConnection(CS);

                string Q = $@"SPSelectKalibrasyonCihaz
           	                        @BasvuruTakipNo,
	                                @MarkaId,
	                                @ModelId,
	                                @SeriNo,
	                                @Sayfa,
	                                @KalibrasyonId,
	                                @Id,
	                                @PageNo,
	                                @PageSize,
	                                @DonusTipi";

                var result = await connection.QueryAsync<KalibrasyonCihazDTO>(Q, filterDTO);

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

        public async Task<ServiceResponse<List<KalibrasyonCihazTakipDTO>>> GetMusteriTakip(MusteriTakipFilterDTO filterDTO, int KisiId)
        {
            var R = new ServiceResponse<List<KalibrasyonCihazTakipDTO>>();

            try
            {
                using var connection = new SqlConnection(CS);

                var result = await connection.QueryAsync<KalibrasyonCihazTakipDTO>("SPSelectMusteriTakip @GSM, @Mail, 'Kalibrasyon'", filterDTO);

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

        public async Task<ServiceResponse<string>> InsertUpdateKalibrasyonCihaz(KalibrasyonCihazDTO dto, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                var result = await connection.ExecuteScalarAsync<string>("SPInsertUpdateKalibrasyonCihaz @KalibrasyonId, @ModelId, @SeriNo, @TeslimTarihi, @GonderilisTarihi, @Id", dto);

                R.Value = result;
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

        public async Task<ServiceResponse<string>> DeleteKalibrasyonCihaz(List<int> Idler, int KisiId)
        {
            var R = new ServiceResponse<string>();

            var dtos = new List<KalibrasyonCihazDTO>();

            try
            {
                using var connection = new SqlConnection(CS);

                foreach (int Id in Idler)
                {
                    var dto = await GetKalibrasyonCihaz(Id, KisiId);

                    dtos.Add(dto.Value);
                }

                foreach (int Id in Idler)
                {
                    await connection.ExecuteAsync($@"   DELETE FROM KalibrasyonCihazHareketDosya WHERE KalibrasyonCihazHareketId = {Id}
                                                        DELETE FROM KalibrasyonCihazHareket WHERE KalibrasyonCihazId = {Id}
                                                        DELETE FROM KalibrasyonCihazOdeme WHERE KalibrasyonCihazId = {Id}
                                                        DELETE FROM KalibrasyonCihazOlcum WHERE KalibrasyonCihazId = {Id}
                                                        DELETE FROM KalibrasyonCihaz WHERE Id = {Id}");
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

        //////////////////////////////////////////// KalibrasyonCihazHareket ////////////////////////////////////////////

        public async Task<ServiceResponse<List<KalibrasyonCihazHareketDTO>>> GetKalibrasyonCihazHareketList(KalibrasyonCihazHareketFilterDTO filterDTO, int KisiId)
        {
            var R = new ServiceResponse<List<KalibrasyonCihazHareketDTO>>();

            try
            {
                using var connection = new SqlConnection(CS);

                var result = await connection.QueryAsync<KalibrasyonCihazHareketDTO>($"SPSelectKalibrasyonCihazHareket {filterDTO.KalibrasyonCihazId}, {filterDTO.HareketTuruId}, {filterDTO.Id}");

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

        public async Task<ServiceResponse<string>> InsertKalibrasyonCihazHareket(KalibrasyonCihazHareketDTO dto, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                string Q = $@"  INSERT INTO KalibrasyonCihazHareket
                                    (KalibrasyonCihazId, HareketTuruId, Zaman, Aciklama)
                                VALUES
                                    (@KalibrasyonCihazId, @HareketTuruId, @Zaman, @Aciklama)

                                SELECT ''";

                var result = await connection.ExecuteScalarAsync<string>(Q, dto);

                R.Value = result;
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

        public async Task<ServiceResponse<string>> InsertUpdateKalibrasyonCihazHareket(KalibrasyonCihazHareketDTO dto, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                string Q = $@"      IF ISNULL(@Id, 0) = 0
                                    BEGIN
	                                    INSERT INTO KalibrasyonCihazHareket
		                                    (KalibrasyonCihazId, HareketTuruId, Zaman, Aciklama)
	                                    VALUES
		                                    (@KalibrasyonCihazId, @HareketTuruId, GETDATE(), @Aciklama)

	                                    SELECT @@IDENTITY
                                    END ELSE
                                    BEGIN
	                                    UPDATE KalibrasyonCihazHareket SET
		                                    Aciklama = @Aciklama
	                                    WHERE
		                                    Id = @Id

	                                    SELECT @Id
                                    END";

                var resultQ = await connection.ExecuteScalarAsync<string>(Q, dto);

                if (dto.Id != 0)
                {
                    string QSil = $@"   DELETE
                                        FROM
                                            KalibrasyonCihazHareketDosya
                                        WHERE
                                            KalibrasyonCihazHareketId = {dto.Id}

                                        SELECT {dto.Id}";

                    await connection.ExecuteScalarAsync<string>(QSil);
                }

                foreach (var item in dto.KalibrasyonCihazHareketDosyaList)
                {
                    string QDosya = $@" INSERT INTO KalibrasyonCihazHareketDosya
	                                        (KalibrasyonCihazHareketId, Dosya, DosyaAdi, Uzanti, Boyut)
                                        VALUES
	                                        ({resultQ}, @Dosya, @DosyaAdi, @Uzanti, @Boyut)

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

            var logReturn = await IslemLog.Logla(configuration, KisiId, MethodBase.GetCurrentMethod().DeclaringType, R.Message, dto, dto.Id);

            if (!R.Success) R.Message = logReturn;

            return R;
        }

        public async Task<ServiceResponse<string>> DeleteKalibrasyonCihazHareket(List<int> Idler, int KisiId)
        {
            var R = new ServiceResponse<string>();

            var dtos = new List<KalibrasyonCihazHareketDTO>();

            try
            {
                using var connection = new SqlConnection(CS);

                foreach (int Id in Idler)
                {
                    var filterDTO = new KalibrasyonCihazHareketFilterDTO() { KalibrasyonCihazId = 0, HareketTuruId = 0, Id = Id };

                    var result = await GetKalibrasyonCihazHareketList(filterDTO, KisiId);

                    var dto = result.Value[0];

                    dtos.Add(dto);
                }

                foreach (int Id in Idler)
                {
                    await connection.ExecuteAsync($@"   DELETE FROM KalibrasyonCihazHareketDosya WHERE KalibrasyonCihazHareketId = {Id}
                                                        DELETE FROM KalibrasyonCihazHareket WHERE Id = {Id}");
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

        //////////////////////////////////////////// KalibrasyonCihazOdeme ////////////////////////////////////////////

        public async Task<ServiceResponse<List<KalibrasyonCihazOdemeDTO>>> GetKalibrasyonCihazOdemeList(KalibrasyonCihazOdemeFilterDTO filterDTO, int KisiId)
        {
            var R = new ServiceResponse<List<KalibrasyonCihazOdemeDTO>>();

            try
            {
                using var connection = new SqlConnection(CS);

                var result = await connection.QueryAsync<KalibrasyonCihazOdemeDTO>($"SPSelectKalibrasyonCihazOdeme {filterDTO.KalibrasyonCihazId}, {filterDTO.Id}");

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

        public async Task<ServiceResponse<string>> InsertUpdateKalibrasyonCihazOdeme(KalibrasyonCihazOdemeDTO dto, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                string Q = $@"  IF ISNULL(@Id, 0) < 0
                                BEGIN
	                                UPDATE KalibrasyonCihaz SET
		                                OdemeTarihi = @OdemeTarihi
	                                WHERE
		                                Id = 0 - @Id

	                                SELECT @Id
                                END ELSE
                                IF ISNULL(@Id, 0) = 0
                                BEGIN
	                                INSERT INTO KalibrasyonCihazOdeme
		                                (KalibrasyonCihazId, OdemeTuruId, Ucret, TahakkukTarihi, OdemeTarihi)
	                                VALUES
		                                (@KalibrasyonCihazId, @OdemeTuruId, @Ucret, @TahakkukTarihi, @OdemeTarihi)

	                                SELECT @@IDENTITY
                                END ELSE
                                BEGIN
	                                UPDATE KalibrasyonCihazOdeme SET
		                                OdemeTuruId = @OdemeTuruId,
		                                Ucret = @Ucret,
		                                TahakkukTarihi = @TahakkukTarihi,
		                                OdemeTarihi = @OdemeTarihi
	                                WHERE
		                                Id = @Id

	                                SELECT @Id
                                END";

                var result = await connection.ExecuteScalarAsync<string>(Q, dto);

                R.Value = result;
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

        public async Task<ServiceResponse<string>> DeleteKalibrasyonCihazOdeme(List<int> Idler, int KisiId)
        {
            var R = new ServiceResponse<string>();

            var dtos = new List<KalibrasyonCihazOdemeDTO>();

            try
            {
                using var connection = new SqlConnection(CS);

                foreach (int Id in Idler)
                {
                    var filterDTO = new KalibrasyonCihazOdemeFilterDTO() { KalibrasyonCihazId = 0, Id = Id };

                    var result = await GetKalibrasyonCihazOdemeList(filterDTO, KisiId);

                    var dto = result.Value[0];

                    dtos.Add(dto);
                }

                foreach (int Id in Idler)
                {
                    await connection.ExecuteAsync($"DELETE FROM KalibrasyonCihazOdeme WHERE Id = {Id}");
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

        //////////////////////////////////////////// KalibrasyonCihazHareketDosya ////////////////////////////////////////////

        public async Task<ServiceResponse<List<KalibrasyonCihazHareketDosyaDTO>>> GetKalibrasyonCihazHareketDosyaList(int Id, int KisiId)
        {
            var R = new ServiceResponse<List<KalibrasyonCihazHareketDosyaDTO>>();

            try
            {
                using var connection = new SqlConnection(CS);

                var result = await connection.QueryAsync<KalibrasyonCihazHareketDosyaDTO>($"SELECT * FROM KalibrasyonCihazHareketDosya WHERE KalibrasyonCihazHareketId = {Id}");

                R.Value = result.ToList();
            }
            catch (Exception ex)
            {
                R.Success = false;
                R.Message = ex.Message;
            }

            if (!R.Success)
            {
                var logReturn = await IslemLog.Logla(configuration, KisiId, MethodBase.GetCurrentMethod().DeclaringType, R.Message, Id, Id);
                R.Message = logReturn;
            }

            return R;
        }

        //////////////////////////////////////////// KalibrasyonCihazOlcum ////////////////////////////////////////////

        public async Task<ServiceResponse<List<KalibrasyonCihazOlcumDTO>>> GetKalibrasyonCihazOlcumList(int Id, int KisiId)
        {
            var R = new ServiceResponse<List<KalibrasyonCihazOlcumDTO>>();

            try
            {
                using var connection = new SqlConnection(CS);

                var result = await connection.QueryAsync<KalibrasyonCihazOlcumDTO>($"SPSelectKalibrasyonCihazOlcum {Id}");

                R.Value = result.ToList();
            }
            catch (Exception ex)
            {
                R.Success = false;
                R.Message = ex.Message;
            }

            if (!R.Success)
            {
                var logReturn = await IslemLog.Logla(configuration, KisiId, MethodBase.GetCurrentMethod().DeclaringType, R.Message, Id, Id);
                R.Message = logReturn;
            }

            return R;
        }

        public async Task<ServiceResponse<string>> UpdateKalibrasyonCihazOlcum(KalibrasyonCihazOlcumForKayitDTO dto, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
               using var connection = new SqlConnection(CS);

                string QCihaz = $@"SPUpdateKalibrasyonCihazOlcumDegerleri
                                        @ReferansTarihi,
	                                    @KalibrasyonTarihi,
	                                    @YayinlandigiTarih,
	                                    @Bozunma,
	                                    @DozHiziTuruId,
	                                    @ToplamDozTuruId,
	                                    @Sicaklik1,
	                                    @Sicaklik2,
	                                    @Basinc1,
	                                    @Basinc2,
	                                    @Nem1,
	                                    @Nem2,
	                                    @ReferansDoz,
	                                    @DDozCarpimDozHizi1,
	                                    @DDozCarpimDozHizi2,
	                                    @DDozCarpimDozHizi3,
	                                    @DDozCarpimDozHizi4,
	                                    @DDozCarpimDozHizi5,
	                                    @DDozCarpimToplamDoz,
	                                    @DDozCikarilanDozHizi1,
	                                    @DDozCikarilanDozHizi2,
	                                    @DDozCikarilanDozHizi3,
	                                    @DDozCikarilanDozHizi4,
	                                    @DDozCikarilanDozHizi5,
	                                    @DDozCikarilanToplamDoz,
                                        @SonucTuruId,
                                        @SonucAciklama,
                                        @Id";

                await connection.ExecuteAsync(QCihaz, dto.KalibrasyonCihaz);

                string QOlcum = $@"SPInsertUpdateDeleteKalibrasyonCihazOlcum
                                        @Id,
                                        @KalibrasyonCihazId,
		                                @DozHiziToplamDoz,
		                                @Sira,
		                                @Uzaklik,
                                        @Sure,
		                                @DiskSayisi,
                                        @DDoz,
		                                @BozulmaDuzelme,
		                                @OlcumNicelik,
		                                @ReferansDeger,
		                                @OlcuBirimiId,
		                                @ReferansBelirsizlik,
		                                @Olcum1,
		                                @Olcum2,
		                                @Olcum3,
		                                @Olcum4,
		                                @Olcum5,
		                                @Olcum6,
		                                @Olcum7,
		                                @Olcum8,
		                                @Olcum9,
		                                @Olcum10,
		                                @StandartSapma,
		                                @MusteriBelirsizligi,
		                                @MusteriCihazi,
		                                @Sapma,
		                                @ToplamBelirsizlik";

                foreach (var item in dto.KalibrasyonCihazOlcumList)
                {
                    await connection.ExecuteAsync(QOlcum, item);
                }

                string QHareket = $@"INSERT INTO KalibrasyonCihazHareket
		                                (KalibrasyonCihazId, HareketTuruId, Zaman)
	                                VALUES
		                                ({dto.KalibrasyonCihaz.Id}, {dto.HareketTuru.Id}, GETDATE())

	                                SELECT @@IDENTITY";

                await connection.ExecuteAsync(QHareket);
            }
            catch (Exception ex)
            {
                R.Success = false;
                R.Message = ex.Message;
            }

            var LogReturn = await IslemLog.Logla(configuration, KisiId, MethodBase.GetCurrentMethod().DeclaringType, R.Message, dto, dto.KalibrasyonCihaz.Id);

            if (!R.Success) R.Message = LogReturn;

            return R;
        }

        //////////////////////////////////////////// Turkak ////////////////////////////////////////////

        public async Task<ServiceResponse<string>> TurkakaGonder(KalibrasyonCihazDTO dto, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                var client = new HttpClient();

                // Ayarları çek

                string QAyar = $@"  SELECT
	                                    TurkakApiUrl,
	                                    TurkakUserName,
	                                    TurkakPassword,
	                                    TurkakSignIn,
	                                    TurkakCalibrationCustomerGetData,
	                                    TurkakCalibrationCertificateGetData,
	                                    TurkakCalibrationCertificateAllData,
	                                    TurkakCalibrationCertificateGetCertificate,
	                                    TurkakCalibrationCertificateGetCustomer,
	                                    TurkakCalibrationCustomerSaveData,
	                                    TurkakCalibrationCertificateSaveData
                                    FROM
	                                    Ayar";

                var ayarDTO = await connection.QuerySingleOrDefaultAsync<AyarTurkakDTO>(QAyar);

                // Token Al

                var requestSignIn = new HttpRequestMessage(HttpMethod.Post, ayarDTO.TurkakApiUrl + ayarDTO.TurkakSignIn);

                string contentSignInString = $"[ 'Username': '{ayarDTO.TurkakUserName}',  'Password': '{ayarDTO.TurkakPassword}' ]";

                var contentSignIn = new StringContent(contentSignInString.Replace("[", "{").Replace("]", "}"), null, "application/json");

                requestSignIn.Content = contentSignIn;

                var responseSignIn = await client.SendAsync(requestSignIn);

                responseSignIn.EnsureSuccessStatusCode();

                var signInDTO = await responseSignIn.Content.ReadFromJsonAsync<SignInDTO>();

                // Müşteriyi Türkakta ekle veya güncelle

                string QCustomer = $@"  SELECT
	                                        ISNULL(KalibrasyonCihaz.TurkakCustomerGuid, '') AS ID,
	                                        Ayar.TurkakCountryID AS CountryID,
	                                        Il.TurkakGuid AS CityID,
	                                        Ayar.TurkakFileID AS FileID,
	                                        dbo.FAdSoyad(Kalibrasyon.AdSoyadBasvuru, 1) AS Name,
	                                        dbo.FAdSoyad(Kalibrasyon.AdSoyadBasvuru, 0) AS Surname,
	                                        LTRIM(RTRIM(Kalibrasyon.AdresBasvuru + ISNULL(' ' + Ilce.IlIlce, '') + ISNULL(' ' + Il.IlIlce, ''))) AS Address,
	                                        CONVERT(BIT, 0) AS UseTitleFromTaxNumber,
	                                        CASE
                                                WHEN ISNULL(LTRIM(RTRIM(Kalibrasyon.UnvanBasvuru)), '') = '' THEN Kalibrasyon.AdSoyadBasvuru
                                                ELSE LTRIM(RTRIM(Kalibrasyon.UnvanBasvuru))
                                            END AS BrandInformation,
	                                        Kalibrasyon.VergiNoBasvuru AS TaxNumber,
	                                        '+90' + Kalibrasyon.TelefonCep AS Phone,
	                                        Kalibrasyon.Mail AS EMail,
	                                        CASE
		                                        WHEN ISNULL(LTRIM(RTRIM(Kalibrasyon.UnvanBasvuru)), '') = '' THEN 1
		                                        ELSE 2
	                                        END AS DVAccountType,
	                                        CASE
		                                        WHEN ISNULL(KalibrasyonCihaz.TurkakCustomerGuid, '') = '' THEN ''
		                                        ELSE CONVERT(VARCHAR(50), GETDATE(), 113) + ' tarihli güncelleme'
	                                        END AS UpdateNote
                                        FROM
	                                        KalibrasyonCihaz
	                                        LEFT JOIN Kalibrasyon ON KalibrasyonCihaz.KalibrasyonId = Kalibrasyon.Id
	                                        LEFT JOIN IlIlce Ilce ON Kalibrasyon.IlIlceIdBasvuru = Ilce.Id
	                                        LEFT JOIN IlIlce Il ON Ilce.ParentId = Il.Id
	                                        LEFT JOIN Ayar ON 0 = 0
                                        WHERE
	                                        KalibrasyonCihaz.Id = @KalibrasyonCihazId";

                var customerDTO = await connection.QueryAsync<CustomerDTO>(QCustomer, new { KalibrasyonCihazId = dto.Id });

                string turkakCustomerGuid = "";

                if (string.IsNullOrEmpty(customerDTO.ToList()[0].ID))
                {
                    var requestCustomerSave = new HttpRequestMessage(HttpMethod.Post, ayarDTO.TurkakApiUrl + ayarDTO.TurkakCalibrationCustomerSaveData);

                    requestCustomerSave.Headers.Add("Authorization", $"Bearer {signInDTO.Token}");

                    string contentString = JsonConvert.SerializeObject(customerDTO);

                    requestCustomerSave.Content = new StringContent(contentString, null, "application/json");

                    var responseCustomerSave = await client.SendAsync(requestCustomerSave);

                    responseCustomerSave.EnsureSuccessStatusCode();

                    var resultCustomerSave = await responseCustomerSave.Content.ReadFromJsonAsync<TurkakReturnDTO>();

                    if (resultCustomerSave.Item2.Count > 0)
                    {
                        if (resultCustomerSave.Item2.Count == 1)
                        {
                            if (resultCustomerSave.Item2[0].ErrorDescription.StartsWith("Aynı kuruluş birden fazla kez eklenemez"))
                            {
                                turkakCustomerGuid = resultCustomerSave.Item2[0].ErrorDescription.Replace("Aynı kuruluş birden fazla kez eklenemez! ID: ", "");
                            }
                        }

                        if (string.IsNullOrEmpty(turkakCustomerGuid))
                        {
                            R.Success = false;
                        }

                        if (R.Success == false)
                        {
                            foreach (var item in resultCustomerSave.Item2)
                            {
                                R.Message += item.ErrorDescription + " ";
                            }
                        }
                    }
                    else
                    {
                        turkakCustomerGuid = resultCustomerSave.Item1[0].ID;
                    }
                }
                else
                {
                    turkakCustomerGuid = customerDTO.ToList()[0].ID;
                }

                // Geri dönen TurkakCustomerGuidi KalibrasyonCihaz tablosuna kaydet

                if (R.Success)
                {
                    await connection.ExecuteAsync("UPDATE KalibrasyonCihaz SET TurkakCustomerGuid = @TurkakCustomerGuid WHERE Id = @Id", new { TurkakCustomerGuid = turkakCustomerGuid, Id = dto.Id });

                    // Certificatei Türkakta ekle

                    string QCertificate = $@"SELECT
	                                            '' AS ID,
	                                            KalibrasyonCihaz.TurkakCustomerGuid AS CustomerID,
	                                            KalibrasyonCihaz.YayinlandigiTarih AS FirstReleaseDateOfTheDocument,
	                                            'Radyasyon Ölçüm Cihazı' AS MachineOrDeviceType,
	                                            ISNULL(Marka.Cihaz, '') + ISNULL(' ' + Model.Cihaz, '') + ISNULL(' ' + KalibrasyonCihaz.SeriNo, '') AS DeviceSerialNumber,
	                                            KisiLaboratuvarSorumlusu.Ad + ' ' + KisiLaboratuvarSorumlusu.Soyad AS PersonnelPerformingCalibration,
	                                            Ayar.Adres AS CalibrationLocation,
	                                            Ayar.UreticiSeriNo AS SerialNumberOfReferenceCalibrator,
	                                            KalibrasyonCihaz.KalibrasyonTarihi AS CalibrationDate,
	                                            NULL AS RevisionDate,
	                                            '' AS RevisionNote
                                            FROM
	                                            KalibrasyonCihaz
	                                            LEFT JOIN Cihaz Model ON KalibrasyonCihaz.CihazId = Model.Id
	                                            LEFT JOIN Cihaz Marka ON Model.ParentId = Marka.Id
	                                            LEFT JOIN Ayar ON 0 = 0
	                                            LEFT JOIN Kisi KisiLaboratuvarSorumlusu ON Ayar.KisiIdLaboratuvarSorumlusu = KisiLaboratuvarSorumlusu.Id
                                            WHERE
	                                            KalibrasyonCihaz.Id = @KalibrasyonCihazId";

                    var certificateDTO = await connection.QueryAsync<CertificateDTO>(QCertificate, new { KalibrasyonCihazId = dto.Id });

                    var requestCertificateSave = new HttpRequestMessage(HttpMethod.Post, ayarDTO.TurkakApiUrl + ayarDTO.TurkakCalibrationCertificateSaveData);

                    requestCertificateSave.Headers.Add("Authorization", $"Bearer {signInDTO.Token}");

                    requestCertificateSave.Content = new StringContent(JsonConvert.SerializeObject(certificateDTO), null, "application/json");

                    var responseCertificateSave = await client.SendAsync(requestCertificateSave);

                    responseCertificateSave.EnsureSuccessStatusCode();

                    var resultCertificateSave = await responseCertificateSave.Content.ReadFromJsonAsync<TurkakReturnDTO>();

                    if (resultCertificateSave.Item2.Count > 0)
                    {
                        R.Success = false;

                        foreach (var item in resultCertificateSave.Item2)
                        {
                            R.Message += item.ErrorDescription + " ";
                        }
                    }
                    else
                    {
                        // Geri dönen TurkakCertificateGuidi KalibrasyonCihaz tablosuna kaydet

                        await connection.ExecuteAsync($@"UPDATE KalibrasyonCihaz SET
                                                            TurkakCertificateGuid = @TurkakCertificateGuid,
                                                            TurkakaGonderilmeZamani = GETDATE()
                                                        WHERE
                                                            Id = @Id", new { TurkakCertificateGuid = resultCertificateSave.Item1[0].ID, Id = dto.Id });
                    }
                }

                if (string.IsNullOrEmpty(R.Message) == false)
                {
                    R.Message = "Hata: " + R.Message;
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

        public async Task<ServiceResponse<string>> TurkakQRKoduAl(KalibrasyonCihazDTO dto, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                var client = new HttpClient();

                // Ayarları çek

                string QAyar = $@"  SELECT
	                                    TurkakApiUrl,
	                                    TurkakUserName,
	                                    TurkakPassword,
	                                    TurkakSignIn,
	                                    TurkakCalibrationCustomerGetData,
	                                    TurkakCalibrationCertificateGetData,
	                                    TurkakCalibrationCertificateAllData,
	                                    TurkakCalibrationCertificateGetCertificate,
	                                    TurkakCalibrationCertificateGetCustomer,
	                                    TurkakCalibrationCustomerSaveData,
	                                    TurkakCalibrationCertificateSaveData
                                    FROM
	                                    Ayar";

                var ayarDTO = await connection.QuerySingleOrDefaultAsync<AyarTurkakDTO>(QAyar);

                // Token Al

                var requestSignIn = new HttpRequestMessage(HttpMethod.Post, ayarDTO.TurkakApiUrl + ayarDTO.TurkakSignIn);

                string contentSignInString = $"[ 'Username': '{ayarDTO.TurkakUserName}',  'Password': '{ayarDTO.TurkakPassword}' ]";

                var contentSignIn = new StringContent(contentSignInString.Replace("[", "{").Replace("]", "}"), null, "application/json");

                requestSignIn.Content = contentSignIn;

                var responseSignIn = await client.SendAsync(requestSignIn);

                responseSignIn.EnsureSuccessStatusCode();

                var signInDTO = await responseSignIn.Content.ReadFromJsonAsync<SignInDTO>();

                if (R.Success)
                {
                    // Certificatei aktifleştirmek için Türkakta güncelle

                    string QCertificate = $@"SELECT
	                                            KalibrasyonCihaz.TurkakCertificateGuid AS ID,
                                                1 AS State,
	                                            KalibrasyonCihaz.TurkakCustomerGuid AS CustomerID,
	                                            KalibrasyonCihaz.YayinlandigiTarih AS FirstReleaseDateOfTheDocument,
	                                            ISNULL(Marka.Cihaz, '') + ISNULL(' ' + Model.Cihaz, '') AS MachineOrDeviceType,
	                                            KalibrasyonCihaz.SeriNo AS DeviceSerialNumber,
	                                            KisiLaboratuvarSorumlusu.Ad + ' ' + KisiLaboratuvarSorumlusu.Soyad AS PersonnelPerformingCalibration,
	                                            Ayar.Adres AS CalibrationLocation,
	                                            Ayar.UreticiSeriNo AS SerialNumberOfReferenceCalibrator,
	                                            KalibrasyonCihaz.KalibrasyonTarihi AS CalibrationDate
                                            FROM
	                                            KalibrasyonCihaz
	                                            LEFT JOIN Cihaz Model ON KalibrasyonCihaz.CihazId = Model.Id
	                                            LEFT JOIN Cihaz Marka ON Model.ParentId = Marka.Id
	                                            LEFT JOIN Ayar ON 0 = 0
	                                            LEFT JOIN Kisi KisiLaboratuvarSorumlusu ON Ayar.KisiIdLaboratuvarSorumlusu = KisiLaboratuvarSorumlusu.Id
                                            WHERE
	                                            KalibrasyonCihaz.Id = @KalibrasyonCihazId";

                    var certificateDTO = await connection.QueryAsync<CertificateWithStateDTO>(QCertificate, new { KalibrasyonCihazId = dto.Id });

                    var requestCertificateSave = new HttpRequestMessage(HttpMethod.Post, ayarDTO.TurkakApiUrl + ayarDTO.TurkakCalibrationCertificateSaveData);

                    requestCertificateSave.Headers.Add("Authorization", $"Bearer {signInDTO.Token}");

                    requestCertificateSave.Content = new StringContent(JsonConvert.SerializeObject(certificateDTO), null, "application/json");

                    var responseCertificateSave = await client.SendAsync(requestCertificateSave);

                    responseCertificateSave.EnsureSuccessStatusCode();

                    var resultCertificateSave = await responseCertificateSave.Content.ReadFromJsonAsync<TurkakReturnDTO>();

                    string certificateID = "";

                    if (resultCertificateSave.Item2.Count > 0)
                    {
                        bool HataVar = true;

                        if (resultCertificateSave.Item2.Count == 1)
                        {
                            if (resultCertificateSave.Item2[0].ErrorDescription == "Sertifika revizyonu yapılırken Revizyon Tarihi ve Revizyon Notu değerleri boş gönderilemez!")
                            {
                                HataVar = false;

                                certificateID = resultCertificateSave.Item2[0].ID;
                            }
                        }

                        if (HataVar)
                        {
                            R.Success = false;

                            foreach (var item in resultCertificateSave.Item2)
                            {
                                R.Message += item.ErrorDescription + " ";
                            }
                        }
                    }
                    else
                    {
                        // Geri dönen TurkakCertificateGuidi KalibrasyonCihaz tablosuna kaydet

                        certificateID = resultCertificateSave.Item1[0].ID;

                        string QCertificateSave = @"UPDATE KalibrasyonCihaz SET
                                                        TurkakCertificateGuid = @TurkakCertificateGuid,
                                                        TurkaktaAktiflestirmeZamani = GETDATE()
                                                    WHERE
                                                        Id = @Id";

                        await connection.ExecuteAsync(QCertificateSave, new { TurkakCertificateGuid = certificateID, Id = dto.Id });
                    }

                    // TBDSNumber ve CertificationBodyDocumentNumber bilgilerini al

                    if (R.Success)
                    {
                        var requestGetCertificate = new HttpRequestMessage(HttpMethod.Get, ayarDTO.TurkakApiUrl + ayarDTO.TurkakCalibrationCertificateGetCertificate + "/" + certificateID);

                        requestGetCertificate.Headers.Add("Authorization", $"Bearer {signInDTO.Token}");

                        var responseGetCertificate = await client.SendAsync(requestGetCertificate);

                        responseGetCertificate.EnsureSuccessStatusCode();

                        var resultGetCertificate = await responseGetCertificate.Content.ReadFromJsonAsync<CertificateForNumbersDTO>();

                        // Geri dönen TBDSNumber ve CertificationBodyDocumentNumber bilgilerini KalibrasyonCihaz tablosuna kaydet

                        string TBDSNumber = resultGetCertificate.TBDSNumber;
                        string CertificationBodyDocumentNumber = resultGetCertificate.CertificationBodyDocumentNumber;

                        string QGetCertificate = @"UPDATE KalibrasyonCihaz SET
                                                        TurkakTBDSNumber = @TBDSNumber,
                                                        TurkakCertificationBodyDocumentNumber = @CertificationBodyDocumentNumber,
                                                        TurkaktaAktiflestirmeZamani = GETDATE()
                                                    WHERE
                                                        Id = @Id";

                        await connection.ExecuteAsync(QGetCertificate, new { TBDSNumber = TBDSNumber, CertificationBodyDocumentNumber = CertificationBodyDocumentNumber, Id = dto.Id });
                    }

                    if (string.IsNullOrEmpty(R.Message) == false)
                    {
                        R.Message = "Hata: " + R.Message;
                    }
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
    }
}