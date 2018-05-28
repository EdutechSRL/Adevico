using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Adevico.WebAPI.Repository;
using System.IO;
using System.Net.Http.Headers;

namespace Adevico.WebAPI.Controllers.API
{
    //[RoutePrefix("api/photo")]
    public class RepositoryController : BaseApiAuthenticatedController //ApiController // 
    {
        private IPhotoManager photoManager;

        public RepositoryController() : this(new LocalPhotoManager(HttpRuntime.AppDomainAppPath + @"\RepositoryFiles"))
        {
        }

        public RepositoryController(IPhotoManager photoManager)
        {
            this.photoManager = photoManager;
        }

        // GET: api/Photo
        public async Task<IHttpActionResult> Get()
        {
            var results = await photoManager.Get();
            return Ok(new { photos = results });
        }
        // GET: api/Photo/?fileName=x
        public async  Task<HttpResponseMessage> Get([FromUri]string fileName)
        {
            if (String.IsNullOrEmpty(fileName))
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            string localFilePath;

            localFilePath =  System.Web.HttpContext.Current.Server.MapPath(@"~/RepositoryFiles/" + fileName);

            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StreamContent(new FileStream(localFilePath, FileMode.Open, FileAccess.Read));
            response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = fileName;
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpg"); //("application/pdf");

            return response;
        }

        // POST: api/Photo
        public async Task<IHttpActionResult> Post()
        {
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent("form-data"))
            {
                return BadRequest("Unsupported media type");
            }

            try
            {
                var photos = await photoManager.Add(Request);
                return Ok(new { Message = "Photos uploaded ok", Photos = photos });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.GetBaseException().Message);
            }
        }

        // DELETE: api/Photo/5
        [HttpDelete]
        [Route("{fileName}")]
        public async Task<IHttpActionResult> Delete(string fileName)
        {
            if (!this.photoManager.FileExists(fileName))
            {
                return NotFound();
            }

            var result = await this.photoManager.Delete(fileName);

            if (result.Successful)
            {
                return Ok(new { message = result.Message });
            }
            else
            {
                return BadRequest(result.Message);
            }
        }
    }
}
