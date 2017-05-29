using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

  namespace lm.Comol.Modules.TaskList
{
   
   [Serializable, CLSCompliant(true)]
    public class ModuleTasklist
    {
      public const string UniqueID = "SRVTASK";
      public virtual Boolean UploadFile {get;set;}
      public virtual Boolean DeleteTask {get;set;}
      public virtual Boolean ManagementPermission {get;set;}        
      public virtual Boolean AddTask {get;set;}
      public virtual Boolean Edit {get;set;}
      public virtual Boolean PrintList {get;set;}
      public virtual Boolean ViewTasks { get; set; }
      public virtual Boolean Administration { get; set; }
      public virtual Boolean CreateCommunityProject{get;set;}
      public virtual Boolean  CreatePersonalCommunityProject{get;set;}
      public virtual Boolean  CreatePersonalProject{get;set;}
      public virtual Boolean  DownloadAllowed{get;set;}
      public virtual Boolean  PrintTaskList{get;set;}
      public virtual Boolean ViewTaskList { get; set; }

      
        public ModuleTasklist() {

         
        }

        public static ModuleTasklist CreatePortalmodule(int UserTypeID){
            ModuleTasklist module = new ModuleTasklist();
            module.Administration = (UserTypeID == (int)UserTypeStandard.SysAdmin || UserTypeID == (int)UserTypeStandard.Administrator);
            module.CreateCommunityProject = (UserTypeID == (int)UserTypeStandard.SysAdmin || UserTypeID == (int)UserTypeStandard.Administrator);
            module.CreatePersonalCommunityProject = (UserTypeID != (int)UserTypeStandard.Guest);
            module.CreatePersonalProject = (UserTypeID != (int)UserTypeStandard.Guest);

            module.DownloadAllowed = (UserTypeID == (int)UserTypeStandard.SysAdmin || UserTypeID == (int)UserTypeStandard.Administrator || UserTypeID == (int)UserTypeStandard.Administrative);
            module.ManagementPermission = (UserTypeID == (int)UserTypeStandard.SysAdmin || UserTypeID == (int)UserTypeStandard.Administrator);
            module.PrintTaskList = (UserTypeID != (int)UserTypeStandard.Guest);
            module.ViewTaskList = (UserTypeID != (int)UserTypeStandard.Guest);
            
            module.UploadFile= (UserTypeID == (int)UserTypeStandard.SysAdmin || UserTypeID == (int)UserTypeStandard.Administrator || UserTypeID == (int)UserTypeStandard.Administrative);
            
            return module;
        }

        public ModuleTasklist(String permission) { 
            
        }
        public ModuleTasklist(long permission)
        {
            Administration = PermissionHelper.CheckPermissionSoft((long)Base2Permission.Administration, permission);
            CreateCommunityProject = PermissionHelper.CheckPermissionSoft((long)Base2Permission.Administration | (long)Base2Permission.AddCommunityProject, permission);
            CreatePersonalCommunityProject = true;   //PermissionHelper.CheckPermissionSoft((long)Base2Permission.ViewLessons | (long)Base2Permission.EditLesson | (long)Base2Permission.AdminService, permission);
            CreatePersonalProject = true;             // PermissionHelper.CheckPermissionSoft((long)Base2Permission.GrantPermission, permission);
            DownloadAllowed = PermissionHelper.CheckPermissionSoft((long)Base2Permission.ViewCommunityProjects| (long)Base2Permission.Administration , permission);
            ManagementPermission = PermissionHelper.CheckPermissionSoft((long)Base2Permission.ManagementPermission , permission);
            PrintTaskList = PermissionHelper.CheckPermissionSoft((long)Base2Permission.Administration | (long)Base2Permission.ViewCommunityProjects, permission);
            ViewTaskList = PermissionHelper.CheckPermissionSoft((long)Base2Permission.Administration | (long)Base2Permission.ViewCommunityProjects, permission); 

            //Administration = PermissionHelper.CheckPermissionSoft(Convert.ToInt64(Base2Permission.Administration), permission);
            //CreateCommunityProject = PermissionHelper.CheckPermissionSoft(Convert.ToInt64(Base2Permission.Administration) | Convert.ToInt64(Base2Permission.AddCommunityProject), permission);
            //CreatePersonalCommunityProject = true;
            //this.CreatePersonalProject = true;
            //this.DownloadAllowed = PermissionHelper.CheckPermissionSoft(Convert.ToInt64(Base2Permission.Administration), permission);
            ////secondo me va rivisto, solo gli admin possono scaricare files?!?!?! Mettere .ViewCommunityTask
            //this.ManagementPermission = PermissionHelper.CheckPermissionSoft(Convert.ToInt64(Base2Permission.ManagementPermission), permission);
            //this.PrintTaskList = PermissionHelper.CheckPermissionSoft(Convert.ToInt64(Base2Permission.Administration) | Convert.ToInt64(Base2Permission.ViewCommunityProjects), permission);
            //this.ViewTaskList = PermissionHelper.CheckPermissionSoft(Convert.ToInt64(Base2Permission.Administration) | Convert.ToInt64(Base2Permission.ViewCommunityProjects), permission);


        }

        [Flags]
        public enum Base2Permission
        {
            AddCommunityProject = 8192, //'13 Add
            Administration = 64, //'6 Admin
            ManagementPermission = 32, //'5 Grant
            ViewCommunityProjects = 1024, //'10 Browse
            AddPersonalProject = 2 // '1 Write
        }

       

         public enum ActionType{
             None = 0,
            NoPermission = 1,
            GenericError = 2,
            StartAddProject = 74000,
            ProjectAdded = 74001,
            AnnulAddProject = 74002,
            StartAddTasks = 74003,
            TaskAdded = 74004,
            AnnulAddTasks = 74005,
            StartVirtualDeleteWithReallocateResource = 74006,
            StartUnDeleteWithReallocateResource = 74007,
            AnnulVirtualDeleteWithReallocateResource = 74008,
            AnnulUnDeleteWithReallocateResource = 74009,
            FinishVirtualDeleteWithReallocateResource = 74010,
            FinishUnDeleteWithReallocateResource = 74011,
            StartUndeleteTask = 74012,
            FinishUndeleteTask = 74013,
            StartVirtualDeleteTask = 74014,
            FinishVirtualDeleteTask = 74015,
            StartDeleteTask = 74016,
            FinishDeleteTask = 74017,
            //'AddedTaskAssignment = 74018
            StartManageTaskAssignment = 74019,
            FinishManageTaskAssignment = 74020,
            StartUpdateTaskDetail = 74021,
            StartViewTaskDetail = 74022,
            FinishUpdateTaskDetail = 74023,
            FinishViewTaskDetail = 74024,
            StartViewProjectMap = 74025,
            FinishViewProjectMap = 74026,
            ViewAssignedTask = 74027,
            ViewTaskManagement = 74028,
            ViewInvolvingProject = 74029,
            ViewGantt = 74030,
            // File
            
            DownloadTaskFile = 74032,
            ShowTaskFile  = 74033,
            HideTaskFile = 74034,
            AddFile = 74035,
            RemoveFile = 74036,
            AddFiles = 74037,
            RemoveFiles = 74038,
            EditItemNoDate = 74040,
            InitUploadMultipleFiles = 74041
            
         }

        public enum ObjectType{
            None = 0,
            Project = 1,
            Task = 2,
            TaskAssignment = 3,
            FileScorm = 4,
            TaskFile = 5,
            TaskLinkedFile = 6
        }

    }
}
