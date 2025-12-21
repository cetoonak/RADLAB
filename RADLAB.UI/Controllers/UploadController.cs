using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using RADLAB.Model.ResponseModels;
using System.Text;

namespace RADLAB.UI.Controllers
{
    public class UploadController : Controller
    {
        private readonly IWebHostEnvironment environment;

        public UploadController(IWebHostEnvironment environment)
        {
            this.environment = environment;
        }

        [HttpPost("upload/image")]
        public IActionResult Image(IFormFile file)
        {
            try
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

                using (var stream = new FileStream(Path.Combine(environment.WebRootPath, fileName), FileMode.Create))
                {
                    // Save the file
                    file.CopyTo(stream);

                    // Return the URL of the file
                    var url = Url.Content($"~/{fileName}");

                    return Ok(new { Url = url });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [AllowAnonymous]
        //[HttpPost("[action]")]
        [HttpPost("Upload/CreateHtmlFile")]
        public ServiceResponse<string> CreateHtmlFile([FromBody] string icerik)
        {
            var R = new ServiceResponse<string>();

            try
            {
                string FN = "VBPB" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".html";

                var path = Path.Combine(environment.ContentRootPath, "wwwroot/upload/VakifBankPostBack/" + FN);

                using (FileStream fs = System.IO.File.Create(path))
                {
                    byte[] content = new UTF8Encoding(true).GetBytes(icerik);

                    fs.Write(content, 0, content.Length);
                }

                R.Value = FN;
            }
            catch (Exception ex)
            {
                R.Message = ex.Message;
                R.Success = false;
            }

            return R;
        }
    }
}
