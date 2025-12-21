using RADLAB.Model.DTO;
using RADLAB.Model.FilterDTO;
using RADLAB.Model.ResponseModels;

namespace RADLAB.UI.Services.Infrastructure
{
    public interface ITestService
    {
        public Task<ServiceResponse<TestDTO>> GetTest(int Id);
        public Task<ServiceResponse<List<TestDTO>>> GetTestList(TestFilterDTO filterDTO);
        public Task<ServiceResponse<string>> InsertOrUpdateTest(TestDTO dto);
        public Task<ServiceResponse<string>> DeleteTest(List<TestDTO> dtos);
    }
}