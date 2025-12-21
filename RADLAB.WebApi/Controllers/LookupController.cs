using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RADLAB.Business.Abstract;
using RADLAB.Business.Concrete;
using RADLAB.Model.DTO;

namespace RADLAB.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LookupController : ControllerBase
    {
        private readonly ILookupManager lookupManager;

        public LookupController(ILookupManager _lookupManager)
        {
            lookupManager = _lookupManager;
        }

        [HttpGet("[action]/{TableAndFieldName}/{OrderFieldName}")]
        public async Task<List<LookupBasicDTO>> GetLookupBasic(string TableAndFieldName, string OrderFieldName)
        {
            return await lookupManager.GetLookupBasic(TableAndFieldName, OrderFieldName);
        }

        [HttpGet("[action]/{TableAndFieldName}/{OrderFieldName}")]
        public async Task<List<LookupBasicDTO>> GetLookupBasicWithKod(string TableAndFieldName, string OrderFieldName)
        {
            return await lookupManager.GetLookupBasicWithKod(TableAndFieldName, OrderFieldName);
        }

        [AllowAnonymous]
        [HttpGet("[action]/{TableAndFieldName}/{ParentId:int}")]
        public async Task<List<LookupBasicDTO>> GetLookupFromMasterDetail(string TableAndFieldName, int ParentId)
        {
            return await lookupManager.GetLookupFromMasterDetail(TableAndFieldName, ParentId);
        }

        [HttpGet("[action]")]
        public async Task<List<LookupBasicDTO>> GetCihaz()
        {
            return await lookupManager.GetCihaz();
        }

        [HttpGet("[action]")]
        public async Task<List<LookupBasicDTO>> GetEgitimYili()
        {
            return await lookupManager.GetEgitimYili();
        }

        [AllowAnonymous]
        [HttpGet("[action]")]
        public async Task<List<LookupBasicDTO>> GetEgitimYapilanIl()
        {
            return await lookupManager.GetEgitimYapilanIl();
        }

        [HttpGet("[action]")]
        public async Task<List<LookupBasicDTO>> GetKullanici()
        {
            return await lookupManager.GetKullanici();
        }

        [HttpGet("[action]/{TabledName}/{FieldName}")]
        public async Task<List<LookupBasicDTO>> GetLookupDistinct(string TabledName, string FieldName)
        {
            return await lookupManager.GetLookupDistinct(TabledName, FieldName);
        }

        [HttpGet("[action]")]
        public async Task<List<LookupBasicDTO>> GetTestVideo()
        {
            return await lookupManager.GetTestVideo();
        }
    }
}