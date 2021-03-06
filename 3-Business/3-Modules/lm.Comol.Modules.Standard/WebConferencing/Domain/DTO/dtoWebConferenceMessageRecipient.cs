﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.MailCommons.Domain;

namespace lm.Comol.Modules.Standard.WebConferencing.Domain
{
    [Serializable]
    public class dtoWebConferenceMessageRecipient : lm.Comol.Core.Mail.Messages.dtoModuleRecipientMessages
    {
        public virtual MailStatus MailStatus { get; set; }
        public virtual UserTypeFilter UserType { get; set; }
        public virtual UserStatus UserStatus { get; set; }
        public virtual String RoleName { get; set; }
        public virtual String ProfileTypeName { get; set; }
        public virtual Int32 IdRole { get; set; }
        public virtual Boolean IsAutoGeneratedPerson { get; set; }
        public override String FirstLetter { get { return (!String.IsNullOrEmpty(Surname)) ? Surname.Trim().Substring(0, 1).ToLower() : (!String.IsNullOrEmpty(MailAddress) ? MailAddress.Substring(0, 1).ToLower() : ""); } } 
       
        public dtoWebConferenceMessageRecipient()
            : base()
        {
            UserStatus = Domain.UserStatus.Unlocked;
            MailStatus = Domain.MailStatus.None;
            UserType = UserTypeFilter.None;
        }

        public dtoWebConferenceMessageRecipient(long id)
            : base(id)
        {
            UserStatus = Domain.UserStatus.Unlocked;
            MailStatus = Domain.MailStatus.None;
            UserType = UserTypeFilter.None;
        }
        public dtoWebConferenceMessageRecipient(lm.Comol.Core.Mail.Messages.dtoModuleRecipientMessages obj)
            : base(obj.Id)
        {
            Deleted = obj.Deleted;
            MessageNumber = obj.MessageNumber;
            IdAgency = obj.IdAgency;
            AgencyName = obj.AgencyName;
            Messages = obj.Messages;
            IdLanguage = obj.IdLanguage;
            CodeLanguage = obj.CodeLanguage;
            Type = obj.Type;
            IdPerson = obj.IdPerson;
            ModuleCode = obj.ModuleCode;
            IdUserModule = obj.IdUserModule;
            MailAddress = obj.MailAddress;
            if (String.IsNullOrEmpty(obj.DisplayName))
                DisplayName = obj.MailAddress;
            else
                DisplayName = obj.DisplayName;
        }
        public dtoWebConferenceMessageRecipient(ModuleObject obj, Person p, String moduleCode = "", String anonymousUser = "")
        {
            MessageNumber = 0;
            IdLanguage = p.LanguageID;
            Type = lm.Comol.Core.MailCommons.Domain.RecipientType.BCC;
            IdPerson = p.Id;
            ModuleCode = moduleCode;
            if (p.TypeID == (int)UserTypeStandard.Guest || p.TypeID == (int)UserTypeStandard.PublicUser)
            {
                MailAddress = "";
                DisplayName = anonymousUser;
            }
            else
            {
                MailAddress = p.Mail;
                DisplayName = p.SurnameAndName;
            }
            Name = p.Name;
            Surname = p.Surname;
            IdProfileType = p.TypeID;
            IsAutoGeneratedPerson = true;
            MailStatus = Domain.MailStatus.Confirmed;
            UserType = UserTypeFilter.None;
            UserStatus = Domain.UserStatus.NotSubscribed;
            IdModuleObject = obj.ObjectLongID;
            IdModuleType = obj.ObjectTypeID;
        }
        public dtoWebConferenceMessageRecipient(ModuleObject obj, Person p, Role role, String moduleCode = "", String anonymousUser = "")
            : this(obj,p, moduleCode, anonymousUser)
        {
            IsAutoGeneratedPerson = true;
            IdRole= (role==null) ? 0 : role.Id;
        }
        public dtoWebConferenceMessageRecipient(ModuleObject obj, WbUser user, String moduleCode = "")
        {
            IdUserModule = user.Id;
            MessageNumber = 0;
            CodeLanguage = user.LanguageCode;
            Type = RecipientType.BCC;
            IdPerson = user.PersonID;
            ModuleCode = moduleCode;
            MailAddress = user.Mail;
            DisplayName = user.DisplayName;
            Name = user.Name;
            Surname = user.SName;
            MailStatus = (user.MailChecked) ? Domain.MailStatus.Confirmed : Domain.MailStatus.WaitingConfirm;
            UserType = (user.IsController) ? UserTypeFilter.Administrator : (user.PersonID > 0) ? UserTypeFilter.InternalParticipant : UserTypeFilter.ExternalParticipant;
            UserStatus = (user.Enabled) ? Domain.UserStatus.Unlocked : Domain.UserStatus.Locked;
            IdModuleObject = obj.ObjectLongID;
            IdModuleType = obj.ObjectTypeID;
        }
        public void UpdateIdUserModule(WbUser user)
        {
            IdUserModule = user.Id;
            MailStatus = (user.MailChecked) ? Domain.MailStatus.Confirmed : Domain.MailStatus.WaitingConfirm;
            UserType = (user.IsController) ? UserTypeFilter.Administrator : (user.PersonID > 0) ? UserTypeFilter.InternalParticipant : UserTypeFilter.ExternalParticipant;
            UserStatus = (user.Enabled) ? Domain.UserStatus.Unlocked : Domain.UserStatus.Locked;
        }
        public void UpdatePersonInfo(Person p, String anonymousUser="")
        {
            IdLanguage = p.LanguageID;
            if (p.TypeID == (int)UserTypeStandard.Guest || p.TypeID == (int)UserTypeStandard.PublicUser)
            {
                MailAddress = "";
                DisplayName = anonymousUser;
            }
            else
            {
                MailAddress = p.Mail;
                DisplayName = p.SurnameAndName;
            }
            Name = p.Name;
            Surname = p.Surname;
            IdProfileType = p.TypeID;
            MailStatus = Domain.MailStatus.Confirmed;
        }
        //public dtoWebConferenceMessageRecipient(UserSubmission s, String moduleCode, String unknownUser, String anonymousUser)
        //    : base((long)0)
        //{
        //    Status = s.Status;
        //    MessageNumber = 0;

        //    Type = Core.Mail.RecipientType.BCC;
        //    if (s.Owner != null)
        //    {
        //        IdPerson = s.Owner.Id;
        //        IdLanguage = s.Owner.LanguageID;
        //        MailAddress = (s.Owner.TypeID == (int)UserTypeStandard.Guest || s.Owner.TypeID == (int)UserTypeStandard.PublicUser) ? "" : s.Owner.Mail;
        //        DisplayName = (s.Owner.TypeID == (int)UserTypeStandard.Guest || s.Owner.TypeID == (int)UserTypeStandard.PublicUser) ? anonymousUser : s.Owner.SurnameAndName;
        //        Name = s.Owner.Name;
        //        Surname = s.Owner.Surname;
        //        IdProfileType = s.Owner.TypeID;
        //    }
        //    else
        //    {
        //        MailAddress = "";
        //        DisplayName = anonymousUser;
        //        IdPerson = -1;
        //        IdLanguage = 0;
        //        IdProfileType = (int)UserTypeStandard.Guest;
        //    }
        //    ModuleCode = moduleCode;
        //    if (s.Type != null)
        //    {
        //        IdSubmitterType = s.Type.Id;
        //        SubmitterType = s.Type.Name;
        //    }
        //    CreatedOn = s.CreatedOn;
        //    ModifiedOn = s.ModifiedOn;
        //    SubmittedOn = s.SubmittedOn;
        //    IdModuleObject = s.Id;
        //    IdModuleType = (s.Call != null && s.Call.Type == CallForPaperType.CallForBids) ? (int)ModuleCallForPaper.ObjectType.UserSubmission : (int)ModuleRequestForMembership.ObjectType.UserSubmission;
        //}
    }
}