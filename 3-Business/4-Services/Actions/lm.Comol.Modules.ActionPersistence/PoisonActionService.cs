using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.ActionDataContract;
using lm.ActionPersistence;
using System.ServiceModel;

namespace lm.ActionPersistence
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single)]
    public class PoisonActionService : iActionService
    {

        #region iActionService Members

        public void InsertLoginAction(LoginAction oLoginAction)
        {
            bool res = DALLoginAction.LoginAction_Insert(oLoginAction);
            
        }

        public void UpdateLoginAction(LoginAction oLoginAction)
        {
            bool res = DALLoginAction.LoginAction_Update(oLoginAction);

        }

        public void InsertUserAction(UserAction oAction)
        {
            bool res = DALUserAction.UserAction_Insert(oAction);

        }

        // da testare
        public void InsertBrowserInfo(BrowserInfo oBrowserInfo)
        {
            bool res = DALBrowserInfo.BrowserInfo_Insert(oBrowserInfo);
        }

        // da testare
        public void UpdateModuleUsageTime(ModuleUsageTime OUsageTime)
        {
            bool res = DALModuleUsageTime.ModuleUsageTime_Update(OUsageTime);
        }

        public void InsertCommunityAction(CommunityAction oAction)
        {
            bool res = DALCommunityAction.CommunityAction_Insert(oAction);

        }

        public void UpdateCommunityAction(CommunityAction oAction)
        {
            bool res = DALCommunityAction.CommunityAction_Update(oAction);

        }

        public void InsertModuleAction(ModuleAction oAction)
        {
            bool res = DALModuleAction.ModuleAction_Insert(oAction);

        }

        public void UpdateModuleAction(ModuleAction oAction)
        {
            bool res = DALModuleAction.ModuleAction_Update(oAction);
        }

        #endregion
        
    }
}
