using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adevico.WebAPI.ActionFilter;

namespace Adevico.WebAPI.Controllers.API
{
    /// <summary>
    /// Effettua la verifica su Token e DeviceId prima della chiamata
    /// e ne gestisce la verifica. In caso di fallimento, la chiamata non sarà eseguita.
    /// </summary>
    [AdevicoAuthorize]
    public class BaseApiAuthenticatedController : BaseApiController
    {

    }
}