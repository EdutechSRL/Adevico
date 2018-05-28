using System;
using System.Web;
using System.Web.Http;
using Adevico.WebAPI.Controllers.API;


using Adevico.APIconnection.Core.dto;

using Adevico.APIconnection.Core;
using Adevico.Core.Core.dto;

using Adevico.APIconnection.Core.Business;
using Adevico.WebAPI.Helper;
using lm.Comol.Core.Data;
using lm.Comol.Core.DomainModel;
using NHibernate;


namespace Adevico.WebAPI.ActionFilter
{
    
    public class AdevicoAuthorizeAttribute : System.Web.Http.AuthorizeAttribute // System.Web.Http.AuthorizeAttribute
    {
        public override void OnAuthorization(
           System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            dtoBaseResponse resp = new dtoBaseResponse();

            resp.ErrorInfo = ContextHelper.UserContextLoad(actionContext);

            resp.Success = (resp.ErrorInfo == GenericError.None);

            Helper.ContextHelper.CheckResponse(resp);
            
        }
        
    }
}