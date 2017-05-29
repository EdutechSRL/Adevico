using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using NHibernate;
using lm.Comol.Core.Data;
using lm.Comol.Core.DomainModel;
using NHibernate.Linq;
using WS_CoreServices.Domain;
using COL_BusinessLogic_v2.UCServices;

namespace WS_CoreServices
{
    /// <summary>
    /// Summary description for CommunityServices
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]


    public class CommunityServices : System.Web.Services.WebService
    {
        [WebMethod]
        public List<int> GetCommunityMembersID(int CommunityID)
        {

            using (ISession session = NHSessionHelper.GetSession())
            {
                DataContext dc = new DataContext(session);
                iCommunity community = dc.GetById<Community>(CommunityID);
                List<int> Members = (from sub in session.Linq<Subscription>() where sub.Community == community && sub.Enabled && sub.Enabled  && sub.Role.Id>0  select sub.Person.Id).ToList<int>();
                
                return Members;
            }
        }

        [WebMethod]
        public List<int> GetCommunityRoleMembersID(int CommunityID, int RoleID)
        {

            using (ISession session = NHSessionHelper.GetSession())
            {
                DataContext dc = new DataContext(session);
                List<int> Members = new List<int>();
                if (CommunityID == 0) {
                    Members = (from p in session.Linq<Person>() where p.TypeID == RoleID && p.isDisabled == false select p.Id).ToList<int>();
                }
                else {
                    iCommunity community = dc.GetById<Community>(CommunityID);
                    Members = (from sub in session.Linq<Subscription>() where sub.Community == community && sub.Enabled && sub.Enabled && sub.Role.Id == RoleID select sub.Person.Id).ToList<int>();
                }
                
                return Members;
            }
        }
        //------------------------------
        // CHE SUCCEDE SE NON CI SONO DATI VALIDI??
        //--------------------------------
        [WebMethod]
        public List<int> GetPermissionMembersID(int CommunityID,int ServiceID,int Permission)
        {

            using (ISession session = NHSessionHelper.GetSession())
            {
                DataContext dc = new DataContext(session);
                iCommunity community = dc.GetById<Community>(CommunityID);
                if (community == null && CommunityID==0)
                    return GetPortalPermissionMembersID(ServiceID,Permission);
                else if (community == null)
                     return new List<int>();

                iModuleDefinition CommunityModule;
                try{
                        CommunityModule = (from cModule in session.Linq<CommunityModuleAssociation>()
                                                     where cModule.Enabled && cModule.Service.Available && cModule.Community == community && cModule.Service.Id == ServiceID
                                                     select cModule.Service).FirstOrDefault<ModuleDefinition>();
                    }
                catch (Exception ex){
                    CommunityModule = null;
                }

                if (CommunityModule == null)
                {
                    return new List<int>();
                }
                else{
                    //Find all Role perission for the input community and the input service
                    List<CommunityRoleModulePermission> RolePermissionList = (from crmp in session.Linq<CommunityRoleModulePermission>() 
                                            where crmp.Community == community && crmp.Service== CommunityModule
                                                                     select crmp).ToList<CommunityRoleModulePermission>();
                    // Find ONLY roles !
                    List<int> RolesList = (from o in RolePermissionList 
                                            where PermissionHelper.CheckPermissionSoft(Permission,o.PermissionInt)
                                           select o.Role.Id).ToList<int>();

                    if (RolesList.Count==0){
                        return new List<int>();
                    }
                    else{
                        //List<int> UsersID = (from r in RolesList
                        //                     join Subscription sub in session.Linq<Subscription>() on r equals sub.Role
                        //                     where sub.Community == community && sub.Accepted && sub.Enabled && sub.Role.Id > 0
                        //                     select sub.Person.Id)..ToList<int>();

                        List<SmallRolePerson> rpList = (from Subscription sub in session.Linq<Subscription>()
                                                        where sub.Community == community && sub.Accepted && sub.Enabled && sub.Role.Id > 0 && RolesList.Contains(sub.Role.Id)
                                              select new SmallRolePerson(sub.Role.Id, sub.Person.Id)).ToList<SmallRolePerson>();

                        List<int> UsersID = (from Subscription sub in session.Linq<Subscription>()
                                             where sub.Community == community && sub.Accepted && sub.Enabled && sub.Role.Id > 0 && RolesList.Contains(sub.Role.Id)
                                             select sub.Person.Id).ToList<int>();
                            
                            //(from roleID in RolesList
                            //                 join SmallRolePerson rp in rpList on roleID equals rp.RoleID
                            //                 select rp.PersonID).ToList<int>();
                        return UsersID;
                    }
                }
            }
        }

        private List<int> GetPermissionMembersID(ISession session, int CommunityID, string ServiceCode, int Permission)
        {
            iCommunity community = session.Get<Community>(CommunityID);
            if (community == null && CommunityID == 0)
                return GetPortalPermissionMembersID(session, ServiceCode, Permission);
            else if (community == null)
                return new List<int>();

                iModuleDefinition CommunityModule;
                try
                {
                    CommunityModule = (from cModule in session.Linq<CommunityModuleAssociation>()
                                       where cModule.Enabled && cModule.Service.Available && cModule.Community == community && cModule.Service.Code == ServiceCode
                                       select cModule.Service).FirstOrDefault<ModuleDefinition>();
                }
                catch (Exception ex)
                {
                    CommunityModule = null;
                }

                if (CommunityModule == null)
                {
                    return new List<int>();
                }
                else
                {
                    //Find all Role perission for the input community and the input service
                    List<CommunityRoleModulePermission> RolePermissionList = (from crmp in session.Linq<CommunityRoleModulePermission>()
                                                                              where crmp.Community == community && crmp.Service == CommunityModule
                                                                              select crmp).ToList<CommunityRoleModulePermission>();
                    // Find ONLY roles !
                    List<int> RolesList = (from o in RolePermissionList
                                           where PermissionHelper.CheckPermissionSoft(Permission, o.PermissionInt)
                                           select o.Role.Id).ToList<int>();

                    if (RolesList.Count == 0)
                    {
                        return new List<int>();
                    }
                    else
                    {

                        List<int> UsersID = (from Subscription sub in session.Linq<Subscription>()
                                                        where sub.Community == community && sub.Accepted && sub.Enabled && sub.Role.Id > 0 && RolesList.Contains(sub.Role.Id)
                                             select sub.Person.Id).ToList<int>();

                        return UsersID;
                    }
                }
        }

        [WebMethod]
        public List<int> GetPortalPermissionMembersID(int ServiceID, int Permission)
        {
            List<int> iResponse = new List<int>();
            string ServiceCode = "";

            using (ISession session = NHSessionHelper.GetSession())
            {
                DataContext dc = new DataContext(session);
                try
                {
                    ServiceCode = (from cModule in session.Linq<ModuleDefinition>()
                                       where cModule.Available && cModule.Id == ServiceID
                                   select cModule.Code).FirstOrDefault<string>();
                }
                catch (Exception ex){}

                if (ServiceCode != "")
                    iResponse = GetPortalPermissionMembersID(session,ServiceCode, Permission);
            }
           
            return iResponse;
        }
        private List<int> GetPortalPermissionMembersID(ISession session,string ServiceCode, int Permission)
        {
            List<int> iResponse = new List<int>();
                if (ServiceCode != "")
                {
                    List<int> AvailableTypes = (from p in session.Linq<Person>() select p.TypeID).Distinct().ToList<int>();
                    List<int> TypesToSearch = GetPersonTypesByService(ServiceCode, AvailableTypes, Permission);

                    foreach (int typeID in TypesToSearch)
                    {
                        List<int> UsersID = (from p in session.Linq<Person>() where p.TypeID == typeID && p.isDisabled == false select p.Id).Distinct().ToList<int>();
                        if (UsersID != null && UsersID.Count > 0)
                            iResponse.AddRange(UsersID);
                    }


                }
                return iResponse;
        }

        [WebMethod]
        public List<int> GetItemGuidMembersID(int CommunityID, string ModuleCode, System.Guid ItemID, int objectTypeID, int Permission)
        {
            List<int> iResponse = new List<int>();
            string ServiceCode = "";

            using (ISession session = NHSessionHelper.GetSession())
            {
                DataContext dc = new DataContext(session);
                try
                {
                    //ServiceCode = (from cModule in session.Linq<ModuleDefinition>()
                    //               where cModule.Available && cModule.Id == ModuleID
                    //               select cModule.Code).FirstOrDefault<string>();
                }
                catch (Exception ex) { }

                if (ServiceCode != "")
                {
                }
            }
            return iResponse;

        }

        [WebMethod]
        public List<int> GetItemIntMembersID(int CommunityID, string ModuleCode, int ItemID, int objectTypeID, int Permission)
        {
            List<int> iResponse = new List<int>();
            string ServiceCode = "";

            using (ISession session = NHSessionHelper.GetSession())
            {
                DataContext dc = new DataContext(session);
                try
                {
                    //ServiceCode = (from cModule in session.Linq<ModuleDefinition>()
                                   //where cModule.Available && cModule.Id == ModuleID
                                   //select cModule.Code).FirstOrDefault<string>();
                }
                catch (Exception ex) { }

                if (ServiceCode != "")
                {
                }
            }
            return iResponse;

        }

        [WebMethod]
        public List<int> GetItemLongMembersID(int CommunityID, string ModuleCode, long ItemID, int objectTypeID,int Permission)
        {
            List<int> iResponse = new List<int>();
            // using (ISession session = NHSessionHelper.GetSession())
            //{
            //    DataContext dc = new DataContext(session);
                try
                {
                    switch (ModuleCode)
                    {
                        case "SRVMATER":
                            iResponse = this.OldGetItemRepositoryMembersID(CommunityID, ItemID, Permission);
                            break;

                    }
                }
                catch (Exception ex) { }

                //if (ServiceCode != "")
                //{
                //}
           // }
            return iResponse;

        }

        private List<int> GetPersonTypesByService(string ServiceCode,List<int> AvailableTypes,long PermissionSearched) {
            List<int> types = new List<int>();
            foreach (int typeID in AvailableTypes){
                int PermissionForPersonType = 0;                
                switch (ServiceCode) { 
                    case "SRVBACH":
                        Services_Bacheca service = Services_Bacheca.Create();
                        service.Admin = (typeID == (int)PersonTypeStandard.AdminSecondario || typeID == (int)PersonTypeStandard.SysAdmin || typeID == (int)PersonTypeStandard.Amministrativo);
                        service.Read = (typeID != (int)PersonTypeStandard.Guest);
                        service.GrantPermission = (typeID == (int)PersonTypeStandard.AdminSecondario || typeID == (int)PersonTypeStandard.SysAdmin);
                        service.Write = (typeID == (int)PersonTypeStandard.AdminSecondario || typeID == (int)PersonTypeStandard.SysAdmin || typeID == (int)PersonTypeStandard.Amministrativo);
                        PermissionForPersonType = service.ConvertToInt();
                        break;
                    case "SRVQUST":
                        Services_Questionario serviceQuest = Services_Questionario.Create();
                        serviceQuest.Compila = (typeID != (int)PersonTypeStandard.Guest);
                        serviceQuest.Admin = (typeID == (int)PersonTypeStandard.AdminSecondario || typeID == (int)PersonTypeStandard.SysAdmin || typeID == (int)PersonTypeStandard.Amministrativo);
                        serviceQuest.QuestionariSuInvito = (typeID == (int)PersonTypeStandard.AdminSecondario || typeID == (int)PersonTypeStandard.SysAdmin || typeID == (int)PersonTypeStandard.Amministrativo);
                        serviceQuest.VisualizzaStatistiche = (typeID == (int)PersonTypeStandard.AdminSecondario || typeID == (int)PersonTypeStandard.SysAdmin || typeID == (int)PersonTypeStandard.Amministrativo);
                        serviceQuest.GrantPermission = (typeID == (int)PersonTypeStandard.AdminSecondario || typeID == (int)PersonTypeStandard.SysAdmin);
                        serviceQuest.GestioneDomande = (typeID == (int)PersonTypeStandard.AdminSecondario || typeID == (int)PersonTypeStandard.SysAdmin || typeID == (int)PersonTypeStandard.Amministrativo);
                        serviceQuest.CancellaQuestionario = (typeID == (int)PersonTypeStandard.AdminSecondario || typeID == (int)PersonTypeStandard.SysAdmin || typeID == (int)PersonTypeStandard.Amministrativo);
                        serviceQuest.CopiaQuestionario = (typeID == (int)PersonTypeStandard.AdminSecondario || typeID == (int)PersonTypeStandard.SysAdmin || typeID == (int)PersonTypeStandard.Amministrativo);
                        PermissionForPersonType = serviceQuest.ConvertToInt();
                        break;
                    case "SRVMATER":
                        Services_File serviceFile = Services_File.Create();
                        
                        serviceFile.Admin=(typeID  == (int)PersonTypeStandard.AdminSecondario || typeID == (int)PersonTypeStandard.SysAdmin);
                        serviceFile.Change = (typeID == (int)PersonTypeStandard.AdminSecondario || typeID == (int)PersonTypeStandard.SysAdmin);
                        serviceFile.Delete = (typeID == (int)PersonTypeStandard.AdminSecondario || typeID == (int)PersonTypeStandard.SysAdmin);
serviceFile.GrantPermission=(typeID  == (int)PersonTypeStandard.AdminSecondario || typeID == (int)PersonTypeStandard.SysAdmin);
serviceFile.Print=(typeID != (int)PersonTypeStandard.Guest);

serviceFile.Read=(typeID != (int)PersonTypeStandard.Guest);

                        serviceFile.Upload=(typeID  == (int)PersonTypeStandard.AdminSecondario || typeID == (int)PersonTypeStandard.SysAdmin  || typeID == (int)PersonTypeStandard.Amministrativo);
                        PermissionForPersonType = serviceFile.ConvertToInt();
                        break;
                    default:
                        PermissionForPersonType =0;
                        break;
                }
                if (PermissionHelper.CheckPermissionSoft(PermissionSearched,PermissionForPersonType))
                    types.Add(typeID);
            }
            return types;
        }

        #region "Repository Permission"
            private List<int> OldGetItemRepositoryMembersID(int CommunityID, long ItemID, int Permission)
            {
                List<int> iResponse = new List<int>();

                using (ISession session = NHSessionHelper.GetSession())
                {
                    DataContext dc = new DataContext(session);
                    try
                    {
                        CommunityFile oItem = dc.GetById<CommunityFile>(ItemID);
                        if (oItem != null && ((CommunityID == 0 && oItem.CommunityOwner == null) || (CommunityID > 0 && oItem.CommunityOwner != null)))
                        {
                            List<int> AssignedCommunity = new List<int>();
                            AssignedCommunity = (from ca in session.Linq<CommunityFileCommunityAssignment>() where ca.AssignedTo == oItem.CommunityOwner && ca.File == oItem && ca.Inherited select ca.AssignedTo.Id).ToList<int>();
                            if (AssignedCommunity.Count > 0)
                            {
                                iResponse = GetPermissionMembersID(session, CommunityID, Services_File.Codex, Permission);
                            }
                            else
                            {
                                List<int> AssignedUsers = new List<int>();
                                List<int> AssignedRoles = new List<int>();
                                List<int> AssignedUsersTypes = new List<int>();
                                if (oItem.CommunityOwner == null)
                                {
                                    AssignedUsersTypes = (from ca in session.Linq<CommunityFilePersonTypeAssignment>() where ca.File == oItem && ca.Inherited select ca.AssignedTo).ToList<int>();
                                    if (AssignedUsersTypes.Count > 0)
                                        iResponse.AddRange(GetByRolePermissionMembersID(session, 0, Services_File.Codex, AssignedUsersTypes, Permission));
                                }
                                else
                                {
                                    AssignedRoles = (from ca in session.Linq<CommunityFileRoleAssignment>() where ca.File == oItem && ca.Inherited select ca.AssignedTo.Id).ToList<int>();
                                    if (AssignedRoles.Count > 0)
                                        iResponse.AddRange(GetByRolePermissionMembersID(session, oItem.CommunityOwner.Id, Services_File.Codex, AssignedRoles, Permission));
                                }
                                AssignedUsers = (from ca in session.Linq<CommunityFilePersonAssignment>() where ca.File == oItem && !iResponse.Contains<int>(ca.AssignedTo.Id) select ca.AssignedTo.Id).ToList<int>();
                                AssignedUsers = EvaluateRepositoryUsers(session, AssignedUsers, oItem.FolderId, oItem.CommunityOwner, Services_File.Codex, Permission);
                                if (AssignedUsers.Count > 0)
                                    iResponse.AddRange(AssignedUsers);

                            }
                            if (PermissionHelper.CheckPermissionSoft((long)(Services_File.Base2Permission.AdminService) | (int)(Services_File.Base2Permission.Moderate), Permission))
                            {
                                List<int> AvailableMembersID = GetPermissionMembersID(session, CommunityID, Services_File.Codex, (int)(Services_File.Base2Permission.AdminService) | (int)(Services_File.Base2Permission.Moderate));
                                iResponse.AddRange((from Id in AvailableMembersID where iResponse.Contains(Id) == false select Id).ToList<int>());
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
                return (from UserID in iResponse select UserID).Distinct().ToList<int>();
            }
        
        //private bool RepositoryCommunityIsInRole(ISession session,int CommunityID,long FatherID, int Permission)
        //{
        //    bool iResponse = false;
        //    if (FatherID == 0)
        //        iResponse = true;
        //    else
        //    {
        //        while (FatherID>0){
        //            CommunityFile oFolder = session.Get<CommunityFile>(FatherID);
        //            if (oFolder != null){
        //                iResponse = (from ca in session.Linq<CommunityFileCommunityAssignment>() where ca.AssignedTo == oFolder.CommunityOwner && ca.File == oFolder && ca.Deny == false select ca.AssignedTo.Id).Any();
        //            }
        //            else
        //                iResponse = false;
        //            if (iResponse)
        //                FatherID = oFolder.FolderId;
        //            else
        //                FatherID=0;
        //        }                       
        //    }
        //    return iResponse;
        //}
        //private bool RepositoryRoleIsInRole(ISession session, int RoleID, long FatherID, int Permission){
        //    bool iResponse = false;
        //    if (FatherID == 0)
        //        iResponse = true;
        //    else
        //    {
        //        while (FatherID > 0)
        //        {
        //            CommunityFile oFolder = session.Get<CommunityFile>(FatherID);
        //            if (oFolder != null)
        //            {
        //                iResponse = (from ca in session.Linq<CommunityFileCommunityAssignment>() where ca.AssignedTo == oFolder.CommunityOwner && ca.File == oFolder && ca.Deny==false  select ca.AssignedTo.Id).Any();
        //                if (iResponse==false)
        //                    iResponse = (from ca in session.Linq<CommunityFileRoleAssignment>() where ca.AssignedTo.Id == RoleID && ca.File == oFolder && ca.Deny==false select ca.AssignedTo.Id).Any();
        //            }
        //            else
        //                iResponse = false;
        //            if (iResponse)
        //                FatherID = oFolder.FolderId;
        //            else
        //                FatherID = 0;
        //        }
        //    }
        //    return iResponse;
        //}
        //private bool RepositoryUserTypeIsInRole(ISession session, int PersonTypeID, long FatherID, int Permission)
        //{
        //    bool iResponse = false;
        //    if (FatherID == 0)
        //        iResponse = true;
        //    else
        //    {
        //        while (FatherID > 0)
        //        {
        //            CommunityFile oFolder = session.Get<CommunityFile>(FatherID);
        //            if (oFolder != null)
        //            {
        //                iResponse = (from ca in session.Linq<CommunityFileCommunityAssignment>() where ca.AssignedTo == oFolder.CommunityOwner && ca.File == oFolder && ca.Deny == false select ca.AssignedTo.Id).Any();
        //                if (iResponse == false)
        //                    iResponse = (from ca in session.Linq<CommunityFilePersonTypeAssignment>() where ca.AssignedTo == PersonTypeID && ca.File == oFolder && ca.Deny == false select ca.AssignedTo).Any();
        //            }
        //            else
        //                iResponse = false;
        //            if (iResponse)
        //                FatherID = oFolder.FolderId;
        //            else
        //                FatherID = 0;
        //        }
        //    }
        //    return iResponse;
        //}
        private List<int> EvaluateRepositoryUsers(ISession session, List<int> AssignedUsers, long FatherID, Community oCommunity, string ServiceCode, int Permission)
        {
            List<int> iResponse = new List<int>();
            if (FatherID == 0)
                iResponse = AssignedUsers;
            else
            {
                if (oCommunity == null)
                {
                    var UserTypes = (from p in session.Linq<litePerson>()
                                     where p.isDisabled == false && AssignedUsers.Contains(p.Id)
                                     select new { PersonID = p.Id, TypeId = p.TypeID }).ToList();
                    List<int> AvailableTypes = (from ut in UserTypes select ut.TypeId).Distinct().ToList<int>();
                    List<int> TypesToSearch = GetPersonTypesByService(ServiceCode, AvailableTypes, Permission);

                    iResponse = (from UserType in UserTypes
                                 join type in TypesToSearch on UserType.TypeId equals type
                                 select UserType.PersonID).ToList<int>();

                }
                else
                {
                    iModuleDefinition CommunityModule;
                    try
                    {
                        CommunityModule = (from cModule in session.Linq<CommunityModuleAssociation>()
                                           where cModule.Enabled && cModule.Service.Available && cModule.Community == oCommunity && cModule.Service.Code == ServiceCode
                                           select cModule.Service).FirstOrDefault<ModuleDefinition>();
                    }
                    catch (Exception ex)
                    {
                        CommunityModule = null;
                    }
                    if (CommunityModule == null)
                    {
                        return new List<int>();
                    }
                    else
                    {
                        var UserRoles = (from us in session.Linq<LazySubscription>()
                                         where us.IdCommunity == oCommunity.Id && us.Accepted && us.Enabled && us.IdRole > 0 && AssignedUsers.Contains(us.IdPerson)
                                         select new { PersonID = us.IdPerson, RoleID = us.IdRole }).ToList();
                        //Find all Role perission for the input community and the input service
                        List<int> UsersRolesList = (from oCouple in UserRoles select oCouple.RoleID).Distinct().ToList<int>();
                        List<CommunityRoleModulePermission> RolePermissionList = (from crmp in session.Linq<CommunityRoleModulePermission>()
                                                                                  where crmp.Community == oCommunity && crmp.Service == CommunityModule && UsersRolesList.Contains(crmp.Role.Id)
                                                                                  select crmp).ToList<CommunityRoleModulePermission>();
                        // Find ONLY roles !
                        List<int> RolesList = (from o in RolePermissionList
                                               where PermissionHelper.CheckPermissionSoft(Permission, o.PermissionInt)
                                               select o.Role.Id).ToList<int>();

                        if (RolesList.Count == 0)
                        {
                            return new List<int>();
                        }
                        else
                        {

                            iResponse = (from useRole in UserRoles
                                         join role in RolesList on useRole.RoleID equals role
                                         select useRole.PersonID).ToList<int>();
                        }
                    }
                }
            }
            return iResponse;
        }
        //private bool RepositoryUserIsInRole(ISession session, int UserID,int RoleID, long FatherID,int CommunityID, int Permission)
        //{
        //    bool iResponse = false;
        //    if (FatherID == 0)
        //        iResponse = true;
        //    else
        //    {
        //        while (FatherID > 0)
        //        {
        //            CommunityFile oFolder = session.Get<CommunityFile>(FatherID);
        //            if (oFolder != null)
        //            {
        //                iResponse = (from ca in session.Linq<CommunityFileCommunityAssignment>() where ca.AssignedTo == oFolder.CommunityOwner && ca.File == oFolder && ca.Deny == false select ca.AssignedTo.Id).Any();
        //                if (iResponse == false && CommunityID==0)
        //                    iResponse = (from ca in session.Linq<CommunityFilePersonTypeAssignment>() where ca.AssignedTo == RoleID && ca.File == oFolder && ca.Deny == false select ca.AssignedTo).Any();
        //                else if (iResponse == false && CommunityID > 0)
        //                    iResponse = (from ca in session.Linq<CommunityFileRoleAssignment>() where ca.AssignedTo.Id  == RoleID && ca.File == oFolder && ca.Deny == false select ca.AssignedTo).Any();

        //                if (!iResponse)
        //                    iResponse = (from ca in session.Linq<CommunityFilePersonAssignment>() where ca.AssignedTo.Id == UserID && ca.File == oFolder && ca.Deny == false select ca.AssignedTo).Any();
        //            }
        //            else
        //                iResponse = false;
        //            if (iResponse)
        //                FatherID = oFolder.FolderId;
        //            else
        //                FatherID = 0;
        //        }
        //    }
        //    return iResponse;
        //}
        private List<int> GetByRolePermissionMembersID(ISession session, int idCommunity, string moduleCode, List<int> AssignedRoles, int Permission)
        {
            iCommunity community = session.Get<Community>(idCommunity);
            if (community == null && idCommunity == 0)
                return GetByTypePermissionMembersID(session, moduleCode, AssignedRoles, Permission);
            else if (community == null)
                return new List<int>();

            iModuleDefinition CommunityModule;
            try
            {
                CommunityModule = (from cModule in session.Linq<CommunityModuleAssociation>()
                                   where cModule.Enabled && cModule.Service.Available && cModule.Community == community && cModule.Service.Code == moduleCode
                                   select cModule.Service).FirstOrDefault<ModuleDefinition>();
            }
            catch (Exception ex)
            {
                CommunityModule = null;
            }

            if (CommunityModule == null)
            {
                return new List<int>();
            }
            else
            {
                //Find all Role perission for the input community and the input service
                List<CommunityRoleModulePermission> RolePermissionList = (from crmp in session.Linq<CommunityRoleModulePermission>()
                                                                          where crmp.Community == community && crmp.Service == CommunityModule && AssignedRoles.Contains(crmp.Role.Id)
                                                                          select crmp).ToList<CommunityRoleModulePermission>();
                // Find ONLY roles !
                List<int> RolesList = (from o in RolePermissionList
                                       where PermissionHelper.CheckPermissionSoft(Permission, o.PermissionInt)
                                       select o.Role.Id).ToList<int>();

                if (RolesList.Count == 0)
                {
                    return new List<int>();
                }
                else
                {
                    return (from LazySubscription sub in session.Linq<LazySubscription>()
                            where sub.IdCommunity == idCommunity && sub.Accepted && sub.Enabled && sub.IdRole > 0 && RolesList.Contains(sub.IdRole)
                            select sub.IdPerson).ToList().Distinct().ToList();
                }
            }
        }
            private List<Int32> GetByTypePermissionMembersID(ISession session, String moduleCode, List<Int32> assignedTypes, Int32 permission)
            {
                List<Int32> result = new List<Int32>();
                if (!String.IsNullOrWhiteSpace(moduleCode))
                {
                    List<Int32> availableTypes = (from p in session.Linq<litePerson>() select p.TypeID).Distinct().ToList<int>();
                    availableTypes = (from t in availableTypes where assignedTypes.Contains(t) select t).ToList<int>();
                    List<Int32> TypesToSearch = GetPersonTypesByService(moduleCode, availableTypes, permission);

                    result = (from p in session.Linq<litePerson>() where TypesToSearch.Contains(p.TypeID) && p.isDisabled == false select p.Id).Distinct().ToList<Int32>();

                }
                return result;
            }
        #endregion
    }


    enum PersonTypeStandard{
		Tutti_WithGuest = -2,
		Tutti_NoGuest = -1,
		Studente = 1,
		Docente = 2,
		Tutor = 3,
		Esterno = 4,
		Altro = 5,
		Amministrativo = 6,
		SysAdmin = 7,
		Copisteria = 8,
		Dottorando = 9,
		StudenteSuperiori = 10,
		DocenteSuperiori = 11,
		Direttore = 12,
		Ricercatore = 13,
		DocenteStandard = 14,
		StudenteStandard = 15,
		ExStudente = 16,
		Tecnico = 17,
		Guest = 18,
		AdminSecondario = 19,
	}
}