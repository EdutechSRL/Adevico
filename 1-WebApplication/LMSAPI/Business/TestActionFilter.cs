using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;


namespace Adevico.WebAPI.ActionFilter
{
    public class TestActionFilter : IActionFilter
    {
        public bool AllowMultiple
        {
            get { return false; }
        }

        public Task<HttpResponseMessage> ExecuteActionFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            return null;
        }
    }
}