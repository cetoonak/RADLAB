using Dapper;
using Microsoft.Extensions.Configuration;
using RADLAB.Business.Abstract;
using RADLAB.Model.DTO;
using System.Data.SqlClient;

namespace RADLAB.Business.Concrete
{
    public class LookupManager : ILookupManager
    {
        private readonly IConfiguration configuration;
        private readonly string CS;

        public LookupManager(IConfiguration _configuration)
        {
            bool debug = false;
#if DEBUG
            debug = true;
#endif

            configuration = _configuration;

            CS = configuration.GetConnectionString(debug ? "Debug" : "Release");
        }

        public async Task<List<LookupBasicDTO>> GetLookupBasic(string TableAndFieldName, string OrderFieldName)
        {
            using var connection = new SqlConnection(CS);

            string order = string.IsNullOrEmpty(OrderFieldName) ? TableAndFieldName : OrderFieldName;

            string Q = $@"SELECT
	                            Id,
                                {TableAndFieldName} AS Ad
                            FROM
	                            {TableAndFieldName}
                            ORDER BY
	                            {order}";

            var result = await connection.QueryAsync<LookupBasicDTO>(Q);

            return result.ToList();
        }

        public async Task<List<LookupBasicDTO>> GetLookupBasicWithKod(string TableAndFieldName, string OrderFieldName)
        {
            using var connection = new SqlConnection(CS);

            string order = string.IsNullOrEmpty(OrderFieldName) ? TableAndFieldName : OrderFieldName;

            string Q = $@"SELECT
	                            Id,
                                Kod + ' - ' + {TableAndFieldName} AS Ad
                            FROM
	                            {TableAndFieldName}
                            ORDER BY
	                            {order}";

            var result = await connection.QueryAsync<LookupBasicDTO>(Q);

            return result.ToList();
        }

        public async Task<List<LookupBasicDTO>> GetLookupFromMasterDetail(string TableAndFieldName, int ParentId)
        {
            using var connection = new SqlConnection(CS);

            string TableAndFieldNameWithoutView = TableAndFieldName.Replace("View", "");

            string Q = $@"SELECT
	                            Id,
                                {TableAndFieldNameWithoutView} AS Ad
                            FROM
	                            {TableAndFieldName}
                            WHERE
                                ISNULL(ParentId, 0) = {ParentId}
                            ORDER BY
	                            {TableAndFieldNameWithoutView}";

            var result = await connection.QueryAsync<LookupBasicDTO>(Q);

            return result.ToList();
        }

        public async Task<List<LookupBasicDTO>> GetCihaz()
        {
            using var connection = new SqlConnection(CS);

            string Q = $@"SELECT
	                            Id,
	                            Cihaz AS Ad
                            FROM
	                            Cihaz
                            WHERE
	                            ISNULL(ParentId, 0) != 0
                            ORDER BY
	                            Cihaz";

            var result = await connection.QueryAsync<LookupBasicDTO>(Q);

            return result.ToList();
        }

        public async Task<List<LookupBasicDTO>> GetEgitimYili()
        {
            using var connection = new SqlConnection(CS);

            string Q = $@"  SELECT DISTINCT
	                            Yil AS Id,
                                Yil AS Ad
                            FROM
	                            (SELECT YEAR(BasvuruBaslangici) AS Yil FROM Kurs UNION ALL
	                            SELECT YEAR(BasvuruBitisi) AS Yil FROM Kurs UNION ALL
	                            SELECT YEAR(EgitimBaslangici) AS Yil FROM Kurs UNION ALL
	                            SELECT YEAR(EgitimBitisi) AS Yil FROM Kurs UNION ALL
	                            SELECT YEAR(YayindanKalkis) AS Yil FROM Kurs) AS S
                            ORDER BY
	                            Yil DESC";

            var result = await connection.QueryAsync<LookupBasicDTO>(Q);

            return result.ToList();
        }

        public async Task<List<LookupBasicDTO>> GetEgitimYapilanIl()
        {
            using var connection = new SqlConnection(CS);

            string Q = $@"SELECT
	                            Id,
                                IlIlce AS Ad
                            FROM
	                            IlIlce
                            WHERE
                                Seviye = 0
                                AND EgitimYapilanIl = 1
                            ORDER BY
	                            IlIlce";

            var result = await connection.QueryAsync<LookupBasicDTO>(Q);

            return result.ToList();
        }

        public async Task<List<LookupBasicDTO>> GetKullanici()
        {
            using var connection = new SqlConnection(CS);

            var result = await connection.QueryAsync<LookupBasicDTO>("SELECT Id, Ad + ' ' + Soyad AS Ad FROM Kisi WHERE Tip = 1 ORDER BY Ad, Soyad");

            return result.ToList();
        }

        public async Task<List<LookupBasicDTO>> GetLookupDistinct(string TabledName, string FieldName)
        {
            using var connection = new SqlConnection(CS);

            string Q = $@"SELECT DISTINCT
                                {FieldName} AS Id,
                                {FieldName} AS Ad
                            FROM
	                            {TabledName}
                            ORDER BY
	                            {FieldName}";

            var result = await connection.QueryAsync<LookupBasicDTO>(Q);

            return result.ToList();
        }

        public async Task<List<LookupBasicDTO>> GetTestVideo()
        {
            using var connection = new SqlConnection(CS);

            string Q = $@"  SELECT
	                            Id,
	                            Test AS Ad,
	                            0 AS Sira
                            FROM
	                            Test

                            UNION ALL

                            SELECT
	                            0 - Id AS Id,
	                            DosyaAdi AS Ad,
	                            1 AS Sira
                            FROM
	                            Video

                            ORDER BY
	                            Sira,
	                            Ad";

            var result = await connection.QueryAsync<LookupBasicDTO>(Q);

            return result.ToList();
        }
    }
}