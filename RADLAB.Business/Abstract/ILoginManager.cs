using RADLAB.Model.DTO;
using RADLAB.Model.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Business.Abstract
{
    public interface ILoginManager
    {
        public Task<ServiceResponse<AuthenticatedUserDTO>> Login(LoginDTO loginDTO);
        public Task<ServiceResponse<AuthenticatedUserDTO>> GetKisiMenu(int Id);
        public Task<ServiceResponse<string>> GetAktivasyonKoduByMail(string mail);
        public Task<ServiceResponse<string>> UpdatePasswordByAktivasyonKodu(LoginDTO dto);
    }
}