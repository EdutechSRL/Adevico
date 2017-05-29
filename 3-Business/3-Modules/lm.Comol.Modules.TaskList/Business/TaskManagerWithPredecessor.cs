//TaskManager con controllo sui predecessor... e il vecchio TaskAssignmentByPerson
// e TaskAssignmentByCommunityRole ... tutti i metodi sono messi come commento 
// x nn fare casini... X ora funziona tutto tranne la storia dei file e TaskAssignmentByCommunityRole
// che è stato fatto solo il test di addTaskAssignmentByCommunityRole

//ControlTaskDateConsistency in questa versione esegue i controlli anche sui pred e succ... in TaskManager no!!!

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.TaskList.Domain;
using lm.Comol.Core.DomainModel;
using NHibernate;
using NHibernate.Util;
using NHibernate.Linq;
using System.Linq.Expressions;
 

namespace lm.Comol.Modules.TaskList.Business
{


    class TaskManagerWithPredecessor
    {
        private ISession session;

        #region Costruttori

        public TaskManagerWithPredecessor()
        {
        }

        public TaskManagerWithPredecessor(ISession session)
        {
            this.session = session;
        }

        #endregion


        //        #region Priority

        //        public TaskPriority AddTaskPriority(String PriorityName, int PriorityOrder)
        //        {
        //            TaskPriority oTP = new TaskPriority ();
        //            oTP.Name = PriorityName;
        //            oTP.isDeleted = false;
        //            oTP.PriorityOrder = PriorityOrder;
        //            SaveOrUpdateTaskPriority(oTP);
        //            return oTP;
        //        }


        //        public TaskPriority UpdateTaskPriority(TaskPriority TaskPriorityToUpdate, String NewTaskCategoryName)
        //        {
        //            if (!(TaskPriorityToUpdate.Name.Equals(NewTaskCategoryName)))
        //            {   
        //                TaskPriorityToUpdate.Name = NewTaskCategoryName;
        //                SaveOrUpdateTaskPriority(TaskPriorityToUpdate);
        //            }
        //            return TaskPriorityToUpdate;
        //        }

        //        public TaskPriority UpdateTaskPriority(TaskPriority TaskPriorityToUpdate,int NewPriorityOrder)
        //        {
        //            if (TaskPriorityToUpdate.PriorityOrder != NewPriorityOrder)
        //            {
        //                TaskPriorityToUpdate.PriorityOrder = NewPriorityOrder;
        //                SaveOrUpdateTaskPriority(TaskPriorityToUpdate);
        //            }
        //            return TaskPriorityToUpdate;
        //        }

        //        public TaskPriority UpdateTaskPriority(TaskPriority TaskPriorityToUpdate, String NewTaskCategoryName, int NewPriorityOrder) 
        //        {
        //            bool isUpdate=false;
        //            if(!(TaskPriorityToUpdate.Name.Equals(NewTaskCategoryName)))
        //            {
        //            isUpdate=true;
        //                TaskPriorityToUpdate.Name=NewTaskCategoryName;
        //            }

        //            if (TaskPriorityToUpdate.PriorityOrder != NewPriorityOrder)
        //            {
        //                isUpdate = true;
        //                TaskPriorityToUpdate.PriorityOrder = NewPriorityOrder;
        //            }

        //            if (isUpdate)
        //            {
        //                SaveOrUpdateTaskPriority(TaskPriorityToUpdate);
        //            }

        //            return TaskPriorityToUpdate;
        //        }

        //        public TaskPriority UnDeleteTaskPriority(TaskPriority TaskPriorityToDelete)
        //        {
        //            TaskPriorityToDelete.isDeleted = false;
        //            SaveOrUpdateTaskPriority(TaskPriorityToDelete);
        //            return TaskPriorityToDelete;
        //        }

        //        public TaskPriority DeleteVirtualTaskPriority(TaskPriority TaskPriorityToDelete)
        //        {
        //            TaskPriorityToDelete.isDeleted = true;
        //            SaveOrUpdateTaskPriority(TaskPriorityToDelete);
        //            return TaskPriorityToDelete;
        //        }

        //        public IList<TaskPriority> GetAllTaskPriority(bool IsDeleted)
        //        {
        //            IList<TaskPriority> listPriority = null;
        //            listPriority = GetTaskPrioritiesGeneric(tc => tc.isDeleted == IsDeleted).ToList();
        //            return listPriority;
        //        }

        //        public TaskPriority GetTaskPriority(String TaskPriorityName, bool IsDeleted)
        //        {
        //            TaskPriority oTP = null;
        //            try
        //            {
        //                oTP = GetTaskPrioritiesGeneric(tc => (tc.Name.Equals(TaskPriorityName)) &&
        //                                                (tc.isDeleted == IsDeleted)).First(); ;
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine("GetTaskCategory " + ex);
        //                return null;
        //            }

        //            return oTP;
        //        }

        //        public TaskPriority GetTaskPriority(int ID)
        //        {
        //            TaskPriority oTaskPriority= null;
        //            try
        //            {
        //                oTaskPriority = session.Load<TaskPriority>(ID);

        //                // NHibernateUtil.Initialize(oTask);//se da menate attivare questo

        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine("Get TaskCategory Try: " + ex);
        //            }
        //            return oTaskPriority;

        //        }


        //        private IList<TaskPriority> GetTaskPrioritiesGeneric(Expression<Func<TaskPriority, bool>> condition)
        //        {

        //            return (from TaskPriority t in session.Linq<TaskPriority>() select t).Where(condition).ToList<TaskPriority>();
        //        }


        //        #endregion



        //        #region Category

        //        public TaskCategory AddTaskCategory(String CategoryName) 
        //        {
        //            TaskCategory oTC = new TaskCategory();
        //            oTC.Name = CategoryName;
        //            oTC.isDeleted = false;
        //            SaveOrUpdateTaskCategory(oTC);
        //            return oTC;
        //        }

        //        public TaskCategory DeleteVirtualTaskCategory(TaskCategory CategoryToDelete) 
        //        {
        //            CategoryToDelete.isDeleted = true;
        //            SaveOrUpdateTaskCategory(CategoryToDelete);
        //            return CategoryToDelete;
        //        }

        //        public TaskCategory UnDeleteTaskCategory(TaskCategory TaskCategoryToUnDelete)
        //        {
        //            TaskCategoryToUnDelete.isDeleted = false;
        //            SaveOrUpdateTaskCategory(TaskCategoryToUnDelete);
        //            return TaskCategoryToUnDelete;
        //        }

        //        public TaskCategory UpdateTaskCategory(TaskCategory TaskCategoryToUpdate, String NewTaskCategoryName)
        //        {
        //            if (!(TaskCategoryToUpdate.Name.Equals(NewTaskCategoryName))) 
        //            {
        //                TaskCategoryToUpdate.Name = NewTaskCategoryName;
        //                SaveOrUpdateTaskCategory(TaskCategoryToUpdate);
        //            }
        //            return TaskCategoryToUpdate;
        //        }

        //        public IList<TaskCategory> GetAllTaskCategories(bool IsDeleted) 
        //        {
        //            IList<TaskCategory> listCategories = null;
        //            listCategories= GetTaskCategoriesGeneric(tc => tc.isDeleted==IsDeleted).ToList();
        //            return listCategories;
        //        }

        //        public TaskCategory GetTaskCategory(String TaskCategoryName, bool IsDeleted) 
        //        {
        //            TaskCategory oTC = null;
        //            try
        //            {
        //                oTC = GetTaskCategoriesGeneric(tc => (tc.Name.Equals(TaskCategoryName)) &&
        //                                                (tc.isDeleted == IsDeleted)).First(); ;
        //            }
        //            catch ( Exception ex)
        //            {
        //                Console.WriteLine("GetTaskCategory "+ex);
        //                return null;
        //            }

        //            return oTC;
        //        }

        //        public TaskCategory GetTaskCategory(int ID)
        //        {
        //            TaskCategory oTaskCategory = null;
        //            try
        //            {
        //                oTaskCategory = session.Load<TaskCategory>(ID);

        //                // NHibernateUtil.Initialize(oTask);//se da menate attivare questo

        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine("Get TaskCategory Try: " + ex);
        //            }
        //            return oTaskCategory;

        //        }

        //        private IList<TaskCategory> GetTaskCategoriesGeneric(Expression<Func<TaskCategory, bool>> condition)
        //        {

        //            return (from TaskCategory t in session.Linq<TaskCategory>() select t).Where(condition).ToList<TaskCategory>();
        //        }


        //        #endregion




        //        private IList<Task> GetTaskGeneric(Expression<Func<Task, bool>> condition, bool IsDeleted) 
        //        {

        //         return (from Task t in session.Linq<Task>() select t).Where(condition).ToList<Task>();
        //        }




        //        public IList<Task> GetProjectAllCommunities(bool IsDeleted) 
        //        {

        //            return GetTaskGeneric(tt => (tt.Level==0)&&(tt.MetaInfo.isDeleted==IsDeleted), IsDeleted);
        //        }


        //        //Restituisce i progetti con la StartDate o ==null o >=AfterDate
        //        public IList<Task> GetProjectAllCommunitiesStartDateStartAfter( bool IsDeleted, DateTime? StartAfterThisDate)
        //        {
        //           return GetTaskGeneric(tt => (tt.Level == 0) && 
        //                                (tt.MetaInfo.isDeleted == IsDeleted)&& 
        //                                ((tt.StartDate==null)||((DateTime)tt.StartDate>=StartAfterThisDate)),//StartDate o null o >=AfterDate
        //                                IsDeleted);
        //        }

        //        //Restituisce i progetti con la EndDate o ==null o <=EndBeforeThisDate
        //        public IList<Task> GetProjectAllCommunitiesDeadlineEndBefore(bool IsDeleted, DateTime? DeadLineBeforeThisDate)
        //        {
        //            return GetTaskGeneric(tt => (tt.Level == 0) &&
        //                                 (tt.MetaInfo.isDeleted == IsDeleted) &&
        //                                 ((tt.EndDate == null) || ((DateTime)tt.EndDate <= DeadLineBeforeThisDate)),//StartDate o null o >=AfterDate
        //                                 IsDeleted);
        //        }


        //        //Restituisce i progetti con la EndDate o ==null o <=EndBeforeThisDate
        //        public IList<Task> GetProjectAllCommunities(bool IsDeleted,DateTime? StartAfterThisDate, DateTime? EndBeforeThisDate)
        //        {
        //            return GetTaskGeneric(tt => (tt.Level == 0) &&
        //                                 (tt.MetaInfo.isDeleted == IsDeleted) &&
        //                                 ((tt.StartDate == null) || (DateTime)tt.StartDate >= StartAfterThisDate)&&//StartDate o null o >=AfterDate                              
        //                                 ((tt.EndDate == null) || ((DateTime)tt.EndDate <= EndBeforeThisDate)),//EndDate o null o <=EndBeforeThisDate
        //                                 IsDeleted);
        //        }


        //        public IList<Task> GetProjectForAuthor(bool IsDeleted,Person AuthorOfProject, DateTime? StartAfterThisDate, DateTime? EndBeforeThisDate)
        //        {
        //            return GetTaskGeneric(tt => (tt.Level == 0) &&
        //                                 (tt.MetaInfo.isDeleted == IsDeleted) &&
        //                                 (tt.MetaInfo.CreatedBy==AuthorOfProject)&&
        //                                 ((tt.StartDate == null) || (DateTime)tt.StartDate >= StartAfterThisDate) &&//StartDate o null o >=AfterDate                              
        //                                 ((tt.EndDate == null) || ((DateTime)tt.EndDate <= EndBeforeThisDate)),//EndDate o null o <=EndBeforeThisDate
        //                                 IsDeleted);
        //        }

        //        public IList<Task> GetProjectSpecificCommunity(Community InterestedCommunity,Person InterestedPerson, bool IsDeleted, DateTime AfterDate)
        //        {
        //            return GetTaskGeneric(tt => ((tt.Community.Id==InterestedCommunity.Id)
        //                                        &&(tt.Level == 0) && (tt.MetaInfo.isDeleted == IsDeleted) 
        //                                        && ((tt.StartDate == null) || tt.StartDate>=AfterDate)), IsDeleted);
        //        }









        //        //OKOKOKOKOKOKOKOK
        //        public Community GetCommunityByID(int CommunityID) {
        //            Community oCommunity = null;
        //            try
        //            {
        //                oCommunity = (from Community c in session.Linq<Community>() where (c.Id == CommunityID) select c).First<Community>();
        //            }
        //            catch (Exception ex){
        //                Console.WriteLine(ex);
        //                return null;
        //            }
        //            return oCommunity;
        //        }

        //        public IList<Community> GetCommunities()
        //        {
        //            IList<Community> listCommunities = null;
        //            try
        //            {
        //                listCommunities = (from Community c in session.Linq<Community>()  select c).ToList<Community>();
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine(ex);
        //                return null;
        //            }
        //            return listCommunities;
        //        }


        //        //OKOKOKOKOKOKOKOK
        //        public Person GetPerson(int PersonID)
        //        {
        //            Person oPerson = null;
        //            try
        //            {
        //                oPerson = session.Load<Person>(PersonID);
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine(ex);
        //            }
        //            return oPerson;
        //        }


        //        public IList<Person> GetPersons()
        //        {
        //           IList< Person> listPerson = null;
        //            try
        //            {
        //                listPerson = (from Person p in session.Linq<Person>()   select p).ToList<Person>();
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine(ex);
        //                return null;
        //            }
        //            return listPerson;
        //        }


        //        public IList<Role> GetCommunityRole()
        //        {
        //            IList<Role> listRole = null;
        //            try
        //            {
        //                listRole = (from Role p in session.Linq<Role>() select p).ToList<Role>();
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine(ex);
        //                return null;
        //            }
        //            return listRole;
        //        }

        //        //OKOKOKOKOKOKOKOK
        //        //Ritorna la lista dei Task/Progetti presenti in una comunità relativi ad uno user
        //        //public List<Task> GetCommunityTaskForUser(int CommunityID, int UserID, bool isDeleted)
        //        //{
        //        //    List<Task> oTaskList = null;
        //        //    oTaskList = (from TaskAssignment t in session.Linq<TaskAssignment>() where ((t.MetaInfo.isDeleted != isDeleted)&&(t.Task.Community.Id == CommunityID) && (t.AssignedUser.Id == UserID)) select t.Task).ToList<Task>();
        //        //    return oTaskList;
        //        //}



        //        //OKOKOKOKOKOKOKOK
        //        //ritorna lista dei progetti in una comunità
        //        //public List<Task> GetCommunityProjects(int CommunityID, bool isDeleted)
        //        //{            
        //        //    List<Task> listProject= null;
        //        //  ////  oProjectList
        //        //  //  oProjectList=(from Task t in session.Linq<Task>() where ((t.MetaInfo.isDeleted!=isDeleted)&&(t.Level==0)&&(t.Community.Id==CommunityID)) select t).ToList<Task>();
        //        //    return listProject;
        //        //}




        //        #region MetaData
        //        private MetaData SetMetaDataCreate(Person Creator) {
        //            MetaData oMetaData = new MetaData();
        //           oMetaData.CreatedBy = Creator;
        //           oMetaData.CreatedOn = DateTime.Now;
        //            oMetaData.canDelete = true;
        //            oMetaData.canModify = true;
        //            oMetaData.isDeleted=false;

        //            return oMetaData;
        //        }

        //        private MetaData SetMetaDataDelete(Person AuthorOfDelete, MetaData MetaInfo) 
        //        {
        //            MetaInfo.DeletedBy = AuthorOfDelete;
        //            MetaInfo.DeletedOn = DateTime.Now;
        //            MetaInfo.isDeleted=true;

        //            return MetaInfo;
        //        }

        //        private MetaData SetMetaDataUpdate(Person AuthorOfUpdate, MetaData MetaInfo) 
        //        {
        //            MetaInfo.ModifiedBy = AuthorOfUpdate;
        //            MetaInfo.ModifiedOn = DateTime.Now;

        //            return MetaInfo;
        //        }

        //        private MetaData SetMetaDataUndelete(MetaData MetaInfo) 
        //        {
        //            MetaInfo.DeletedBy = null;
        //            MetaInfo.DeletedOn = null;
        //            MetaInfo.isDeleted = false;
        //            return MetaInfo;
        //        }


        //        #endregion

        //        ///Fare select specific task che carica figli successor e menate varie!!!!!!!! e TUTTO SUI FILE
        //        #region Operazioni di Assegnamento dei Task

        //        private IList<TaskAssignmentByPerson> GetTaskAssignmentByPersonGeneric(Expression<Func<TaskAssignmentByPerson, bool>> condition) 
        //        {
        //            IList<TaskAssignmentByPerson> list;
        //            list = (from TaskAssignmentByPerson t in session.Linq<TaskAssignmentByPerson>() select t).Where(condition).ToList<TaskAssignmentByPerson>();
        //            return list;
        //        }

        //        private IList<TaskAssignmentByCommunityRole> GetTaskAssignmentByCommunityRoleGeneric(Expression<Func<TaskAssignmentByCommunityRole, bool>> condition)
        //        {
        //            IList<TaskAssignmentByCommunityRole> list;
        //            list = (from TaskAssignmentByCommunityRole t in session.Linq<TaskAssignmentByCommunityRole>() select t).Where(condition).ToList<TaskAssignmentByCommunityRole>();
        //            return list;
        //        }

        //        public IList<TaskAssignmentByCommunityRole> GetTaskAssignmentByCommunityRole(int TaskID, bool IsDeleted, Role CommunityRole)
        //        {
        //            IList<TaskAssignmentByCommunityRole> listTABC;
        //            listTABC = GetTaskAssignmentByCommunityRoleGeneric(ta => (ta.Task.ID == TaskID) &&
        //                                                                (ta.MetaInfo.isDeleted == IsDeleted)&&
        //                                                                (ta.CommunityRole.Id==CommunityRole.Id));
        //            return listTABC;
        //        }


        //        public IList<TaskAssignmentByCommunityRole> GetTaskAssignmentByCommunityRole(int TaskID, bool IsDeleted, TaskRole TaskRole)
        //        {
        //            IList<TaskAssignmentByCommunityRole> listTABC;
        //            listTABC = GetTaskAssignmentByCommunityRoleGeneric(ta => (ta.Task.ID == TaskID) &&
        //                                                                (ta.MetaInfo.isDeleted == IsDeleted) &&
        //                                                                (ta.TaskRole == TaskRole));
        //            return listTABC;
        //        }

        //        public IList<TaskAssignmentByCommunityRole> GetTaskAssignmentByCommunityRole(int TaskID, bool IsDeleted) 
        //        {
        //            IList<TaskAssignmentByCommunityRole> listTABC;
        //            listTABC=GetTaskAssignmentByCommunityRoleGeneric(ta=>(ta.Task.ID==TaskID) &&
        //                                                                (ta.MetaInfo.isDeleted==IsDeleted));
        //            return listTABC;
        //        }

        //        public TaskAssignmentByCommunityRole GetTaskAssignmentByCommunityRole(int InterestedTaskAssignmentByCommunityRoleID)
        //        {
        //            TaskAssignmentByCommunityRole oTaskAssignmentByCommunityRole = null;
        //            try
        //            {


        //                oTaskAssignmentByCommunityRole = session.Load<TaskAssignmentByCommunityRole>(InterestedTaskAssignmentByCommunityRoleID);

        //                // NHibernateUtil.Initialize(oTask);//se da menate attivare questo

        //            }
        //            catch (Exception ex)
        //            {

        //                Console.WriteLine("Get Task Try: " + ex);
        //                return null;
        //            }
        //            return oTaskAssignmentByCommunityRole;
        //        }

        //        public IList<TaskAssignmentByPerson> AddAssignmentTaskByPerson(List<Person> AssignedPerson,  TaskRole oTaskRole, Task InterestedTask, Person AuthorOfAssignment)
        //        {
        //            IList<TaskAssignmentByPerson> listOfTaskAssigment = new List<TaskAssignmentByPerson>();
        //            TaskAssignmentByPerson oTaskAssigmentByPerson = null;

        //            foreach (Person oPerson in AssignedPerson) 
        //            {

        //                oTaskAssigmentByPerson=new TaskAssignmentByPerson();
        //               //oTaskAssigmentByPerson.Discriminator = "Person"; 

        //            oTaskAssigmentByPerson.AssignedUser = oPerson;
        //            oTaskAssigmentByPerson.TaskRole = oTaskRole;
        //            oTaskAssigmentByPerson.TaskPermissions = -1;
        //            oTaskAssigmentByPerson.Task = InterestedTask;
        //            oTaskAssigmentByPerson.MetaInfo = SetMetaDataCreate(AuthorOfAssignment);
        //            oTaskAssigmentByPerson.TreeVisibility = TreeVisibility.Total;
        //            listOfTaskAssigment.Add(oTaskAssigmentByPerson);
        //            SaveOrUpdateTaskAssignment(oTaskAssigmentByPerson);

        //            }
        //            return listOfTaskAssigment;
        //        }

        //        public TaskAssignmentByPerson GetTaskAssignmentByPerson(int InterestedTaskAssignmentByPersonID)
        //        {
        //            TaskAssignmentByPerson oTaskAssignmentByPerson = null;
        //            try
        //            {
        //                oTaskAssignmentByPerson = new TaskAssignmentByPerson();
        //                //oTask = (from Task t in session.Linq<Task>() where t.ID == InterestedTaskID select t).First<Task>();

        //                oTaskAssignmentByPerson = session.Load<TaskAssignmentByPerson>(InterestedTaskAssignmentByPersonID);

        //                // NHibernateUtil.Initialize(oTask);//se da menate attivare questo

        //            }
        //            catch (Exception ex)
        //            {

        //                Console.WriteLine("Get Task Try: " + ex);
        //                return null;
        //            }
        //            return oTaskAssignmentByPerson;
        //        }


        //        public IList<TaskAssignmentByPerson> DeleteVirtualAssignmentsTaskByPerson(IList<TaskAssignmentByPerson> TaskAssigmentByPersonToDelete, Person AutorOfDelete)
        //        {
        //           // MetaData oMetaInfoTemp = null;
        //            IList<TaskAssignmentByPerson> listTaskAssigmentByPersonToDelete = new List<TaskAssignmentByPerson>();
        //            foreach (TaskAssignmentByPerson t in TaskAssigmentByPersonToDelete)
        //            {
        //                SetMetaDataDelete(AutorOfDelete, t.MetaInfo);       
        //                SaveOrUpdateTaskAssignment(t);
        //                listTaskAssigmentByPersonToDelete.Add(t);
        //            }
        //            return listTaskAssigmentByPersonToDelete;
        //        }

        //        public TaskAssignmentByPerson DeleteVirtualTaskAssignmentByPerson(TaskAssignmentByPerson TaskAssigmentByPersonToDelete, Person AutorOfDelete)
        //        {
        //            MetaData oMetaInfoTemp = null;
        //            oMetaInfoTemp= SetMetaDataDelete(AutorOfDelete, TaskAssigmentByPersonToDelete.MetaInfo);
        //            TaskAssigmentByPersonToDelete.MetaInfo = oMetaInfoTemp;
        //            SaveOrUpdateTaskAssignment(TaskAssigmentByPersonToDelete);
        //            return TaskAssigmentByPersonToDelete;
        //        }


        //        public TaskAssignmentByPerson UnDeleteTaskAssignmentByPerson(TaskAssignmentByPerson TaskAssignment) 
        //        {           
        //            SetMetaDataUndelete(TaskAssignment.MetaInfo);
        //            SaveOrUpdateTaskAssignment(TaskAssignment);
        //            return TaskAssignment;       
        //        }

        //        public IList<TaskAssignmentByPerson> UnDeleteTaskAssignmentByPersons(IList<TaskAssignmentByPerson> ListOfTaskAssignment)
        //        {
        //            foreach (TaskAssignmentByPerson ta in ListOfTaskAssignment)
        //            {
        //                UnDeleteTaskAssignmentByPerson(ta);  
        //            }
        //            return ListOfTaskAssignment;
        //        }

        //        public TaskAssignmentByCommunityRole UnDeleteTaskAssignmentByCommunityRole(TaskAssignmentByCommunityRole TaskAssignment)
        //        {
        //            SetMetaDataUndelete(TaskAssignment.MetaInfo);
        //            SaveOrUpdateTaskAssignment(TaskAssignment);
        //            return TaskAssignment;
        //        }

        //        public TaskAssignmentByPerson UpdateTaskAssignmentByPerson(TaskAssignmentByPerson TaskAssignmentByPersonToUpdate, Person AuthorOfUpdate, Task Task, TreeVisibility TreeVisibility, TaskRole TaskListRole)
        //        {
        //            bool isUpdated=false;
        //            if(TaskAssignmentByPersonToUpdate.Task.ID!=Task.ID)
        //            {
        //            isUpdated=true;
        //            TaskAssignmentByPersonToUpdate.Task=Task;
        //            }
        //            if(!((TaskAssignmentByPersonToUpdate.TreeVisibility).Equals(TreeVisibility)))
        //            {
        //            isUpdated=true;
        //                TaskAssignmentByPersonToUpdate.TreeVisibility=TreeVisibility;
        //            }
        //            if (!((TaskAssignmentByPersonToUpdate.TaskRole).Equals(TaskListRole)))
        //            {
        //                isUpdated = true;
        //                TaskAssignmentByPersonToUpdate.TaskRole = TaskListRole;
        //            }
        //            if(isUpdated)
        //            {
        //                MetaData oMetaInfoTemp=null;
        //                oMetaInfoTemp= SetMetaDataUpdate(AuthorOfUpdate, TaskAssignmentByPersonToUpdate.MetaInfo);
        //                TaskAssignmentByPersonToUpdate.MetaInfo=oMetaInfoTemp;
        //                SaveOrUpdateTaskAssignment(TaskAssignmentByPersonToUpdate);
        //            }

        //            return TaskAssignmentByPersonToUpdate;
        //        }

        //        public IList<TaskAssignmentByPerson> GetTaskAssignmentByPerson(Task Task, bool isDeleted) 
        //        {
        //            IList<TaskAssignmentByPerson> listTaskAssignmentByPerson = null;

        //            listTaskAssignmentByPerson = GetTaskAssignmentByPersonGeneric(tap => (tap.Task.ID == Task.ID) && (tap.MetaInfo.isDeleted == isDeleted));
        //            return listTaskAssignmentByPerson;
        //        }


        //        public IList<TaskAssignmentByPerson> GetTaskAssignmentByPerson(Task Task, Person InterestedPerson, bool isDeleted)
        //        {
        //            IList<TaskAssignmentByPerson> listTaskAssignmentByPerson = null;

        //            //listTaskAssignmentByPerson = GetTaskAssignmentByPersonGeneric(tap => (tap.AssignedUser.Id==tap.AssignedUser.Id)&&
        //            //                                                            (tap.Task.ID == Task.ID) &&
        //            //                                                            (tap.MetaInfo.isDeleted == isDeleted));

        //            Expression<Func<TaskAssignmentByPerson, bool>> condition = tap => (tap.Task.ID == Task.ID) && (tap.MetaInfo.isDeleted == isDeleted);
        //            listTaskAssignmentByPerson = (from TaskAssignmentByPerson t 
        //                                              in session.Linq<TaskAssignmentByPerson>()
        //                                          where t.AssignedUser.Id==InterestedPerson.Id
        //                                          select t).Where(condition).ToList<TaskAssignmentByPerson>();

        //            return listTaskAssignmentByPerson;
        //        }

        //        public IList<TaskAssignmentByPerson> GetTaskAssignmentByPerson( Person InterestedPerson, bool isDeleted)
        //        {
        //            IList<TaskAssignmentByPerson> listTaskAssignmentByPerson = null;

        //            Expression<Func<TaskAssignmentByPerson, bool>> condition = tap => (tap.MetaInfo.isDeleted == isDeleted);
        //            listTaskAssignmentByPerson = (from TaskAssignmentByPerson t
        //                                              in session.Linq<TaskAssignmentByPerson>()
        //                                          where t.AssignedUser.Id == InterestedPerson.Id
        //                                          select t).Where(condition).ToList<TaskAssignmentByPerson>(); 
        //            return listTaskAssignmentByPerson;
        //        }



        //        public TaskAssignmentByCommunityRole AddAssignmentTaskByCommunityRole(Role CommunityRole, TaskRole TaskListRole, Task InterestedTask, Person AutorOfAssignment, TreeVisibility TreeVisibility)
        //        {
        //            TaskAssignmentByCommunityRole oTaskAssigmentByCommunityRole= null;

        //                oTaskAssigmentByCommunityRole = new TaskAssignmentByCommunityRole();
        //                oTaskAssigmentByCommunityRole.CommunityRole=CommunityRole;
        //                oTaskAssigmentByCommunityRole.TaskRole = TaskListRole;
        //                oTaskAssigmentByCommunityRole.Task = InterestedTask;
        //                oTaskAssigmentByCommunityRole.MetaInfo = SetMetaDataCreate(AutorOfAssignment);
        //                oTaskAssigmentByCommunityRole.TreeVisibility = TreeVisibility;
        //                oTaskAssigmentByCommunityRole.TaskPermissions = -1;
        //                SaveOrUpdateTaskAssignment(oTaskAssigmentByCommunityRole);
        //            return oTaskAssigmentByCommunityRole;
        //        }


        //        public void PrintTaskAssignmentByCommunityRole(TaskAssignmentByCommunityRole TaskAssignmentByCommunityRole)
        //        {
        //            Console.WriteLine(TaskAssignmentByCommunityRole.ID + " | " + TaskAssignmentByCommunityRole.Task.Name
        //                +"| "+TaskAssignmentByCommunityRole.CommunityRole.Description);
        //        }

        //        public TaskAssignmentByCommunityRole DeleteVirtualAssignmentTaskByCommunityRole(TaskAssignmentByCommunityRole TaskAssigmentByCommunityRoleToDelete, Person AutorOfDelete)
        //        {
        //            MetaData oMetaInfoTemp = null;
        //            oMetaInfoTemp = SetMetaDataDelete(AutorOfDelete, TaskAssigmentByCommunityRoleToDelete.MetaInfo);
        //            TaskAssigmentByCommunityRoleToDelete.MetaInfo = oMetaInfoTemp;
        //            SaveOrUpdateTaskAssignment(TaskAssigmentByCommunityRoleToDelete);
        //            return TaskAssigmentByCommunityRoleToDelete;
        //        }

        //        public TaskAssignmentByCommunityRole UpdateTaskAssignmentByPerson(TaskAssignmentByCommunityRole TaskAssignmentByCommunityRoleToUpdate, Person AutorOfUpdate, Task Task, TreeVisibility TreeVisibility, TaskRole TaskListRole)
        //        {
        //            bool isUpdated = false;
        //            if (TaskAssignmentByCommunityRoleToUpdate.Task.ID != Task.ID)
        //            {
        //                isUpdated = true;
        //                TaskAssignmentByCommunityRoleToUpdate.Task = Task;
        //            }
        //            if (!((TaskAssignmentByCommunityRoleToUpdate.TreeVisibility).Equals(TreeVisibility)))
        //            {
        //                isUpdated = true;
        //                TaskAssignmentByCommunityRoleToUpdate.TreeVisibility = TreeVisibility;
        //            }
        //            if (!((TaskAssignmentByCommunityRoleToUpdate.TaskRole).Equals(TaskListRole)))
        //            {
        //                isUpdated = true;
        //                TaskAssignmentByCommunityRoleToUpdate.TaskRole = TaskListRole;
        //            }



        //            if (isUpdated)
        //            {

        //                SetMetaDataUpdate(AutorOfUpdate, TaskAssignmentByCommunityRoleToUpdate.MetaInfo);                
        //                SaveOrUpdateTaskAssignment(TaskAssignmentByCommunityRoleToUpdate);
        //            }

        //            return TaskAssignmentByCommunityRoleToUpdate;
        //        }



        //        public IList<TaskAssignmentByCommunityRole> GetTaskAssignmentByCommunityRole(Task Task)
        //        {
        //            IList<TaskAssignmentByCommunityRole> listTaskAssignmentByCommunityRole = null;
        //            listTaskAssignmentByCommunityRole = (from TaskAssignmentByCommunityRole t in session.Linq<TaskAssignmentByCommunityRole>() where (t.Task.ID == Task.ID)select t).ToList<TaskAssignmentByCommunityRole>();
        //            return listTaskAssignmentByCommunityRole;
        //        }

        //        #endregion

        //        #region Operazioni su Task

        //        public void PrintTaskToConsole(Task InterestedTask) 
        //        {
        //            Console.WriteLine("\n ID: "+InterestedTask.ID);
        //            Console.WriteLine("Nome: " + InterestedTask.Name);
        //            Console.WriteLine("Descrizione: " + InterestedTask.Description);
        //            Console.WriteLine("start date: " + InterestedTask.StartDate);
        //            Console.WriteLine("End Date: " + InterestedTask.EndDate);
        //            Console.WriteLine("Deadline: " + InterestedTask.Deadline);
        //            Console.WriteLine("Completeness: " + InterestedTask.Completeness);
        //            Console.WriteLine("Category: " + InterestedTask.Category);
        //            Console.WriteLine("Level: " + InterestedTask.Level);
        //            Console.WriteLine("Priority: " + InterestedTask.Priority);
        //        }

        //        private int SetLevel(Task ParentTask)
        //        {
        //            if (ParentTask != null)
        //            {
        //                return ParentTask.Level + 1;
        //            }
        //            return 0;
        //        }

        //        private String SetTaskName(String Name, Task ParentTask) 
        //        {
        //            String definitiveName = "";
        //            String[] temp;
        //            int nParentChild;


        //            nParentChild = GetNumberOfChildren(ParentTask.ID, false);
        //            nParentChild++;

        //            if (ParentTask.Level == 0)
        //            {
        //                definitiveName = nParentChild + " " + Name;
        //            }
        //            else
        //            {
        //                temp = ParentTask.Name.Split(' ');
        //                definitiveName=temp.First()+"."+nParentChild+" "+Name;
        //            }
        //            return definitiveName;
        //        }


        //        public Task AddTask(String Name, String Description, Community Community, Person Creator, DateTime? StartDate, DateTime? Deadline, DateTime? EndDate, bool isArchived,  TaskPriority Priority, Status Status, int Completeness, TaskCategory Category, Task TaskParent)
        //        {
        //           Task oTask =new Task();
        //           oTask.Description = Description;

        //           oTask.Community = Community;
        //           oTask.MetaInfo = SetMetaDataCreate(Creator);      
        //           oTask.StartDate = StartDate;
        //           oTask.Deadline = Deadline;
        //           oTask.EndDate = EndDate;
        //           oTask.isArchived = isArchived;
        //           oTask.Level = SetLevel(TaskParent);
        //           oTask.Priority = Priority;
        //           oTask.Status = Status;
        //           oTask.Completeness = Completeness;
        //           oTask.Category = Category;
        //           oTask.TaskParent = TaskParent;

        //           if (oTask.Level > 0)
        //           {
        //               oTask.isPersonal = TaskParent.isPersonal;
        //               if (!ControlTaskDateConsistency(oTask))
        //               {

        //                   Console.WriteLine("ERRORE: Impossibile inserire il Task verifca le date inserite");
        //                   return null;
        //               }
        //               oTask.Name = SetTaskName(Name, oTask.TaskParent);
        //           }
        //           else 
        //           {
        //               oTask.isPersonal = false;
        //               oTask.Name = Name;
        //           }


        //          SaveOrUpdateTask(oTask);
        //          if (oTask.Level != 0)
        //          {
        //              UpdateDateAndCompletenessForParent(oTask, Creator);
        //          }
        //            return oTask;
        //        }



        //        public Task AddProject(String Name, String Description, Community Community, Person Creator, DateTime? StartDate, DateTime? Deadline, DateTime? EndDate, bool isArchived, TaskPriority Priority, Status Status, int Completeness, TaskCategory Category, bool isPersonal)
        //        {
        //            Task oTask = new Task();
        //            oTask.Name = Name;
        //            oTask.Description = Description;
        //            oTask.Community = Community;
        //            oTask.MetaInfo = SetMetaDataCreate(Creator);
        //            oTask.StartDate = StartDate;
        //            oTask.Deadline = Deadline;
        //            oTask.EndDate = EndDate;
        //            oTask.isArchived = isArchived;
        //            oTask.Level = 0;
        //            oTask.Priority = Priority;
        //            oTask.Status = Status;
        //            oTask.Completeness = Completeness;
        //            oTask.Category = Category;
        //            oTask.TaskParent = null;
        //            oTask.isPersonal = isPersonal;
        //            return oTask;
        //        }




        //        //vedere se invece della serie d if si può sostituire con session.merge
        //        //Non fa l'update delle liste presenti... c sono i rispettivi metodi
        //        public Task UpdateTask(Task TaskForUpdate, String Name, String Description, Community Community, Person AutorOfUpdate, DateTime? StartDate, DateTime? Deadline, DateTime? EndDate, bool isArchived, TaskPriority Priority, Status Status, int Completeness, TaskCategory Category, Task TaskParent)
        //        {
        //            Task oTask = TaskForUpdate;
        //            bool isUpdated = false;
        //            bool isNecessaryUpdateTaskParent = false;

        //            if (!(oTask.Name.Equals(SetTaskName(Name, TaskParent))))
        //            {
        //                oTask.Name = SetTaskName(Name,TaskParent);
        //                isUpdated = true;
        //            }

        //            if (!(oTask.Description.Equals(Description)))
        //            {
        //                //Console.WriteLine("AGGIORNO DESCRIPTION: " + Description); 
        //                oTask.Description = Description;
        //                isUpdated = true;
        //            }
        //            if (oTask.Community.Id != Community.Id)
        //            {
        //                oTask.Community = Community;
        //                isUpdated = true;
        //            }
        //            if (!(oTask.StartDate.Equals(StartDate)))
        //            {
        //                oTask.StartDate = StartDate;
        //                isUpdated = true;
        //                isNecessaryUpdateTaskParent = true;
        //            }
        //            if (!(oTask.Deadline.Equals(Deadline)))
        //            {
        //            oTask.Deadline = Deadline;
        //                  isUpdated = true;
        //                  isNecessaryUpdateTaskParent = true;
        //            }
        //            if (!(oTask.EndDate.Equals(EndDate)))
        //            {
        //            oTask.EndDate = EndDate;
        //              isUpdated = true;
        //              isNecessaryUpdateTaskParent = true;
        //            }
        //            if (oTask.isArchived != isArchived)
        //            oTask.isArchived = isArchived;


        //            if (!(oTask.Priority.Equals(Priority)))
        //            {
        //            oTask.Priority = Priority;
        //                  isUpdated = true;
        //            }
        //            if (!(oTask.Status.Equals(Status)))
        //            {
        //            oTask.Status = Status;
        //            isUpdated = true;
        //            }
        //            if (oTask.Completeness != Completeness)
        //            {
        //            oTask.Completeness = Completeness;
        //                      isUpdated = true;
        //                      isNecessaryUpdateTaskParent = true;
        //            }
        //            if (!(oTask.Category.Equals(Category)))
        //            {
        //            oTask.Category = Category;
        //                  isUpdated = true;
        //            }

        //            int tempLevel = SetLevel(TaskParent);
        //            if (oTask.Level != tempLevel)
        //            {
        //                oTask.Level = tempLevel;
        //                isUpdated = true;
        //            }
        //            if (oTask.TaskParent != null && TaskParent!=null)
        //            {
        //                if (oTask.TaskParent.ID != TaskParent.ID)
        //                {
        //                    oTask.TaskParent = TaskParent;
        //                    isUpdated = true;
        //                    isNecessaryUpdateTaskParent = true;
        //                }
        //            }
        //            else if (oTask.TaskParent == null || TaskParent == null) 
        //            {
        //                oTask.TaskParent = TaskParent;
        //                isUpdated = true;
        //                isNecessaryUpdateTaskParent = true;
        //            }


        //            if (!ControlTaskDateConsistency(oTask))
        //            {
        //                Console.WriteLine("ERRORE: Impossibile aggiornare il Task verifca consistenza date fallita");
        //                return null;
        //            }

        //            if (isUpdated)
        //            {
        //                Console.WriteLine("Riilevate modifiche, EFFETTUO UPDATE");
        //                SetMetaDataUpdate(AutorOfUpdate, oTask.MetaInfo);

        //                SaveOrUpdateTask(oTask);
        //                if (isNecessaryUpdateTaskParent && oTask.Level != 0)
        //                {
        //                    UpdateDateAndCompletenessForParent(oTask, AutorOfUpdate);
        //                }
        //            }
        //            return oTask;
        //        }



        //        public Task UpdateProject(Task TaskForUpdate, String Name, String Description, Community Community, Person AutorOfUpdate, bool isArchived, TaskPriority Priority, Status Status, TaskCategory Category, bool isPersonal)
        //        {
        //            Task oTask = TaskForUpdate;
        //            bool isUpdated = false;
        //            bool isNecessaryUpdateTaskParent = false;

        //            if (!(oTask.isPersonal == isPersonal)) 
        //            {
        //                oTask.isPersonal = isPersonal;
        //                isUpdated = true;
        //            }

        //            if (!(oTask.Description.Equals(Description)))
        //            {
        //                //Console.WriteLine("AGGIORNO DESCRIPTION: " + Description); 
        //                oTask.Description = Description;
        //                isUpdated = true;
        //            }
        //            if (oTask.Community.Id != Community.Id)
        //            {
        //                oTask.Community = Community;
        //                isUpdated = true;
        //            }

        //            if (oTask.isArchived != isArchived)
        //                oTask.isArchived = isArchived;


        //            if (!(oTask.Priority.Equals(Priority)))
        //            {
        //                oTask.Priority = Priority;
        //                isUpdated = true;
        //            }
        //            if (!(oTask.Status.Equals(Status)))
        //            {
        //                oTask.Status = Status;
        //                isUpdated = true;
        //            }

        //            if (!(oTask.Category.Equals(Category)))
        //            {
        //                oTask.Category = Category;
        //                isUpdated = true;
        //            }          
        //            if (isUpdated)
        //            {
        //                Console.WriteLine("Riilevate modifiche, EFFETTUO UPDATE");
        //                SetMetaDataUpdate(AutorOfUpdate, oTask.MetaInfo);

        //                SaveOrUpdateTask(oTask);
        //                if (isNecessaryUpdateTaskParent && oTask.Level != 0)
        //                {
        //                    UpdateDateAndCompletenessForParent(oTask, AutorOfUpdate);
        //                }
        //            }
        //            return oTask;
        //        }





        //        public Task UnDeleteTask(Task Task, Person AuthorOfUnDelete)
        //        {
        //           SetMetaDataUndelete(Task.MetaInfo);

        //            if (!ControlTaskDateConsistency(Task))
        //            {
        //                Console.WriteLine("ERRORE: Impossibile l'operazione di UnDelete, consistenza delle date fallita");
        //                return null;
        //            }
        //            if (Task.Level != 0)
        //            {
        //                UpdateDateAndCompletenessForParent(Task, AuthorOfUnDelete);
        //            }
        //            SaveOrUpdateTask(Task);
        //            return Task;
        //        }



        //        //setta icampi di cancellazione in metadata del task e dei suoi figli 
        //        //nn trattati i controlli sulle date relativi ai successor
        //        public Task DeleteVirtualTask(Task InterestedTask, Person AuthorOfDelete) 
        //        {
        //            Task oTask = InterestedTask;
        //             SetMetaDataDelete(AuthorOfDelete, oTask.MetaInfo);
        //            //Console.WriteLine("DeleteVirtualTask " + oTask.Name + " ---level=" + oTask.Level);

        //            SaveOrUpdateTask(oTask);
        //            if (oTask.Level != 0)
        //            {
        //                UpdateDateAndCompletenessForParent(oTask, AuthorOfDelete);
        //            }
        //            DeleteVirtualAllTaskPredecessorLink(oTask.ID, AuthorOfDelete);

        //            oTask.TaskChildren = GetChildListToTask(oTask.ID, false);

        //            //richiama la stessa funzione x i figli
        //            foreach (Task oTaskChild in oTask.TaskChildren) 
        //            {
        //                DeleteVirtualTask(oTaskChild, AuthorOfDelete);
        //            }
        //            return oTask;
        //        }


        //        //seleziona da db un task... nn c sono caricati i figli e i succ e pred
        //        public Task GetTask(int InterestedTaskID)
        //        {
        //            Task oTask = null;
        //            try
        //            {

        //                //oTask = (from Task t in session.Linq<Task>() where t.ID == InterestedTaskID select t).First<Task>();

        //                oTask = session.Load<Task>(InterestedTaskID);

        //               // NHibernateUtil.Initialize(oTask);//se da menate attivare questo

        //            }
        //            catch (Exception ex)
        //            {

        //                Console.WriteLine("Get Task Try: "+ex);

        //            }
        //            return oTask;
        //        }




        //        //carica un task con figli succ e pred
        //        public Task ViewTask(int InterestedTaskID, bool IsDeleted)
        //        {
        //            Task oTask = GetTask(InterestedTaskID);
        //            return ViewTask(oTask, IsDeleted);
        //        }



        //        //carica un task con figli succ e pred
        //        public Task ViewTask(Task Task, bool IsDeleted)
        //        {
        //            Task oTask = Task;
        //            oTask.Predecessors = GetTaskPredecessor(oTask, IsDeleted);
        //            oTask.Successors = GetTaskSuccessor(oTask, IsDeleted);
        //            oTask.TaskChildren = GetChildListToTask(oTask.ID, IsDeleted);
        //            return oTask;
        //        }


        //        public PredecessionLink DeleteVirtualPredecessorLink(PredecessionLink InterestedPredecessorLink, Person AuthorOfDelete)
        //        {
        //            PredecessionLink oPredecessorLink = InterestedPredecessorLink;          
        //            SetMetaDataDelete(AuthorOfDelete, oPredecessorLink.MetaInfo);
        //            SaveOrUpdatePredecessionLink(oPredecessorLink);
        //            return oPredecessorLink;
        //        }

        //        //cancella virtualmente la lista di tutti i PredecessorLink dove il Task è coinvolto (sia come Predecessor che come Successor)
        //        //nn efffettua la selezione x i PredecessorLink dei figli
        //        public void DeleteVirtualAllTaskPredecessorLink(int TaskID, Person AuthorOfDelete)
        //        { 
        //            foreach(PredecessionLink pl in GetAllPredecessorLink(TaskID))
        //            {
        //                if (pl.MetaInfo.isDeleted == false)
        //                {
        //                    DeleteVirtualPredecessorLink(pl, AuthorOfDelete);
        //                }
        //            }       
        //        }


        //        //restituisce lista dei figli di un task
        //        public IList<Task> GetChildListToTask(int InteresedTaskID, bool isDeleted)
        //        {
        //            return GetTaskGeneric(tt => (tt.TaskParent.ID == InteresedTaskID) && (tt.MetaInfo.isDeleted == isDeleted), isDeleted);
        //        }


        //        private int GetNumberOfChildren(int InteresedTaskID, bool IsDeleted)
        //        {
        //            Expression<Func<Task, bool>> condition = tt => (tt.ID == InteresedTaskID)&&
        //                                                            (tt.MetaInfo.isDeleted==IsDeleted); 
        //            return (from Task t in session.Linq<Task>() select t).Where(condition).Count();
        //        }


        //        private IList<PredecessionLink> GetPredecessionLinkGeneric(Expression<Func<PredecessionLink, bool>> condition)
        //        {
        //            return (from PredecessionLink pl in session.Linq<PredecessionLink>()
        //                    select pl).Where(condition).ToList<PredecessionLink>();
        //        }





        //        //predecessionLinkCondition deve avere sempre dentro InterestedTask.ID==pl.PredecessorTask && condizioni che m interessano
        //        private IList<Task> GetSuccessorTaskGeneric(Expression<Func<PredecessionLink, bool>> predecessionLinkCondition, Expression<Func<Task, bool>> successorTaskCondition)
        //        {

        //            var temp = (from PredecessionLink pl in session.Linq<PredecessionLink>()
        //                        select pl).Where(predecessionLinkCondition );
        //            return (from PredecessionLink pl in temp select pl.SuccessorTask).Where(successorTaskCondition).ToList<Task>();
        //        }


        //        //predecessionLinkCondition deve avere sempre dentro InterestedTask.ID==pl.SuccessorTask && condizioni che m interessano
        //        private IList<Task> GetPredecessorTaskGeneric(Expression<Func<PredecessionLink, bool>> predecessionLinkCondition)
        //        {
        //            var temp = (from PredecessionLink pl in session.Linq<PredecessionLink>()
        //                        select pl).Where(predecessionLinkCondition).ToList<PredecessionLink>();

        //            return (from PredecessionLink plink in temp 
        //                    select plink.PredecessorTask).ToList<Task>();
        //        }

        //        //predecessionLinkCondition deve avere sempre dentro InterestedTask.ID==pl.SuccessorTask && condizioni che m interessano
        //        private IList<Task> GetPredecessorTaskGeneric(Expression<Func<PredecessionLink, bool>> predecessionLinkCondition, Expression<Func<Task, bool>> predecessorTaskCondition)
        //        {
        //            var temp = (from PredecessionLink pl in session.Linq<PredecessionLink>()
        //                        where pl.ID > 0
        //                        select pl).Where(predecessionLinkCondition);
        //            if (temp.Count() == 0)
        //            {
        //                Console.WriteLine("riverdere GetPredecessorTaskGeneric");
        //            }
        //            return (from PredecessionLink pl 
        //                        in temp
        //                        //in
        //                        //    ((from PredecessionLink pl in session.Linq<PredecessionLink>()
        //                        //      where pl.ID > 0
        //                        //      select pl).Where(predecessionLinkCondition))
        //                    where pl.PredecessorTask.ID > 0
        //                    select pl.PredecessorTask).Where(predecessorTaskCondition).ToList<Task>();
        //        }

        //        //predecessionLinkCondition deve avere sempre dentro InterestedTask.ID==pl.PredecessorTask && condizioni che m interessano
        //        private IList<Task> GetSuccessorTaskGeneric(Expression<Func<PredecessionLink, bool>> predecessionLinkCondition)
        //        {

        //            var temp = (from PredecessionLink pl in session.Linq<PredecessionLink>()
        //                        select pl).Where(predecessionLinkCondition).ToList<PredecessionLink>();
        //            return (from PredecessionLink pl in temp select pl.SuccessorTask).ToList<Task>();
        //        }


        //        //restituisce la lista di tutti i PredecessorLink dove il Task è coinvolto (sia come Predecessor che come Successor)
        //        //nn efffettua la selezione x i PredecessorLink dei figli 
        //        public IList<PredecessionLink> GetAllPredecessorLink(int TaskID)
        //        {
        //            return GetPredecessionLinkGeneric(pl=>(pl.PredecessorTask.ID==TaskID || pl.SuccessorTask.ID==TaskID));
        //        }





        //        //restituisce la lista di tutti i PredecessorLink dove il Task è coinvolto (sia come Predecessor che come Successor)
        //        //nn efffettua la selezione x i PredecessorLink dei figli 
        //        public IList<PredecessionLink> GetAllTaskPredecessorLink(int TaskID, bool IsDeleted)
        //        {
        //            return GetPredecessionLinkGeneric(pl => ((pl.PredecessorTask.ID == TaskID) || (pl.SuccessorTask.ID == TaskID))&&
        //                                                (pl.MetaInfo.isDeleted==IsDeleted));
        //        }


        //        //restituisce lista dei PredecessorLink con riferimento ai task che devono essere fatti PRIMA dell'Interested Task
        //        public IList<Task> GetTaskPredecessor(Task InterestedTask, bool IsDeleted)
        //        {
        //            IList<Task> listPredecessor = null;
        //          listPredecessor = GetPredecessorTaskGeneric(pl => ((pl.SuccessorTask.ID == InterestedTask.ID)&&(pl.MetaInfo.isDeleted==IsDeleted)));
        //            //listPredecessor = GetPredecessorTaskGeneric(pl => ((pl.SuccessorTask.ID == InterestedTask.ID) &&
        //            //                                                 (pl.MetaInfo.isDeleted == isDeleted)),
        //            //                                            t => (t.ID >0));

        //            if (InterestedTask.Level >= 1)
        //            {

        //                try
        //                {
        //                       listPredecessor= listPredecessor.Concat<Task>(GetTaskPredecessor(InterestedTask.TaskParent, IsDeleted)).ToList<Task>();
        //                }
        //                catch (Exception ex)
        //                {

        //                    Console.WriteLine("GetTaskPredecessor CATCH:"+ex);
        //                }
        //            }  
        //            return listPredecessor;
        //        }

        //        //restituisce lista dei PredecessorLink con riferimento ai task che devono essere fatti DOPO dell'Interested Task
        //        public IList<Task> GetTaskSuccessor(Task InterestedTask, bool isDeleted)
        //        {
        //            IList<Task> listSuccessors = null;

        //            listSuccessors = GetSuccessorTaskGeneric(pl => (pl.PredecessorTask.ID == InterestedTask.ID) &&
        //                                                             (pl.MetaInfo.isDeleted == isDeleted));
        //            if (InterestedTask.Level >= 1)
        //            {
        //                try
        //                {
        //                    listSuccessors = listSuccessors.Concat<Task>(GetTaskSuccessor(InterestedTask.TaskParent, isDeleted)).ToList<Task>();
        //                }
        //                catch (Exception ex)
        //                {
        //                    Console.WriteLine("GetTaskSuccessor CATCH:" + ex);
        //                }
        //            }
        //            return listSuccessors;

        //        }

        //        public PredecessionLink AddPredecessorLink(Person PersonCreatorLink, Task Predecessor, Task Successor ) 
        //        {
        //            if (Successor.StartDate != null && Predecessor.EndDate != null)
        //            {
        //                if (!ControlFirstDateBiggerOrEqualSecondDate((DateTime)Successor.StartDate, (DateTime)Predecessor.EndDate))
        //                {
        //                    Console.WriteLine("ERRORE: Impossibile inserire il link tra i due Task causa inconsistenza tra l'EndDate del Task predecessor e la StartDate del Task Successor");
        //                    return null;
        //                }
        //            }
        //            PredecessionLink oPredecessorLink = new PredecessionLink();
        //            oPredecessorLink.PredecessorTask=Predecessor;
        //            oPredecessorLink.SuccessorTask=Successor;
        //            oPredecessorLink.MetaInfo = SetMetaDataCreate(PersonCreatorLink);
        //            SaveOrUpdatePredecessionLink(oPredecessorLink);

        //            return oPredecessorLink;
        //        }

        //        public PredecessionLink UnDeletePredecessionLink(PredecessionLink PredeccessionLink) 
        //        {
        //            MetaData oMetaInfoTemp = null;
        //            oMetaInfoTemp = SetMetaDataUndelete(PredeccessionLink.MetaInfo);
        //            PredeccessionLink.MetaInfo = oMetaInfoTemp;
        //            SaveOrUpdatePredecessionLink(PredeccessionLink);
        //            return PredeccessionLink;
        //        }


        //#endregion


        //        #region Operazioni relative i file

        //        public IList<BaseFile> GetTaskFiles(int TaskID) 
        //        {
        //            IList<BaseFile> listTaskFiles = null;
        //            listTaskFiles=(from TaskFileAssociation tfa in session.Linq<TaskFileAssociation>() where (tfa.TaskOwner.ID==TaskID) orderby tfa.File.DisplayName select tfa.File).ToList<BaseFile>();
        //            return listTaskFiles; 
        //        }

        //        public IList<TaskFileAssociation> GetTaskFileAssociations(int TaskID)
        //        {
        //            IList<TaskFileAssociation> listTaskFiles = null;
        //            listTaskFiles = (from TaskFileAssociation tfa in session.Linq<TaskFileAssociation>() where (tfa.TaskOwner.ID == TaskID) orderby tfa.File.DisplayName select tfa).ToList<TaskFileAssociation>();
        //            return listTaskFiles;
        //        }

        //        public BaseFile GetSpecificTaskFile(Guid BaseFileID) 
        //        {
        //            BaseFile oBf = null;
        //            try
        //            {
        //                oBf = (from TaskFileAssociation tfa in session.Linq<TaskFileAssociation>() where (tfa.File.Id.Equals(BaseFileID)) orderby tfa.File.DisplayName select tfa.File).First<BaseFile>();
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine("GetSpecificTaskFile Exception: "+ex);
        //                return null;
        //            }
        //            return oBf;
        //        }

        //        public TaskFileAssociation GetSpecificTaskFileAssociation(int TaskFileAssociationID)
        //        {
        //            TaskFileAssociation oTfa = null;
        //            try
        //            {
        //                oTfa = (from TaskFileAssociation tfa in session.Linq<TaskFileAssociation>() where (tfa.ID==TaskFileAssociationID) orderby tfa.File.DisplayName select tfa).First<TaskFileAssociation>();
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine(ex);
        //                return null;
        //            }
        //            return oTfa;
        //        }

        //        public TaskFileAssociation DeleteVirtualTaskFileAssociation(TaskFileAssociation TaskFileAssociation, Person AutorOfDelete)
        //        {
        //            TaskFileAssociation oTaskFileAssociation = TaskFileAssociation;
        //            MetaData oMetaInfoTemp = null;
        //            oMetaInfoTemp= SetMetaDataDelete(AutorOfDelete, oTaskFileAssociation.MetaInfo);
        //            oTaskFileAssociation.MetaInfo = oMetaInfoTemp;
        //            SaveOrUpdateTaskFileAssociation(oTaskFileAssociation);

        //            return oTaskFileAssociation;

        //        }

        //        private TaskFileAssociation UnDeleteVirtualTaskFileAssociations(TaskFileAssociation TaskFileAssociation)
        //        {
        //            MetaData oMetaInfoTemp;
        //            oMetaInfoTemp = SetMetaDataUndelete(TaskFileAssociation.MetaInfo);
        //            TaskFileAssociation.MetaInfo = oMetaInfoTemp;
        //            SaveOrUpdateTaskFileAssociation(TaskFileAssociation);
        //            return TaskFileAssociation;

        //        }


        //        public TaskFileAssociation AddTaskFileAssociation(Person Creator, Task TaskOwner, BaseFile BaseFile)
        //        {
        //            TaskFileAssociation oTaskFileAssociation = new TaskFileAssociation();
        //            oTaskFileAssociation.File = BaseFile;
        //            oTaskFileAssociation.MetaInfo = SetMetaDataCreate(Creator);
        //            oTaskFileAssociation.TaskOwner = TaskOwner;
        //            SaveOrUpdateTaskFileAssociation(oTaskFileAssociation);
        //            return oTaskFileAssociation;
        //        }


        //        public BaseFile GetBaseFile()
        //        {
        //            BaseFile oBaseFile;
        //            oBaseFile = (from BaseFile b in session.Linq<BaseFile>() select b).First<BaseFile>();
        //            return oBaseFile;
        //        }



        //        #endregion



        //        #region Funzioni di SaveOrUpdate

        //        private void SaveOrUpdateTaskCategory(TaskCategory TaskCategory)
        //        {
        //            session.SaveOrUpdate(TaskCategory);
        //        }

        //        private void SaveOrUpdateTaskPriority(TaskPriority TaskPriority)
        //        {
        //            session.SaveOrUpdate(TaskPriority);
        //        }

        //        private void SaveOrUpdateTask(Task t)
        //        {
        //           Console.WriteLine("STO PER SALVARE IL TASK: "+t.Name+t.MetaInfo.isDeleted);
        //            session.SaveOrUpdate(t);
        //           Console.WriteLine("TASK SALVATO SU DB: "+t.Name);
        //        }

        //        private void SaveOrUpdatePredecessionLink(PredecessionLink pl)
        //        {
        //            session.SaveOrUpdate(pl);
        //        }

        //        private void SaveOrUpdateTaskFileAssociation(TaskFileAssociation TFA)
        //        {
        //            session.SaveOrUpdate(TFA);

        //        }

        //        private void SaveOrUpdateTaskAssignment(TaskAssignment TaskAssignment)
        //        {
        //            session.SaveOrUpdate(TaskAssignment);
        //        }



        //        #endregion




        //        #region controllo predecessor/successor

        //        #endregion


        //        #region utility e controlli



        //        private bool ControlTaskDateConsistency(Task InterestedTask) 
        //        {
        //            Task oTask = InterestedTask;
        //            DateTime oDateUtility;

        //            if (oTask.StartDate != null && oTask.EndDate != null)
        //            {
        //                //controllo StartDate<EndDAte
        //                if (!ControlFirstDateMinOrEqualSecondDate((DateTime)oTask.StartDate, (DateTime)oTask.EndDate))
        //                {
        //                    Console.WriteLine("ERRORE: Impossibile inserire il Task -> Start Date>End Date");
        //                    return false;
        //                }
        //                //controllo end date<=deadline
        //                if (oTask.Deadline != null)
        //                {
        //                    if (!ControlFirstDateMinOrEqualSecondDate((DateTime)oTask.EndDate, (DateTime)oTask.Deadline))
        //                    {
        //                        Console.WriteLine("ERRORE: Impossibile inserire il Task -> End Date>Deadline");
        //                        return false;
        //                    }
        //                }
        //                //recupero i predecessor e successor ed effettuo i loro controlli
        //                oTask.Predecessors = GetTaskPredecessor(oTask, false);
        //                oTask.Successors= GetTaskSuccessor(oTask, false);

        //                //oDateUtility utilizzato x i controlli sul predecessor
        //                oDateUtility = GetMaxPredecessorEndDate((List<Task>)oTask.Predecessors);
        //                if (!ControlFirstDateBiggerOrEqualSecondDate((DateTime)oTask.StartDate, oDateUtility))
        //                {
        //                    Console.WriteLine("ERRORE:  La Start Date deve iniziare dopo " + oDateUtility + " (causa Task predecessori)");
        //                    return false;
        //                }

        //                //oDateUtility utilizzato x controllo con i successor
        //                oDateUtility = GetMinSuccessorsStartDate((List<Task>)oTask.Successors);
        //                if (!ControlFirstDateBiggerOrEqualSecondDate(oDateUtility, (DateTime)oTask.EndDate))
        //                {
        //                    Console.WriteLine("ERRORE:  La End Date deve essere uguale o inferiore a " + oDateUtility + " (causa Task sucessori)");
        //                    return false;
        //                }

        //            }

        //            return true;
        //        }

        //        private bool ControlFirstDateMinOrEqualSecondDate(DateTime FirstDate, DateTime SecondDate)
        //        {

        //            int compareDate = DateTime.Compare(FirstDate, SecondDate);
        //            if (compareDate <= 0) 
        //            {
        //                return true;
        //            }
        //            return false;
        //        }

        //        private bool ControlFirstDateBiggerOrEqualSecondDate(DateTime FirstDate, DateTime SecondDate)
        //        {

        //            int compareDate = DateTime.Compare(FirstDate, SecondDate);
        //            if (compareDate >= 0)
        //            {
        //                return true;
        //            }
        //            return false;
        //        }

        //        private DateTime GetMaxPredecessorEndDate(List<Task> ListPredecessor)
        //        {
        //            DateTime oMaxPredecessorEndDate=new DateTime();
        //            List<DateTime> listPredecessorEndDate = new List<DateTime>();

        //            foreach (Task t in ListPredecessor) 
        //            {
        //                if (t.EndDate != null)
        //                {
        //                    listPredecessorEndDate.Add((DateTime)t.EndDate);
        //                }
        //            }

        //            try
        //            {
        //                oMaxPredecessorEndDate = listPredecessorEndDate.Max<DateTime>();
        //            }
        //            catch 
        //            {
        //                //Console.WriteLine("GetMaxPredecessorEndDate "+ex);
        //                //Console.WriteLine("TRANQUI ECCEZIONE GESTITA!!!!");
        //                oMaxPredecessorEndDate = DateTime.MinValue;
        //            }
        //            return oMaxPredecessorEndDate;
        //        }

        //        private DateTime GetMinSuccessorsStartDate(List<Task> ListSuccessors)
        //        {
        //            DateTime oMinSuccessorsStartDate = new DateTime();
        //            List<DateTime> listSuccessorsStartDate = new List<DateTime>();

        //            foreach (Task t in ListSuccessors)
        //            {
        //                if (t.StartDate != null)
        //                {
        //                    listSuccessorsStartDate.Add((DateTime)t.StartDate);
        //                }
        //            }

        //            try
        //            {
        //                oMinSuccessorsStartDate = listSuccessorsStartDate.Min<DateTime>();
        //            }
        //            catch //(Exception ex)
        //            {
        //                //Console.WriteLine(ex);
        //                oMinSuccessorsStartDate = DateTime.MaxValue;
        //            }
        //            return oMinSuccessorsStartDate;
        //        }





        //        private void UpdateDateAndCompletenessForParent(Task Task, Person AuthorOfUpdate)
        //        {

        //            DateTime? oUtilityDate;         
        //            bool isUpdate;
        //            bool tempCondition;
        //            int completeness;
        //            List<DateTime> listChildrenStartDate=new List<DateTime>();
        //            List<DateTime> listChildrenEndDate = new List<DateTime>();
        //            List<int> listOfChildrenCompleteness = new List<int>();
        //            List<DateTime> listOfChildrenDeadline = new List<DateTime>();
        //           // Console.WriteLine("\n INIZIO-> UpdateDateAndCompletenessForParent");


        //            if (Task.TaskParent == null) 
        //            {
        //                return;
        //            }
        //            Task oTaskParent = Task.TaskParent;
        //            if (oTaskParent.MetaInfo.isDeleted == true)
        //            {//se il padre è cancellato nn occorre effettuare nessun aggiornamento
        //                return;
        //            }

        //            isUpdate = false;
        //            oTaskParent.TaskChildren = null;
        //            oTaskParent.TaskChildren = GetChildListToTask(oTaskParent.ID, false);

        //          //  Console.WriteLine("Numero di filgi: " + oTaskParent.TaskChildren.Count);
        //            //setto le varie liste
        //            foreach (Task t in oTaskParent.TaskChildren) 
        //            {
        //                if (t.EndDate != null)
        //                {
        //                    listChildrenEndDate.Add((DateTime)t.EndDate);
        //                    //Console.WriteLine("Stampo end date aggihnta alla lista: "+t.EndDate);
        //                }

        //                if (t.StartDate != null)
        //                {
        //                    listChildrenStartDate.Add((DateTime)t.StartDate);
        //                }

        //                if (t.Deadline != null) 
        //                {
        //                    listOfChildrenDeadline.Add((DateTime)t.Deadline);
        //                }

        //                listOfChildrenCompleteness.Add(t.Completeness);
        //            }

        //            //controllo se devo aggiornare la start date del task padre
        //            try
        //            {
        //                oUtilityDate = listChildrenStartDate.Min<DateTime>();//oUtilityDate==Min StartDate dei figli
        //            } 
        //            catch
        //            {
        //                oUtilityDate = null;
        //            }

        //            if (oUtilityDate == null) {
        //            //se anche oTaskParent.StartDate==null nn devo aggiornare

        //                if (oTaskParent.StartDate != null) //se oTaskParent.StartDate!=null devo aggiornare
        //                {
        //                    oTaskParent.StartDate = oUtilityDate;
        //                    isUpdate = true;
        //                }
        //            } 
        //            else//oUtilityDate!=null
        //            {
        //                if (oTaskParent.StartDate == null)//se oTaskParent.StartDate==null devo aggiornare
        //                {
        //                    oTaskParent.StartDate = oUtilityDate;
        //                    isUpdate = true;
        //                }
        //                else//oTaskParent.StartDate!=null
        //                {
        //                    if (DateTime.Compare((DateTime)oTaskParent.StartDate, (DateTime)oUtilityDate) != 0)
        //                    { //se oParentStartDate!=oUtilityDate devo aggiornare
        //                        oTaskParent.StartDate = oUtilityDate;
        //                        isUpdate = true;
        //                    }
        //                }
        //            }
        //            listChildrenStartDate = null;
        //            oUtilityDate = null;

        //            //aggiorno end date
        //            try
        //            {
        //                oUtilityDate= listChildrenEndDate.Max<DateTime>();
        //            }
        //            catch
        //            {
        //                oUtilityDate = null;
        //            }
        //            if (oUtilityDate == null)
        //            {
        //                //se anche oTaskParent.EndDate==null nn devo aggiornare

        //                if (oTaskParent.EndDate != null) //se oTaskParent.EndDate!=null devo aggiornare
        //                {
        //                    oTaskParent.EndDate = oUtilityDate;
        //                    isUpdate = true;
        //                }
        //            }
        //            else//oUtilityDate!=null
        //            {
        //                if (oTaskParent.EndDate == null)//se oTaskParent.EndDate==null devo aggiornare
        //                {
        //                    oTaskParent.EndDate = oUtilityDate;
        //                    isUpdate = true;
        //                }
        //                else//oTaskParent.StartDate!=null
        //                {
        //                    if (DateTime.Compare((DateTime)oTaskParent.EndDate, (DateTime)oUtilityDate) != 0)
        //                    { //se oParentEndDateDate!=oUtilityDate devo aggiornare
        //                        oTaskParent.EndDate = oUtilityDate;
        //                        isUpdate = true;
        //                    }
        //                }
        //            }

        //            listChildrenEndDate = null;
        //            oUtilityDate = null;

        //            //aggiorno deadline
        //            try
        //            {
        //                tempCondition = listOfChildrenDeadline.Count() == oTaskParent.TaskChildren.Count();
        //            }
        //            catch 
        //            {//setto la oTaskParent.Deadline a null perchè nn tutti i figli hanno una deadline
        //                tempCondition = false;
        //                if (oTaskParent.Deadline != null)
        //                {
        //                    oTaskParent.Deadline = null;
        //                    isUpdate = true;
        //                }
        //            }



        //            if (tempCondition)
        //            {
        //                 try
        //                {
        //                 oUtilityDate=listOfChildrenDeadline.Max<DateTime>();
        //                }
        //                catch (Exception ex)
        //                {//qui nn dovrebbe mai entrare... perchè se tempCondition=true significa che tutti i task figli hanno una deadline
        //                    Console.WriteLine("qualcosa nn va... qui nn dovrei mai arrivare...: " + ex);
        //                    oUtilityDate = null;

        //                }
        //                if(oTaskParent.Deadline!=null)
        //                {//se oTaskParent.Deadline==maxDeadline(oUtilityDate) nn aggiorno

        //                    if ((DateTime.Compare((DateTime)oUtilityDate, (DateTime)oTaskParent.Deadline)) != 0)
        //                    {//se oTaskParent.Deadline!=maxDeadline(oUtilityDate) nn aggiorno
        //                        oTaskParent.Deadline=oUtilityDate;
        //                        isUpdate=true;
        //                    }
        //                }
        //                else
        //                {
        //                    oTaskParent.Deadline=oUtilityDate;
        //                    isUpdate=true;
        //                }
        //            }



        //            //aggiornamento completeness
        //            try
        //            {
        //                completeness = (int)listOfChildrenCompleteness.Average();
        //                listOfChildrenCompleteness = null;
        //            }
        //            catch (Exception ex) 
        //            {//qui nn dovrebbe mai entrare
        //                Console.WriteLine(ex);
        //                completeness = -1;
        //            }

        //            if (completeness != oTaskParent.Completeness) 
        //            {
        //                isUpdate = true;
        //                oTaskParent.Completeness = completeness;
        //            }



        //            //aggiorno su DB
        //            if (isUpdate) 
        //            {
        //                SetMetaDataUpdate(AuthorOfUpdate, oTaskParent.MetaInfo); 
        //                SaveOrUpdateTask(oTaskParent);
        //                if (oTaskParent.Level != 0)
        //                {
        //                    UpdateDateAndCompletenessForParent(oTaskParent, AuthorOfUpdate); 
        //                }
        //            }
        //            Console.WriteLine("OK");
        //            return;
        //        }



        //        #endregion

    }
}


 
