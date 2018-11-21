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
            var provider = new MultipartFormDataStreamProvider(root);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<string> strings = new List<string>();
            try
            {
                await request.Content.ReadAsMultipartAsync(provider);
                foreach (MultipartFileData file in provider.FileData)
                {
                    strings.Add(file.LocalFileName);
                }

                return Ok(serializer.Serialize(strings));
            }
            catch (System.Exception e)
            {
                return BadRequest(e.StackTrace);
            }
        }
    }
}