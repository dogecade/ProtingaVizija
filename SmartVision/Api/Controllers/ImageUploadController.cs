using Api.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace Api.Controllers
{
    public class ImageUploadController : ApiController
    {
        // post /api/ImageUpload , image in request body , multipart format
        // if all ok - 200, file location in response JSON
        [HttpPost]
        public async Task<IHttpActionResult> UploadFile(HttpRequestMessage request)
        {

            //patikrinam ar geras requestas, t.y. ar yra failas
            if (!request.Content.IsMimeMultipartContent())
            {
                throw new System.Web.Http.HttpResponseException(System.Net.HttpStatusCode.UnsupportedMediaType);
            }
            string root = System.Web.HttpContext.Current.Server.MapPath("~/img");
            var provider = new MyMultipartFormDataStreamProvider(root);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<string> strings = new List<string>();
            try
            {
                await request.Content.ReadAsMultipartAsync(provider);
                foreach (MultipartFileData file in provider.FileData)
                {
                    strings.Add(file.LocalFileName.Substring(file.LocalFileName.LastIndexOf('\\')));
                }

                return Ok(serializer.Serialize(strings));
            }
            catch (System.Exception e)
            {
                return BadRequest(e.StackTrace);
            }
        }
    }
    public class MyMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
    {
        public MyMultipartFormDataStreamProvider(string path) : base(path)
        { }

        public override string GetLocalFileName(System.Net.Http.Headers.HttpContentHeaders headers)
        {
            // override the filename which is stored by the provider (by default is bodypart_x)
            string originalFileName = headers.ContentDisposition.FileName.Trim('\"');

            return originalFileName;
        }
    }

}