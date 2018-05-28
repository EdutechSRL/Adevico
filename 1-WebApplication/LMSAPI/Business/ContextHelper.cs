using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Adevico.APIconnection.Core;
using Adevico.APIconnection.Core.dto;
using lm.Comol.Core.DomainModel;
using NHibernate;
using NHibernate.Type;
using System.Net.Http.Headers;
using Adevico.Core.Domain;
using Adevico.Core.Presentation;
using Adevico.DataManager;
using Adevico.WebAPI.ActionFilter;
using LMSAPI.Business;
using DataContext = lm.Comol.Core.Data.DataContext;

namespace Adevico.WebAPI.Helper
{
    public static class ContextHelper
    {
        //public static string AppContextKey = "ApplicationContext";

        public static string ContextUserKey = "UserContext";
        public static string ContextDataKey = "DataContext";
        public static string ContextStatisticsDataContextKey = "StatisticsDataContext";
        public static string ContextAPIKey = "APIContext";
        public static string ContextLastErrorKey = "LastError";
        public static string ContextFactoryKey = "HibernateFactory";

        public static string QueryStringToken = "Token";
        public static string QueryStringTokenType = "TokenType";
        public static string QueryStringDeviceId = "DeviceId";
        public static string QueryStringCommnuityId = "CommunityId";

        public static string QueryStringLinkId = "LinkId";

        public static string ActionConnectionFile = "~/hibernate.userAction.cfg.xml.config";

        public static iDataContext DataContextLoad()
        {
            NHibernate.ISession currentSession = FactoryGet().OpenSession(); //factory.OpenSession();
            return new DataContext(currentSession);
        }

        public static ISessionFactory FactoryGet()
        {
            ISessionFactory factory = null;

            if (HttpContext.Current != null)
            {
                try
                {
                    factory = (ISessionFactory) HttpContext.Current.Application[ContextFactoryKey];
                }
                catch (Exception)
                {   
                }
                    
            }

            if (factory == null)
            {
                try
                {
                    NHibernate.Cfg.Configuration cfg = new NHibernate.Cfg.Configuration();
                    cfg.Configure(System.Web.HttpContext.Current.Server.MapPath("~/hibernate.cfg.xml.config"));
                    factory = cfg.BuildSessionFactory();

                    if (HttpContext.Current != null)
                    {
                        HttpContext.Current.Application[ContextFactoryKey] = factory;
                    }
                }
                catch (Exception)
                {
                }
            }
            
            return factory;
        }

        public static void FactoryDestroy()
        {
            ISessionFactory factory = FactoryGet();
            if (factory != null)
            {
                factory.Close();
                if (HttpContext.Current != null)
                {
                    HttpContext.Current.Application[ContextFactoryKey] = null;
                }

            }
        }

        public static string TokenGet(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            string token = "";

            try
            {
                IEnumerable<string> headerValues = actionContext.Request.Headers.GetValues(QueryStringToken);
                token = headerValues.FirstOrDefault();
            }
            catch (Exception ex)
            {
                string err = ex.ToString();
            }


            //ToDo: Togliere x SOLO HEADER
            if (string.IsNullOrEmpty(token))
            {
                try
                {
                    var queryStringCollection = HttpUtility.ParseQueryString(actionContext.Request.RequestUri.Query);
                    token = queryStringCollection[QueryStringToken];
                }
                catch (Exception ex)
                {
                    string err = ex.ToString();
                }
            }


            if (String.IsNullOrEmpty(token))
            {
                try
                {

                    CookieHeaderValue headerValues = actionContext.Request.Headers.GetCookies(QueryStringToken).FirstOrDefault();
                    if (headerValues != null)
                        token = headerValues[QueryStringToken].Value;
                }
                catch (Exception ex)
                {
                    string err = ex.ToString();
                }    
            }

            return token;
        }


        public static string DeviceIdGet(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            string deviceId = "";

            try
            {
                IEnumerable<string> headerValues = actionContext.Request.Headers.GetValues(QueryStringDeviceId);
                deviceId = headerValues.FirstOrDefault();
            }
            catch (Exception)
            {

            }

            //ToDo: Togliere x SOLO HEADER
            if (string.IsNullOrEmpty(deviceId))
            {
                try
                {
                    var queryStringCollection = HttpUtility.ParseQueryString(actionContext.Request.RequestUri.Query);
                    deviceId = queryStringCollection[QueryStringDeviceId];
                }
                catch (Exception)
                {
                }
            }

            if (string.IsNullOrEmpty(deviceId))
            {
                try
                {
                    CookieHeaderValue headerValues =
                        actionContext.Request.Headers.GetCookies(QueryStringDeviceId).FirstOrDefault();
                    if (headerValues != null)
                        deviceId = headerValues[QueryStringDeviceId].Value;
                }
                catch
                {

                }
            }

            return deviceId;
        }

        public static TokenType TokenTypeGet(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            TokenType type = TokenType.None;

            try
            {
                IEnumerable<string> headerValues = actionContext.Request.Headers.GetValues(QueryStringTokenType);
                type = (TokenType)System.Convert.ToInt32(headerValues.FirstOrDefault());
            }
            catch (Exception)
            {

            }

            if (type == TokenType.None)
            {
                try
                {
                    CookieHeaderValue headerValues =
                        actionContext.Request.Headers.GetCookies(QueryStringTokenType).FirstOrDefault();
                    if (headerValues != null)
                        type = (TokenType)System.Convert.ToInt32(headerValues[QueryStringTokenType].Value);
                }
                catch
                {

                }
            }

            //ToDo: Togliere x SOLO HEADER
            if (type == TokenType.None)
            {
                try
                {
                    var queryStringCollection = HttpUtility.ParseQueryString(actionContext.Request.RequestUri.Query);
                    type = (TokenType)System.Convert.ToInt32(queryStringCollection[QueryStringTokenType]);
                }
                catch (Exception)
                {
                }
            }
            return type;
        }



        /// <summary>
        /// Recupera il Community Id dall'Header o dalla querystring
        /// </summary>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        public static int CommunityIdGet(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            int communityId = 0;

            try
            {
                IEnumerable<string> headerValues = actionContext.Request.Headers.GetValues(QueryStringCommnuityId);
                communityId = System.Convert.ToInt32(headerValues.FirstOrDefault());
            }
            catch (Exception)
            {

            }

            if (communityId <= 0)
            {
                try
                {
                    var queryStringCollection = HttpUtility.ParseQueryString(actionContext.Request.RequestUri.Query);
                    communityId = System.Convert.ToInt32(queryStringCollection[QueryStringCommnuityId]);
                }
                catch (Exception)
                {
                }
            }

            if (communityId <= 0)
            {
                try
                {
                    CookieHeaderValue headerValues = actionContext.Request.Headers.GetCookies().FirstOrDefault();
                    if (headerValues != null)
                        communityId = System.Convert.ToInt32(headerValues[CookieHelper.CookieKeyCommunityId].Value);
                }
                catch
                {

                }
            }
            
            return communityId;
        }

        public static Int64 LinkIdGet(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            Int64 linkId = 0;

            //Header Value
            try
            {
                IEnumerable<string> headerValues = actionContext.Request.Headers.GetValues(QueryStringLinkId);
                linkId = System.Convert.ToInt64(headerValues.FirstOrDefault());
            }
            catch (Exception)
            {

            }

            //Cookie: NO!
            //if (linkId <= 0)
            //{
            //    try
            //    {
            //        CookieHeaderValue headerValues = actionContext.Request.Headers.GetCookies().FirstOrDefault();
            //        if (headerValues != null)
            //            linkId = System.Convert.ToInt64(headerValues[CookieHelper.CookieKeyLinkId].Value);
            //    }
            //    catch
            //    {

            //    }
            //}

            // QueryString
            if (linkId <= 0)
            {
                try
                {
                    var queryStringCollection = HttpUtility.ParseQueryString(actionContext.Request.RequestUri.Query);
                    linkId = System.Convert.ToInt32(queryStringCollection[QueryStringLinkId]);
                }
                catch (Exception)
                {
                }
            }

            return linkId;
        }

       public static iUserContext UserContext
        {
            get
            {
                iUserContext uc = null;

                if (HttpContext.Current != null)
                {
                    try
                    {
                        uc = (iUserContext)HttpContext.Current.Items[Helper.ContextHelper.ContextUserKey];
                    }
                    catch { }

                }

                return uc;
            }
            set
            {
                if (HttpContext.Current != null)
                {
                    HttpContext.Current.Items[Helper.ContextHelper.ContextUserKey] = value;
                }
            }
        }

        public static ApiContext APIContext
        {
            get
            {
                ApiContext ac = null;

                if (HttpContext.Current != null)
                {
                    try
                    {
                        ac = (ApiContext)HttpContext.Current.Items[Helper.ContextHelper.ContextAPIKey];
                    }
                    catch { }

                }

                return ac;
            }
            set
            {
                if (HttpContext.Current != null)
                {
                    HttpContext.Current.Items[Helper.ContextHelper.ContextAPIKey] = value;
                }
            }
        }


        //ToDo: cercare di impostarlo su APPLICATION!
        public static iDataContext DataContext
        {
            get
            {

                iDataContext preLoadedContext = null;

                if (HttpContext.Current != null)
                {
                    try
                    {
                        preLoadedContext = (iDataContext)HttpContext.Current.Items[Helper.ContextHelper.ContextDataKey];
                    }
                    catch { }

                }

                if (preLoadedContext == null)
                {
                    preLoadedContext = Helper.ContextHelper.DataContextLoad();
                    HttpContext.Current.Items[Helper.ContextHelper.ContextDataKey] = preLoadedContext;
                }
                
                return preLoadedContext;
            }
        }

        public static iApplicationContext AppContext
        {
            get
            {
                iApplicationContext ctx = new ApplicationContext();
                ctx.DataContext = DataContext;
                ctx.UserContext = UserContext;
                return ctx;
            }
        }

        public static GenericError UserContextLoad(
            System.Web.Http.Controllers.HttpActionContext actionContext = null, 
            string token = "",
            TokenType type = TokenType.None,
            string deviceId = "",
            int communityId = 0,
            int languageId = 1,
            Int64 linkId = 0
            )
        {
            LastError = GenericError.None;

            if(String.IsNullOrEmpty(token) && actionContext != null)
                token = Helper.ContextHelper.TokenGet(actionContext);

            if (type == TokenType.None && actionContext != null)
                type = Helper.ContextHelper.TokenTypeGet(actionContext);

            if (String.IsNullOrEmpty(deviceId) && actionContext != null)
                deviceId = Helper.ContextHelper.DeviceIdGet(actionContext);
            
            
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(deviceId))
            {
                LastError = GenericError.TokenInvalid;
                return LastError;
            }

            if (communityId <= 0)
            {
                communityId = CommunityIdGet(actionContext);
            }

            if (linkId <= 0)
            {
                linkId = LinkIdGet(actionContext);
            }
            
            iDataContext dataContext = Helper.ContextHelper.DataContext;

            
            Adevico.APIconnection.Core.Business.CoreAPIService coreApiService =
                new Adevico.APIconnection.Core.Business.CoreAPIService(dataContext);

            iPerson currentUser = coreApiService.PersonGetFromToken(token, deviceId, type);

            iUserContext userContext;

            if (languageId <= 0)
                languageId = currentUser.LanguageID;

            Language lang = coreApiService.LanguageGet(languageId);

            int workingCommunityId = coreApiService.CommunityIdGetFromLink(linkId);

            if (currentUser != null && currentUser.Id > 0)
            {
                userContext = new UserContext
                {
                    CurrentCommunityID = communityId,
                    CurrentCommunityOrganizationID = 0,
                    CurrentUser = currentUser,
                    IpAddress = "--", //Todo
                    ProxyIpAddress = "--", //Todo
                    isAnonymous = false,
                    Language = lang,
                    WorkingCommunityID = workingCommunityId
                };

            }
            else
            {
                userContext = new UserContext();
                LastError = GenericError.TokenExpired;
                //return LastError;
            }

            //API Context
            APIContext = new ApiContext();
            APIContext.CurrentLinkId = linkId;


            Helper.ContextHelper.UserContext = userContext;

            return LastError;
        }

        public static GenericError LastError
        {
            get
            {
                GenericError err = GenericError.None;
                if (HttpContext.Current != null)
                {
                    err = (GenericError)HttpContext.Current.Items[Helper.ContextHelper.ContextLastErrorKey];
                }

                return err;
            }

            set
            {
                if (HttpContext.Current != null)
                {
                    HttpContext.Current.Items[Helper.ContextHelper.ContextLastErrorKey] = value;
                }
            }

        }

        public static void CheckResponse(dtoBaseResponse response)
        {
            if (!response.Success)
            {
                HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.NotImplemented);

                switch (response.ErrorInfo)
                {
                    case GenericError.None:
                        message = new HttpResponseMessage(HttpStatusCode.Accepted);
                        break;
                    case GenericError.TokenExpired:
                    case GenericError.TokenInvalid:
                        message = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                        break;

                    case GenericError.InvalidDataInput:
                        message = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        break;

                    case GenericError.NoServicePermission:
                        message = new HttpResponseMessage(HttpStatusCode.Forbidden);
                        break;

                    //case GenericError.Internal:
                    //case GenericError.Unknow:
                    default:
                        message = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                        break;
                }

                message.Content = new StringContent(String.Format("Code: {0}\r\nMessage: {1}", ((int)response.ErrorInfo).ToString(), response.ErrorInfo.ToString()));

                throw new HttpResponseException(message);
            }



        }



        public static Adevico.DataManager.Interface.iDataContext StatisticsDataContext
        {
            get
            {

                Adevico.DataManager.Interface.iDataContext preLoadedContext = null;

                if (HttpContext.Current != null)
                {
                    try
                    {
                        preLoadedContext = (Adevico.DataManager.Interface.iDataContext)HttpContext.Current.Items[Helper.ContextHelper.ContextStatisticsDataContextKey];
                    }
                    catch { }

                }

                if (preLoadedContext == null)
                {
                    Adevico.DataManager.SessionHelper helper = new SessionHelper(ActionConnectionFile);
                    //SessionHelper session = new SessionHelper(configFile);
                    //using (DataContext dc = new DataContext(session.GetNewSession()))
                    preLoadedContext = new Adevico.DataManager.DataContext(helper.GetNewSession());

                    HttpContext.Current.Items[Helper.ContextHelper.ContextStatisticsDataContextKey] = preLoadedContext;
                }

                return preLoadedContext;
            }
        }
    }




}