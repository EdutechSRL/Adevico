using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable]
    public class ModuleCallForPaper
    {
        public const String UniqueCode ="SRVCFP";
        public virtual Boolean ViewCallForPapers {get;set;}
        public virtual Boolean AddSubmission {get;set;}
        public virtual Boolean CreateCallForPaper {get;set;}
        public virtual Boolean EditCallForPaper { get; set; }
        public virtual Boolean ManageCallForPapers {get;set;}
        public virtual Boolean DeleteOwnCallForPaper {get;set;}
        public virtual Boolean Administration { get; set; }
        public ModuleCallForPaper() {
            ViewCallForPapers = true;
        }
        public static ModuleCallForPaper CreatePortalmodule(int UserTypeID){
            ModuleCallForPaper module = new ModuleCallForPaper();
            module.ViewCallForPapers = true;
            module.AddSubmission=(UserTypeID != (int)UserTypeStandard.Guest );
            module.Administration = (UserTypeID == (int)UserTypeStandard.SysAdmin || UserTypeID == (int)UserTypeStandard.Administrator);
            module.CreateCallForPaper= (UserTypeID == (int)UserTypeStandard.SysAdmin || UserTypeID == (int)UserTypeStandard.Administrator || UserTypeID == (int)UserTypeStandard.Administrative   );
            module.EditCallForPaper = (UserTypeID == (int)UserTypeStandard.SysAdmin || UserTypeID == (int)UserTypeStandard.Administrator || UserTypeID == (int)UserTypeStandard.Administrative);
            module.ManageCallForPapers = (UserTypeID == (int)UserTypeStandard.SysAdmin || UserTypeID == (int)UserTypeStandard.Administrator || UserTypeID == (int)UserTypeStandard.Administrative);
            module.DeleteOwnCallForPaper = (UserTypeID == (int)UserTypeStandard.SysAdmin || UserTypeID == (int)UserTypeStandard.Administrator || UserTypeID == (int)UserTypeStandard.Administrative);
            return module;
        }

        public ModuleCallForPaper(long permission)
        {
            ViewCallForPapers = PermissionHelper.CheckPermissionSoft((long)Base2Permission.ListCalls, permission);
            AddSubmission = PermissionHelper.CheckPermissionSoft((long)Base2Permission.AddSubmission, permission);
            CreateCallForPaper = PermissionHelper.CheckPermissionSoft((long)Base2Permission.AddCall, permission);
            EditCallForPaper = PermissionHelper.CheckPermissionSoft((long)Base2Permission.EditCall, permission);
            Administration = PermissionHelper.CheckPermissionSoft((long)Base2Permission.Admin, permission);
            ManageCallForPapers = PermissionHelper.CheckPermissionSoft((long)Base2Permission.ManageCalls, permission);
            DeleteOwnCallForPaper = PermissionHelper.CheckPermissionSoft((long)Base2Permission.DeleteCall, permission); 
        }
        public lm.Comol.Core.TemplateMessages.ModuleGenericTemplateMessages ToTemplateModule() {
            lm.Comol.Core.TemplateMessages.ModuleGenericTemplateMessages m = new lm.Comol.Core.TemplateMessages.ModuleGenericTemplateMessages(UniqueCode);
            m.Add = ManageCallForPapers || Administration || CreateCallForPaper;
            m.Administration = ManageCallForPapers || Administration;
            m.Clone = ManageCallForPapers || Administration;
            m.DeleteMyTemplates = ManageCallForPapers || Administration || CreateCallForPaper;
            m.DeleteOtherTemplates = ManageCallForPapers || Administration;
            m.Edit = ManageCallForPapers || Administration;
            m.List = ManageCallForPapers || Administration;
            m.SendMessage = ManageCallForPapers || Administration || CreateCallForPaper;
            m.ManageModulePermission = ManageCallForPapers || Administration || CreateCallForPaper;
            return m;
        }
        public List<ActionType> GetAutomaticActions(lm.Comol.Core.Notification.Domain.NotificationMode mode)
        {
            List<ActionType> actions = new List<ActionType>();
            switch (mode) {
                case lm.Comol.Core.Notification.Domain.NotificationMode.Automatic:
                    break;
                case lm.Comol.Core.Notification.Domain.NotificationMode.Scheduling:
                    break;
                case lm.Comol.Core.Notification.Domain.NotificationMode.Manual:
                    break;
            }
            return actions;
        }

        [Flags,Serializable]
        public enum Base2Permission{
            ListCalls = 1,
            AddSubmission = 2 ,
            EditCall = 4,
            DeleteCall = 8,
            ManageCalls = 16,
            Admin = 64,
            AddCall = 8192
        }
        [Serializable]
        public enum ActionType{
            None = 85000,
            NoPermission = 85001,
            GenericError = 85002,
            List = 85003,
            Manage = 85004,
            EditCall = 85008,
            DeleteCall = 85009,
            AddSubmission = 85010,
            EditSubmission = 85011,
            ConfirmSubmission = 85012,
            DeleteSubmission = 85013,
            EvaluateSubmission = 85014,
            ViewSubmission = 85015,
            ViewUnknowSubmission = 85016,
            ViewCallSubmission = 85017,
            ViewUnknowCallForPaper = 85018,
            ViewPreviewCallForPaper = 85019,
            DownloadSubmittedFile = 85020,
            DownloadCallForPaperFile =85021,

            ManageCriterion = 85022,
            AddCriterion = 85023,
            EditCriterion = 85024,
            DeleteCriterion = 85025,
            VirtualDeleteCriterion = 85026,
            VirtualUndeleteCriterion = 85027,
            ManageEvaluators = 85028,
            AddEvaluator = 85029,
            EditEvaluator = 85030,
            DeleteEvaluator = 85031,
            VirtualDeleteEvaluator = 85032,
            VirtualUndeleteEvaluator = 85033,
            ManageEvaluations = 85034,
            ViewEvaluation = 85035,
            EditAssignedEvaluations = 85036,
            ViewAndEditEvaluators = 85037,
            ViewAssignedEvaluations = 85038,
            AcceptSubmission = 85039,
            RejectSubmission = 85040,
            StartSubmission = 85041,
            VirtualDeleteSubmission = 85042,
            VirtualDeleteSubmittedFile = 85043,
            VirtualDeleteCallForPaper = 85044,
            VirtualUndeleteCallForPaper = 85045,
            StartCallCreation = 85046,
            StartCallEdit = 85047,
            CreateCallForPaper = 85048,
            VirtualDeleteCallField = 85049,
            VirtualUndeleteCallField = 85050,
            VirtualDeleteCallSection = 85051,
            VirtualUndeleteCallSection = 85052,
            AddFieldToCall = 85053,
            AddSectionToCall = 85054,
            SaveCallField = 85055,
            SaveCallSection = 85056,
            EditAssignedSubmissions = 85057,
            ViewUnassignedEvaluation = 85058,
            AddCallSettings = 85059,
            SaveCallSettings = 85060,
            ViewCallAvailability = 85061,
            SaveCallAvailability = 85062,
            EditSubmittersType = 85063,
            SaveSubmittersType = 85064,
            EditAttachments = 85065,
            SaveAttachments = 85066,
            AddAttachments = 85067,
            VirtualDeleteAttachment = 85068,
            VirtualDeleteSubmitterType = 85069,
            LoadCallEditor = 85070,
            SaveCallSections = 85071,
            VirtualDeleteFieldOption = 85072,
            AddFieldOption = 85073,
            ViewSubmittersTemplate = 85074,
            EditSubmitterTemplate = 85075,
            AddSubmitterTemplate = 85076,
            VirtualDeleteSubmitterTemplate = 85077,
            SaveSubmittersTemplate = 85078,
            EditManagerTemplate = 85079,
            SaveManagerTemplate = 85080,
            ViewRequestedFiles = 85081,
            AddRequestedFile = 85082,
            EditRequestedFile = 85083,
            VirtualDeleteRequestedFile = 85084,
            ViewRevision = 85085,
            SaveRevision = 85086,
            CompleteRevision = 85087,
            EditFieldsAssociation = 85088,
            SaveFieldsAssociation = 85089,
            LoadRevisionsList = 85090,
            LoadSubmissionsList = 85091,
            ManageCommittee = 85092,
            SaveCommitteeSettings = 85093,
            AddCommittee = 85094,
            VirtualDeleteCommittee = 85095,
            VirtualUndeleteCommittee = 85096,
            PhisicalDeleteCommittee = 85097,
            VirtualDeleteCriterionOption = 85101,
            VirtualUndeleteCriterionOption = 85102,
            PhisicalDeleteCriterionOption = 85103,
            AddCriterionOption = 85104,
            SaveEvaluatorsSettings = 85105,
            SaveSingleSubmissionAssignmentForEvaluation = 85106,
            ViewEvaluationsSummary = 85107,
            ViewEvaluationsCommitteesSummary = 85108,
            ViewEvaluationsCommitteeSummary = 85109,
            CloneCall = 85110,
            SetAsDefaultOption = 85111,
            EditDisclaimerType = 85112,
            UploadFileToUnknownCall = 85113,
            AttachmentsNotAddedFiles= 85114,
            AttachmentsAddedFiles= 85114,
        }
        [Serializable]
        public enum ObjectType{
            None = 0,
            CallForPaper = 1,
            FieldsSection = 2,
            FieldDefinition = 3,
            FieldValue = 4,
            RequestedFile = 5,
            SubmittedFile = 6,
            SubmitterType = 7,
            UserSubmission = 8,
            AttachmentFile = 9,
            Criterion = 10,
            Evaluator = 11,
            Evaluation = 12,
            Revision = 13
        }
    }
}