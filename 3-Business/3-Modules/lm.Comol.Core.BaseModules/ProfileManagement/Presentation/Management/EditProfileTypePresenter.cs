﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Business;
using lm.Comol.Core.Authentication.Business;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.BaseModules.ProfileManagement.Business;
using lm.Comol.Core.Authentication;

namespace lm.Comol.Core.BaseModules.ProfileManagement.Presentation
{
    public class EditProfileTypePresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
        private int _ModuleID;
        private ProfileManagementService _Service;
        //private int ModuleID
        //{
        //    get
        //    {
        //        if (_ModuleID <= 0)
        //        {
        //            _ModuleID = this.Service.ServiceModuleID();
        //        }
        //        return _ModuleID;
        //    }
        //}
        public virtual BaseModuleManager CurrentManager { get; set; }
        protected virtual IViewEditProfileType View
        {
            get { return (IViewEditProfileType)base.View; }
        }
        private ProfileManagementService Service
        {
            get
            {
                if (_Service == null)
                    _Service = new ProfileManagementService(AppContext);
                return _Service;
            }
        }
        public EditProfileTypePresenter(iApplicationContext oContext)
            : base(oContext)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        public EditProfileTypePresenter(iApplicationContext oContext, IViewEditProfileType view)
            : base(oContext, view)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        #endregion

        public void InitView(Int32 idProfile)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                ModuleProfileManagement module = ModuleProfileManagement.CreatePortalmodule(UserContext.UserTypeID);
                View.AllowManagement = (module.EditProfile || module.Administration || module.ViewProfiles);
                if (module.EditProfile || module.Administration)
                {
                    Person person = CurrentManager.GetPerson(idProfile);
                    if (person == null)
                        View.DisplayProfileUnknown();
                    else
                    {
                        View.IdProfile = person.Id;
                        View.IdProfileType = person.TypeID;
                        View.LoadProfileTypes(person.TypeID, GetExceptedProfileTypes(UserContext.UserTypeID));
                        dtoProfilePermission permission = new dtoProfilePermission(UserContext.UserTypeID, person.TypeID);
                        View.AllowEdit = permission.ChangeProfileType;
                        View.LoadProfileName(person.SurnameAndName);
                        if (permission.ChangeProfileType)
                            View.LoadProfile(person.Id, person.TypeID);
                        else
                            View.DisplayNoPermission();
                    }
                }
                else
                    View.DisplayNoPermission();
            }
        }

        private List<Int32> GetExceptedProfileTypes(Int32 idType) {
            List<Int32> items = new List<Int32>();
            switch (UserContext.UserTypeID) { 
                case (int) UserTypeStandard.Administrator:
                    items.Add((Int32)UserTypeStandard.SysAdmin);
                    items.Add((Int32)UserTypeStandard.Administrator);
                    break;
                case (int)UserTypeStandard.Administrative:
                    items.Add((Int32)UserTypeStandard.SysAdmin);
                    items.Add((Int32)UserTypeStandard.Administrator);
                    items.Add((Int32)UserTypeStandard.Administrative);
                    break;
                case (int)UserTypeStandard.SysAdmin:
                    break;
                default:
                    items.Add((Int32)UserTypeStandard.SysAdmin);
                    items.Add((Int32)UserTypeStandard.Administrator);
                    items.Add((Int32)UserTypeStandard.Administrative);
                    break;
            }
            if (items.Contains(idType))
                items.Remove(idType);
            return items;
        }
        public void ProfileChanged(Boolean saved) {
            if (saved)
                View.GotoManagement();
            else {
                View.DisplayErrorSaving();
            }
        }
    }
}