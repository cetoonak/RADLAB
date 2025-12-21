using RADLAB.Model.DTO;
using RADLAB.Model.FilterDTO;
using RADLAB.Model.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Business.Abstract
{
    public interface ITestManager
    {
        public Task<ServiceResponse<TestDTO>> GetTest(int Id, int KisiId);
        public Task<ServiceResponse<List<TestDTO>>> GetTestList(TestFilterDTO filterDTO, int KisiId);
        public Task<ServiceResponse<string>> InsertOrUpdateTest(TestDTO dto, int KisiId);
        public Task<ServiceResponse<string>> DeleteTest(List<TestDTO> dtos, int KisiId);
    }
}