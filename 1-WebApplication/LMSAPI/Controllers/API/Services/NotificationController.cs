using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Adevico.APIconnection.Core;
using Adevico.APIconnection.Core.dto;

namespace Adevico.WebAPI.Controllers.API
{
    public class NotificationController : BaseApiAuthenticatedController
    {
        // GET api/Notification
        public Adevico.APIconnection.Core.dto.dtoNotificationResponse Get(
            [FromUri]DateTime dataRif)
        {

            Adevico.APIconnection.Core.dto.dtoNotificationResponse response = new dtoNotificationResponse();
            response.ErrorInfo = base.LastError;


            response = coreApiService.NotificationGetList(dataRif, base.UserContext.CurrentUserID);
            
            //ActionFilters
            CheckResponse(response);

            return response;
        }
    }
}