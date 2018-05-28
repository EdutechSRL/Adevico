using System;
using System.Net;
using System.Web;
using System.Web.Http;
using Adevico.APIconnection.Core;
using Adevico.APIconnection.Core.dto;
using Adevico.WebAPI.ActionFilter;



namespace Adevico.WebAPI.Controllers.API
{
    //[OverrideActionFilters]
    public class SampleController : BaseApiAuthenticatedController // ApiController
    {
        // GET api/esempio
        //[AdevicoAuthorizeAttribute]
        [ThrowOnErrorActionFilter]
        public FooResponse Get()
        {
            
            
            FooResponse model = new FooResponse();

            try
            {
                model.token = base.UserContext.CurrentUserID.ToString();
                model.deviceId = base.UserContext.CurrentUser.Name
                    + "-"
                    + base.UserContext.CurrentUser.Name;
            }

            catch (Exception ex)
            {
                model.token = ex.ToString();
            }

            // Error Sample

            model.Success = false;
            model.ErrorInfo = GenericError.Internal;
            model.ServiceErrorCode = (int)GenericError.InvalidDataInput;
            
            return model;

        }

       

        // POST api/esempio
        public void Post([FromBody]string value)
        {
            throw new HttpResponseException(HttpStatusCode.MethodNotAllowed);
        }

        // PUT api/esempio/5
        public void Put(int id, [FromBody]string value)
        {
            throw new HttpResponseException(HttpStatusCode.MethodNotAllowed);
        }

        // DELETE api/esempio/5
        public void Delete(int id)
        {
            throw new HttpResponseException(HttpStatusCode.MethodNotAllowed);
        }
    }


    public class FooResponse : dtoBaseResponse
    {
        public string token { get; set; }
        public string deviceId { get; set; }

    }

}
