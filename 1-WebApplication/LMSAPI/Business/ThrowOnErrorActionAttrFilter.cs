using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Diagnostics;
using System.Net.Http;
using System.Web.Http.Controllers;
//using System.Web.Mvc;
using System.Web.Http.Filters;
using System.Web.Management;
using System.Web.Routing;
using Adevico.APIconnection.Core.dto;
using Adevico.WebAPI.Controllers.API;


namespace Adevico.WebAPI.ActionFilter
{
    public class ThrowOnErrorActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            // pre-processing
            //Debug.WriteLine("ACTION 1 DEBUG pre-processing logging");
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            //Debug.WriteLine("ACTION 1 DEBUG  OnActionExecuted Response " + actionExecutedContext.Response.StatusCode.ToString());

            dtoBaseResponse response = null;

            
            dtoBaseResponse resp = null;
            try
            {   
                object test = null;actionExecutedContext.ActionContext.Response.TryGetContentValue(out test);

                if(test != null)
                    resp = test as dtoBaseResponse;
            }
            catch (Exception) {}
            
            if (resp != null && !resp.Success)
            {
                throw new LMSAPI.ResponseException(
                    "Service Exception: " + resp.ServiceErrorCode + " SysException: " + resp.ErrorInfo,
                    resp.ServiceErrorCode,
                    resp.ErrorInfo.ToString());

                //throw new Exception("Service Exception: " + resp.ServiceErrorCode + " SysException: " + resp.ErrorInfo);
            }    
            
            

        }

        //public override void OnResultExecuting(HttpActionExecutedContext actionExecutedContext)
        //{
        //    Debug.WriteLine("ACTION 1 DEBUG  OnActionExecuted Response " + actionExecutedContext.Response.StatusCode.ToString());
        //}

        //public override void OnResultExecuted(HttpActionExecutedContext actionExecutedContext)
        //{
        //    Debug.WriteLine("ACTION 1 DEBUG  OnActionExecuted Response " + actionExecutedContext.Response.StatusCode.ToString());
        //}

    }

    //public class ThrowOnErrorActionFilter : ActionFilterAttribute
    //{
    //    public override void OnActionExecuting(ActionExecutingContext filterContext)
    //    {
    //        Log("OnActionExecuting", filterContext.RouteData);
    //    }

    //     public override void OnActionExecuted(ActionExecutedContext filterContext)
    //    {
    //        Log("OnActionExecuted", filterContext.RouteData);
    //    }

    //    public override void OnResultExecuting(ResultExecutingContext filterContext)
    //    {
    //        Log("OnResultExecuting", filterContext.RouteData);
    //    }

    //    public override void OnResultExecuted(ResultExecutedContext filterContext)
    //    {
    //        Log("OnResultExecuted", filterContext.RouteData);
    //    }


    //    private void Log(string methodName, RouteData routeData)
    //    {
    //        var controllerName = routeData.Values["controller"];
    //        var actionName = routeData.Values["action"];
    //        var message = String.Format("{0} controller:{1} action:{2}", methodName, controllerName, actionName);
    //        Debug.WriteLine(message, "Action Filter Log");
    //    }

    //}
}