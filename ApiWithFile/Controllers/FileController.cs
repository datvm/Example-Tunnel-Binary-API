using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiWithFile.Controllers
{

    [Route("file")]
    [ApiController]
    public class FileController : ControllerBase
    {

        // This API call another API to get the file, it cannot access the file
        [Route("api1")]
        public async Task<IActionResult> Api1()
        {
            // In production you should not initialize HttpClient yourself
            using (var httpClient = new HttpClient())
            {
                // Url should be a server in production
                var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:44322/file/api2");

                using (var response = await httpClient.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();

                    var content = await response.Content.ReadAsByteArrayAsync();
                    return this.File(content, "image/png", "background.png");
                }
            }
        }

        // This API returns a file
        [Route("api2")]
        public IActionResult Api2()
        {
            var filePath = Path.Combine(
                Path.GetDirectoryName(typeof(FileController).Assembly.Location),
                "background.png");

            var fileStream = System.IO.File.OpenRead(filePath);
            return this.File(fileStream, "image/png");
        }

    }
}