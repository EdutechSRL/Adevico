using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adevico.APIconnection.Core.dto;

namespace LMSAPI.Controllers.API.dto
{
    public class dtoUserStatResult : dtoBaseResponse
    {
        public IList<DateTime> DatesList { get; set; }
    }
}