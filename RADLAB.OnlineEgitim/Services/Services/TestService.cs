using DocumentFormat.OpenXml.Office2010.Excel;
using RADLAB.Model.DTO;
using RADLAB.Model.FilterDTO;
using RADLAB.Model.ResponseModels;
using RADLAB.OnlineEgitim.Services.Infrastructure;

namespace RADLAB.OnlineEgitim.Services.Services
{
    public class TestService : ITestService
    {
        private readonly HttpClient httpClient;

        public TestService(HttpClient _httpClient)
        {
            httpClient = _httpClient;
        }

        public async Task<ServiceResponse<TestDTO>> GetTest(int Id)
        {
            return await httpClient.GetFromJsonAsync<ServiceResponse<TestDTO>>("Test/GetTest/" + Id.ToString());
        }

        public async Task<ServiceResponse<List<TestDTO>>> GetTestList(TestFilterDTO filterDTO)
        {
            var result = await httpClient.PostAsJsonAsync("Test/GetTestList", filterDTO);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<List<TestDTO>>>();
        }

        public async Task<ServiceResponse<string>> InsertOrUpdateTest(TestDTO dto)
        {
            var result = await httpClient.PostAsJsonAsync("Test/InsertOrUpdateTest", dto);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<string>> DeleteTest(List<TestDTO> dtos)
        {
            var result = await httpClient.PostAsJsonAsync("Test/DeleteTest", dtos);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }
    }
}