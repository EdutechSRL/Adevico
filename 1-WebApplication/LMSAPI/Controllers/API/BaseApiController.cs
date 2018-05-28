using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

using System.Web.Http;

using Adevico.APIconnection.Core.Business;
using lm.Comol.Core.Data;
using lm.Comol.Core.DomainModel;
using NHibernate;
using Adevico.APIconnection.Core.dto;

using Adevico.APIconnection.Core;
using Adevico.Core.Core.dto;
using Adevico.Core.Presentation;
using Adevico.WebAPI.ActionFilter;


namespace Adevico.WebAPI.Controllers.API
{
    public class BaseApiController : ApiController
    {
        internal bool UpdateService { get; set; }


        //internal GenericError CheckCurrentUserContext(int communityId, String token, string deviceId)
        //{
        //    return Helper.ContextHelper.UserContextLoad(null, token, deviceId, communityId);
        //}


        #region "ToDo: generalizzare!"
        /// <summary>
        /// Service CORE per chiamate su oggetti vecchio COMOL
        /// </summary>
        internal Adevico.APIconnection.Core.Business.CoreAPIService coreApiService
        {
            get
            {
                //iApplicationContext appContext = new 
                Adevico.APIconnection.Core.Business.CoreAPIService service = new CoreAPIService(AppContext);
                //Adevico.APIconnection.Core.Business.CoreAPIService service = new CoreAPIService(DataContext);

                return service;
            }
        }

        /// <summary>
        /// ToDo: usare ActionFilters su output!
        /// </summary>
        /// <param name="response"></param>
        internal void CheckResponse(dtoBaseResponse response)
        {
            Helper.ContextHelper.CheckResponse(response);
        }

        #region "New Context Implementation"
        internal iApplicationContext AppContext
        {
            get { return Helper.ContextHelper.AppContext; }
        }

        //Get e Set (quando cambiano utente o comunità)
        internal iUserContext UserContext
        {
            get { return Helper.ContextHelper.UserContext; }
            set
            {
                UpdateService = true;
                Helper.ContextHelper.UserContext = value;
            }
        }


        //Solo GET: se vuoto lo carica per conto suo.
        internal iDataContext DataContext
        {
            get { return Helper.ContextHelper.DataContext; }
        }

        
        internal GenericError LastError
        { get { return Helper.ContextHelper.LastError; } }

        internal String ServiceCode
        {
            get
            {
                string srv = "";
                CookieHeaderValue headerValues = Request.Headers.GetCookies().FirstOrDefault();
                
                if (headerValues != null)
                    srv = headerValues[CookieHelper.CookieKeyServiceCode].Value;

                if (srv == "")
                {
                    IEnumerable<KeyValuePair<string, string>> kvpList = Request.GetQueryNameValuePairs().Where(p => p.Key == "ServiceCode");
                    if (kvpList.Count() > 0)
                        srv = kvpList.First().Value;
                }

                return srv;
            }
        }
        #endregion

#region Other DataContext - Generic for other database
        internal Adevico.DataManager.Interface.iDataContext StatsDataContext
        {
            get { return Helper.ContextHelper.StatisticsDataContext; }
        }

#endregion
       #endregion

        #region Permissions

        private GenericError InternalError { get; set; }

        #endregion



        //Sincrona!
        /// <summary>
        /// Funzione cancella cache, diventerà ufficiale,
        /// richiamando l'altra funzione!
        /// Al momento lascio qui, con codice SINCRONO!
        /// </summary>
        /// <param name="CacheKey"></param>
        /// <param name="uri"></param>
        internal void TEST_ClearCache(string CacheKey, string uri)
        {

            throw new NotImplementedException("Necessario creare gestione su WebApp");

            HttpWebRequest webrequest =
             (HttpWebRequest)WebRequest.Create(uri);

            webrequest.KeepAlive = false;
            webrequest.Method = "GET";

            webrequest.ContentType = "text/html";
            webrequest.AllowAutoRedirect = false;

            try
            {
                HttpWebResponse response = (HttpWebResponse)webrequest.GetResponse();
            } catch (Exception ex)
            {

            }
        }



        //Asincrona!
        /// <summary>
        /// ToDo!!!  Farla funzioanre sincrona e fare funzione WebApp + sicurezza?
        /// </summary>
        /// <param name="_StrJsonData"></param>
        /// <param name="_UrlAPI"></param>
        /// <param name="_RoutingAPI"></param>
        /// <returns></returns>
        internal async System.Threading.Tasks.Task<SimpleHttpResponse> TEST_ClearCache_PostAPI(
            string _StrJsonData, 
            string _UrlAPI, 
            string _RoutingAPI)
        {

            throw new NotImplementedException("Necessario creare gestione su WebApp");

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_UrlAPI);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    //JObject jsonData = JObject.Parse(_StrJsonData);

                    //StringContent content = new StringContent(jsonData.ToString(), Encoding.UTF8, "application/json");
                    StringContent content = new StringContent(_StrJsonData, System.Text.Encoding.UTF8, "application/json");
                    
                    HttpResponseMessage _response = await client.PostAsync(_RoutingAPI, content).ConfigureAwait(false);
                    SimpleHttpResponse SimpHttpResp = new SimpleHttpResponse();
                    if (_response.IsSuccessStatusCode)
                    {
                        SimpHttpResp.Message = await _response.Content.ReadAsStringAsync().ConfigureAwait(false);
                        SimpHttpResp.StatusCode = _response.StatusCode;
                        SimpHttpResp.IsSuccess = true;
                        return SimpHttpResp;
                    }
                    else
                    {
                        SimpHttpResp.Message = await _response.Content.ReadAsStringAsync().ConfigureAwait(false);
                        SimpHttpResp.StatusCode = _response.StatusCode;
                        SimpHttpResp.IsSuccess = false;
                        return SimpHttpResp;
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error: " + e.Message);
            }
            throw new Exception("Error: PostAPI");

        }

    }

    /// <summary>
    /// Simple http response
    /// </summary>
    public class SimpleHttpResponse
    {
        /// <summary>
        /// Output message
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// HTTP status code
        /// </summary>
        public  HttpStatusCode StatusCode { get; set; }
        /// <summary>
        /// Success
        /// </summary>
        public bool IsSuccess { get; set; }
    }
}
