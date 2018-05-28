using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Adevico.APIconnection.Core;
using Adevico.APIconnection.Core.dto;
using Adevico.Core.Core.dto;

namespace Adevico.WebAPI.Controllers.API
{
    public class LoginController : BaseApiController
    {
        // POST api/login
        /// <summary>
        /// Login al sistema. Funziona SOLO il metodo in POST.
        /// </summary>
        /// <param name="data">JSon Object</param>
        /// <returns></returns>
        /// <example>
        /// data:
        ///  { "User":"login", "password":"pwd", "deviceid":"deviceCode", "TokenType":" None = 0|AdevicoWeb = 1|Mobile = 20"  }
        /// 
        /// User = login
        /// password = password
        /// deviceid = id dispositivo. Vuoto o null per "SingleSignOn".
        /// </example>
        public dtoLogin Post(dtoLoginData data)
        {

            dtoLogin loginInfo = new dtoLogin();

            if (!String.IsNullOrEmpty(data.User) || !String.IsNullOrEmpty(data.Password))
            {
                loginInfo = coreApiService.PersonLogin(data.User, data.Password, data.DeviceId, data.TokenType);
            }
            else
            {
                loginInfo.Error = AuthenticationError.ParameterInvalid;
            }
            
            //if (obj["User"] + "" != "" && obj["password"] + "" != "")
            //{
            //    string deviceInfo = (obj["deviceid"] != null) ? obj["deviceid"].ToString() : "";
            //    loginInfo = coreApiService.PersonLogin(obj["User"].ToString(), obj["password"].ToString(), deviceInfo);
            //}

            switch (loginInfo.Error)
            {
                case AuthenticationError.Internal:
                    throw new HttpResponseException(HttpStatusCode.InternalServerError);
                    break;
                case AuthenticationError.UserDisabled:
                    throw new HttpResponseException(HttpStatusCode.Forbidden);
                    break;
                case AuthenticationError.PasswordExpired:
                    throw new HttpResponseException(HttpStatusCode.Forbidden);
                    break;
                case AuthenticationError.CredentialInvalid:
                    throw new HttpResponseException(HttpStatusCode.Forbidden);
                    break;
                case AuthenticationError.TokenInvalid:
                    throw new HttpResponseException(HttpStatusCode.Forbidden);
                    break;
                case AuthenticationError.UserNotFound:
                    throw new HttpResponseException(HttpStatusCode.Forbidden);
                    break;
            }

            //if(loginInfo.Error != AuthenticationError.None || loginInfo.Error != AuthenticationError.PolicyPending)
            //    throw new Exception(loginInfo.Error.ToString());

            return loginInfo;

            //if (ManagerLogin.hasValidAccess(obj["User"].ToString(), obj["password"].ToString()))
            //{
            //    Guid g = Guid.NewGuid();
            //    return g;
            //}
            //throw new HttpResponseException(HttpStatusCode.Forbidden);
        }

        [Route("api/TokenLogin")]
        [HttpPost]
        public dtoLogin PostTokenLogin(dtoLoginTokenData data)
        {
            dtoLogin loginInfo = new dtoLogin();

            if (!String.IsNullOrEmpty(data.Token) || !String.IsNullOrEmpty(data.DeviceId))
            {
                loginInfo = coreApiService.PersonTokenLogin(data.Token, data.DeviceId, data.TokenType); //.PersonLogin(data.User, data.Password, data.DeviceId, data.TokenType);
            }
            else
            {
                loginInfo.Error = AuthenticationError.ParameterInvalid;
            }

            switch (loginInfo.Error)
            {
                case AuthenticationError.Internal:
                    throw new HttpResponseException(HttpStatusCode.InternalServerError);
                    break;
                case AuthenticationError.UserDisabled:
                    throw new HttpResponseException(HttpStatusCode.Forbidden);
                    break;
                case AuthenticationError.PasswordExpired:
                    throw new HttpResponseException(HttpStatusCode.Forbidden);
                    break;
                case AuthenticationError.CredentialInvalid:
                    throw new HttpResponseException(HttpStatusCode.Forbidden);
                    break;
                case AuthenticationError.TokenInvalid:
                    throw new HttpResponseException(HttpStatusCode.Forbidden);
                    break;
                case AuthenticationError.UserNotFound:
                    throw new HttpResponseException(HttpStatusCode.Forbidden);
                    break;
            }

            //if(loginInfo.Error != AuthenticationError.None || loginInfo.Error != AuthenticationError.PolicyPending)
            //    throw new Exception(loginInfo.Error.ToString());

            return loginInfo;

            //if (ManagerLogin.hasValidAccess(obj["User"].ToString(), obj["password"].ToString()))
            //{
            //    Guid g = Guid.NewGuid();
            //    return g;
            //}
            //throw new HttpResponseException(HttpStatusCode.Forbidden);
        }
        public bool Get()
        {
            //HttpResponseMessage msg = new HttpResponseMessage();
            //msg.Content.Headers.Add("InternalMessage", "Use POST for login!");
                //RequestMessage = "Use POST for login!"; 
            HttpResponseException ex = new HttpResponseException(HttpStatusCode.BadRequest);
            ex.Data.Add("InternalError", "Use POST for login!");
            
            throw ex;
        }
        
    }
}