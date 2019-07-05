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

        [HttpPost("UploadMedia"), DisableRequestSizeLimit]
        public IActionResult UploadMedia([FromForm] IFormFile[] File)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            UploadResultDto result = new UploadResultDto();
            var uploadResult = new ImageUploadResult();

            if (File.Length > 0)
            {
                for (int i = 0; i < File.Length; i++)
                {
                    using (var stream = File[i].OpenReadStream())
                    {
                        var uploadParams = new ImageUploadParams()
                        {
                            File = new FileDescription(File[i].Name, stream),
                            Transformation = new Transformation()
                                .Width(800),
                        };

                        uploadResult = _cloudinary.Upload(uploadParams);
                    }

                    result.MediaUrl.Add(uploadResult.Uri.ToString());
                }
            }
            return Ok(result);
        }
    }
}