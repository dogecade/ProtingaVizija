using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Api.Controllers
{
    public class ImageUploadController : ApiController
    {
        [HttpPost]
        public async Task<HttpResponseMessage> UploadFile(HttpRequestMessage request)
        {

            //patikrinam ar geras requestas, t.y. ar yra failas
            if (!request.Content.IsMimeMultipartContent())
            {
                throw new System.Web.Http.HttpResponseException(System.Net.HttpStatusCode.UnsupportedMediaType);
            }
            string root = System.Web.HttpContext.Current.Server.MapPath("~/faces/");
            var provider = new MultipartFormDataStreamProvider(root);
            try
            {
                await request.Content.ReadAsMultipartAsync(provider);
                foreach (MultipartFileData file in provider.FileData)
                {
                    Trace.WriteLine(file.Headers.ContentDisposition.FileName);
                    Trace.WriteLine("Server file path: " + file.LocalFileName);
                }
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (System.Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.PaymentRequired, "waddup nigga u bad");
            }
        }
    }
}