using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CloudinaryMediaUpload.API.Filters;
using CloudinaryMediaUpload.API.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CloudinaryMediaUpload.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaUploadController : ControllerBase
    {
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private Cloudinary _cloudinary;

        public MediaUploadController(IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _cloudinaryConfig = cloudinaryConfig;

            Account acc = new Account(
               _cloudinaryConfig.Value.CloudName,
               _cloudinaryConfig.Value.ApiKey,
               _cloudinaryConfig.Value.ApiSecret
           );

            _cloudinary = new Cloudinary(acc);
        }

        [ServiceFilter(typeof(TokenFilter))]
        [HttpPost("UploadMedia"), DisableRequestSizeLimit]
        public async Task<IActionResult> UploadMedia([FromForm] IFormFile File)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var uploadResult = new ImageUploadResult();

            if (File.Length > 0)
            {
                using (var stream = File.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(File.Name, stream),
                        Transformation = new Transformation()
                            .Width(800),
                    };

                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }

            return Ok(uploadResult.Uri.ToString());
        }
    }
}