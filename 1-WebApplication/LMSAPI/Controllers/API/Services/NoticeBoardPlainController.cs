using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Adevico.APIconnection.Core;
using Adevico.SocialNoticeBoard.Domain.DTO;

namespace Adevico.WebAPI.Controllers.API
{
    public class NoticeBoardPlainController : BaseApiAuthenticatedController
    {
        // GET api/noticeboardplain
        public SocialNoticeBoard.Domain.DTO.dtoNoticeBoardPlainListResponse Get(
           [FromUri]Int32 communityId)
        {
            SocialNoticeBoard.Domain.DTO.dtoNoticeBoardPlainListResponse response = new dtoNoticeBoardPlainListResponse();
            response.ErrorInfo = base.LastError;

            dtoMessageListPlain list = Service.MessageGetListPlain_Full(communityId);

            response.Messages = list.Messages;
            response.ReferenceDateTime = list.ReferenceDateTime;

            response.Success = true;
            response.ErrorInfo = GenericError.None;

            //ActionFilters
            CheckResponse(response);
            return response;
        }

        // GET api/noticeboardplain
        public SocialNoticeBoard.Domain.DTO.dtoNoticeBoardPlainListResponse Get(
           [FromUri]DateTime referenceDateTime
           )
        {
            // ,
            //[FromUri]string token,
            //[FromUri]string deviceId = ""


            // Blocco da rendere "generico" su TUTTE le chiamate, esclusa login!!!
            SocialNoticeBoard.Domain.DTO.dtoNoticeBoardPlainListResponse response = new dtoNoticeBoardPlainListResponse();
            response.ErrorInfo = base.LastError;

            dtoMessageListPlain list = Service.MessageGetListPlain_Update(base.UserContext.CurrentCommunityID, referenceDateTime);

            response.Messages = list.Messages;
            response.ReferenceDateTime = list.ReferenceDateTime;
            //.(communityId);

            response.Success = true;
            response.ErrorInfo = GenericError.None;

            //ActionFilters
            CheckResponse(response);

            return response;
        }



        private Adevico.SocialNoticeBoard.Service.SocialNoticeBoardService _service;

        internal Adevico.SocialNoticeBoard.Service.SocialNoticeBoardService Service
        {
            get
            {

                if (_service == null || base.UpdateService)
                {
                    _service = new Adevico.SocialNoticeBoard.Service.SocialNoticeBoardService(base.AppContext);
                    base.UpdateService = false;
                }

                if (_service == null)
                {

                }

                return _service;
            }
        }

    }
}
