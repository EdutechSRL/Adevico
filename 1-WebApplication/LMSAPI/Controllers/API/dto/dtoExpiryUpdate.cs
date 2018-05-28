using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMSAPI.Controllers.API.dto
{
    public class dtoExpiryUpdate
    {
        public int CommunityId { get; set; }
        public IList<int> UsersId { get; set; }
        public bool SetVoid { get; set; }
        public DateTime? StartDateTime { get; set; }
    }
}