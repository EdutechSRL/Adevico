using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Adevico.APIconnection.Core;
using Adevico.SocialNoticeBoard;
using Adevico.SocialNoticeBoard.Domain.DTO;
using lm.Comol.Core.Business;
using lm.Comol.Core.DomainModel;
using Adevico.APIconnection.Core.Business;
using Adevico.Core.Core.dto;

namespace Adevico.WebAPI.Controllers.API
{
    public class NoticeBoardController : BaseApiAuthenticatedController
    {

        // GET api/noticeboardplain
        public SocialNoticeBoard.Domain.DTO.dtoNoticeBoardPlainListResponse Get(
           [FromUri]Int32 communityId)
        {
            // Blocco da rendere "generico" su TUTTE le chiamate, esclusa login!!!
            dtoSocialDashboardPermissionResponse dpr = (dtoSocialDashboardPermissionResponse)CoreService.PermissionGet(
                COL_BusinessLogic_v2.UCServices.Services_SocialNoticeboard.Codex,
                UserContext.CurrentUserID,
                UserContext.CurrentCommunityID);

            dtoNoticeBoardPlainListResponse response = new dtoNoticeBoardPlainListResponse();
            response.ErrorInfo = base.LastError;

            if (!dpr.Admin && !dpr.View)
            {
                response.Success = false;
                response.ErrorInfo = GenericError.NoServicePermission;
            }

            dtoMessageListPlain list = Service.MessageGetListPlain_Full(communityId);

            response.Messages = list.Messages;
            response.ReferenceDateTime = list.ReferenceDateTime;

            response.Success = true;
            response.ErrorInfo = GenericError.None;

            //ActionFilters
            CheckResponse(response);
            return response;
        }

        // GET api/noticeboard
        public SocialNoticeBoard.Domain.DTO.dtoNoticeBoardPlainListResponse Get(
           [FromUri]DateTime referenceDateTime
           )
        {
            // Blocco da rendere "generico" su TUTTE le chiamate, esclusa login!!!
            dtoSocialDashboardPermissionResponse dpr = (dtoSocialDashboardPermissionResponse)CoreService.PermissionGet(
                COL_BusinessLogic_v2.UCServices.Services_SocialNoticeboard.Codex,
                UserContext.CurrentUserID,
                UserContext.CurrentCommunityID);

            dtoNoticeBoardPlainListResponse response = new dtoNoticeBoardPlainListResponse();
            response.ErrorInfo = base.LastError;

            if (!dpr.Admin && !dpr.View)
            {
                response.Success = false;
                response.ErrorInfo = GenericError.NoServicePermission;
            }

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





        //
        // GET: /NoticeBoard/
        //public SocialNoticeBoard.Domain.DTO.dtoNoticeBoardListResponse Get(
        //    [FromUri]Int32 communityId)
        //{

        //    SocialNoticeBoard.Domain.DTO.dtoNoticeBoardListResponse
        //        response = new dtoNoticeBoardListResponse();
        //    response.ErrorInfo = base.LastError;

        //    response.Comments = Service.PostGetList(communityId);

        //    response.Success = true;
        //    response.ErrorInfo = GenericError.None;
           
        //    //FilterAction sarebbero perfette!!!
        //    CheckResponse(response);

        //    return response;
        //}

        // PUT api/noticeboard/5
        public dtoCretePostResponse Put(
            SocialNoticeBoard.Domain.DTO.dtoCommentCreate data)
        {

            // Blocco da rendere "generico" su TUTTE le chiamate, esclusa login!!!
            dtoSocialDashboardPermissionResponse dpr = (dtoSocialDashboardPermissionResponse)CoreService.PermissionGet(
                COL_BusinessLogic_v2.UCServices.Services_SocialNoticeboard.Codex,
                UserContext.CurrentUserID,
                UserContext.CurrentCommunityID);

            dtoCretePostResponse response = new dtoCretePostResponse();
            response.ErrorInfo = base.LastError;

            if (!dpr.Admin && !(dpr.Comment && data.ParentMessageId != null) && !(dpr.NewPost && data.ParentMessageId == null))
            {
                response.Success = false;
                response.ErrorInfo = GenericError.NoServicePermission;
            }

            
            if (data == null)
            {
                response.Success = false;
                response.ErrorInfo = GenericError.InvalidDataInput;
            }

            if (response.ErrorInfo == GenericError.None)
            {
                response.Comment = Service.PostCreate(data);

                if (response.Comment != null && response.Comment.Id <= 0)
                {
                    response.ErrorInfo = GenericError.Internal;
                }
                else
                {
                    response.Success = true;
                }
            }    
            
            //In ActionFilters
            CheckResponse(response);
            return response;
        }

        // DELETE api/noticeboard/5
        public dtoCretePostResponse Delete([FromUri]int id)
        {
            dtoSocialDashboardPermissionResponse dpr = (dtoSocialDashboardPermissionResponse)CoreService.PermissionGet(
                COL_BusinessLogic_v2.UCServices.Services_SocialNoticeboard.Codex,
                UserContext.CurrentUserID,
                UserContext.CurrentCommunityID);

            dtoCretePostResponse response = new dtoCretePostResponse();
            response.ErrorInfo = base.LastError;

            if (!dpr.Admin && !dpr.Delete)
            {
                response.Success = false;
                response.ErrorInfo = GenericError.NoServicePermission;
            }
            if (id < 1)
            {
                response.Success = false;
                response.ErrorInfo = GenericError.InvalidDataInput;
            }

            if (id < 1 && (!dpr.Admin && !dpr.Delete))
            {
                response.Success = false;
                response.ErrorInfo = GenericError.InvalidDataInput;
            }

            if (response.ErrorInfo == GenericError.None)
            {
                response.Comment = Service.PostDelete(id);

                if (response.Comment != null && response.Comment.Id <= 0)
                {
                    response.ErrorInfo = GenericError.Internal;
                }
                else
                {
                    response.Success = true;
                }
            }

            //In ActionFilters
            CheckResponse(response);
            return response;
        }

        // Post api/esempio/
        /// <summary>
        /// Crea un nuovo messaggio nella bacheca social
        /// </summary>
        /// <param name="data">dtoCommentCreate</param>
        /// <returns>dtoCretePostResponse</returns>
        public dtoCretePostResponse Post(
            SocialNoticeBoard.Domain.DTO.dtoCommentCreate data)
        {
            // Blocco da rendere "generico" su TUTTE le chiamate, esclusa login!!!
            dtoSocialDashboardPermissionResponse dpr = (dtoSocialDashboardPermissionResponse)CoreService.PermissionGet(
                COL_BusinessLogic_v2.UCServices.Services_SocialNoticeboard.Codex,
                UserContext.CurrentUserID,
                UserContext.CurrentCommunityID);

            dtoCretePostResponse response = new dtoCretePostResponse();
            response.ErrorInfo = base.LastError;

            if (!dpr.Admin && !(dpr.Comment && data.ParentMessageId != null) && !(dpr.NewPost && data.ParentMessageId == null))
            {
                response.Success = false;
                response.ErrorInfo = GenericError.NoServicePermission;
            }



            if (data == null)
            {
                response.Success = false;
                response.ErrorInfo = GenericError.InvalidDataInput;
            }

            if (response.ErrorInfo == GenericError.None)
            {
                response.Comment = Service.PostCreate(data);

                if (response.Comment != null && response.Comment.Id <= 0)
                {
                    response.ErrorInfo = GenericError.Internal;
                }
                else
                {
                    response.Success = true;
                }
            }

            //In ActionFilters
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
        private Adevico.APIconnection.Core.Business.CoreAPIService _coreService { get; set; }
        internal Adevico.APIconnection.Core.Business.CoreAPIService CoreService
        {
            get
            {
                if (_coreService == null || base.UpdateService)
                {
                    _coreService = new CoreAPIService(DataContext);
                }

                return _coreService;
            }
        }

    }
}
