using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adevico.APIconnection.Core.dto;
using Adevico.UserStatistics.Domain;

namespace LMSAPI.Controllers.API.dto
{
    public class dtoModuleActionResult : dtoBaseResponse
    {
        public IList<ModuleAction> Actions { get; set; }

    }
}