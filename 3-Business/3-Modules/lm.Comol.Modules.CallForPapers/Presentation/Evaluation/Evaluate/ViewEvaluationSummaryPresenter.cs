﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Domain.Evaluation;
using lm.Comol.Modules.CallForPapers.Business;
using lm.Comol.Core.Business;
using lm.Comol.Modules.CallForPapers.Domain;

namespace lm.Comol.Modules.CallForPapers.Presentation.Evaluation
{
    public class ViewEvaluationSummaryPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
         #region "Initialize"
            private ServiceCallOfPapers _ServiceCall;
            private ServiceRequestForMembership _ServiceRequest;
            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewEvaluationSummary View
            {
                get { return (IViewEvaluationSummary)base.View; }
            }
            private ServiceEvaluation _Service;
            private ServiceEvaluation Service
            {
                get
                {
                    if (_Service == null)
                        _Service = new ServiceEvaluation(AppContext);
                    return _Service;
                }
            }
            private ServiceCallOfPapers ServiceCall
            {
                get
                {
                    if (_ServiceCall == null)
                        _ServiceCall = new ServiceCallOfPapers(AppContext);
                    return _ServiceCall;
                }
            }
            private ServiceRequestForMembership ServiceRequest
            {
                get
                {
                    if (_ServiceRequest == null)
                        _ServiceRequest = new ServiceRequestForMembership(AppContext);
                    return _ServiceRequest;
                }
            }
            public ViewEvaluationSummaryPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public ViewEvaluationSummaryPresenter(iApplicationContext oContext, IViewEvaluationSummary view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView()
        {
            Boolean isAnonymousUser = UserContext.isAnonymous;
            long idCall = View.PreloadIdCall;
            long idSubmission = View.PreloadIdSubmission;
            Int32 idUser = UserContext.CurrentUserID;

            lm.Comol.Modules.CallForPapers.Domain.CallForPaperType type = ServiceCall.GetCallType(idCall);
            int idModule = (type == CallForPaperType.CallForBids) ? ServiceCall.ServiceModuleID() : ServiceRequest.ServiceModuleID();

            dtoBaseForPaper call = ServiceCall.GetDtoBaseCall(idCall);
            int idCommunity = GetCurrentCommunity(call);

            View.IdCall = idCall;
            View.IdCallModule = idModule;
            View.IdCallCommunity = idCommunity;
            View.IdSubmission = idSubmission;
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout(RootObject.ViewSubmissionTableEvaluations(idSubmission, idCall, idCommunity));
            else
            {
                if (call == null)
                    View.DisplayUnknownCall(idCommunity, idModule, idCall,type);
                else if (type == CallForPaperType.RequestForMembership)
                    View.DisplayEvaluationUnavailable();
                else
                {
                    ModuleCallForPaper module = ServiceCall.CallForPaperServicePermission(idUser, idCommunity);
                    Boolean allowAdmin = ((module.ManageCallForPapers || module.Administration || ((module.CreateCallForPaper || module.EditCallForPaper) && call.Owner.Id == idUser)));
                    EvaluationType evaluationType = ServiceCall.GetEvaluationType(idCall);
                    View.CurrentEvaluationType = evaluationType;
                    if (!allowAdmin)
                        View.DisplayNoPermissionToView();
                    else
                    {
                        dtoSubmissionRevision submission = ServiceCall.GetSubmissionWithRevisions(idSubmission, false);
                        if (submission == null)
                            View.DisplayUnknownSubmission(idCommunity, idModule, idSubmission, type);
                        else
                        {
                            LoadData(idCommunity, call, evaluationType, submission);
                        }
                    }
                }
            }
        }

        private int GetCurrentCommunity(dtoBaseForPaper call)
        {
            int idCommunity = 0;
            Community currentCommunity = CurrentManager.GetCommunity(this.UserContext.CurrentCommunityID);
            Community community = null;
            if (call != null)
                idCommunity = (call.IsPortal) ? 0 : (call.Community != null) ? call.Community.Id : 0;
            community = CurrentManager.GetCommunity(idCommunity);

            if (community == null && currentCommunity != null && !call.IsPortal)
                idCommunity = this.UserContext.CurrentCommunityID;
            else if (community==null)
                idCommunity = 0;
            View.IdCallCommunity = idCommunity;
            return idCommunity;
        }
        private void LoadData(Int32 idCommunity, dtoBaseForPaper call, EvaluationType type, dtoSubmissionRevision submission)
        {
            List<dtoCommitteeEvaluationInfo> committees = Service.GetCommitteesInfoForSubmission(submission.Id, call.Id );
            View.CommitteesCount = committees.Count;
            String owner = (submission == null || (submission != null && (submission.IsAnonymous))) ? View.AnonymousDisplayName : ((submission.Owner != null) ? submission.Owner.SurnameAndName : View.UnknownDisplayname);
            litePerson submitter = CurrentManager.GetLitePerson(submission.IdSubmittedBy);
            String submittedBy = (submission.IdPerson == submission.IdSubmittedBy) ? "" : (submitter == null || submitter.TypeID == (int)UserTypeStandard.Guest) ? View.AnonymousDisplayName : submitter.SurnameAndName;

            View.SetViewEvaluationsUrl(RootObject.ViewSubmissionEvaluations(submission.Id,call.Id, idCommunity));
           // View.SetViewSubmissionUrl(RootObject.ViewSubmissionAsManager(
            
            View.LoadSubmissionInfo( call.Name,owner, submission.SubmittedOn, submittedBy, committees, (committees == null || !committees.Any()) ? 0 : committees[0].IdCommittee);
            if (type == EvaluationType.Dss)
                InitializeDssInfo(call.Id);
            LoadEvaluations(submission.Id, call.Id,type, View.IdCurrentCommittee, committees.Count);
        }
        public void LoadEvaluations(long idSubmission, long idCall, EvaluationType type, long idCommittee, Int32 count)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout(RootObject.ViewSubmissionTableEvaluations(idSubmission, idCall, View.IdCallCommunity));
            else
            {
                List<dtoSubmissionCommitteeItem> evaluations = Service.GetSubmissionEvaluations(idCall, idSubmission, idCommittee, View.UnknownDisplayname);
                View.AllowHideComments = (evaluations != null && evaluations.Where(e => e.HasComments()).Any());
                View.AllowExportCurrent = (evaluations != null && (idCommittee >0 || count==1));
                View.AllowExportAll = (evaluations != null && count >1);
                View.LoadEvaluations(evaluations);
            }
        }
        private void InitializeDssInfo(long idCall)
        {
            View.CallUseFuzzy = Service.CallUseFuzzy(idCall);
            View.CommitteeIsFuzzy = Service.GetCommitteeDssMethodIsFuzzy(idCall);
            List<DssCallEvaluation> items = Service.DssRatingGetValues(idCall);
            DateTime lastUpdate = (items.Any() ? items.Select(i => i.LastUpdateOn).Min() : DateTime.MinValue);
            if (Service.DssRatingMustUpdate(idCall, lastUpdate))
            {
                Service.DssRatingSetForCall(idCall, out lastUpdate);
                items = Service.DssRatingGetValues(idCall);
            }
            if (items.Any())
                View.DisplayDssWarning(lastUpdate, !items.Any(i => !i.IsCompleted || !i.IsValid));
            else
                View.HideDssWarning();

        }
        public String GetFileName(String filename, SummaryType summaryType, long idCall, long idSubmission, long idCommittee)
        {
            return Service.GetStatisticFileName(idCall, ServiceCall.GetCallName(idCall),idCommittee, Service.GetCommitteeDisplayOrder(idCommittee), Service.GetCommitteeName(idCommittee),idSubmission, filename, summaryType);
        }

        public String ExportTo(SummaryType summaryType, long idCall, long idSubmission, long idCommittee, ExportData exportData, lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType fileType, Dictionary<lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationTranslations, String> translations, Dictionary<lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationStatus, String> status)
        {
            if (UserContext.isAnonymous)
            {
                View.DisplaySessionTimeout(RootObject.ViewSubmissionTableEvaluations(idSubmission, idCall, View.IdCallCommunity));
                return "";
            }
            else
                return Service.ExportSummaryStatistics(summaryType, ServiceCall.GetDtoCall(idCall), ServiceCall.GetSubmissionWithRevisions(idSubmission, false), idSubmission, idCommittee, View.AnonymousDisplayName, View.UnknownDisplayname, exportData, fileType, translations, status);
        }

        private litePerson GetCurrentUser(ref Int32 idUser)
        {
            litePerson person = null;
            if (UserContext.isAnonymous)
            {
                person = (from p in CurrentManager.GetIQ<litePerson>() where p.TypeID == (int)UserTypeStandard.Guest select p).Skip(0).Take(1).ToList().FirstOrDefault();//CurrentManager.GetLitePerson(UserContext.CurrentUserID);
                idUser = (person != null) ? person.Id : UserContext.CurrentUserID; //if(Person!=null) { IdUser = PersonId} else {IdUser = UserContext...}
            }
            else
                person = CurrentManager.GetLitePerson(idUser);
            return person;
        }
    }
}