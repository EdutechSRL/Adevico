﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.EduPath.Domain
{

    [Serializable]
    public class dtoSubActivity : dtoGenericItem
    {

        public Int64 Completion { get; set; }
        public StatusStatistic StatusStat { get; set; }
        public String Link { get; set; }
        public SubActivityType ContentType { get; set; }
        public long ActivityParentId { get; set; }
        public lm.Comol.Core.DomainModel.ModuleLink ModuleLink { get; set; }

        public Int64 IdCertificate { get; set; }
        public Int64 IdCertificateVersion { get; set; }
        public Boolean AutoGenerated { get; set; }
        public Boolean SaveCertificate { get; set; }

        public Boolean ActiveOnMinCompletion { get; set; }
        public Boolean ActiveOnMinMark { get; set; }

        public virtual Boolean UsePathEndDateStatistics { get; set; }
        public virtual Boolean ActiveAfterPathEndDate { get; set; }
        public virtual Boolean AllowWithEmptyPlaceHolders { get; set; }
        public virtual DisplayPolicy Display { get; set; }
        public virtual long IdObject { get; set; }
        public virtual long IdObjectVersion { get; set; }
        public virtual Boolean AllowAddVersion { get; set; }
        public dtoSubActivity()
            : base()
        {

            Completion = -1;
            Link = "";
            StatusStat = StatusStatistic.None;
            ContentType = SubActivityType.None;
            ActivityParentId = -1;
            ModuleLink = null;
            AllowWithEmptyPlaceHolders = true;
            Display = DisplayPolicy.InheritedByPath;
        }

        public dtoSubActivity(SubActivity oSubAct, Status PersonalStatus, Int64 completion)
            : base(oSubAct, PersonalStatus)
        {

            Completion = completion;
            Link = oSubAct.Link;
            StatusStat = StatusStatistic.None;
            Status = oSubAct.Status;
            ContentType = oSubAct.ContentType;
            ActivityParentId = oSubAct.ParentActivity.Id;
            ModuleLink = oSubAct.ModuleLink;

            IdCertificate = oSubAct.IdCertificate;
            IdCertificateVersion = oSubAct.IdCertificateVersion;
            Name = oSubAct.Name;
            SaveCertificate = oSubAct.SaveCertificate;
            AllowWithEmptyPlaceHolders = oSubAct.AllowWithEmptyPlaceHolders;
            ActiveOnMinCompletion = oSubAct.ActiveOnMinCompletion;
            ActiveOnMinMark = oSubAct.ActiveOnMinMark;
            ActiveAfterPathEndDate = oSubAct.ActiveAfterPathEndDate;
            UsePathEndDateStatistics = oSubAct.UsePathEndDateStatistics;
            AutoGenerated = oSubAct.AutoGenerated;
            Display = oSubAct.Display;
            IdObject = oSubAct.IdObjectLong;
            IdObjectVersion = oSubAct.IdObjectVersion;
        }
        public dtoSubActivity(SubActivity oSubAct, Status PersonalStatus, dtoStatusCompletion dto)
            : base(oSubAct, PersonalStatus)
        {
            ContentType = oSubAct.ContentType;
            ActivityParentId = oSubAct.ParentActivity.Id;
            Link = oSubAct.Link;
            Completion = dto.Completion;
            StatusStat = dto.Status;
            ModuleLink = oSubAct.ModuleLink;
            Status = oSubAct.Status;

            IdCertificate = oSubAct.IdCertificate;
            IdCertificateVersion = oSubAct.IdCertificateVersion;
            Name = oSubAct.Name;
            SaveCertificate = oSubAct.SaveCertificate;
            AllowWithEmptyPlaceHolders = oSubAct.AllowWithEmptyPlaceHolders;
            ActiveOnMinCompletion = oSubAct.ActiveOnMinCompletion;
            ActiveOnMinMark = oSubAct.ActiveOnMinMark;
            ActiveAfterPathEndDate = oSubAct.ActiveAfterPathEndDate;
            UsePathEndDateStatistics = oSubAct.UsePathEndDateStatistics;
            AutoGenerated = oSubAct.AutoGenerated;
            Display = oSubAct.Display;
            IdObject = oSubAct.IdObjectLong;
            IdObjectVersion = oSubAct.IdObjectVersion;
        }

        public dtoSubActivity(SubActivity oSubAct, Status PersonalStatus)
            : base(oSubAct, PersonalStatus)
        {
            ActivityParentId = oSubAct.ParentActivity.Id;
            ContentType = oSubAct.ContentType;
            Completion = -1;
            Link = oSubAct.Link;
            StatusStat = StatusStatistic.None;
            ModuleLink = oSubAct.ModuleLink;
            Status = oSubAct.Status;

            IdCertificate = oSubAct.IdCertificate;
            IdCertificateVersion = oSubAct.IdCertificateVersion;
            Name = oSubAct.Name;
            SaveCertificate = oSubAct.SaveCertificate;
            AllowWithEmptyPlaceHolders = oSubAct.AllowWithEmptyPlaceHolders;
            ActiveOnMinCompletion = oSubAct.ActiveOnMinCompletion;
            ActiveOnMinMark = oSubAct.ActiveOnMinMark;
            ActiveAfterPathEndDate = oSubAct.ActiveAfterPathEndDate;
            UsePathEndDateStatistics = oSubAct.UsePathEndDateStatistics;
            AutoGenerated = oSubAct.AutoGenerated;
            Display = oSubAct.Display;
            IdObject = oSubAct.IdObjectLong;
            IdObjectVersion = oSubAct.IdObjectVersion;
        }
        public dtoSubActivity(SubActivity oSubAct, Status PersonalStatus, RoleEP RoleEp)
            : base(oSubAct, PersonalStatus, RoleEp)
        {
            ActivityParentId = oSubAct.ParentActivity.Id;
            ContentType = oSubAct.ContentType;
            Completion = -1;
            Link = oSubAct.Link;
            StatusStat = StatusStatistic.None;
            ModuleLink = oSubAct.ModuleLink;
            Status = oSubAct.Status;

            IdCertificate = oSubAct.IdCertificate;
            IdCertificateVersion = oSubAct.IdCertificateVersion;
            Name = oSubAct.Name;
            SaveCertificate = oSubAct.SaveCertificate;
            AllowWithEmptyPlaceHolders = oSubAct.AllowWithEmptyPlaceHolders;
            ActiveOnMinCompletion = oSubAct.ActiveOnMinCompletion;
            ActiveOnMinMark = oSubAct.ActiveOnMinMark;
            ActiveAfterPathEndDate = oSubAct.ActiveAfterPathEndDate;
            UsePathEndDateStatistics = oSubAct.UsePathEndDateStatistics;
            AutoGenerated = oSubAct.AutoGenerated;
            Display = oSubAct.Display;
            IdObject = oSubAct.IdObjectLong;
            IdObjectVersion = oSubAct.IdObjectVersion;
        }

        public dtoSubActivity(SubActivity oSubAct)
        {
            Completion = -1;
            RoleEP = RoleEP.None;
            PermissionEP = new PermissionEP(RoleEP);
            if (oSubAct != null)
            {
                Weight = oSubAct.Weight;

                Id = oSubAct.Id;
                ActivityParentId = oSubAct.ParentActivity.Id;
                ContentType = oSubAct.ContentType;
                Link = oSubAct.Link;
                ModuleLink = oSubAct.ModuleLink;
                Status = oSubAct.Status;

                IdCertificate = oSubAct.IdCertificate;
                IdCertificateVersion = oSubAct.IdCertificateVersion;
                Name = oSubAct.Name;
                Description = oSubAct.Description;
                SaveCertificate = oSubAct.SaveCertificate;
                AllowWithEmptyPlaceHolders = oSubAct.AllowWithEmptyPlaceHolders;
                ActiveOnMinCompletion = oSubAct.ActiveOnMinCompletion;
                ActiveOnMinMark = oSubAct.ActiveOnMinMark;
                ActiveAfterPathEndDate = oSubAct.ActiveAfterPathEndDate;
                UsePathEndDateStatistics = oSubAct.UsePathEndDateStatistics;
                AutoGenerated = oSubAct.AutoGenerated;
                Display = oSubAct.Display;
                IdObject = oSubAct.IdObjectLong;
                IdObjectVersion = oSubAct.IdObjectVersion;
            }
        }
    }
}