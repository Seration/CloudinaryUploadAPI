using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudinaryMediaUpload.API.Model
{
    public class UploadResultDto
    {
        public UploadResultDto()
        {
            MediaUrl = new List<string>();
        }

        public List<string> MediaUrl { get; set; }
    }
}
