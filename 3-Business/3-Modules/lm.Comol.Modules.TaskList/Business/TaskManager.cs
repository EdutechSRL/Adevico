using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.TaskList.Domain;
using COL_BusinessLogic_v2.Comunita;
using lm.Comol.Core.DomainModel;
using NHibernate;
using NHibernate.Util;
using NHibernate.Linq;
using System.Linq.Expressions;
using COL_BusinessLogic_v2.UCServices;
using COL_BusinessLogic_v2.Comol.Manager;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.TaskList;



namespace lm.Comol.Modules.TaskList.Business
{
    public class TaskManager
    {

        private ISession session;

        #region Costruttori

        public TaskManager()
        {
        }

        public TaskManager(ISession session)
        {
            this.session = session;
        }

        public TaskManager(iApplicationContext iAppContext)
        {
            this.session = iAppContext.DataContext.GetCurrentSession();
        }

        #endregion


        #region Category

        public TaskCategory AddTaskCategory(String CategoryName)
        {
            TaskCategory oTC = null;
            ITransaction tx = session.BeginTransaction();
            try
            {
                oTC = new TaskCategory();
                oTC.Name = CategoryName;
                oTC.isDeleted = false;
                SaveOrUpdateTaskCategory(oTC);
                tx.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                if (tx != null && tx.IsActive)
                {
                    tx.Rollback();
                }
            }
            return oTC;
        }

        public TaskCategory DeleteVirtualTaskCategory(TaskCategory CategoryToDelete)
        {
            ITransaction tx = session.BeginTransaction();
            try
            {
                CategoryToDelete.isDeleted = true;
                SaveOrUpdateTaskCategory(CategoryToDelete);
                tx.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                if (tx != null && tx.IsActive)
                {
                    tx.Rollback();
                }
            }

            return CategoryToDelete;
        }

        public TaskCategory UnDeleteTaskCategory(TaskCategory TaskCategoryToUnDelete)
        {
            ITransaction tx = session.BeginTransaction();
            try
            {
                TaskCategoryToUnDelete.isDeleted = false;
                SaveOrUpdateTaskCategory(TaskCategoryToUnDelete);
                tx.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                if (tx != null && tx.IsActive)
                {
                    tx.Rollback();
                }
            }

            return TaskCategoryToUnDelete;
        }

        public TaskCategory UpdateTaskCategory(TaskCategory TaskCategoryToUpdate, String NewTaskCategoryName)
        {
            if (!(TaskCategoryToUpdate.Name.Equals(NewTaskCategoryName)))
            {
                ITransaction tx = session.BeginTransaction();
                try
                {

                    TaskCategoryToUpdate.Name = NewTaskCategoryName;
                    SaveOrUpdateTaskCategory(TaskCategoryToUpdate);

                    tx.Commit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    if (tx != null && tx.IsActive)
                    {
                        tx.Rollback();
                    }
                }
            }
            return TaskCategoryToUpdate;
        }

        public IList<TaskCategory> GetAllTaskCategories(bool IsDeleted)
        {
            IList<TaskCategory> listCategories = null;
            listCategories = GetTaskCategoriesGeneric(tc => tc.isDeleted == IsDeleted).ToList();
            return listCategories;
        }

        public TaskCategory GetTaskCategory(String TaskCategoryName, bool IsDeleted)
        {
            TaskCategory oTC = null;
            try
            {
                oTC = GetTaskCategoriesGeneric(tc => (tc.Name.Equals(TaskCategoryName)) &&
                                                (tc.isDeleted == IsDeleted)).First(); ;
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetTaskCategory " + ex);
                return null;
            }

            return oTC;
        }

        public TaskCategory GetTaskCategory(int ID)
        {
            TaskCategory oTaskCategory = null;
            try
            {
                oTaskCategory = session.Load<TaskCategory>(ID);

                // NHibernateUtil.Initialize(oTask);//se da menate attivare questo

            }
            catch (Exception ex)
            {
                Console.WriteLine("Get TaskCategory Try: " + ex);
            }
            return oTaskCategory;

        }

        private IList<TaskCategory> GetTaskCategoriesGeneric(Expression<Func<TaskCategory, bool>> condition)
        {

            return (from TaskCategory t in session.Linq<TaskCategory>() select t).Where(condition).ToList<TaskCategory>();
        }


        #endregion


        public bool GetIfTaskIsDeleted(long TaskID)
        {
            bool isDeleted;
            isDeleted = GetTask(TaskID).MetaInfo.isDeleted;
            return isDeleted;
        }

        public bool GetIfTaskAssignmentIsDeleted(long TaskAssignmentID)
        {
            bool isDeleted;
            isDeleted = GetTaskAssignment(TaskAssignmentID).MetaInfo.isDeleted;
            return isDeleted;
        }

        private IList<Task> GetTaskGeneric(Expression<Func<Task, bool>> condition)
        {
            IList<Task> ListOfTask;
            try
            {
                ListOfTask = (from Task t in session.Linq<Task>() orderby t.Name select t).Where(condition).ToList<Task>();
            }
            catch (Exception)
            {

                ListOfTask = new List<Task>();
            }
            return ListOfTask;
        }

        private IQueryable<Task> GetIQuerableTaskGeneric(Expression<Func<Task, bool>> condition)
        {
            return (from Task t in session.Linq<Task>() orderby t.Name select t).Where(condition);
        }


        public IList<Task> GetProjectAllCommunities(bool IsDeleted)
        {

            return GetTaskGeneric(tt => (tt.Level == 0) && (tt.MetaInfo.isDeleted == IsDeleted));
        }


        //Restituisce i progetti con la StartDate o ==null o >=AfterDate
        public IList<Task> GetProjectAllCommunitiesStartDateStartAfter(bool IsDeleted, DateTime? StartAfterThisDate)
        {
            return GetTaskGeneric(tt => (tt.Level == 0) &&
                                 (tt.MetaInfo.isDeleted == IsDeleted) &&
                                 ((tt.StartDate == null) || ((DateTime)tt.StartDate >= StartAfterThisDate)));
        }

        //Restituisce i progetti con la EndDate o ==null o <=EndBeforeThisDate
        public IList<Task> GetProjectAllCommunitiesDeadlineEndBefore(bool IsDeleted, DateTime? DeadLineBeforeThisDate)
        {
            return GetTaskGeneric(tt => (tt.Level == 0) &&
                                 (tt.MetaInfo.isDeleted == IsDeleted) &&
                                 ((tt.EndDate == null) || ((DateTime)tt.EndDate <= DeadLineBeforeThisDate)));//StartDate o null o >=AfterDate

        }


        //Restituisce i progetti con la EndDate o ==null o <=EndBeforeThisDate
        public IList<Task> GetProjectAllCommunities(bool IsDeleted, DateTime? StartAfterThisDate, DateTime? EndBeforeThisDate)
        {
            return GetTaskGeneric(tt => (tt.Level == 0) &&
                                 (tt.MetaInfo.isDeleted == IsDeleted) &&
                                 ((tt.StartDate == null) || (DateTime)tt.StartDate >= StartAfterThisDate) &&//StartDate o null o >=AfterDate                              
                                 ((tt.EndDate == null) || ((DateTime)tt.EndDate <= EndBeforeThisDate)));//EndDate o null o <=EndBeforeThisDate

        }


        public IList<Task> GetProjectForAuthor(bool IsDeleted, Person AuthorOfProject, DateTime? StartAfterThisDate, DateTime? EndBeforeThisDate)
        {
            return GetTaskGeneric(tt => (tt.Level == 0) &&
                                 (tt.MetaInfo.isDeleted == IsDeleted) &&
                                 (tt.MetaInfo.CreatedBy == AuthorOfProject) &&
                                 ((tt.StartDate == null) || (DateTime)tt.StartDate >= StartAfterThisDate) &&//StartDate o null o >=AfterDate                              
                                 ((tt.EndDate == null) || ((DateTime)tt.EndDate <= EndBeforeThisDate)));//EndDate o null o <=EndBeforeThisDate

        }

        public IList<Task> GetProjectSpecificCommunity(Community InterestedCommunity, Person InterestedPerson, bool IsDeleted, DateTime AfterDate)
        {
            return GetTaskGeneric(tt => ((tt.Community.Id == InterestedCommunity.Id)
                                        && (tt.Level == 0) && (tt.MetaInfo.isDeleted == IsDeleted)
                                        && ((tt.StartDate == null) || tt.StartDate >= AfterDate)));
        }



        //OKOKOKOKOKOKOKOK
        public Community GetCommunityByID(int CommunityID)
        {
            Community oCommunity = null;
            try
            {
                oCommunity = (from Community c in session.Linq<Community>() where (c.Id == CommunityID) select c).First<Community>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
            return oCommunity;
        }

        public IList<Community> GetCommunities()
        {
            IList<Community> listCommunities = null;
            try
            {
                listCommunities = (from Community c in session.Linq<Community>() select c).ToList<Community>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
            return listCommunities;
        }


        public Community GetCommunity(int ID)
        {
            Community oCommunity = null;
            try
            {
                oCommunity = session.Load<Community>(ID);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Get Community Try: " + ex);
            }
            return oCommunity;

        }

        public int GetCommunityID(long TaskID)
        {
            int CommunityID;
            try
            {
                CommunityID = (from t in session.Linq<Task>() where t.ID == TaskID select t.Community.Id).First();
            }
            catch (Exception)
            {
                CommunityID = 0;
            }
            return CommunityID;
        }


        //OKOKOKOKOKOKOKOK
        public Person GetPerson(int PersonID)
        {
            Person oPerson = null;
            try
            {
                oPerson = session.Load<Person>(PersonID);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return oPerson;
        }


        public IList<Person> GetPersons()
        {
            IList<Person> listPerson = null;
            try
            {
                listPerson = (from Person p in session.Linq<Person>() select p).ToList<Person>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
            return listPerson;
        }

        public IList<Role> GetCommunityRole()
        {
            IList<Role> listRole = null;
            try
            {
                listRole = (from Role p in session.Linq<Role>() select p).ToList<Role>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
            return listRole;
        }


        #region MetaData
        private MetaData SetMetaDataCreate(Person Creator)
        {
            //Console.Write("SetMetaDataCreate ->");
            MetaData oMetaData = new MetaData();
            oMetaData.CreatedBy = Creator;
            oMetaData.CreatedOn = DateTime.Now;
            oMetaData.canDelete = true;
            oMetaData.canModify = true;
            oMetaData.isDeleted = false;

            return oMetaData;
        }

        private MetaData SetMetaDataDelete(Person AuthorOfDelete, MetaData MetaInfo)
        {
            Console.Write("SetMetaDataDelete-> ");
            MetaInfo.DeletedBy = AuthorOfDelete;
            MetaInfo.DeletedOn = DateTime.Now;
            MetaInfo.isDeleted = true;

            return MetaInfo;
        }

        private MetaData SetMetaDataUpdate(Person AuthorOfUpdate, MetaData MetaInfo)
        {
            Console.Write("SetMetaDataUpdate-> ");
            MetaInfo.ModifiedBy = AuthorOfUpdate;
            MetaInfo.ModifiedOn = DateTime.Now;

            return MetaInfo;
        }

        private MetaData SetMetaDataUndelete(MetaData MetaInfo)
        {
            Console.Write("SetMetaDataUndelete ->");
            MetaInfo.DeletedBy = null;
            MetaInfo.DeletedOn = null;
            MetaInfo.isDeleted = false;
            return MetaInfo;
        }


        #endregion

        ///Fare select specific task che carica figli successor e menate varie!!!!!!!! e TUTTO SUI FILE
        #region Operazioni di Assegnamento dei Task

        private IList<TaskAssignment> GetTaskAssignmentGeneric(Expression<Func<TaskAssignment, bool>> condition)
        {
            //Console.WriteLine("Load: GetTaskAssignmentGeneric |||| ");
            IList<TaskAssignment> list;
            list = (from TaskAssignment t in session.Linq<TaskAssignment>() select t).Where(condition).ToList<TaskAssignment>();
            return list;
        }

        private IQueryable<TaskAssignment> GetTaskAssignmentGenericIQuerable(Expression<Func<TaskAssignment, bool>> condition)
        {
            //Console.WriteLine("Load: GetTaskAssignmentGeneric |||| ");
            IQueryable<TaskAssignment> list;
            list = (from TaskAssignment t in session.Linq<TaskAssignment>() select t).Where(condition);
            return list;
        }


        public int GetTaskAssignmentGenericCount(Expression<Func<TaskAssignment, bool>> condition)
        {
            //Console.WriteLine("GetTaskAssignmentGenericCount ->");
            int n;
            try
            {
                n = (from TaskAssignment t in session.Linq<TaskAssignment>() select t).Where(condition).Count();
            }
            catch (Exception)
            {

                n = 0;
            }


            return n;
        }


        private IQueryable<TaskAssignment> GetTaskAssignmentGenericQuery(Expression<Func<TaskAssignment, bool>> condition)
        {
            //Console.WriteLine("Load: GetTaskAssignmentGeneric |||| ");
            return (from TaskAssignment t in session.Linq<TaskAssignment>() select t).Where(condition);
        }

        //------------------------------------------------------------------------------------------------------------------------------------------------------------------
        public IList<long> AddTaskAssignments(List<Person> AssignedPerson, TaskRole oTaskRole,
                                                    Task InterestedTask, Person AuthorOfAssignment)
        {
            //Console.Write("AddTaskAssignment-> ");
            IList<long> listOfTaskAssigmentID = new List<long>();

            TaskAssignment oTaskAssigment = null;
            if (GetNumberOfChildren(InterestedTask.ID, InterestedTask.MetaInfo.isDeleted) != 0 && (oTaskRole == TaskRole.Resource || oTaskRole == TaskRole.Customized_Resource))
            {
                Console.WriteLine("Impossibile effettuare l'assegnamento, il Task assegnato non è una foglia!!!!");
                return null;
            }

            ITransaction tx = session.BeginTransaction();
            try
            {
                foreach (Person oPerson in AssignedPerson)
                {
                    if (GetTaskAssignmentGenericCount(tt => (tt.AssignedUser == oPerson) && (tt.TaskRole == oTaskRole) && (tt.Task == InterestedTask)) > 0) //;  .ToList<TaskAssignment>()
                    {
                        Console.WriteLine("Impossibile effettuare l'assegnamento, perchè già esistente.");
                        return null;
                    }
                    else
                    {
                        oTaskAssigment = new TaskAssignment();
                        oTaskAssigment.AssignedUser = oPerson;
                        oTaskAssigment.TaskRole = oTaskRole;
                        oTaskAssigment.TaskPermissions = -1;
                        oTaskAssigment.Task = InterestedTask;
                        oTaskAssigment.Completeness = 0;
                        if (InterestedTask.Level == 0)
                        {
                            oTaskAssigment.Project = InterestedTask;
                        }
                        else
                        {
                            oTaskAssigment.Project = InterestedTask.Project;
                        }
                        oTaskAssigment.MetaInfo = SetMetaDataCreate(AuthorOfAssignment);
                        oTaskAssigment.TreeVisibility = TreeVisibility.Total;
                        listOfTaskAssigmentID.Add(oTaskAssigment.ID);
                        SaveOrUpdateTaskAssignment(oTaskAssigment);
                    }

                    UpdateTaskChildCompletenessAndParentCompleteness(InterestedTask);
                    tx.Commit();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                if (tx != null && tx.IsActive)
                {
                    tx.Rollback();
                }
            }

            return listOfTaskAssigmentID;
        }


        //public IList<TaskAssignment> AddTaskAssignments(List<Person> AssignedPerson, TaskRole oTaskRole, 
        //                                            Task InterestedTask, Person AuthorOfAssignment)
        //{
        //    Console.Write("AddTaskAssignment-> ");
        //    IList<TaskAssignment> listOfTaskAssigment = new List<TaskAssignment>();
        //    TaskAssignment oTaskAssigment = null;
        //    if (GetNumberOfChildren(InterestedTask.ID, InterestedTask.MetaInfo.isDeleted) != 0 && (oTaskRole == TaskRole.Resource || oTaskRole == TaskRole.Customized_Resource))
        //    {
        //        Console.WriteLine("Impossibile effettuare l'assegnamento, il Task assegnato non è una foglia!!!!");
        //        return null;
        //    }
        //    ITransaction tx = session.BeginTransaction();
        //    try
        //    {
        //        foreach (Person oPerson in AssignedPerson)
        //        {
        //            oTaskAssigment = new TaskAssignment();
        //            oTaskAssigment.AssignedUser = oPerson;
        //            oTaskAssigment.TaskRole = oTaskRole;
        //            oTaskAssigment.TaskPermissions = -1;
        //            oTaskAssigment.Task = InterestedTask;
        //            oTaskAssigment.Completeness = 0;
        //            oTaskAssigment.MetaInfo = SetMetaDataCreate(AuthorOfAssignment);
        //            oTaskAssigment.TreeVisibility = TreeVisibility.Total;
        //            listOfTaskAssigment.Add(oTaskAssigment);
        //            SaveOrUpdateTaskAssignment(oTaskAssigment);
        //        }

        //        UpdateTaskChildCompletenessAndParentCompleteness(InterestedTask);
        //        tx.Commit();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex);
        //        if (tx != null && tx.IsActive)
        //        {
        //            tx.Rollback();
        //        }
        //    } 

        //    return listOfTaskAssigment;
        //}




        // OK
        public TaskAssignment AddTaskAssignment(Person AssignedPerson, TaskRole oTaskRole, Task InterestedTask, Person AuthorOfAssignment)
        {
            //Console.Write("AddTaskAssignment -> ");
            TaskAssignment oTaskAssigment = null;

            if (GetNumberOfChildren(InterestedTask.ID, InterestedTask.MetaInfo.isDeleted) != 0 && (oTaskRole == TaskRole.Resource || oTaskRole == TaskRole.Customized_Resource))
            {
                //Console.WriteLine("Impossibile effettuare l'assegnamento, il Task assegnato non è una foglia!!!!");
                return null;
            }
            ITransaction tx = session.BeginTransaction();

            try
            {
                if (GetTaskAssignmentGenericCount(tt => (tt.AssignedUser == AssignedPerson) && (tt.TaskRole == oTaskRole) && (tt.Task == InterestedTask)) > 0) //;  .ToList<TaskAssignment>()
                {
                    //Console.WriteLine("Impossibile effettuare l'assegnamento, perchè già esistente.");
                    return null;
                }
                else
                {
                    oTaskAssigment = new TaskAssignment();
                    oTaskAssigment.AssignedUser = AssignedPerson;
                    oTaskAssigment.TaskRole = oTaskRole;
                    oTaskAssigment.TaskPermissions = -1;
                    oTaskAssigment.Task = InterestedTask;
                    oTaskAssigment.Completeness = 0;
                    if (InterestedTask.Level == 0)
                    {
                        oTaskAssigment.Project = InterestedTask;
                    }
                    else
                    {
                        oTaskAssigment.Project = InterestedTask.Project;
                    }

                    oTaskAssigment.MetaInfo = SetMetaDataCreate(AuthorOfAssignment);
                    oTaskAssigment.TreeVisibility = TreeVisibility.Total;
                    SaveOrUpdateTaskAssignment(oTaskAssigment);
                    UpdateTaskChildCompletenessAndParentCompleteness(InterestedTask);
                    tx.Commit();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                if (tx != null && tx.IsActive)
                {
                    tx.Rollback();
                }
            }
            return oTaskAssigment;
        }


        public TaskAssignment GetTaskAssignment(long InterestedTaskAssignmentID)
        {
            //Console.WriteLine("Load: GetTaskAssignment ||| ");
            TaskAssignment oTaskAssignment = null;
            try
            {
                oTaskAssignment = new TaskAssignment();
                //   oTaskAssignment = session.Load<TaskAssignment>(InterestedTaskAssignmentID);
                oTaskAssignment = GetTaskAssignmentGeneric(ta => (ta.ID == InterestedTaskAssignmentID)).First();
            }
            catch (Exception ex)
            {

                Console.WriteLine("GetTaskAssignment Try: " + ex);
                return null;
            }
            return oTaskAssignment;
        }


        public IList<TaskAssignment> GetAllTaskAssignments(bool isDeleted)
        {
            IList<TaskAssignment> listTaskAssignments;
            listTaskAssignments = GetTaskAssignmentGeneric(ta => (ta.MetaInfo.isDeleted == isDeleted));
            return listTaskAssignments;
        }


        public IList<TaskAssignment> GetAllTaskAssignments(Person InterestedPerson, bool isDeleted)
        {
            IList<TaskAssignment> listTaskAssignments;
            listTaskAssignments = GetTaskAssignmentGeneric(ta => (ta.MetaInfo.isDeleted == isDeleted) && (ta.AssignedUser.Id == InterestedPerson.Id));
            return listTaskAssignments;
        }

        public IList<TaskAssignment> DeleteVirtualTaskAssignments(IList<TaskAssignment> TaskAssigmentByPersonToDelete, Person AutorOfDelete)
        {
            Console.Write("DeleteVirtualTaskAssignments -> ");


            ITransaction tx = session.BeginTransaction();
            try
            {
                foreach (TaskAssignment t in TaskAssigmentByPersonToDelete)
                {

                    SetMetaDataDelete(AutorOfDelete, t.MetaInfo);
                    SaveOrUpdateTaskAssignment(t);

                }

                try
                {
                    TaskAssignment oTaskAssignment = TaskAssigmentByPersonToDelete.First();
                    UpdateTaskChildCompletenessAndParentCompleteness(oTaskAssignment.Task);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("DeleteVirtualTaskAssignments: nessun elemento nella lista... Nessun Problema: " + ex);
                }
                tx.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                if (tx != null && tx.IsActive)
                {
                    tx.Rollback();
                }
            }
            return TaskAssigmentByPersonToDelete;
        }

        public TaskAssignment DeleteVirtualTaskAssignment(TaskAssignment TaskAssigmentByPersonToDelete, Person AutorOfDelete)
        {
            Console.Write("DeleteVirtualTaskAssignment -> ");

            ITransaction tx = session.BeginTransaction();
            try
            {

                TaskAssigmentByPersonToDelete.MetaInfo = SetMetaDataDelete(AutorOfDelete, TaskAssigmentByPersonToDelete.MetaInfo);
                SaveOrUpdateTaskAssignment(TaskAssigmentByPersonToDelete);
                UpdateTaskChildCompletenessAndParentCompleteness(TaskAssigmentByPersonToDelete.Task);
                tx.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                if (tx != null && tx.IsActive)
                {
                    tx.Rollback();
                }
            }
            return TaskAssigmentByPersonToDelete;
        }



        public void DeleteTaskAssignment(long TaskAssignmentID)
        {
            ITransaction tx = session.BeginTransaction();
            try
            {
                TaskAssignment oTA = GetTaskAssignment(TaskAssignmentID);
                session.Delete(oTA);
                tx.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                if (tx != null && tx.IsActive)
                {
                    tx.Rollback();
                }
            }
        }


        public void DeleteResourcesTaskAssignments(long TaskID)
        {
            ITransaction tx = session.BeginTransaction();
            try
            {
                IList<TaskAssignment> ListTA = GetTaskAssignmentGeneric(ta => ta.Task.ID == TaskID && (ta.TaskRole == TaskRole.Resource || ta.TaskRole == TaskRole.Customized_Resource));
                foreach (TaskAssignment item in ListTA)
                {
                    session.Delete(item);

                }
                Task oTask = GetTask(TaskID);
                UpdateTaskChildCompletenessAndParentCompleteness(oTask);
                tx.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                if (tx != null && tx.IsActive)
                {
                    tx.Rollback();
                }
            }
        }



        public TaskAssignment UnDeleteTaskAssignment(long TaskAssignmentID)
        {
            TaskAssignment oTA = GetTaskAssignment(TaskAssignmentID);
            return UnDeleteTaskAssignment(oTA);

        }
        public TaskAssignment UnDeleteTaskAssignment(TaskAssignment TaskAssignment)
        {
            Console.Write("UnDeleteTaskAssignment -> ");
            ITransaction tx = session.BeginTransaction();
            try
            {
                SetMetaDataUndelete(TaskAssignment.MetaInfo);
                SaveOrUpdateTaskAssignment(TaskAssignment);

                UpdateTaskChildCompletenessAndParentCompleteness(TaskAssignment.Task);
                tx.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                if (tx != null && tx.IsActive)
                {
                    tx.Rollback();
                }
            }
            return TaskAssignment;
        }

        public IList<TaskAssignment> UnDeleteTaskAssignments(IList<TaskAssignment> ListOfTaskAssignment)
        {
            Console.Write("UnDeleteTaskAssignments -> ");
            ITransaction tx = session.BeginTransaction();
            try
            {
                foreach (TaskAssignment ta in ListOfTaskAssignment)
                {
                    SetMetaDataUndelete(ta.MetaInfo);
                    SaveOrUpdateTaskAssignment(ta);
                }
                try
                {
                    TaskAssignment oTaskAssignment = ListOfTaskAssignment.First();
                    UpdateTaskChildCompletenessAndParentCompleteness(oTaskAssignment.Task);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("DeleteVirtualTaskAssignments: nessun elemento nella lista... Nessun Problema: " + ex);
                }
                tx.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                if (tx != null && tx.IsActive)
                {
                    tx.Rollback();
                }
            }
            return ListOfTaskAssignment;
        }

        public int GetAverageCompletenessForSingleTask(Task InterestedTask)
        {
            Console.Write("GetAverageCompletenessForSingleTask-> ");
            int CompletenessAVG;
            IList<TaskAssignment> ListTA = GetTaskAssignmentGeneric(ta => ta.Task.ID == InterestedTask.ID && ta.Task.MetaInfo.isDeleted == false
                                                && ta.MetaInfo.isDeleted == false && (ta.TaskRole == TaskRole.Resource || ta.TaskRole == TaskRole.Customized_Resource));
            for (int i = 0; i < ListTA.Count; i++)
            {
                if (ListTA.ElementAt(i).MetaInfo.isDeleted || ListTA.ElementAt(i).Task.MetaInfo.isDeleted)
                {
                    ListTA.RemoveAt(i);
                    i--;
                }
            }
            try
            {
                CompletenessAVG = (int)(from TaskAssignment ta in ListTA select ta.Completeness).Average();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                CompletenessAVG = 0;
            }
            return CompletenessAVG;
        }

        public TaskAssignment UpdateTaskAssignmentCompleteness(TaskAssignment TaskAssignmentToUpdate, int Completeness)
        {

            Console.Write("UpdateTaskAssignmentCompleteness-> ");
            if (TaskAssignmentToUpdate.Completeness != Completeness)
            {
                ITransaction tx = session.BeginTransaction();
                try
                {
                    TaskAssignmentToUpdate.Completeness = Completeness;
                    SaveOrUpdateTaskAssignment(TaskAssignmentToUpdate);
                    UpdateTaskChildCompletenessAndParentCompleteness(TaskAssignmentToUpdate.Task);

                    tx.Commit();
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex);
                    if (tx != null && tx.IsActive)
                    {
                        tx.Rollback();
                    }
                }

            }
            return TaskAssignmentToUpdate;

        }

        public bool UpdateTaskAssignmentCompleteness(List<long> TaskAssignmentsID, long NewTaskID, Person AuthorOfUpdate)
        {
            Console.Write("UpdateTaskAssignmentCompleteness-> ");
            TaskAssignment oTA = null;
            Task oTask = GetTask(NewTaskID);
            ITransaction tx = session.BeginTransaction();
            try
            {
                foreach (long taID in TaskAssignmentsID)
                {
                    oTA = GetTaskAssignment(taID);
                    oTA.Task = oTask;
                    SaveOrUpdateTaskAssignment(oTA);
                    UpdateTaskChildCompletenessAndParentCompleteness(oTA.Task);
                }
                tx.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                if (tx != null && tx.IsActive)
                {
                    tx.Rollback();
                }
                return false;
            }
            return true;
        }


        public bool UpdateTaskAssignmentTask(List<long> ListOtTaskAssignmentID, long TaskID, Person AuthorOfUpdate)
        {
            TaskAssignment TaskAssignmentToUpdate = null;
            ITransaction tx = session.BeginTransaction();
            try
            {
                Task Task = GetTask(TaskID);
                foreach (long TaskAssignmentID in ListOtTaskAssignmentID)
                {
                    TaskAssignmentToUpdate = GetTaskAssignment(TaskAssignmentID);
                    TaskAssignmentToUpdate.Task = Task;
                    SetMetaDataUpdate(AuthorOfUpdate, TaskAssignmentToUpdate.MetaInfo);
                    SaveOrUpdateTaskAssignment(TaskAssignmentToUpdate);
                    UpdateTaskChildCompletenessAndParentCompleteness(Task);
                }
                tx.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                if (tx != null && tx.IsActive)
                {
                    tx.Rollback();
                }
                return false;
            }

            return true;
        }


        public IList<TaskAssignment> GetTaskAssignments(Task Task, bool isDeleted)
        {
            Console.WriteLine("Load: GetTaskAssignments");
            IList<TaskAssignment> listTaskAssignment = null;

            listTaskAssignment = GetTaskAssignmentGeneric(tap => (tap.MetaInfo.isDeleted == isDeleted) && (tap.Task.ID == Task.ID));
            return listTaskAssignment;
        }

        public IList<TaskAssignment> GetTaskAssignments(long TaskID, bool isDeleted)
        {
            Console.WriteLine("Load: GetTaskAssignments");
            IList<TaskAssignment> listTaskAssignment = null;

            listTaskAssignment = GetTaskAssignmentGeneric(tap => (tap.MetaInfo.isDeleted == isDeleted) && (tap.Task.ID == TaskID));
            return listTaskAssignment;
        }

        public IList<TaskAssignment> GetTaskAssignments(Task Task, Person InterestedPerson, bool isDeleted)
        {
            Console.WriteLine("Load: GetTaskAssignments");
            IList<TaskAssignment> listTaskAssignment = null;

            //listTaskAssignment = GetTaskAssignmentGeneric(tap => (tap.AssignedUser.Id==tap.AssignedUser.Id)&&
            //                                                            (tap.Task.ID == Task.ID) &&
            //                                                            (tap.MetaInfo.isDeleted == isDeleted));

            Expression<Func<TaskAssignment, bool>> condition = tap => (tap.Task.ID == Task.ID) && (tap.MetaInfo.isDeleted == isDeleted);
            listTaskAssignment = (from TaskAssignment t
                                              in session.Linq<TaskAssignment>()
                                  where t.AssignedUser.Id == InterestedPerson.Id
                                  select t).Where(condition).ToList<TaskAssignment>();

            return listTaskAssignment;
        }

        public IList<TaskAssignment> GetTaskAssignments(Person InterestedPerson, bool isDeleted)
        {
            Console.WriteLine("Load: GetTaskAssignments");
            IList<TaskAssignment> listTaskAssignment = null;

            Expression<Func<TaskAssignment, bool>> condition = tap => (tap.MetaInfo.isDeleted == isDeleted);
            listTaskAssignment = (from TaskAssignment t
                                              in session.Linq<TaskAssignment>()
                                  where t.AssignedUser.Id == InterestedPerson.Id
                                  select t).Where(condition).ToList<TaskAssignment>();
            return listTaskAssignment;

        }

        public IList<TaskAssignment> GetTaskAssignments(long TaskID, TaskRole Role, bool isDeleted)
        {
            Console.WriteLine("Load: GetTaskAssignments");
            IList<TaskAssignment> listTaskAssignment = null;

            Expression<Func<TaskAssignment, bool>> condition = tap => (tap.MetaInfo.isDeleted == isDeleted) &&
                                                                       (tap.TaskRole == Role) && tap.Task.ID == TaskID;
            listTaskAssignment = (from TaskAssignment t
                                              in session.Linq<TaskAssignment>()
                                  select t).Where(condition).ToList<TaskAssignment>();
            return listTaskAssignment;

        }

        public IList<TaskAssignment> GetTaskAssignments(long TaskID, TaskRole Role)
        {
            Console.WriteLine("Load: GetTaskAssignments");
            IList<TaskAssignment> listTaskAssignment = null;

            Expression<Func<TaskAssignment, bool>> condition = tap => (tap.TaskRole == Role) && tap.Task.ID == TaskID;
            listTaskAssignment = (from TaskAssignment t in session.Linq<TaskAssignment>()
                                  select t).Where(condition).ToList<TaskAssignment>();
            return listTaskAssignment;

        }


        public TaskAssignment GetTaskAssignment(long InterestedTaskID, int PersonID)
        {
            TaskAssignment oTaskAssignment;
            try
            {
                oTaskAssignment = (from TaskAssignment ta in session.Linq<TaskAssignment>()
                                   where ta.AssignedUser.Id == PersonID && ta.Task.ID == InterestedTaskID
                                   select ta).First<TaskAssignment>();
            }
            catch (Exception ex)
            {
                oTaskAssignment = null;
                Console.WriteLine("GetTaskAssignment " + ex);
            }

            return oTaskAssignment;
        }

        public IList<Community> GetCommunitiesInTaskAssignment(IList<TaskAssignment> ListOfTaskAssignment)
        {
            IList<Community> listCommunityInTaskAssignment = null;

            listCommunityInTaskAssignment = (from TaskAssignment t in ListOfTaskAssignment
                                             select t.Task.Community).Distinct<Community>().ToList();
            return listCommunityInTaskAssignment;
        }

        #endregion

        #region Operazioni su Task


        public long GetProjectID(long TaskID)
        {
            long ProjectID;
            try
            {
                ProjectID = (from t in session.Linq<Task>() where t.ID == TaskID select t.Project.ID).First();
            }
            catch (Exception)
            {
                ProjectID = TaskID;
            }
            return ProjectID;
        }


        public void PrintTaskToConsole(Task InterestedTask)
        {
            Console.WriteLine("\n ID: " + InterestedTask.ID);
            Console.WriteLine("Nome: " + InterestedTask.Name);
            Console.WriteLine("Descrizione: " + InterestedTask.Description);
            Console.WriteLine("start date: " + InterestedTask.StartDate);
            Console.WriteLine("End Date: " + InterestedTask.EndDate);
            Console.WriteLine("Deadline: " + InterestedTask.Deadline);
            Console.WriteLine("Completeness: " + InterestedTask.Completeness);
            Console.WriteLine("Category: " + InterestedTask.Category);
            Console.WriteLine("Level: " + InterestedTask.Level);
            Console.WriteLine("Priority: " + InterestedTask.Priority);
        }

        private void UpdateTaskChildCompletenessAndParentCompleteness(Task InterestedTask)
        {
            Console.Write("UpdateTaskChildCompletenessAndParentCompleteness -> ");
            int newCompleteness = GetAverageCompletenessForSingleTask(InterestedTask);

            if (InterestedTask.Completeness != newCompleteness)
            {
                InterestedTask.Completeness = newCompleteness;
                Console.WriteLine("Task: " + InterestedTask.Name + "Compl: " + InterestedTask.Completeness);
                SaveOrUpdateTask(InterestedTask);
                UpdateParentCompleteness(InterestedTask.TaskParent);
            }
        }

        private void UpdateParentCompleteness(Task ParentTask)
        {
            Console.Write("UpdateParentCompleteness-> ");
            if (ParentTask != null)
            {
                int newCompleteness = GetAverageChildCompletenes(ParentTask);
                if (ParentTask.Completeness != newCompleteness)
                {
                    ParentTask.Completeness = newCompleteness;
                    Console.WriteLine("GetAverageChildCompletenes: " + ParentTask.Name + " COMPL: " + ParentTask.Completeness);
                    SaveOrUpdateTask(ParentTask);

                    UpdateParentCompleteness(ParentTask.TaskParent);
                }
            }
            return;
        }

        private int SetLevel(Task ParentTask)
        {
            Console.Write("SetLevel -> ");
            if (ParentTask != null)
            {
                return ParentTask.Level + 1;
            }
            return 0;
        }

        public Task AddTask(String Name, String Description, String Notes, Community Community, Person Creator, DateTime? StartDate, DateTime? Deadline, DateTime? EndDate, bool isArchived, TaskPriority Priority, TaskStatus Status, TaskCategory Category, Task TaskParent)
        {
            Console.Write("AddTask-> ");
            Task oTask = null;
            ITransaction tx = session.BeginTransaction();
            try
            {
                oTask = new Task();
                oTask.Name = Name;
                oTask.Description = Description;
                oTask.Notes = Notes;
                oTask.Community = Community;
                oTask.MetaInfo = SetMetaDataCreate(Creator);
                oTask.StartDate = ((DateTime)StartDate).Date;
                oTask.Deadline = ((DateTime)Deadline).Date;
                oTask.EndDate = ((DateTime)EndDate).Date;
                oTask.isArchived = isArchived;
                oTask.Level = SetLevel(TaskParent);
                oTask.Priority = Priority;
                oTask.Status = Status;
                oTask.Completeness = 0;
                oTask.Category = Category;
                oTask.TaskParent = TaskParent;
                if (oTask.Level == 1)
                {
                    oTask.Project = oTask.TaskParent;
                }
                else if (oTask.Level > 1)
                {
                    oTask.Project = oTask.TaskParent.Project;
                }

                if (oTask.Level > 0)
                {
                    oTask.Community = TaskParent.Community;
                    oTask.isPersonal = TaskParent.isPersonal;
                    oTask.isPortal = TaskParent.isPortal;

                }

                SetWBSforNewTask(oTask);

                SaveOrUpdateTask(oTask);
                if (oTask.Level != 0)
                {
                    UpdateDateForParent(oTask.TaskParent, Creator);
                    UpdateParentCompleteness(oTask.TaskParent);
                }
                tx.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                if (tx != null && tx.IsActive)
                {
                    tx.Rollback();
                }
            }

            return oTask;
        }

        public List<long> AddTaskChildren(List<Task> NewChildren, List<dtoReallocateTA> NewAssignmentsForChildren, long ParentID, Person Creator)
        {
            List<long> ListOfTaskId = new List<long>();
            ITransaction tx = session.BeginTransaction();
            try
            {
                Task Parent = GetTask(ParentID);
                List<dtoReallocateTA> ListOfAssignmentsForSingleChild;
                IList<TaskAssignment> ListOfParentResourceToDelete = GetTaskAssignmentGeneric(ta => ta.Task.ID == ParentID && (ta.TaskRole == TaskRole.Resource || ta.TaskRole == TaskRole.Customized_Resource));
                foreach (TaskAssignment item in ListOfParentResourceToDelete)
                {
                    session.Delete(item);
                }
                Task oNewTask;
                foreach (Task child in NewChildren)
                {
                    ListOfAssignmentsForSingleChild = (from o in NewAssignmentsForChildren where o.TaskID == child.ID && !o.isDeleted select o).ToList();
                    child.ID = 0;
                    oNewTask = SubAddTaskWithoutTransaction(child, Parent, Creator);
                    AddTaskAssignmentsWithoutTransaction(ListOfAssignmentsForSingleChild, oNewTask, Creator);
                    oNewTask.Completeness = GetAverageCompletenessForSingleTask(oNewTask);
                    SaveOrUpdateTask(oNewTask);
                    ListOfTaskId.Add(oNewTask.ID);
                    //fare fun
                }
                UpdateDateForParent(Parent, Creator);
                UpdateParentCompleteness(Parent);
                tx.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                if (tx != null && tx.IsActive)
                {
                    tx.Rollback();
                }
            }
            return ListOfTaskId;
        }


        //OK
        private IList<TaskAssignment> AddTaskAssignmentsWithoutTransaction(List<dtoReallocateTA> ListOfDtoReallocateTA,
                                            Task InterestedTask, Person AuthorOfAssignment)
        {
            Console.Write("AddTaskAssignment-> ");
            IList<TaskAssignment> listOfTaskAssigment = new List<TaskAssignment>();
            TaskAssignment oTaskAssigment = null;

            foreach (dtoReallocateTA item in ListOfDtoReallocateTA)
            {
                if (GetTaskAssignmentGenericCount(tt => (tt.AssignedUser.Id == item.PersonID) && (tt.TaskRole == item.Role) && (tt.Task == InterestedTask)) > 0) //;  .ToList<TaskAssignment>()
                {
                    Console.WriteLine("Impossibile effettuare l'assegnamento, perchè già esistente.");
                    return listOfTaskAssigment;
                }
                else
                {
                    oTaskAssigment = new TaskAssignment();
                    oTaskAssigment.AssignedUser = GetPerson(item.PersonID);
                    oTaskAssigment.TaskRole = item.Role;
                    oTaskAssigment.TaskPermissions = -1;
                    oTaskAssigment.Task = InterestedTask;
                    oTaskAssigment.Completeness = item.Completeness;
                    oTaskAssigment.Project = InterestedTask.Project;
                    oTaskAssigment.MetaInfo = SetMetaDataCreate(AuthorOfAssignment);
                    oTaskAssigment.TreeVisibility = TreeVisibility.Total;
                    listOfTaskAssigment.Add(oTaskAssigment);
                    SaveOrUpdateTaskAssignment(oTaskAssigment);
                }
            }
            return listOfTaskAssigment;
        }


        private Task SubAddTaskWithoutTransaction(Task newTask, Task Parent, Person Creator)
        {

            newTask.Community = Parent.Community;
            newTask.MetaInfo = SetMetaDataCreate(Creator);
            newTask.isArchived = false;
            newTask.Level = SetLevel(Parent);
            newTask.Completeness = 0;
            newTask.TaskParent = Parent;
            newTask.isPersonal = Parent.isPersonal;
            newTask.isPortal = Parent.isPortal;
            if (newTask.Level == 1)
            {
                newTask.Project = Parent;
            }
            else if (newTask.Level > 1)
            {
                newTask.Project = Parent.Project;
            }
            SetWBSforNewTask(newTask);//forse da modificare... e assegnarli a mano tanto sò quanti figli sono...
            SaveOrUpdateTask(newTask);
            return newTask;
        }

        public Task AddTask(Task newTask, long TaskParentID, Person Creator)
        {
            Console.Write("AddTask-> ");

            ITransaction tx = session.BeginTransaction();
            try
            {
                Task oParentTask = GetTask(TaskParentID);


                newTask.Community = oParentTask.Community;
                newTask.MetaInfo = SetMetaDataCreate(Creator);
                newTask.isArchived = false;
                newTask.Level = SetLevel(oParentTask);
                newTask.Completeness = 0;
                newTask.TaskParent = oParentTask;
                newTask.isPersonal = oParentTask.isPersonal;
                newTask.isPortal = oParentTask.isPortal;
                if (newTask.Level == 1)
                {
                    newTask.Project = oParentTask;
                }
                else if (newTask.Level > 1)
                {
                    newTask.Project = oParentTask.Project;
                }

                SetWBSforNewTask(newTask);

                SaveOrUpdateTask(newTask);
                if (newTask.Level != 0)
                {
                    UpdateDateForParent(newTask.TaskParent, Creator);
                    UpdateParentCompleteness(newTask.TaskParent);
                }
                tx.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                if (tx != null && tx.IsActive)
                {
                    tx.Rollback();
                }
            }

            return newTask;
        }


        public Task AddProject(Task oTask, Person Creator)
        {
            Console.Write("AddProject ->");
            //Task oTask = new Task();
            ITransaction tx = session.BeginTransaction();
            try
            {

                oTask.MetaInfo = SetMetaDataCreate(Creator);
                oTask.ID = 0;
                oTask.isArchived = false;
                oTask.Level = 0;
                oTask.Completeness = 0;
                oTask.TaskParent = null;
                SetWBSforNewTask(oTask);
                SaveOrUpdateTask(oTask);
                tx.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                if (tx != null && tx.IsActive)
                {
                    tx.Rollback();
                }
            }
            return oTask;
        }

        //no update community e parent
        public Task UpdateTaskDetail(long TaskToUpdateID, Task oNewTask, Person AutorOfUpdate)
        {
            Console.Write("UpdateTaskDetail-> ");
            Task oTask = null;
            ITransaction tx = session.BeginTransaction();
            try
            {
                oTask = GetTask(TaskToUpdateID);
                oTask.Name = oNewTask.Name;
                oTask.Description = oNewTask.Description;
                oTask.Notes = oNewTask.Notes;
                SetMetaDataUpdate(AutorOfUpdate, oTask.MetaInfo);
                oTask.isArchived = false;
                oTask.Priority = oNewTask.Priority;
                oTask.Category = oNewTask.Category;
                oTask.Status = oNewTask.Status;

                bool isNecessaryUpdateTaskParent = false;

                if (!(oTask.StartDate.Equals(oNewTask.StartDate)))
                {
                    oTask.StartDate = oNewTask.StartDate;
                    isNecessaryUpdateTaskParent = true;
                }

                if (!(oTask.Deadline.Equals(oNewTask.Deadline)))
                {
                    oTask.Deadline = oNewTask.Deadline;
                    isNecessaryUpdateTaskParent = true;
                }

                if (!(oTask.EndDate.Equals(oNewTask.EndDate)))
                {
                    oTask.EndDate = oNewTask.EndDate;

                    isNecessaryUpdateTaskParent = true;
                }
                SaveOrUpdateTask(oTask);
                if (isNecessaryUpdateTaskParent && oTask.Level != 0)
                {
                    UpdateDateForParent(oTask.TaskParent, AutorOfUpdate);
                    UpdateParentCompleteness(oTask.TaskParent);
                }
                tx.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                if (tx != null && tx.IsActive)
                {
                    tx.Rollback();
                }
            }

            return oTask;



        }



        //Non fa l'update delle liste presenti... c sono i rispettivi metodi
        public Task UpdateTask(Task TaskForUpdate, String Name, String Description, String Notes, Community Community,
                                Person AutorOfUpdate, DateTime? StartDate, DateTime? Deadline, DateTime? EndDate,
                                bool isArchived, TaskPriority Priority, TaskStatus Status, TaskCategory Category,
                                Task TaskParent)
        {
            Console.Write("UpdateTask-> ");
            Task oTask = null;
            ITransaction tx = session.BeginTransaction();
            try
            {
                oTask = TaskForUpdate;
                oTask.Name = Name;
                oTask.Description = Description;
                oTask.Notes = Notes;
                oTask.Community = Community;
                SetMetaDataUpdate(AutorOfUpdate, oTask.MetaInfo);
                //oTask.StartDate = StartDate;
                //oTask.Deadline = Deadline;
                //oTask.EndDate = EndDate;
                oTask.isArchived = false;
                oTask.Priority = Priority;
                oTask.Category = Category;
                oTask.TaskParent = TaskParent;
                oTask.Level = SetLevel(TaskParent);

                bool isNecessaryUpdateTaskParent = false;

                if (!(oTask.StartDate.Equals(StartDate)))
                {
                    oTask.StartDate = StartDate;

                    isNecessaryUpdateTaskParent = true;
                }

                if (!(oTask.Deadline.Equals(Deadline)))
                {
                    oTask.Deadline = Deadline;

                    isNecessaryUpdateTaskParent = true;
                }

                if (!(oTask.EndDate.Equals(EndDate)))
                {
                    oTask.EndDate = EndDate;

                    isNecessaryUpdateTaskParent = true;
                }

                if (oTask.TaskParent != null && TaskParent != null)
                {
                    if (oTask.TaskParent.ID != TaskParent.ID)
                    {
                        oTask.TaskParent = TaskParent;
                        isNecessaryUpdateTaskParent = true;
                    }
                }
                else if (oTask.TaskParent == null || TaskParent == null)
                {
                    oTask.TaskParent = TaskParent;

                    isNecessaryUpdateTaskParent = true;
                }


                SaveOrUpdateTask(oTask);
                if (isNecessaryUpdateTaskParent && oTask.Level != 0)
                {

                    UpdateDateForParent(oTask.TaskParent, AutorOfUpdate);
                    UpdateParentCompleteness(oTask.TaskParent);
                }
                tx.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                if (tx != null && tx.IsActive)
                {
                    tx.Rollback();
                }
            }

            return oTask;
        }



        public Task UnDeleteTask(long TaskID, Person AuthorOfUnDelete)
        {
            Task oTask = GetTask(TaskID);
            return UnDeleteTask(oTask, AuthorOfUnDelete);
        }




        public Task UnDeleteTask(Task oTask, Person AuthorOfUnDelete)
        {

            Console.Write("UnDeleteTask-> ");
            if ((oTask.TaskParent != null) && (oTask.TaskParent.MetaInfo.isDeleted))
            {
                Console.WriteLine("ERRORE: Impossibile l'operazione di UnDelete, il padre del Task è stato cancellato");
                return null;
            }

            ITransaction tx = session.BeginTransaction();
            try
            {
                SetMetaDataUndelete(oTask.MetaInfo);
                SetWBSforNewTask(oTask);
                UpdateWBSforMeAndChildren(oTask, oTask.TaskWBSstring, 0);

                if (oTask.Level != 0)
                {
                    UpdateDateForParent(oTask.TaskParent, AuthorOfUnDelete);
                    UpdateParentCompleteness(oTask.TaskParent);
                }
                SaveOrUpdateTask(oTask);
                tx.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                if (tx != null && tx.IsActive)
                {
                    tx.Rollback();
                }
            }



            return oTask;
        }


        private void SubDeleteTask(IList<Task> ChildList)
        {
            IList<TaskAssignment> ListOfTA;
            foreach (Task item in ChildList)
            {
                SubDeleteTask(item.TaskChildren);
                ListOfTA = GetTaskAssignmentGeneric(ta => ta.Task.ID == item.ID);
                foreach (TaskAssignment ta in ListOfTA)
                {
                    session.Delete(ta);
                }
                session.Delete(item);
            }
        }


        public void DeleteTask(long TaskID)
        {
            ITransaction tx = session.BeginTransaction();
            try
            {
                IList<TaskAssignment> ListOfTA;

                Task oTask = GetTask(TaskID);
                SubDeleteTask(oTask.TaskChildren);
                ListOfTA = GetTaskAssignmentGeneric(ta => ta.Task.ID == TaskID);
                foreach (TaskAssignment item in ListOfTA)
                {
                    session.Delete(item);
                }
                session.Delete(oTask);
                tx.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                if (tx != null && tx.IsActive)
                {
                    tx.Rollback();
                }

            }

        }

        public dtoTaskSimple GetTaskNameAndID(long TaskID)
        {

            dtoTaskSimple dto;
            try
            {
                dto = (from t in session.Linq<Task>().ToList() where t.ID == TaskID select new dtoTaskSimple(t)).First();

            }
            catch (Exception)
            {

                dto = null;
            }
            return dto;
        }

        public dtoTaskSimple GetParentNameAndID(long TaskID)
        {

            dtoTaskSimple dto;

            Task oTask = GetTask(TaskID);

            if (oTask.Level > 0)
            {
                dto = new dtoTaskSimple(oTask.TaskParent);
            }
            else
            {
                dto = new dtoTaskSimple(oTask);
            }

            return dto;
        }

        public dtoReallocateTAWithHeader GetDtoReallacateUsersWhithHeaderForSingleTask(long TaskID)
        {

            dtoReallocateTAWithHeader dto = null;
            try
            {
                Task oTask = GetTask(TaskID);
                dto = new dtoReallocateTAWithHeader(oTask, (from ta in session.Linq<TaskAssignment>().ToList()
                                                            where ta.Task.ID == TaskID && ta.TaskRole != TaskRole.ProjectOwner
                                                            select new dtoReallocateTA(ta)).ToList());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }

            return dto;
        }

        public List<dtoReallocateTAWithHeader> GetDtoReallacateUsersWhithHeaderForTaskTree(long TaskID)
        {
            List<dtoReallocateTAWithHeader> ListDto = new List<dtoReallocateTAWithHeader>();
            try
            {
                Task oTask = GetTask(TaskID);

                ListDto.Add(new dtoReallocateTAWithHeader(oTask, (from ta in session.Linq<TaskAssignment>().ToList()
                                                                  where ta.Task.ID == TaskID
                                                                  select new dtoReallocateTA(ta)).ToList()));
                SubGetDtoReallacateUsersWhithHeaderForTaskTree(oTask.TaskChildren, ListDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }

            return ListDto;
        }

        public List<dtoReallocateTAWithHeader> SubGetDtoReallacateUsersWhithHeaderForTaskTree(IList<Task> TaskChildren, List<dtoReallocateTAWithHeader> ListDto)
        {
            foreach (Task child in TaskChildren)
            {
                ListDto.Add(new dtoReallocateTAWithHeader(child, (from ta in session.Linq<TaskAssignment>().ToList()
                                                                  where ta.Task.ID == child.ID
                                                                  select new dtoReallocateTA(ta)).ToList()));
                SubGetDtoReallacateUsersWhithHeaderForTaskTree(child.TaskChildren, ListDto);
            }

            return ListDto;
        }


        public List<long> ReallocateResourceAfterUndeleteOfTask(long TaskID, long ParentID, List<dtoReallocateTA> ListTa, Person AuthorOfAssignment)
        {
            List<long> listOfTAID = new List<long>();
            ITransaction tx = session.BeginTransaction();
            TaskAssignment oTA;
            try
            {
                Task oTask = GetTask(TaskID);

                SetMetaDataUndelete(oTask.MetaInfo);
                SetWBSforNewTask(oTask);
                UpdateWBSforMeAndChildren(oTask, oTask.TaskWBSstring, 0);
                IList<TaskAssignment> ListTA = GetTaskAssignmentGeneric(ta => ta.Task.ID == ParentID);
                IList<TaskAssignment> ListTAonlyResource = (from ta in ListTA where ta.TaskRole == TaskRole.Resource || ta.TaskRole == TaskRole.Customized_Resource select ta).ToList();

                foreach (TaskAssignment item in ListTAonlyResource)
                {
                    session.Delete(item);
                }

                foreach (dtoReallocateTA item in ListTa)
                {
                    if (item.Role == TaskRole.Manager)
                    {
                        int ExistManager = (from ta in ListTA where ta.AssignedUser.Id == item.PersonID select ta).Count();
                        if (ExistManager == 0)
                        {
                            oTA = new TaskAssignment();
                            oTA.ID = 0;
                            oTA.MetaInfo = SetMetaDataCreate(AuthorOfAssignment);
                            if (oTask.Level == 1)
                            {
                                oTA.Project = oTask.TaskParent;
                            }
                            else
                            {
                                oTA.Project = oTask.Project;
                            }
                            oTA.Task = oTask;
                            oTA.TaskPermissions = -1;
                            oTA.TreeVisibility = TreeVisibility.Total;
                            oTA.TaskRole = item.Role;
                            oTA.Completeness = item.Completeness;
                            oTA.AssignedUser = GetPerson(item.PersonID);
                            SaveOrUpdateTaskAssignment(oTA);
                            listOfTAID.Add(oTA.ID);
                        }
                    }
                    else
                    {
                        oTA = new TaskAssignment();
                        oTA.ID = 0;
                        oTA.MetaInfo = SetMetaDataCreate(AuthorOfAssignment);
                        if (oTask.Level == 1)
                        {
                            oTA.Project = oTask.TaskParent;
                        }
                        else
                        {
                            oTA.Project = oTask.Project;
                        }
                        oTA.Task = oTask;
                        oTA.TaskPermissions = -1;
                        oTA.TreeVisibility = TreeVisibility.Total;
                        oTA.TaskRole = item.Role;
                        oTA.Completeness = item.Completeness;
                        oTA.AssignedUser = GetPerson(item.PersonID);
                        SaveOrUpdateTaskAssignment(oTA);
                        listOfTAID.Add(oTA.ID);
                    }

                }
                UpdateDateForParent(oTask.TaskParent, AuthorOfAssignment);
                UpdateTaskChildCompletenessAndParentCompleteness(oTask);
                tx.Commit();
            }
            catch (Exception)
            {
                throw;
            }


            return listOfTAID;
        }

        public List<long> ReallocateResourceAfterVirtualDeleteOfTask(long TaskID, List<dtoReallocateTA> ListTa, Person AuthorOfAssignment)
        {
            Task oTask = GetTask(TaskID);
            List<long> listOfTAID = new List<long>();
            ITransaction tx = session.BeginTransaction();
            TaskAssignment oTA;
            try
            {
                DeleteAssigmentsAfterVirtualDelete(TaskID, AuthorOfAssignment);
                SetMetaDataDelete(AuthorOfAssignment, oTask.MetaInfo);
                UpdateWBSforBrothers(oTask);
                SaveOrUpdateTask(oTask);
                foreach (dtoReallocateTA item in ListTa)
                {
                    if (item.Role == TaskRole.Manager)
                    {
                        if (GetTaskAssignmentGenericCount(ta => ta.Task.ID == oTask.TaskParent.ID && ta.AssignedUser.Id == item.PersonID && ta.TaskRole == TaskRole.Manager) == 0)
                        {
                            oTA = new TaskAssignment();
                            oTA.ID = 0;
                            oTA.MetaInfo = SetMetaDataCreate(AuthorOfAssignment);
                            if (oTask.Level == 1)
                            {
                                oTA.Project = oTask.TaskParent;
                            }
                            else
                            {
                                oTA.Project = oTask.Project;
                            }
                            oTA.Task = oTask.TaskParent;
                            oTA.TaskPermissions = -1;
                            oTA.TreeVisibility = TreeVisibility.Total;
                            oTA.TaskRole = item.Role;
                            oTA.Completeness = item.Completeness;
                            oTA.AssignedUser = GetPerson(item.PersonID);
                            SaveOrUpdateTaskAssignment(oTA);
                            listOfTAID.Add(oTA.ID);
                        }
                    }
                    else
                    {
                        oTA = new TaskAssignment();
                        oTA.ID = 0;
                        oTA.MetaInfo = SetMetaDataCreate(AuthorOfAssignment);
                        if (oTask.Level == 1)
                        {
                            oTA.Project = oTask.TaskParent;
                        }
                        else
                        {
                            oTA.Project = oTask.Project;
                        }
                        oTA.Task = oTask.TaskParent;
                        oTA.TaskPermissions = -1;
                        oTA.TreeVisibility = TreeVisibility.Total;
                        oTA.TaskRole = item.Role;
                        oTA.Completeness = item.Completeness;
                        oTA.AssignedUser = GetPerson(item.PersonID);
                        SaveOrUpdateTaskAssignment(oTA);
                        listOfTAID.Add(oTA.ID);
                    }

                }
                UpdateDateForParent(oTask.TaskParent, AuthorOfAssignment);
                UpdateTaskChildCompletenessAndParentCompleteness(oTask.TaskParent);

                tx.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                if (tx != null && tx.IsActive)
                {
                    tx.Rollback();
                }
            }
            return listOfTAID;
        }

        private void DeleteAssigmentsAfterVirtualDelete(long TaskID, Person AuthorOfAssignment)
        {
            IList<TaskAssignment> ListTA = GetTaskAssignmentGeneric(ta => ta.Task.ID == TaskID);
            Task oTask = GetTask(TaskID);
            SetMetaDataDelete(AuthorOfAssignment, oTask.MetaInfo);
            oTask.Completeness = 0;
            SaveOrUpdateTask(oTask);
            foreach (TaskAssignment item in ListTA)
            {
                if (item.TaskRole != TaskRole.Manager)
                {
                    session.Delete(item);
                }
            }

            foreach (Task child in oTask.TaskChildren)
            {
                SubDeleteAssigmentsAfterVirtualDelete(child.ID, AuthorOfAssignment);
            }
        }

        private void SubDeleteAssigmentsAfterVirtualDelete(long TaskID, Person AuthorOfAssignment)
        {
            Task oTask = GetTask(TaskID);
            SetMetaDataDelete(AuthorOfAssignment, oTask.MetaInfo);
            oTask.Completeness = 0;
            SaveOrUpdateTask(oTask);
            IList<TaskAssignment> ListTA = GetTaskAssignmentGeneric(ta => ta.Task.ID == TaskID);
            foreach (TaskAssignment item in ListTA)
            {
                if (item.TaskRole != TaskRole.Manager)
                {
                    session.Delete(item);
                }
            }
            foreach (Task child in oTask.TaskChildren)
            {
                SubDeleteAssigmentsAfterVirtualDelete(child.ID, AuthorOfAssignment);
            }
            if (oTask.TaskChildren.Count == 0)
            {
                UpdateParentCompleteness(oTask);
            }
        }


        //setta icampi di cancellazione in metadata del task e dei suoi 
        public void DeleteVirtualTask(long TaskID, Person AuthorOfDelete)
        {
            Task oTask = GetTask(TaskID);
            DeleteVirtualTask(oTask, AuthorOfDelete);
        }

        public void DeleteVirtualTask(Task InterestedTask, Person AuthorOfDelete)
        {

            ITransaction tx = session.BeginTransaction();
            try
            {
                DeleteVirtualTaskWithoutTransaction(InterestedTask, AuthorOfDelete);
                tx.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                if (tx != null && tx.IsActive)
                {
                    tx.Rollback();
                }

            }

        }

        private void DeleteVirtualTaskWithoutTransaction(Task InterestedTask, Person AuthorOfDelete)
        {
            Console.Write("DeleteVirtualTask-> ");

            Task oTask = InterestedTask;
            List<dtoReallocateTAWithHeader> ListDto = new List<dtoReallocateTAWithHeader>();

            foreach (Task item in oTask.TaskChildren)
            {
                if (!item.MetaInfo.isDeleted)
                {
                    SubDeleteVirtualTaskWithoutTransaction(item, AuthorOfDelete);
                }
            }
            SetMetaDataDelete(AuthorOfDelete, oTask.MetaInfo);
            if (oTask.TaskChildren.Count > 0)
            {
                UpdateParentCompleteness(oTask);
                UpdateDateForParent(oTask, AuthorOfDelete);
            }

            UpdateWBSforBrothers(oTask);
            SaveOrUpdateTask(oTask);
            if (oTask.Level != 0)
            {
                if (!oTask.TaskParent.MetaInfo.isDeleted)
                {
                    UpdateDateForParent(oTask.TaskParent, AuthorOfDelete);
                    UpdateParentCompleteness(oTask.TaskParent);
                }
            }


        }

        private void SubDeleteVirtualTaskWithoutTransaction(Task oTask, Person AuthorOfDelete)
        {
            foreach (Task item in oTask.TaskChildren)
            {
                SubDeleteVirtualTaskWithoutTransaction(item, AuthorOfDelete);
            }
            UpdateDateForParent(oTask, AuthorOfDelete);
            if (oTask.TaskChildren.Count > 0)
            {
                UpdateParentCompleteness(oTask);
            }
            UpdateWBSforBrothers(oTask);
            SetMetaDataDelete(AuthorOfDelete, oTask.MetaInfo);
            SaveOrUpdateTask(oTask);

        }



        //seleziona da db un task... nn c sono caricati i figli e i succ e pred
        public Task GetTask(long InterestedTaskID)
        {
            Task oTask = null;
            try
            {
                oTask = session.Load<Task>(InterestedTaskID);
            }
            catch (Exception ex)
            {
                oTask = null;             

            }
            return oTask;
        }


        public IList<Task> GetAllTaskOfSpecificProject(long InterestedProjectID)
        {
            IList<Task> listOfTask;
            listOfTask = GetTaskGeneric(t => t.Project.ID == InterestedProjectID);
            return listOfTask;
        }

        //restituisce lista dei figli di un task
        public IList<Task> GetChildrenListToTask(long InteresedTaskID, bool isDeleted)
        {
            Console.WriteLine("Load: GetChildrenListToTask");

            return GetTaskGeneric(tt => (tt.TaskParent.ID == InteresedTaskID) && (tt.MetaInfo.isDeleted == isDeleted));
        }

        private long GetTaskDuration(Task oTask)
        {
            return ((DateTime)oTask.EndDate).Ticks - ((DateTime)oTask.StartDate).Ticks;
        }

        private int GetAverageChildCompletenes(Task InterestedTask)
        {
            Console.Write("GetAverageChildCompletenes -> ");
            double CompletenessAVG = 0;
            long ChildWeigh;
            Console.WriteLine("GetAverageChildCompletenes " + InterestedTask.Description);

            int avg;

            IList<Task> ChildrenList = GetChildrenListToTask(InterestedTask.ID, false);

            if (ChildrenList.Count == 0)
            {
                avg = 0;
            }
            else
            {
                long TotalWeigh = (from t in ChildrenList select GetTaskDuration(t)).Sum();
                foreach (Task child in ChildrenList)
                {
                    ChildWeigh = ((DateTime)child.EndDate).Ticks - ((DateTime)child.StartDate).Ticks;
                    double temp = (double)ChildWeigh / (double)TotalWeigh;
                    double childComplPesata = temp * child.Completeness;
                    CompletenessAVG += childComplPesata;
                }
                avg = Math.Min((int)Math.Ceiling(CompletenessAVG), (int)100);
            }
            return avg;
        }

        public int GetNumberOfChildren(long InteresedTaskID, bool IsDeleted)
        {
            Console.WriteLine("GetNumberOfChildren ->");
            int n;
            Expression<Func<Task, bool>> condition = tt => (tt.TaskParent.ID == InteresedTaskID) &&
                                                            (tt.MetaInfo.isDeleted == IsDeleted);

            n = (from Task t in session.Linq<Task>() select t).Where(condition).Count();

            return n;
        }


        #endregion


        #region Operazioni relative i file

        //        public TaskListCommunityFile GetTaskCommunityFileByID(System.Guid TaskListItemFileID)
        //        {
        //            TaskListCommunityFile oFile = null;
        //            try
        //            {
        //                oFile = session.Load<TaskListCommunityFile>(TaskListItemFileID);
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine("Get Community Try: " + ex);
        //            }
        //            return oFile;

        //        }
        ////--------------------------------------------------------------------------------------------------------------------------------------
        //        public TaskListFile GetTaskListFile(System.Guid FileID)
        //        {
        //            TaskListFile oFile = null;
        //            try
        //            {
        //                oFile = session.Load<TaskListFile>(FileID);
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine(ex);

        //                return null;
        //            }
        //            return oFile;
        //        }
        ////--------------------------------------------------------------------------------------------------------------------------------------
        //        public void UnLinkToCommunityFileFromTask(System.Guid TaskListItemFileID)
        //        {
        //            ITransaction tx = session.BeginTransaction();
        //            try
        //            {

        //                TaskListCommunityFile oFile = GetTaskCommunityFileByID(TaskListItemFileID);
        //                if (!((oFile == null)))
        //                {
        //                    session.Delete(oFile);
        //                }
        //                tx.Commit();
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine(ex);
        //                if (tx != null && tx.IsActive)
        //                {
        //                    tx.Rollback();
        //                }
        //            }
        //        }
        ////--------------------------------------------------------------------------------------------------------------------------------------
        //        public void VirtualDeleteFileFromTask(System.Guid ItemFileID, int DeletedByID) 
        //            {
        //              ITransaction tx = session.BeginTransaction();
        //              try
        //                {
        //                    SetVirtualDeleteToFile(ItemFileID, DeletedByID, true);
        //                }
        //              catch (Exception ex) 
        //                {
        //                    Console.WriteLine(ex);
        //                    if (tx != null && tx.IsActive)
        //                    {
        //                        tx.Rollback();
        //                    }
        //                }
        //            }
        ////--------------------------------------------------------------------------------------------------------------------------------------
        //        public void VirtualUnDeleteFileFromTask(System.Guid ItemFileID, int DeletedByID)
        //        {
        //            ITransaction tx = session.BeginTransaction();
        //            try
        //            {
        //                SetVirtualDeleteToFile(ItemFileID, DeletedByID, true);
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine(ex);
        //                if (tx != null && tx.IsActive)
        //                {
        //                    tx.Rollback();
        //                }
        //            }
        //        }
        ////--------------------------------------------------------------------------------------------------------------------------------------
        //        public void SetVirtualDeleteToFile(System.Guid ItemFileID, int DeletedByID, bool isdeleted)
        //        {
        //            Person oPerson = this.GetPerson(DeletedByID);
        //            TaskListFile oFile = this.GetTaskListFile(ItemFileID);
        //            ITransaction tx = session.BeginTransaction();
        //            try
        //            {

        //                if ((oPerson != null) && (oFile != null))
        //                {                
        //                    oFile.ModifiedOn = DateTime.Now;
        //                    oFile.ModifiedBy = oPerson;
        //                    oFile.isDeleted = isdeleted;
        //                    session.SaveOrUpdate(oFile);
        //                }
        //                tx.Commit();
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine(ex);
        //                if (tx != null && tx.IsActive)
        //                {
        //                    tx.Rollback();
        //                }
        //            }
        //        }
        ////--------------------------------------------------------------------------------------------------------------------------------------
        //        public void RemoveFileFromTask(System.Guid FileID, string BaseUserRepositoryPath)
        //        {
        //            TaskListFile oItemFile = this.NEW_GetTaskFile(FileID);

        //            if ((oItemFile != null))
        //            {
        //                ITransaction tx = session.BeginTransaction();
        //                try
        //                {
        //                    bool isInternal = false;
        //                    string FileSystemPath = BaseUserRepositoryPath;                 
        //                    if (oItemFile is TaskListCommunityFile)
        //                    {
        //                        session.Delete(oItemFile);
        //                    }
        //                    else
        //                    {
        //                        isInternal = true;
        //                        FileSystemPath = ((TaskListInternalFile)oItemFile).File.Id.ToString()+ ".stored";
        //                        session.Delete(((TaskListInternalFile)oItemFile).File);
        //                        session.Delete(oItemFile);
        //                    }
        //                   tx.Commit();

        //                    if (isInternal)
        //                    {
        //                        try
        //                        {
        //                            System.IO.File.Delete(FileSystemPath);
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                        }
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    Console.WriteLine(ex);
        //                    if (tx != null && tx.IsActive)
        //                    {
        //                        tx.Rollback();
        //                    }
        //                }
        //            }
        //        }
        ////--------------------------------------------------------------------------------------------------------------------------------------
        //        public TaskListFile NEW_GetTaskFile(System.Guid TaskListFileID)
        //        {
        //            TaskListFile oFile = null;

        //            try
        //            {
        //                oFile = session.Load<TaskListFile>(TaskListFileID);
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine(ex);
        //            }
        //            return oFile;
        //        }
        ////--------------------------------------------------------------------------------------------------------------------------------------
        //        public List<dtoTaskListFile> NEW_GetTaskDTOFiles(Task oItem, bool OnlyVisibleFiles, TaskPermissionEnum oPermission, CoreModuleRepository  oModule)
        //        {

        //            List<dtoTaskListFile> oList = new List<dtoTaskListFile>();


        //            //try
        //            //{

        //            //    List<TaskListInternalFile> oInternalList = (from f in session.Linq<TaskListInternalFile>()
        //            //                                                where f.OwnerTask.ID == oItem.ID && (!OnlyVisibleFiles || (OnlyVisibleFiles && f.isDeleted == false))
        //            //                                                select f).ToList();

        //            //    if (oInternalList.Count > 0)
        //            //    {
        //            //        oList = (from f in oInternalList select new dtoTaskListFile(f, oPermission, oItem.MetaInfo.isDeleted, oItem.Community != null)).ToList();
        //            //    }
        //            //    List<TaskListCommunityFile> oCommunityFiles = (from f in session.Linq<TaskListCommunityFile>() where f.OwnerTask.ID == oItem.ID && (!OnlyVisibleFiles || (OnlyVisibleFiles && f.isDeleted == false)) select f).ToList();
        //            //    if (oCommunityFiles.Count > 0)
        //            //    {
        //            //        oList.AddRange((from f in oCommunityFiles select new dtoTaskListFile(f, oPermission, oModule, oItem.MetaInfo.isDeleted)).ToList());
        //            //    }
        //            //}
        //            //catch (Exception ex)
        //            //{

        //            //    Console.WriteLine(ex);
        //            //}

        //            //return oList.OrderByDescending(dto => dto.ModifiedBy).ThenBy(dto => dto.Name).ToList();
        //            return oList;
        //        }



        #endregion



        #region Funzioni di SaveOrUpdate

        private void SaveOrUpdateTaskCategory(TaskCategory TaskCategory)
        {
            session.SaveOrUpdate(TaskCategory);
        }

        private void SaveOrUpdateTaskPriority(TaskPriority TaskPriority)
        {
            session.SaveOrUpdate(TaskPriority);
        }

        private void SaveOrUpdateTask(Task t)
        {
            Console.WriteLine("SaveOrUpdateTask " + t.Name);
            try
            {
                session.SaveOrUpdate(t);
            }
            catch (Exception ex)
            {

                Console.WriteLine("problemi nel salvataggio del Task: " + ex);
                Console.WriteLine("Fine Stack Errori ");
            }

            // Console.WriteLine("TASK SALVATO SU DB: "+t.Name);
        }

        private void SaveOrUpdatePredecessionLink(PredecessionLink pl)
        {
            Console.WriteLine("SaveOrUpdatePredecessionLink " + pl.PredecessorTask.Name + "->" + pl.SuccessorTask.Name);
            session.SaveOrUpdate(pl);
        }



        private void SaveOrUpdateTaskAssignment(TaskAssignment TaskAssignment)
        {
            session.SaveOrUpdateCopy(TaskAssignment);
            //Console.WriteLine("SaveOrUpdateTaskAssignment " + TaskAssignment.ID + " " + TaskAssignment.AssignedUser.Name + " " + TaskAssignment.TaskRole);

        }

        #endregion




        #region utility e controlli





        private bool ControlFirstDateMinOrEqualSecondDate(DateTime FirstDate, DateTime SecondDate)
        {

            int compareDate = DateTime.Compare(FirstDate, SecondDate);
            if (compareDate <= 0)
            {
                return true;
            }
            return false;
        }

        private bool ControlFirstDateBiggerOrEqualSecondDate(DateTime FirstDate, DateTime SecondDate)
        {

            int compareDate = DateTime.Compare(FirstDate, SecondDate);
            if (compareDate >= 0)
            {
                return true;
            }
            return false;
        }





        private void UpdateDateForParent(Task Parent, Person AuthorOfUpdate)
        {
            Console.Write("UpdateDateForParent-> ");
            DateTime? oUtilityDate;
            bool isUpdate;
            bool tempCondition;

            List<DateTime> listChildrenStartDate = new List<DateTime>();
            List<DateTime> listChildrenEndDate = new List<DateTime>();

            List<DateTime> listOfChildrenDeadline = new List<DateTime>();
            // Console.WriteLine("\n INIZIO-> UpdateDateAndCompletenessForParent");
            if (Parent.MetaInfo.isDeleted == true)
            {//se il padre è cancellato nn occorre effettuare nessun aggiornamento
                return;
            }

            isUpdate = false;

            IList<Task> ListOfChildren = GetChildrenListToTask(Parent.ID, false);

            //setto le varie liste
            if (ListOfChildren.Count == 0)
            {
                return;
            }
            foreach (Task t in ListOfChildren)
            {
                if (t.EndDate != null)
                {
                    listChildrenEndDate.Add((DateTime)t.EndDate);
                    //Console.WriteLine("Stampo end date aggihnta alla lista: "+t.EndDate);
                }

                if (t.StartDate != null)
                {
                    listChildrenStartDate.Add((DateTime)t.StartDate);
                }

                if (t.Deadline != null)
                {
                    listOfChildrenDeadline.Add((DateTime)t.Deadline);
                }
            }

            //controllo se devo aggiornare la start date del task padre
            try
            {
                oUtilityDate = listChildrenStartDate.Min<DateTime>();//oUtilityDate==Min StartDate dei figli
            }
            catch
            {
                oUtilityDate = null;
            }

            if (oUtilityDate == null)
            {
                //se anche oTaskParent.StartDate==null nn devo aggiornare

                if (Parent.StartDate != null) //se oTaskParent.StartDate!=null devo aggiornare
                {
                    Parent.StartDate = oUtilityDate;
                    isUpdate = true;
                }
            }
            else//oUtilityDate!=null
            {
                if (Parent.StartDate == null)//se oTaskParent.StartDate==null devo aggiornare
                {
                    Parent.StartDate = oUtilityDate;
                    isUpdate = true;
                }
                else//oTaskParent.StartDate!=null
                {
                    if (DateTime.Compare((DateTime)Parent.StartDate, (DateTime)oUtilityDate) != 0)
                    { //se oParentStartDate!=oUtilityDate devo aggiornare
                        Parent.StartDate = oUtilityDate;
                        isUpdate = true;
                    }
                }
            }
            listChildrenStartDate = null;
            oUtilityDate = null;

            //aggiorno end date
            try
            {
                oUtilityDate = listChildrenEndDate.Max<DateTime>();
            }
            catch
            {
                oUtilityDate = null;
            }
            if (oUtilityDate == null)
            {
                //se anche oTaskParent.EndDate==null nn devo aggiornare

                if (Parent.EndDate != null) //se oTaskParent.EndDate!=null devo aggiornare
                {
                    Parent.EndDate = oUtilityDate;
                    isUpdate = true;
                }
            }
            else//oUtilityDate!=null
            {
                if (Parent.EndDate == null)//se oTaskParent.EndDate==null devo aggiornare
                {
                    Parent.EndDate = oUtilityDate;
                    isUpdate = true;
                }
                else//oTaskParent.StartDate!=null
                {
                    if (DateTime.Compare((DateTime)Parent.EndDate, (DateTime)oUtilityDate) != 0)
                    { //se oParentEndDateDate!=oUtilityDate devo aggiornare
                        Parent.EndDate = oUtilityDate;
                        isUpdate = true;
                    }
                }
            }

            listChildrenEndDate = null;
            oUtilityDate = null;

            //aggiorno deadline
            try
            {
                tempCondition = listOfChildrenDeadline.Count() == ListOfChildren.Count;
            }
            catch
            {//setto la oTaskParent.Deadline a null perchè nn tutti i figli hanno una deadline
                tempCondition = false;
                if (Parent.Deadline != null)
                {
                    Parent.Deadline = null;
                    isUpdate = true;
                }
            }

            if (tempCondition)
            {
                try
                {
                    oUtilityDate = listOfChildrenDeadline.Max<DateTime>();
                }
                catch (Exception ex)
                {//qui nn dovrebbe mai entrare... perchè se tempCondition=true significa che tutti i task figli hanno una deadline
                    Console.WriteLine("qualcosa nn va... qui nn dovrei mai arrivare...: " + ex);
                    oUtilityDate = null;

                }
                if (Parent.Deadline != null)
                {//se oTaskParent.Deadline==maxDeadline(oUtilityDate) nn aggiorno

                    if ((DateTime.Compare((DateTime)oUtilityDate, (DateTime)Parent.Deadline)) != 0)
                    {//se oTaskParent.Deadline!=maxDeadline(oUtilityDate) nn aggiorno
                        Parent.Deadline = oUtilityDate;
                        isUpdate = true;
                    }
                }
                else
                {
                    Parent.Deadline = oUtilityDate;
                    isUpdate = true;
                }
            }

            //aggiorno su DB
            if (isUpdate)
            {
                SetMetaDataUpdate(AuthorOfUpdate, Parent.MetaInfo);
                SaveOrUpdateTask(Parent);
                if (Parent.TaskParent != null)
                {
                    UpdateDateForParent(Parent.TaskParent, AuthorOfUpdate);
                }
            }
            Console.WriteLine("OK");
            return;
        }

        #endregion



        #region MATTIA:

        #region "AssignedTasksPage"

        //Dato l id di una persona e di una comunita, restituisce l elenco dei Task assegnati alla persona in quella comunita
        public IList<Task> GetTasksByCommunityPersonId(int cId, int pId)
        {
            IList<Task> tasklist;
            ITransaction tx = session.BeginTransaction();
            try
            {
                IList<TaskAssignment> list = (from TaskAssignment ta in session.Linq<TaskAssignment>()
                                              where ta.AssignedUser.Id == pId && ta.Task.Community.Id == cId
                                              select ta).ToList<TaskAssignment>();

                tasklist = (from TaskAssignment ta in list select ta.Task).ToList<Task>();

                tx.Commit();
            }
            catch (Exception ex)
            {
                if (tx.IsActive)
                {
                    tx.Rollback();
                }
                Console.WriteLine(ex);
                tasklist = new List<Task>();
            }

            return tasklist;
        }
        //--------------------------------------------------------------------------------------------------------------
        //Per la Paginazione: Restituisce il numero di Tasks assegnati alla persona di cui passo l'ID e il filtro (AllCommunities,Personal,Portal,CurrentCommunity) come parametro;
        public int GetAssignedTasksCount(int PersonId, TaskFilter Filter, int CommunityId)
        {
            int taskcount = 0;
            switch (Filter)
            {
                case TaskFilter.AllCommunities:
                    taskcount = GetTaskAssignmentGenericCount(tt => (tt.MetaInfo.isDeleted == false) && (tt.TaskRole == TaskRole.Resource) && (tt.AssignedUser.Id == PersonId) && (tt.Task.Status != TaskStatus.completed));
                    break;

                case TaskFilter.CommunityPersonal:
                    taskcount = GetTaskAssignmentGenericCount(tt => (tt.MetaInfo.isDeleted == false) && (tt.TaskRole == TaskRole.Resource) && (tt.AssignedUser.Id == PersonId) && (tt.Task.Community.Id == CommunityId) && (tt.Task.isPersonal == true) && (tt.Task.Status != TaskStatus.completed));
                    break;
                case TaskFilter.CurrentCommunity:
                    taskcount = GetTaskAssignmentGenericCount(tt => (tt.MetaInfo.isDeleted == false) && (tt.TaskRole == TaskRole.Resource) && (tt.AssignedUser.Id == PersonId) && (tt.Task.Community.Id == CommunityId) && (tt.Task.Status != TaskStatus.completed));
                    break;
                case TaskFilter.PortalPersonal:
                    taskcount = GetTaskAssignmentGenericCount(tt => (tt.MetaInfo.isDeleted == false) && (tt.TaskRole == TaskRole.Resource) && (tt.AssignedUser.Id == PersonId) && (tt.Task.Community == null) && (tt.Task.isPersonal == true) && (tt.Task.isPortal == true) && (tt.Task.Status != TaskStatus.completed));
                    break;
                case TaskFilter.AllCommunitiesPersonal:
                    taskcount = GetTaskAssignmentGenericCount(tt => (tt.MetaInfo.isDeleted == false) && (tt.TaskRole == TaskRole.Resource) && (tt.AssignedUser.Id == PersonId) && (tt.Task.isPersonal == true) && (tt.Task.Status != TaskStatus.completed));
                    break;
                default:
                    taskcount = GetTaskAssignmentGenericCount(tt => (tt.MetaInfo.isDeleted == false) && (tt.TaskRole == TaskRole.Resource) && (tt.AssignedUser.Id == PersonId) && (tt.Task.Status != TaskStatus.completed));
                    break;
            }

            return taskcount;
        }
        //--------------------------------------------------------------------------------------------------------------

        #region "Tasks filtered By Community Ordered By..."
        //In base al TaskFilter selezionato carica il metodo appropriato per recuperare i task asegnati all utente
        public IList<dtoAssignedTasksWithCommunityHeader> GetPagedAssignedTasksByCommunity(int PersonId, int PageSize, int PageSkip, TaskFilter Filter, int CommunityId, Sorting oSorting, String portalName)
        {
            List<dtoAssignedTasksWithCommunityHeader> olist = new List<dtoAssignedTasksWithCommunityHeader>();
            IList<TaskAssignment> blist = new List<TaskAssignment>();
            IList<dtoTaskWithPortalComm> plist = new List<dtoTaskWithPortalComm>();
            ITransaction tx = session.BeginTransaction();
            try
            {

                //switch (oSorting)
                //{
                //    case Sorting.DeadlineOrder:
                //        blist = (GetTaskAssignmentGenericQuery(tt => (tt.MetaInfo.isDeleted == false) && (tt.TaskRole == TaskRole.Resource) && (tt.AssignedUser.Id == PersonId) && (tt.Task.Status != TaskStatus.completed))).ToList<TaskAssignment>();
                //        plist = (from pl in blist select new dtoTaskWithPortalComm(pl)).OrderBy(f => f.Deadline).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                //        //(f => f.ProjectName).ThenBy
                //        break;

                //    case Sorting.AlphabeticalOrder:
                //        blist = (GetTaskAssignmentGenericQuery(tt => (tt.MetaInfo.isDeleted == false) && (tt.TaskRole == TaskRole.Resource) && (tt.AssignedUser.Id == PersonId) && (tt.Task.Status != TaskStatus.completed))).ToList<TaskAssignment>();
                //        plist = (from pl in blist select new dtoTaskWithPortalComm(pl)).OrderBy(f => f.ProjectName).ThenBy(f => f.Name).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();   //.OrderBy(tt => tt.Task.Project.Name).ThenBy(tt => tt.Task.Name).Skip(PageSkip).Take(PageSize).ToList<TaskAssignment>();
                //        break;

                //    default:
                //        blist = (GetTaskAssignmentGenericQuery(tt => (tt.MetaInfo.isDeleted == false) && (tt.TaskRole == TaskRole.Resource) && (tt.AssignedUser.Id == PersonId) && (tt.Task.Status != TaskStatus.completed))).ToList<TaskAssignment>();
                //        plist = (from pl in blist select new dtoTaskWithPortalComm(pl)).OrderBy(f => f.Deadline).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                //        break;
                //}

                switch (Filter)
                {
                    case TaskFilter.AllCommunities:
                        switch (oSorting)
                        {
                            case Sorting.DeadlineOrder:
                                blist = (GetTaskAssignmentGenericQuery(tt => (tt.MetaInfo.isDeleted == false) && (tt.TaskRole == TaskRole.Resource) && (tt.AssignedUser.Id == PersonId) && (tt.Task.Status != TaskStatus.completed))).ToList<TaskAssignment>(); //.OrderBy(tt => tt.Task.Deadline).ThenBy(tt => tt.Task.Community.Name).Skip(PageSkip).Take(PageSize)
                                plist = (from pl in blist select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(f => f.CommunityName).ThenBy(f => f.Deadline).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                break;

                            case Sorting.AlphabeticalOrder:
                                blist = (GetTaskAssignmentGenericQuery(tt => (tt.MetaInfo.isDeleted == false) && (tt.TaskRole == TaskRole.Resource) && (tt.AssignedUser.Id == PersonId) && (tt.Task.Status != TaskStatus.completed))).ToList<TaskAssignment>(); //.OrderBy(tt => tt.Task.Name).ThenBy(tt => tt.Task.Community.Name).Skip(PageSkip).Take(PageSize)
                                plist = (from pl in blist select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(f => f.CommunityName).ThenBy(f => f.Name).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                break;

                            default:
                                blist = (GetTaskAssignmentGenericQuery(tt => (tt.MetaInfo.isDeleted == false) && (tt.TaskRole == TaskRole.Resource) && (tt.AssignedUser.Id == PersonId) && (tt.Task.Status != TaskStatus.completed))).ToList<TaskAssignment>(); //.OrderBy(tt => tt.Task.Deadline).ThenBy(tt => tt.Task.Community.Name).Skip(PageSkip).Take(PageSize)
                                plist = (from pl in blist select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(f => f.CommunityName).ThenBy(f => f.Deadline).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                break;
                        }
                        break;

                    case TaskFilter.PortalPersonal:
                        switch (oSorting)
                        {
                            case Sorting.DeadlineOrder:
                                blist = (GetTaskAssignmentGenericQuery(tt => (tt.MetaInfo.isDeleted == false) && (tt.TaskRole == TaskRole.Resource) && (tt.AssignedUser.Id == PersonId) && (tt.Task.Community.Id == null) && (tt.Task.isPortal == true) && (tt.Task.isPersonal == true) && (tt.Task.Status != TaskStatus.completed))).ToList<TaskAssignment>();
                                plist = (from pl in blist select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(f => f.CommunityName).ThenBy(f => f.Deadline).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                break;

                            case Sorting.AlphabeticalOrder:
                                blist = (GetTaskAssignmentGenericQuery(tt => (tt.MetaInfo.isDeleted == false) && (tt.TaskRole == TaskRole.Resource) && (tt.AssignedUser.Id == PersonId) && (tt.Task.Community.Id == null) && (tt.Task.isPortal == true) && (tt.Task.isPersonal == true) && (tt.Task.Status != TaskStatus.completed))).ToList<TaskAssignment>();
                                plist = (from pl in blist select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(f => f.CommunityName).ThenBy(f => f.Name).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                break;
                        }
                        break;

                    case TaskFilter.CommunityPersonal:
                        switch (oSorting)
                        {
                            case Sorting.DeadlineOrder:
                                blist = (GetTaskAssignmentGenericQuery(tt => (tt.MetaInfo.isDeleted == false) &&
                                        (tt.TaskRole == TaskRole.Resource) && (tt.AssignedUser.Id == PersonId) && (tt.Task.Community.Id == CommunityId) && (tt.Task.Status != TaskStatus.completed)
                                        && (tt.Task.isPersonal == true))).OrderBy(tt => tt.Task.Community.Name).ToList<TaskAssignment>();
                                plist = (from pl in blist select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(f => f.CommunityName).ThenBy(f => f.Deadline).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                break;

                            case Sorting.AlphabeticalOrder:
                                blist = (GetTaskAssignmentGenericQuery(tt => (tt.MetaInfo.isDeleted == false) &&
                                        (tt.TaskRole == TaskRole.Resource) && (tt.AssignedUser.Id == PersonId) && (tt.Task.Community.Id == CommunityId) && (tt.Task.Status != TaskStatus.completed)
                                        && (tt.Task.isPersonal == true))).ToList<TaskAssignment>();
                                plist = (from pl in blist select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(f => f.CommunityName).ThenBy(f => f.Name).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                break;
                        }
                        break;

                    case TaskFilter.CurrentCommunity:
                        switch (oSorting)
                        {
                            case Sorting.DeadlineOrder:
                                blist = (GetTaskAssignmentGenericQuery(tt => (tt.MetaInfo.isDeleted == false) && (tt.TaskRole == TaskRole.Resource) && (tt.AssignedUser.Id == PersonId) && (tt.Task.Community.Id == CommunityId) && (tt.Task.Status != TaskStatus.completed))).ToList<TaskAssignment>();
                                plist = (from pl in blist select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(f => f.CommunityName).ThenBy(f => f.Deadline).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                break;

                            case Sorting.AlphabeticalOrder:
                                blist = (GetTaskAssignmentGenericQuery(tt => (tt.MetaInfo.isDeleted == false) && (tt.TaskRole == TaskRole.Resource) && (tt.AssignedUser.Id == PersonId) && (tt.Task.Community.Id == CommunityId) && (tt.Task.Status != TaskStatus.completed))).ToList<TaskAssignment>();
                                plist = (from pl in blist select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(f => f.CommunityName).ThenBy(f => f.Name).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                break;
                        }
                        break;
                    case TaskFilter.AllCommunitiesPersonal:
                        switch (oSorting)
                        {
                            case Sorting.DeadlineOrder:
                                blist = (GetTaskAssignmentGenericQuery(tt => (tt.MetaInfo.isDeleted == false) && (tt.TaskRole == TaskRole.Resource) && (tt.AssignedUser.Id == PersonId) && (tt.Task.isPersonal == true) && (tt.Task.Status != TaskStatus.completed))).ToList<TaskAssignment>();
                                plist = (from pl in blist select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(f => f.CommunityName).ThenBy(f => f.Deadline).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                break;

                            case Sorting.AlphabeticalOrder:
                                blist = (GetTaskAssignmentGenericQuery(tt => (tt.MetaInfo.isDeleted == false) && (tt.TaskRole == TaskRole.Resource) && (tt.AssignedUser.Id == PersonId) && (tt.Task.isPersonal == true) && (tt.Task.Status != TaskStatus.completed))).ToList<TaskAssignment>();
                                plist = (from pl in blist select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(f => f.CommunityName).ThenBy(f => f.Name).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                break;
                        }
                        break;

                    case TaskFilter.None:
                        switch (oSorting)
                        {
                            case Sorting.DeadlineOrder:
                                blist = (GetTaskAssignmentGenericQuery(tt => (tt.MetaInfo.isDeleted == false) && (tt.TaskRole == TaskRole.Resource) && (tt.AssignedUser.Id == PersonId) && (tt.Task.Status != TaskStatus.completed))).ToList<TaskAssignment>();
                                plist = (from pl in blist select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(f => f.CommunityName).ThenBy(f => f.Deadline).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                break;

                            case Sorting.AlphabeticalOrder:
                                blist = (GetTaskAssignmentGenericQuery(tt => (tt.MetaInfo.isDeleted == false) && (tt.TaskRole == TaskRole.Resource) && (tt.AssignedUser.Id == PersonId) && (tt.Task.Status != TaskStatus.completed))).ToList<TaskAssignment>();
                                plist = (from pl in blist select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(f => f.CommunityName).ThenBy(f => f.Name).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                break;
                        }
                        break;

                    default:
                        switch (oSorting)
                        {
                            case Sorting.DeadlineOrder:
                                blist = (GetTaskAssignmentGenericQuery(tt => (tt.MetaInfo.isDeleted == false) && (tt.TaskRole == TaskRole.Resource) && (tt.AssignedUser.Id == PersonId) && (tt.Task.Status != TaskStatus.completed))).ToList<TaskAssignment>();
                                plist = (from pl in blist select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(f => f.CommunityName).ThenBy(f => f.Deadline).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                break;

                            case Sorting.AlphabeticalOrder:
                                blist = (GetTaskAssignmentGenericQuery(tt => (tt.MetaInfo.isDeleted == false) && (tt.TaskRole == TaskRole.Resource) && (tt.AssignedUser.Id == PersonId) && (tt.Task.Status != TaskStatus.completed))).ToList<TaskAssignment>();
                                plist = (from pl in blist select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(f => f.CommunityName).ThenBy(f => f.Name).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                break;
                        }
                        break;
                }

                olist = (from ta in plist
                         group ta by ta.CommunityId into g
                         select new dtoAssignedTasksWithCommunityHeader(GetCommunityByID(g.Key),
                             (from tt in g select new dtoAssignedTasks(tt, GetPermissionsOverTask(tt.ID, PersonId))).ToList<dtoAssignedTasks>()
                               )).ToList<dtoAssignedTasksWithCommunityHeader>();


                tx.Commit();
            }
            catch (Exception ex)
            {

            }
            return olist;
        }
        //--------------------------------------------------------------------------------------------------------------
        public IList<dtoAssignedTasksWithCommunityHeader> GetPagedAssignedTasksByCommunityAllCommunities(int PersonId, int PageSize, int PageSkip, TaskFilter Filter, int CommunityId, Sorting oSorting)
        {
            List<dtoAssignedTasksWithCommunityHeader> olist = new List<dtoAssignedTasksWithCommunityHeader>();
            IList<TaskAssignment> plist = new List<TaskAssignment>();
            ITransaction tx = session.BeginTransaction();
            try
            {
                //var p = (GetTaskAssignmentGenericQuery(tt => (tt.MetaInfo.isDeleted == false) && (tt.TaskRole == TaskRole.Resource) && (tt.AssignedUser.Id == PersonId) && (tt.Task.Status != TaskStatus.completed)));

                switch (oSorting)
                {
                    case Sorting.DeadlineOrder:
                        plist = (GetTaskAssignmentGenericQuery(tt => (tt.MetaInfo.isDeleted == false) && (tt.TaskRole == TaskRole.Resource) && (tt.AssignedUser.Id == PersonId) && (tt.Task.Status != TaskStatus.completed))).OrderBy(tt => tt.Task.Community.Name).ThenBy(tt => tt.Task.Deadline).Skip(PageSkip).Take(PageSize).ToList<TaskAssignment>();

                        break;

                    case Sorting.AlphabeticalOrder:
                        plist = (GetTaskAssignmentGenericQuery(tt => (tt.MetaInfo.isDeleted == false) && (tt.TaskRole == TaskRole.Resource) && (tt.AssignedUser.Id == PersonId) && (tt.Task.Status != TaskStatus.completed))).OrderBy(tt => tt.Task.Community.Name).ThenBy(tt => tt.Task.Name).Skip(PageSkip).Take(PageSize).ToList<TaskAssignment>();

                        break;
                }
                //trovo una lista di tutti gli assegnamenti dell utente con qualsiasi ruolo per quel task / progetto a cui appartiene il task:
                //IList<TaskAssignment> pr = GetTaskAssignmentGenericQuery(tt => (tt.MetaInfo.isDeleted == false) && (tt.AssignedUser.Id == PersonId)).OrderBy(tt => tt.Task.Community.Name).OrderBy(tt => tt.Task.Deadline).Skip(PageSkip).Take(PageSize).ToList<TaskAssignment>();
                // plist=p.ToList<TaskAssignment>(); 

                olist = (from ta in plist
                         group ta by ta.Task.Community into g
                         select new dtoAssignedTasksWithCommunityHeader(g.Key,
                             (from tt in g select new dtoAssignedTasks(tt.Task, GetPermissionsOverTask(tt.Task.ID, PersonId))).ToList<dtoAssignedTasks>()
                               )).ToList<dtoAssignedTasksWithCommunityHeader>();
                tx.Commit();
            }
            catch (Exception ex)
            {

            }
            return olist;
        }
        //--------------------------------------------------------------------------------------------------------------
        public IList<dtoAssignedTasksWithCommunityHeader> GetPagedAssignedTasksByCommunityCurrentCommunity(int PersonId, int PageSize, int PageSkip, TaskFilter Filter, int CommunityId, Sorting oSorting)
        {
            List<dtoAssignedTasksWithCommunityHeader> olist = new List<dtoAssignedTasksWithCommunityHeader>();
            ITransaction tx = session.BeginTransaction();
            try
            {
                var p = (GetTaskAssignmentGenericQuery(tt => (tt.MetaInfo.isDeleted == false) && (tt.TaskRole == TaskRole.Resource) && (tt.AssignedUser.Id == PersonId) && (tt.Task.Community.Id == CommunityId) && (tt.Task.Status != TaskStatus.completed)));
                //IList<TaskAssignment> p = GetTaskAssignmentGenericQuery(tt => (tt.MetaInfo.isDeleted == false) && (tt.TaskRole == TaskRole.Resource) && (tt.AssignedUser.Id == PersonId) && (tt.Task.Community.Id == CommunityId) && (tt.Task.Status != TaskStatus.completed)).OrderBy(tt => tt.Task.Community.Name).OrderBy(tt => tt.Task.Deadline).Skip(PageSkip).Take(PageSize).ToList<TaskAssignment>();
                switch (oSorting)
                {
                    case Sorting.DeadlineOrder:
                        p = p.OrderBy(tt => tt.Task.Community.Name).ThenBy(tt => tt.Task.Deadline).Skip(PageSkip).Take(PageSize);
                        break;

                    case Sorting.AlphabeticalOrder:
                        p = p.OrderBy(tt => tt.Task.Community.Name).ThenBy(tt => tt.Task.Name).Skip(PageSkip).Take(PageSize);
                        break;
                }


                olist = (from ta in p
                         group ta by ta.Task.Community into g
                         select new dtoAssignedTasksWithCommunityHeader(g.Key,
                             (from tt in g select new dtoAssignedTasks(tt.Task, GetPermissionsOverTask(tt.Task.ID, PersonId))).ToList<dtoAssignedTasks>()
                               )).ToList<dtoAssignedTasksWithCommunityHeader>();
                tx.Commit();
            }
            catch (Exception ex)
            {

            }
            return olist;
        }
        //--------------------------------------------------------------------------------------------------------------
        public IList<dtoAssignedTasksWithCommunityHeader> GetPagedAssignedTasksByCommunityCommunityPersonal(int PersonId, int PageSize, int PageSkip, TaskFilter Filter, int CommunityId, Sorting oSorting)
        {
            List<dtoAssignedTasksWithCommunityHeader> olist = new List<dtoAssignedTasksWithCommunityHeader>();
            ITransaction tx = session.BeginTransaction();
            try
            {

                var p = GetTaskAssignmentGenericQuery(tt => (tt.MetaInfo.isDeleted == false) &&
                    (tt.TaskRole == TaskRole.Resource) && (tt.AssignedUser.Id == PersonId) && (tt.Task.Community.Id == CommunityId) && (tt.Task.Status != TaskStatus.completed)
                    && (tt.Task.isPersonal == true));

                switch (oSorting)
                {
                    case Sorting.DeadlineOrder:
                        p = p.OrderBy(tt => tt.Task.Community.Name).ThenBy(tt => tt.Task.Deadline).Skip(PageSkip).Take(PageSize);//.ToList<TaskAssignment>();
                        break;

                    case Sorting.AlphabeticalOrder:
                        p = p.OrderBy(tt => tt.Task.Community.Name).ThenBy(tt => tt.Task.Name).Skip(PageSkip).Take(PageSize);//.ToList<TaskAssignment>();
                        break;
                }
                olist = (from ta in p
                         group ta by ta.Task.Community into g
                         select new dtoAssignedTasksWithCommunityHeader(g.Key,
                             (from tt in g select new dtoAssignedTasks(tt.Task, GetPermissionsOverTask(tt.Task.ID, PersonId))).ToList<dtoAssignedTasks>()
                               )).ToList<dtoAssignedTasksWithCommunityHeader>();
                tx.Commit();
            }
            catch (Exception ex)
            {

            }
            return olist;
        }
        //--------------------------------------------------------------------------------------------------------------
        //IsPortal = true x definire un task di portale e anche ID di communita = null e isPersonal serve. 
        public IList<dtoAssignedTasksWithCommunityHeader> GetPagedAssignedTasksByCommunityPortalPersonal(int PersonId, int PageSize, int PageSkip, TaskFilter Filter, int CommunityId, Sorting oSorting)
        {
            List<dtoAssignedTasksWithCommunityHeader> olist = new List<dtoAssignedTasksWithCommunityHeader>();
            ITransaction tx = session.BeginTransaction();
            try
            {
                var p = GetTaskAssignmentGenericQuery(tt => (tt.MetaInfo.isDeleted == false) && (tt.TaskRole == TaskRole.Resource) && (tt.AssignedUser.Id == PersonId) && (tt.Task.Community.Id == null) && (tt.Task.isPortal == true) && (tt.Task.isPersonal == true) && (tt.Task.Status != TaskStatus.completed));

                switch (oSorting)
                {
                    case Sorting.DeadlineOrder:
                        p = p.OrderBy(tt => tt.Task.Community.Name).ThenBy(tt => tt.Task.Deadline).Skip(PageSkip).Take(PageSize);
                        break;

                    case Sorting.AlphabeticalOrder:
                        p = p.OrderBy(tt => tt.Task.Community.Name).ThenBy(tt => tt.Task.Name).Skip(PageSkip).Take(PageSize);
                        break;
                }
                //IList<TaskAssignment> p = GetTaskAssignmentGenericQuery(tt => (tt.MetaInfo.isDeleted == false) && (tt.TaskRole == TaskRole.Resource) && (tt.AssignedUser.Id == PersonId) && (tt.Task.Community.Id == null) && (tt.Task.isPortal == true) && (tt.Task.isPersonal == true) && (tt.Task.Status != TaskStatus.completed)).OrderBy(tt => tt.Task.Community.Name).OrderBy(tt => tt.Task.Deadline).Skip(PageSkip).Take(PageSize).ToList<TaskAssignment>();

                // var q = p.GroupBy(tt=> tt.Task.Community,tt=> tt.Task ).AsQueryable();
                olist = (from ta in p
                         group ta by ta.Task.Community into g
                         select new dtoAssignedTasksWithCommunityHeader(g.Key,
                             (from tt in g select new dtoAssignedTasks(tt.Task, GetPermissionsOverTask(tt.Task.ID, PersonId))).ToList<dtoAssignedTasks>()
                               )).ToList<dtoAssignedTasksWithCommunityHeader>();
                tx.Commit();
            }
            catch (Exception ex)
            {

            }
            return olist;
        }
        //--------------------------------------------------------------------------------------------------------------
        public IList<dtoAssignedTasksWithCommunityHeader> GetPagedAssignedTasksByCommunityAllCommunitiesPersonal(int PersonId, int PageSize, int PageSkip, TaskFilter Filter, int CommunityId, Sorting oSorting)
        {
            List<dtoAssignedTasksWithCommunityHeader> olist = new List<dtoAssignedTasksWithCommunityHeader>();
            ITransaction tx = session.BeginTransaction();
            try
            {

                var p = GetTaskAssignmentGenericQuery(tt => (tt.MetaInfo.isDeleted == false) && (tt.TaskRole == TaskRole.Resource) && (tt.AssignedUser.Id == PersonId) && (tt.Task.isPersonal == true) && (tt.Task.Status != TaskStatus.completed));
                //trovo una lista di tutti gli assegnamenti dell utente con qualsiasi ruolo per quel task / progetto a cui appartiene il task:
                //IList<TaskAssignment> pr = GetTaskAssignmentGenericQuery(tt => (tt.MetaInfo.isDeleted == false) && (tt.AssignedUser.Id == PersonId)).OrderBy(tt => tt.Task.Community.Name).OrderBy(tt => tt.Task.EndDate).Skip(PageSkip).Take(PageSize).ToList<TaskAssignment>();

                switch (oSorting)
                {
                    case Sorting.DeadlineOrder:
                        p = p.OrderBy(tt => tt.Task.Community.Name).ThenBy(tt => tt.Task.Deadline).Skip(PageSkip).Take(PageSize);
                        break;

                    case Sorting.AlphabeticalOrder:
                        p = p.OrderBy(tt => tt.Task.Community.Name).ThenBy(tt => tt.Task.Name).Skip(PageSkip).Take(PageSize);
                        break;
                }

                olist = (from ta in p
                         group ta by ta.Task.Community into g
                         select new dtoAssignedTasksWithCommunityHeader(g.Key,
                             (from tt in g select new dtoAssignedTasks(tt.Task, GetPermissionsOverTask(tt.Task.ID, PersonId))).ToList<dtoAssignedTasks>()
                               )).ToList<dtoAssignedTasksWithCommunityHeader>();
                tx.Commit();
            }
            catch (Exception ex)
            {

            }
            return olist;

        }

        #endregion "Tasks filtered By Community Ordered By..."
        //--------------------------------------------------------------------------------------------------------------

        #region "Tasks filtered by Project Ordered By..."
        //PROTOTIPO : Metodo che utilizza l' ID di una persona per recuperare tutti i Tasks IN CORSO , ossia dove la data odierna è all interno della durata del task, in cui ricopre il ruolo "Risorsa"
        public IList<dtoAssignedTasksWithProjectHeader> ProtoTypeGetPagedAssignedTasksByProject(int PersonId, int PageSize, int PageSkip, TaskFilter Filter, int CommunityId, Sorting oSorting)
        {
            List<dtoAssignedTasksWithProjectHeader> olist = new List<dtoAssignedTasksWithProjectHeader>();
            ITransaction tx = session.BeginTransaction();
            try
            {
                //DateTime NowDate = DateTime.Now;
                IList<TaskAssignment> p = GetTaskAssignmentGenericQuery(tt => (tt.MetaInfo.isDeleted == false) && (tt.TaskRole == TaskRole.Resource) && (tt.AssignedUser.Id == PersonId)).OrderBy(tt => tt.Task.Project.ID).OrderBy(tt => tt.Task.Deadline).Skip(PageSkip).Take(PageSize).ToList<TaskAssignment>();

                olist = (from ta in p
                         group ta by ta.Task.Project into g
                         select new dtoAssignedTasksWithProjectHeader(g.Key,
                             (from tt in g select new dtoAssignedTasks(tt.Task, GetPermissionsOverTask(tt.Task.ID, PersonId))).ToList<dtoAssignedTasks>()
                              )).ToList<dtoAssignedTasksWithProjectHeader>();
                tx.Commit();
            }
            catch (Exception ex)
            {

            }
            return olist;
        }
        //--------------------------------------------------------------------------------------------------------------

        //Metodo che restituisce una lista differente di Task in base al filtro applicato (AllCommunities,Portal,CurrentCommunity,AllCommunitiesPersonal e CommunityPersonal)
        public IList<dtoAssignedTasksWithProjectHeader> GetPagedAssignedTasksByProject(int PersonId, int PageSize, int PageSkip, TaskFilter Filter, int CommunityId, Sorting oSorting, String portalName)
        {
            List<dtoAssignedTasksWithProjectHeader> olist = new List<dtoAssignedTasksWithProjectHeader>();
            IList<dtoTaskWithPortalComm> plist = new List<dtoTaskWithPortalComm>();
            IList<TaskAssignment> blist = new List<TaskAssignment>();
            ITransaction tx = session.BeginTransaction();
            try
            {
                switch (Filter)
                {
                    case TaskFilter.AllCommunities:
                        switch (oSorting)
                        {
                            case Sorting.DeadlineOrder:
                                blist = (GetTaskAssignmentGenericQuery(tt => (tt.MetaInfo.isDeleted == false) && (tt.TaskRole == TaskRole.Resource) && (tt.AssignedUser.Id == PersonId) && (tt.Task.Status != TaskStatus.completed))).ToList<TaskAssignment>();
                                plist = (from pl in blist select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(f => f.Deadline).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                //(f => f.ProjectName).ThenBy
                                break;

                            case Sorting.AlphabeticalOrder:
                                blist = (GetTaskAssignmentGenericQuery(tt => (tt.MetaInfo.isDeleted == false) && (tt.TaskRole == TaskRole.Resource) && (tt.AssignedUser.Id == PersonId) && (tt.Task.Status != TaskStatus.completed))).ToList<TaskAssignment>();
                                plist = (from pl in blist select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(f => f.ProjectName).ThenBy(f => f.Name).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();   //.OrderBy(tt => tt.Task.Project.Name).ThenBy(tt => tt.Task.Name).Skip(PageSkip).Take(PageSize).ToList<TaskAssignment>();
                                break;

                            default:
                                blist = (GetTaskAssignmentGenericQuery(tt => (tt.MetaInfo.isDeleted == false) && (tt.TaskRole == TaskRole.Resource) && (tt.AssignedUser.Id == PersonId) && (tt.Task.Status != TaskStatus.completed))).ToList<TaskAssignment>();
                                plist = (from pl in blist select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(f => f.Deadline).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                break;
                        }
                        break;

                    case TaskFilter.PortalPersonal:
                        switch (oSorting)
                        {
                            case Sorting.DeadlineOrder:
                                blist = (GetTaskAssignmentGenericQuery(tt => (tt.MetaInfo.isDeleted == false) && (tt.TaskRole == TaskRole.Resource) && (tt.AssignedUser.Id == PersonId) && (tt.Task.Community.Id == null) && (tt.Task.isPortal == true) && (tt.Task.isPersonal == true) && (tt.Task.Status != TaskStatus.completed))).ToList<TaskAssignment>();//.OrderBy(tt => tt.Task.Project.Deadline).ThenBy(tt => tt.Task.Deadline).Skip(PageSkip).Take(PageSize).ToList<TaskAssignment>();
                                plist = (from pl in blist select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(f => f.Deadline).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();

                                break;

                            case Sorting.AlphabeticalOrder:
                                blist = GetTaskAssignmentGenericQuery(tt => (tt.MetaInfo.isDeleted == false) && (tt.TaskRole == TaskRole.Resource) && (tt.AssignedUser.Id == PersonId) && (tt.Task.Community.Id == null) && (tt.Task.isPortal == true) && (tt.Task.isPersonal == true) && (tt.Task.Status != TaskStatus.completed)).ToList<TaskAssignment>();//.OrderBy(tt => tt.Task.Project.Name).ThenBy(tt => tt.Task.Name).Skip(PageSkip).Take(PageSize).ToList<TaskAssignment>();
                                plist = (from pl in blist select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(f => f.ProjectName).ThenBy(f => f.Name).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();   //.OrderBy(tt => tt.Task.Project.Name).ThenBy(tt => tt.Task.Name).Skip(PageSkip).Take(PageSize).ToList<TaskAssignment>();
                                break;

                            default:
                                blist = (GetTaskAssignmentGenericQuery(tt => (tt.MetaInfo.isDeleted == false) && (tt.TaskRole == TaskRole.Resource) && (tt.AssignedUser.Id == PersonId) && (tt.Task.Community.Id == null) && (tt.Task.isPortal == true) && (tt.Task.isPersonal == true) && (tt.Task.Status != TaskStatus.completed))).ToList<TaskAssignment>();//.OrderBy(tt => tt.Task.Project.Deadline).ThenBy(tt => tt.Task.Deadline).Skip(PageSkip).Take(PageSize).ToList<TaskAssignment>();
                                plist = (from pl in blist select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(f => f.Deadline).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                break;
                        }
                        break;

                    case TaskFilter.CommunityPersonal:
                        switch (oSorting)
                        {
                            case Sorting.DeadlineOrder:
                                blist = (GetTaskAssignmentGenericQuery(tt => (tt.MetaInfo.isDeleted == false) &&
                                        (tt.TaskRole == TaskRole.Resource) && (tt.AssignedUser.Id == PersonId) && (tt.Task.Community.Id == CommunityId) && (tt.Task.Status != TaskStatus.completed)
                                        && (tt.Task.isPersonal == true))).ToList<TaskAssignment>();
                                plist = (from pl in blist select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(f => f.Deadline).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();   //.OrderBy(tt => tt.Task.Project.Name).ThenBy(tt => tt.Task.Name).Skip(PageSkip).Take(PageSize).ToList<TaskAssignment>();
                                break;

                            case Sorting.AlphabeticalOrder:
                                blist = (GetTaskAssignmentGenericQuery(tt => (tt.MetaInfo.isDeleted == false) &&
                                        (tt.TaskRole == TaskRole.Resource) && (tt.AssignedUser.Id == PersonId) && (tt.Task.Community.Id == CommunityId) && (tt.Task.Status != TaskStatus.completed)
                                        && (tt.Task.isPersonal == true))).ToList<TaskAssignment>();
                                plist = (from pl in blist select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(f => f.ProjectName).ThenBy(f => f.Name).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();   //.OrderBy(tt => tt.Task.Project.Name).ThenBy(tt => tt.Task.Name).Skip(PageSkip).Take(PageSize).ToList<TaskAssignment>();
                                break;
                        }
                        break;

                    case TaskFilter.CurrentCommunity:
                        switch (oSorting)
                        {
                            case Sorting.DeadlineOrder:
                                blist = (GetTaskAssignmentGenericQuery(tt => (tt.MetaInfo.isDeleted == false) && (tt.TaskRole == TaskRole.Resource) && (tt.AssignedUser.Id == PersonId)
                                         && (tt.Task.Community.Id == CommunityId) && (tt.Task.Status != TaskStatus.completed)).ToList<TaskAssignment>());
                                plist = (from pl in blist select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(f => f.Deadline).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();   //.OrderBy(tt => tt.Task.Project.Name).ThenBy(tt => tt.Task.Name).Skip(PageSkip).Take(PageSize).ToList<TaskAssignment>();
                                break;

                            case Sorting.AlphabeticalOrder:
                                blist = (GetTaskAssignmentGenericQuery(tt => (tt.MetaInfo.isDeleted == false) && (tt.TaskRole == TaskRole.Resource) && (tt.AssignedUser.Id == PersonId) && (tt.Task.Community.Id == CommunityId)
                                    && (tt.Task.Status != TaskStatus.completed))).ToList<TaskAssignment>();
                                plist = (from pl in blist select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(f => f.ProjectName).ThenBy(f => f.Name).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();   //.OrderBy(tt => tt.Task.Project.Name).ThenBy(tt => tt.Task.Name).Skip(PageSkip).Take(PageSize).ToList<TaskAssignment>();
                                break;
                        }
                        break;

                    case TaskFilter.AllCommunitiesPersonal:
                        switch (oSorting)
                        {
                            case Sorting.DeadlineOrder:
                                blist = (GetTaskAssignmentGenericQuery(tt => (tt.MetaInfo.isDeleted == false) && (tt.TaskRole == TaskRole.Resource) && (tt.AssignedUser.Id == PersonId) && (tt.Task.isPersonal == true)
                                    && (tt.Task.Status != TaskStatus.completed))).ToList<TaskAssignment>();
                                plist = (from pl in blist select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(f => f.Deadline).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();   //.OrderBy(tt => tt.Task.Project.Name).ThenBy(tt => tt.Task.Name).Skip(PageSkip).Take(PageSize).ToList<TaskAssignment>();
                                break;

                            case Sorting.AlphabeticalOrder:
                                blist = (GetTaskAssignmentGenericQuery(tt => (tt.MetaInfo.isDeleted == false) && (tt.TaskRole == TaskRole.Resource) && (tt.AssignedUser.Id == PersonId) && (tt.Task.isPersonal == true) && (tt.Task.Status != TaskStatus.completed))).ToList<TaskAssignment>();
                                plist = (from pl in blist select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(f => f.ProjectName).ThenBy(f => f.Name).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();   //.OrderBy(tt => tt.Task.Project.Name).ThenBy(tt => tt.Task.Name).Skip(PageSkip).Take(PageSize).ToList<TaskAssignment>();
                                break;
                        }
                        break;

                    case TaskFilter.None:
                        switch (oSorting)
                        {
                            case Sorting.DeadlineOrder:
                                blist = (GetTaskAssignmentGenericQuery(tt => (tt.MetaInfo.isDeleted == false) && (tt.TaskRole == TaskRole.Resource) && (tt.AssignedUser.Id == PersonId) && (tt.Task.Status != TaskStatus.completed))).ToList<TaskAssignment>();
                                plist = (from pl in blist select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(f => f.Deadline).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                break;

                            case Sorting.AlphabeticalOrder:
                                blist = (GetTaskAssignmentGenericQuery(tt => (tt.MetaInfo.isDeleted == false) && (tt.TaskRole == TaskRole.Resource) && (tt.AssignedUser.Id == PersonId) && (tt.Task.Status != TaskStatus.completed))).ToList<TaskAssignment>();
                                plist = (from pl in blist select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(f => f.ProjectName).ThenBy(f => f.Name).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();   //.OrderBy(tt => tt.Task.Project.Name).ThenBy(tt => tt.Task.Name).Skip(PageSkip).Take(PageSize).ToList<TaskAssignment>();
                                break;

                            default:
                                blist = (GetTaskAssignmentGenericQuery(tt => (tt.MetaInfo.isDeleted == false) && (tt.TaskRole == TaskRole.Resource) && (tt.AssignedUser.Id == PersonId) && (tt.Task.Status != TaskStatus.completed))).ToList<TaskAssignment>();
                                plist = (from pl in blist select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(f => f.Deadline).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                break;
                        }
                        break;

                    default:
                        switch (oSorting)
                        {
                            case Sorting.DeadlineOrder:
                                blist = (GetTaskAssignmentGenericQuery(tt => (tt.MetaInfo.isDeleted == false) && (tt.TaskRole == TaskRole.Resource) && (tt.AssignedUser.Id == PersonId) && (tt.Task.Status != TaskStatus.completed))).ToList<TaskAssignment>();
                                plist = (from pl in blist select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(f => f.Deadline).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                break;

                            case Sorting.AlphabeticalOrder:
                                blist = (GetTaskAssignmentGenericQuery(tt => (tt.MetaInfo.isDeleted == false) && (tt.TaskRole == TaskRole.Resource) && (tt.AssignedUser.Id == PersonId) && (tt.Task.Status != TaskStatus.completed))).ToList<TaskAssignment>();
                                plist = (from pl in blist select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(f => f.ProjectName).ThenBy(f => f.Name).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();   //.OrderBy(tt => tt.Task.Project.Name).ThenBy(tt => tt.Task.Name).Skip(PageSkip).Take(PageSize).ToList<TaskAssignment>();
                                break;

                            default:
                                blist = (GetTaskAssignmentGenericQuery(tt => (tt.MetaInfo.isDeleted == false) && (tt.TaskRole == TaskRole.Resource) && (tt.AssignedUser.Id == PersonId) && (tt.Task.Status != TaskStatus.completed))).ToList<TaskAssignment>();
                                plist = (from pl in blist select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(f => f.Deadline).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                break;
                        }
                        break;
                }

                olist = (from ta in plist
                         group ta by ta.ProjectID into g
                         select new dtoAssignedTasksWithProjectHeader(GetTask(g.Key),
                             (from tt in g select new dtoAssignedTasks(tt, GetPermissionsOverTask(tt.ID, PersonId))).ToList<dtoAssignedTasks>()
                              )).ToList<dtoAssignedTasksWithProjectHeader>();


                tx.Commit();
            }
            catch (Exception ex)
            {

            }
            return olist;
        }

        //--------------------------------------------------------------------------------------------------------------
        //Metodo che restituisce la lista di Task assegnati per tutte le comunita
        public IList<dtoAssignedTasksWithProjectHeader> GetPagedAssignedTasksByProjectAllCommunities(int PersonId, int PageSize, int PageSkip, TaskFilter Filter, int CommunityId, Sorting oSorting)
        {
            List<dtoAssignedTasksWithProjectHeader> olist = new List<dtoAssignedTasksWithProjectHeader>();

            ITransaction tx = session.BeginTransaction();
            try
            {
                var p = GetTaskAssignmentGenericQuery(tt => (tt.MetaInfo.isDeleted == false) && (tt.TaskRole == TaskRole.Resource) && (tt.AssignedUser.Id == PersonId) && (tt.Task.Status != TaskStatus.completed));

                switch (oSorting)
                {
                    case Sorting.DeadlineOrder:
                        p = p.OrderBy(tt => tt.Task.Project.Deadline).ThenBy(tt => tt.Task.Deadline).Skip(PageSkip).Take(PageSize);
                        break;

                    case Sorting.AlphabeticalOrder:
                        p = p.OrderBy(tt => tt.Task.Project.Name).ThenBy(tt => tt.Task.Name).Skip(PageSkip).Take(PageSize);
                        break;
                }

                olist = (from ta in p
                         group ta by ta.Project into g
                         select new dtoAssignedTasksWithProjectHeader(g.Key,
                             (from tt in g select new dtoAssignedTasks(tt.Task, GetPermissionsOverTask(tt.Task.ID, PersonId))).ToList<dtoAssignedTasks>()
                              )).ToList<dtoAssignedTasksWithProjectHeader>();
                tx.Commit();
            }
            catch (Exception ex)
            {

            }
            return olist;
        }
        //--------------------------------------------------------------------------------------------------------------
        //Metodo che restituisce la lista di Task assegnati per la comunita corrente
        public IList<dtoAssignedTasksWithProjectHeader> GetPagedAssignedTasksByProjectCurrentCommunity(int PersonId, int PageSize, int PageSkip, TaskFilter Filter, int CommunityId, Sorting oSorting)
        {
            List<dtoAssignedTasksWithProjectHeader> olist = new List<dtoAssignedTasksWithProjectHeader>();
            ITransaction tx = session.BeginTransaction();
            try
            {
                var p = GetTaskAssignmentGenericQuery(tt => (tt.MetaInfo.isDeleted == false) && (tt.TaskRole == TaskRole.Resource) && (tt.AssignedUser.Id == PersonId) && (tt.Task.Community.Id == CommunityId) && (tt.Task.Status != TaskStatus.completed));

                switch (oSorting)
                {
                    case Sorting.DeadlineOrder:
                        p = p.OrderBy(tt => tt.Task.Project.Deadline).ThenBy(tt => tt.Task.Deadline).Skip(PageSkip).Take(PageSize);
                        break;

                    case Sorting.AlphabeticalOrder:
                        p = p.OrderBy(tt => tt.Task.Project.Name).ThenBy(tt => tt.Task.Name).Skip(PageSkip).Take(PageSize);
                        break;
                }
                olist = (from ta in p
                         group ta by ta.Task.Project into g
                         select new dtoAssignedTasksWithProjectHeader(g.Key,
                             (from tt in g select new dtoAssignedTasks(tt.Task, GetPermissionsOverTask(tt.Task.ID, PersonId))).ToList<dtoAssignedTasks>()
                              )).ToList<dtoAssignedTasksWithProjectHeader>();
                tx.Commit();
            }
            catch (Exception ex)
            {

            }
            return olist;
        }
        //--------------------------------------------------------------------------------------------------------------
        //Metodo che restituisce la lista di Task assegnati per tutte le comunita
        public IList<dtoAssignedTasksWithProjectHeader> GetPagedAssignedTasksByProjectCommunityPersonal(int PersonId, int PageSize, int PageSkip, TaskFilter Filter, int CommunityId, Sorting oSorting)
        {
            List<dtoAssignedTasksWithProjectHeader> olist = new List<dtoAssignedTasksWithProjectHeader>();
            ITransaction tx = session.BeginTransaction();
            try
            {
                var p = GetTaskAssignmentGenericQuery(tt => (tt.MetaInfo.isDeleted == false) && (tt.TaskRole == TaskRole.Resource)
                    && (tt.AssignedUser.Id == PersonId) && (tt.Task.Community.Id == CommunityId) && (tt.Task.isPersonal == true) && (tt.Task.Status != TaskStatus.completed));

                switch (oSorting)
                {
                    case Sorting.DeadlineOrder:
                        p = p.OrderBy(tt => tt.Task.Project.Deadline).ThenBy(tt => tt.Task.Deadline).Skip(PageSkip).Take(PageSize);
                        break;

                    case Sorting.AlphabeticalOrder:
                        p = p.OrderBy(tt => tt.Task.Project.Name).ThenBy(tt => tt.Task.Name).Skip(PageSkip).Take(PageSize);
                        break;
                }

                olist = (from ta in p
                         group ta by ta.Task.Project into g
                         select new dtoAssignedTasksWithProjectHeader(g.Key,
                             (from tt in g select new dtoAssignedTasks(tt.Task, GetPermissionsOverTask(tt.Task.ID, PersonId))).ToList<dtoAssignedTasks>()
                              )).ToList<dtoAssignedTasksWithProjectHeader>();
                tx.Commit();
            }
            catch (Exception ex)
            {

            }
            return olist;
        }
        //--------------------------------------------------------------------------------------------------------------      
        //IsPortal = true x definire un task di portale e anche ID di communita = null e isPersonal = true serve. 
        public IList<dtoAssignedTasksWithProjectHeader> GetPagedAssignedTasksByProjectPortalPersonal(int PersonId, int PageSize, int PageSkip, TaskFilter Filter, int CommunityId, Sorting oSorting)
        {
            List<dtoAssignedTasksWithProjectHeader> olist = new List<dtoAssignedTasksWithProjectHeader>();
            ITransaction tx = session.BeginTransaction();
            try
            {
                var p = GetTaskAssignmentGenericQuery(tt => (tt.MetaInfo.isDeleted == false) && (tt.TaskRole == TaskRole.Resource) && (tt.AssignedUser.Id == PersonId) && (tt.Task.isPersonal == true) && (tt.Task.isPortal == true) && (tt.Task.Status != TaskStatus.completed));


                switch (oSorting)
                {
                    case Sorting.DeadlineOrder:
                        p = p.OrderBy(tt => tt.Task.Project.Deadline).ThenBy(tt => tt.Task.Deadline).Skip(PageSkip).Take(PageSize);
                        break;

                    case Sorting.AlphabeticalOrder:
                        p = p.OrderBy(tt => tt.Task.Project.Name).ThenBy(tt => tt.Task.Name).Skip(PageSkip).Take(PageSize);
                        break;

                }
                olist = (from ta in p
                         group ta by ta.Task.Project into g
                         select new dtoAssignedTasksWithProjectHeader(g.Key,
                             (from tt in g select new dtoAssignedTasks(tt.Task, GetPermissionsOverTask(tt.Task.ID, PersonId))).ToList<dtoAssignedTasks>()
                              )).ToList<dtoAssignedTasksWithProjectHeader>();
                tx.Commit();
            }
            catch (Exception ex)
            {

            }
            return olist;
        }
        //--------------------------------------------------------------------------------------------------------------

        public IList<dtoAssignedTasksWithProjectHeader> GetPagedAssignedTasksByProjectAllCommunitiesPersonal(int PersonId, int PageSize, int PageSkip, TaskFilter Filter, int CommunityId, Sorting oSorting)
        {
            List<dtoAssignedTasksWithProjectHeader> olist = new List<dtoAssignedTasksWithProjectHeader>();
            IList<TaskAssignment> plist = new List<TaskAssignment>();
            ITransaction tx = session.BeginTransaction();
            try
            {
                var p = GetTaskAssignmentGenericQuery(tt => (tt.MetaInfo.isDeleted == false) && (tt.TaskRole == TaskRole.Resource) && (tt.AssignedUser.Id == PersonId) && (tt.Task.isPersonal == true) && (tt.Task.Status != TaskStatus.completed));

                switch (oSorting)
                {
                    case Sorting.DeadlineOrder:
                        plist = p.OrderBy(tt => tt.Task.Project.Deadline).ThenBy(tt => tt.Task.Deadline).Skip(PageSkip).Take(PageSize).ToList<TaskAssignment>();
                        break;

                    case Sorting.AlphabeticalOrder:
                        plist = p.OrderBy(tt => tt.Task.Project.Name).ThenBy(tt => tt.Task.Name).Skip(PageSkip).Take(PageSize).ToList<TaskAssignment>();
                        break;
                }

                olist = (from ta in plist
                         group ta by ta.Task.Project into g
                         select new dtoAssignedTasksWithProjectHeader(g.Key,
                             (from tt in g select new dtoAssignedTasks(tt.Task, GetPermissionsOverTask(tt.Task.ID, PersonId))).ToList<dtoAssignedTasks>()
                              )).ToList<dtoAssignedTasksWithProjectHeader>();
                tx.Commit();
            }
            catch (Exception ex)
            {

            }
            return olist;
        }

        //--------------------------------------------------------------------------------------------------------------
        #endregion "Tasks filtered by Project Ordered By..."

        #endregion "AssignedTasksPage"


        #region "InvolvingProjects"

        //OK: restituisce TUTTI i Projects in cui si è coinvolti paginati, secondo i Task filtri( AllCommunities,Personal,PortalPersonal,etc) e Project filtri(attivi,completati,futuri)
        public int GetInvolvingProjectsCount(int PersonId, TaskFilter Filter, int CommunityId, ProjectOrderBy PFilter)
        {
            IList<TaskAssignment> list = null;
            IList<Task> listP = null;
            int taskcount = 0;
            ITransaction tx = session.BeginTransaction();
            try
            {

                switch (Filter)
                {
                    case TaskFilter.AllCommunities:
                        list = GetTaskAssignmentGeneric(tt => (
                            (PFilter == ProjectOrderBy.AllCompleted && tt.Project.Status == TaskStatus.completed)
                            ||
                            (PFilter == ProjectOrderBy.AllActive && (tt.Project.Status != TaskStatus.completed) && (tt.Project.StartDate <= DateTime.Now)) // && (tt.Project.Deadline >= DateTime.Now))
                            ||
                            (PFilter == ProjectOrderBy.AllFuture && tt.Project.Status != TaskStatus.completed && tt.Project.StartDate > DateTime.Now) // && tt.Project.Deadline > DateTime.Now)
                                                    )
                            && (tt.MetaInfo.isDeleted == false) && (tt.AssignedUser.Id == PersonId));

                        break;

                    case TaskFilter.CommunityPersonal:
                        list = GetTaskAssignmentGeneric(tt =>
                            ((PFilter == ProjectOrderBy.AllCompleted) && (tt.Project.Status == TaskStatus.completed))
                            ||
                            ((PFilter == ProjectOrderBy.AllActive) && (tt.Project.Status != TaskStatus.completed) && (tt.Project.StartDate <= DateTime.Now)) //&& tt.Project.Deadline >= DateTime.Now)
                            ||
                            ((PFilter == ProjectOrderBy.AllFuture) && (tt.Project.Status != TaskStatus.completed) && (tt.Project.StartDate > DateTime.Now)) //&& tt.Project.Deadline > DateTime.Now))

                            && (tt.MetaInfo.isDeleted == false) && (tt.AssignedUser.Id == PersonId) && (tt.Project.Community.Id == CommunityId) && (tt.Project.isPersonal == true));
                        break;

                    case TaskFilter.CurrentCommunity:
                        list = GetTaskAssignmentGeneric(tt =>
                            ((PFilter == ProjectOrderBy.AllCompleted && tt.Project.Status == TaskStatus.completed)
                            ||
                            ((PFilter == ProjectOrderBy.AllActive) && (tt.Project.Status != TaskStatus.completed) && (tt.Project.StartDate <= DateTime.Now)) // && tt.Project.Deadline >= DateTime.Now)
                            ||
                            (PFilter == ProjectOrderBy.AllFuture && tt.Project.Status != TaskStatus.completed && tt.Project.StartDate > DateTime.Now)) // && tt.Project.Deadline > DateTime.Now))

                            && (tt.MetaInfo.isDeleted == false) && (tt.AssignedUser.Id == PersonId) && (tt.Project.Community.Id == CommunityId));
                        break;

                    case TaskFilter.PortalPersonal:
                        list = GetTaskAssignmentGeneric(tt =>
                            ((PFilter == ProjectOrderBy.AllCompleted && tt.Project.Status == TaskStatus.completed)
                            ||
                            (PFilter == ProjectOrderBy.AllActive && tt.Project.Status != TaskStatus.completed && tt.Project.StartDate <= DateTime.Now) // && tt.Project.Deadline >= DateTime.Now)
                            ||
                            (PFilter == ProjectOrderBy.AllFuture && tt.Project.Status != TaskStatus.completed && tt.Project.StartDate > DateTime.Now) // && tt.Project.Deadline > DateTime.Now))

                            && (tt.MetaInfo.isDeleted == false) && (tt.AssignedUser.Id == PersonId) && (tt.Project.Community == null) && (tt.Project.isPersonal == true) && (tt.Project.isPortal == true)));
                        break;

                    case TaskFilter.AllCommunitiesPersonal:
                        list = GetTaskAssignmentGeneric(tt =>
                            ((PFilter == ProjectOrderBy.AllCompleted && tt.Project.Status == TaskStatus.completed)
                            ||
                            (PFilter == ProjectOrderBy.AllActive && tt.Project.Status != TaskStatus.completed && tt.Project.StartDate <= DateTime.Now) // && tt.Project.Deadline >= DateTime.Now)
                            ||
                            ((PFilter == ProjectOrderBy.AllFuture) && (tt.Project.Status != TaskStatus.completed) && (tt.Project.StartDate > DateTime.Now))) // && tt.Project.Deadline > DateTime.Now))

                            && (tt.MetaInfo.isDeleted == false) && (tt.AssignedUser.Id == PersonId) && (tt.Project.isPersonal == true) && (tt.Project.isPortal == false));
                        break;

                    default:
                        list = GetTaskAssignmentGeneric(tt => (
                            (PFilter == ProjectOrderBy.AllCompleted && tt.Project.Status == TaskStatus.completed)
                            ||
                            (PFilter == ProjectOrderBy.AllActive && (tt.Project.Status != TaskStatus.completed) && (tt.Project.StartDate <= DateTime.Now)) // && (tt.Project.Deadline >= DateTime.Now))
                            ||
                            (PFilter == ProjectOrderBy.AllFuture && tt.Project.Status != TaskStatus.completed && tt.Project.StartDate > DateTime.Now) // && tt.Project.Deadline > DateTime.Now)
                                                    )
                            && (tt.MetaInfo.isDeleted == false) && (tt.AssignedUser.Id == PersonId));
                        break;

                }

                listP = (from ta in list select ta.Project).Distinct().ToList<Task>();
                taskcount = listP.Count;

                tx.Commit();
            }
            catch (Exception ex)
            {

            }

            return taskcount;
        }
        //--------------------------------------------------------------------------------------------------------------

        //public class ProjectEqualityComparer : IEqualityComparer<Task>
        //{
        //    public bool Equals(Task x, Task y)
        //    {
        //        if ((x.ID == y.ID))
        //            return true;
        //        else
        //            return false;
        //    }

        //    public int GetHashCode(Task obj)
        //    {

        //        return base.GetHashCode();

        //    }
        //}   


        //OK: Metodo che recupera tutti i TaskAssignment di progetto di una persona. Ne recupera la lista dei progetti e li filtra in base al ProjectOrderBy (attivi,completati e futuri) oltre ai soliti community filters (allcomm, personal, currentComm,etc )  
        public IList<dtoInvolvingProjectsWithRolesWithHeader> GetPagedInvolvingProjects(int PersonId, int CommunityId, int PageSize, int PageSkip, TaskFilter TFilter, ProjectOrderBy PFilter, Sorting oSorting, String portalName)
        {
            List<dtoInvolvingProjectsWithRolesWithHeader> olist = new List<dtoInvolvingProjectsWithRolesWithHeader>();
            IList<TaskAssignment> list = null;
            IList<Task> listP = null;//new List<Task>();
            IList<dtoTaskWithPortalComm> listT = new List<dtoTaskWithPortalComm>();
            //int taskcount = 0;
            ProjectEqualityComparer projectEqualityComparer = new ProjectEqualityComparer();


            ITransaction tx = session.BeginTransaction();
            try
            {
                switch (TFilter)
                {
                    case TaskFilter.AllCommunities:

                        list = GetTaskAssignmentGenericQuery(tt => (
                        (PFilter == ProjectOrderBy.AllCompleted && tt.Project.Status == TaskStatus.completed)
                        ||
                        (PFilter == ProjectOrderBy.AllActive && (tt.Project.Status != TaskStatus.completed) && (tt.Project.StartDate <= DateTime.Now)) //&& (tt.Project.Deadline >= DateTime.Now))
                        ||
                        (PFilter == ProjectOrderBy.AllFuture && tt.Project.Status != TaskStatus.completed && tt.Project.StartDate > DateTime.Now) // && tt.Project.Deadline > DateTime.Now)
                                                )
                        && (tt.MetaInfo.isDeleted == false) && (tt.AssignedUser.Id == PersonId)).ToList<TaskAssignment>();

                        listP = (from tt in list select (tt.Project)).Distinct(projectEqualityComparer).ToList<Task>();
                        //listT = (from p in listP select new dtoTaskWithPortalComm(p)).OrderBy(tt => tt.CommunityName).ToList<dtoTaskWithPortalComm>(); 

                        switch (oSorting)
                        {
                            case Sorting.DeadlineOrder:
                                var q = (from p in listP select new dtoTaskWithPortalComm(p, portalName));
                                listT = (from d in q orderby d.CommunityName, d.Deadline select d).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                break;

                            case Sorting.AlphabeticalOrder:
                                q = (from p in listP select new dtoTaskWithPortalComm(p, portalName));
                                listT = (from d in q orderby d.CommunityName, d.Name select d).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                break;

                            default:
                                q = (from p in listP select new dtoTaskWithPortalComm(p, portalName));
                                listT = (from d in q orderby d.CommunityName, d.Deadline select d).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                break;
                        }

                        break;

                    case TaskFilter.CommunityPersonal:
                        list = GetTaskAssignmentGenericQuery(tt =>
                            ((PFilter == ProjectOrderBy.AllCompleted && tt.Project.Status == TaskStatus.completed)
                            ||
                            (PFilter == ProjectOrderBy.AllActive && tt.Project.Status != TaskStatus.completed && tt.Project.StartDate <= DateTime.Now) // && tt.Project.Deadline >= DateTime.Now)
                            ||
                            (PFilter == ProjectOrderBy.AllFuture && tt.Project.Status != TaskStatus.completed && tt.Project.StartDate > DateTime.Now)) // && tt.Project.Deadline > DateTime.Now))

                            && (tt.MetaInfo.isDeleted == false) && (tt.AssignedUser.Id == PersonId) && (tt.Project.Community.Id == CommunityId) && (tt.Project.isPersonal == true)).ToList<TaskAssignment>();

                        listP = (from tt in list select tt.Project).Distinct(projectEqualityComparer).ToList<Task>();

                        switch (oSorting)
                        {
                            case Sorting.DeadlineOrder:
                                var q = (from p in listP select new dtoTaskWithPortalComm(p, portalName));
                                listT = (from d in q orderby d.CommunityName, d.Deadline select d).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                break;

                            case Sorting.AlphabeticalOrder:
                                q = (from p in listP select new dtoTaskWithPortalComm(p, portalName));
                                listT = (from d in q orderby d.CommunityName, d.ProjectName select d).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                break;

                            default:
                                q = (from p in listP select new dtoTaskWithPortalComm(p, portalName));
                                listT = (from d in q orderby d.CommunityName, d.Deadline select d).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                break;
                        }

                        break;

                    case TaskFilter.CurrentCommunity:
                        list = GetTaskAssignmentGenericQuery(tt =>
                        ((PFilter == ProjectOrderBy.AllCompleted && tt.Project.Status == TaskStatus.completed)
                        ||
                        (PFilter == ProjectOrderBy.AllActive && tt.Project.Status != TaskStatus.completed && tt.Project.StartDate <= DateTime.Now) // && tt.Project.Deadline >= DateTime.Now)
                        ||
                        (PFilter == ProjectOrderBy.AllFuture && tt.Project.Status != TaskStatus.completed && tt.Project.StartDate > DateTime.Now)) // && tt.Project.Deadline > DateTime.Now))

                        && (tt.MetaInfo.isDeleted == false) && (tt.AssignedUser.Id == PersonId) && (tt.Project.Community.Id == CommunityId)).ToList<TaskAssignment>();

                        listP = (from tt in list select tt.Project).Distinct(projectEqualityComparer).ToList<Task>();

                        switch (oSorting)
                        {
                            case Sorting.DeadlineOrder:
                                var q = (from p in listP select new dtoTaskWithPortalComm(p, portalName));
                                listT = (from d in q orderby d.CommunityName, d.Deadline select d).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                break;

                            case Sorting.AlphabeticalOrder:
                                q = (from p in listP select new dtoTaskWithPortalComm(p, portalName));
                                listT = (from d in q orderby d.CommunityName, d.ProjectName select d).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                break;

                            default:
                                q = (from p in listP select new dtoTaskWithPortalComm(p, portalName));
                                listT = (from d in q orderby d.CommunityName, d.Deadline select d).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                break;
                        }

                        break;

                    case TaskFilter.PortalPersonal:
                        list = GetTaskAssignmentGenericQuery(tt =>
                            ((PFilter == ProjectOrderBy.AllCompleted && tt.Project.Status == TaskStatus.completed)
                            ||
                            (PFilter == ProjectOrderBy.AllActive && tt.Project.Status != TaskStatus.completed && tt.Project.StartDate <= DateTime.Now) // && tt.Project.Deadline >= DateTime.Now)
                            ||
                            (PFilter == ProjectOrderBy.AllFuture && tt.Project.Status != TaskStatus.completed && tt.Project.StartDate > DateTime.Now)) // && tt.Project.Deadline > DateTime.Now))

                            && (tt.MetaInfo.isDeleted == false) && (tt.AssignedUser.Id == PersonId) && (tt.Project.Community == null) && (tt.Project.isPersonal == true) && (tt.Project.isPortal == true)).ToList<TaskAssignment>();

                        listP = (from tt in list select tt.Project).Distinct(projectEqualityComparer).ToList<Task>();

                        switch (oSorting)
                        {
                            case Sorting.DeadlineOrder:
                                var q = (from p in listP select new dtoTaskWithPortalComm(p, portalName));
                                listT = (from d in q orderby d.CommunityName, d.Deadline select d).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                break;

                            case Sorting.AlphabeticalOrder:
                                q = (from p in listP select new dtoTaskWithPortalComm(p, portalName));
                                listT = (from d in q orderby d.CommunityName, d.ProjectName select d).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                break;

                            default:
                                q = (from p in listP select new dtoTaskWithPortalComm(p, portalName));
                                listT = (from d in q orderby d.CommunityName, d.Deadline select d).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                break;
                        }

                        break;

                    case TaskFilter.AllCommunitiesPersonal:
                        list = GetTaskAssignmentGenericQuery(tt =>
                            ((PFilter == ProjectOrderBy.AllCompleted && tt.Project.Status == TaskStatus.completed)
                            ||
                            (PFilter == ProjectOrderBy.AllActive && tt.Project.Status != TaskStatus.completed && tt.Project.StartDate <= DateTime.Now) // && tt.Project.Deadline >= DateTime.Now)
                            ||
                            (PFilter == ProjectOrderBy.AllFuture && tt.Project.Status != TaskStatus.completed && tt.Project.StartDate > DateTime.Now))// && tt.Project.Deadline > DateTime.Now))

                            && (tt.MetaInfo.isDeleted == false) && (tt.AssignedUser.Id == PersonId) && (tt.Project.isPersonal == true)).ToList<TaskAssignment>();  //&& (tt.Project.isPortal == false));

                        listP = (from tt in list select tt.Project).Distinct(projectEqualityComparer).ToList<Task>();

                        switch (oSorting)
                        {
                            case Sorting.DeadlineOrder:
                                var q = (from p in listP select new dtoTaskWithPortalComm(p, portalName));
                                listT = (from d in q orderby d.CommunityName, d.Deadline select d).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                break;

                            case Sorting.AlphabeticalOrder:
                                q = (from p in listP select new dtoTaskWithPortalComm(p, portalName));
                                listT = (from d in q orderby d.CommunityName, d.ProjectName select d).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                break;

                            default:
                                q = (from p in listP select new dtoTaskWithPortalComm(p, portalName));
                                listT = (from d in q orderby d.CommunityName, d.Deadline select d).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                break;
                        }
                        break;

                    default:
                        list = GetTaskAssignmentGenericQuery(tt => (
                        (PFilter == ProjectOrderBy.AllCompleted && tt.Project.Status == TaskStatus.completed)
                        ||
                        (PFilter == ProjectOrderBy.AllActive && (tt.Project.Status != TaskStatus.completed) && (tt.Project.StartDate <= DateTime.Now)) // && (tt.Project.Deadline >= DateTime.Now))
                        ||
                        (PFilter == ProjectOrderBy.AllFuture && tt.Project.Status != TaskStatus.completed && tt.Project.StartDate > DateTime.Now) // && tt.Project.Deadline > DateTime.Now)
                                                )
                        && (tt.MetaInfo.isDeleted == false) && (tt.AssignedUser.Id == PersonId)).ToList<TaskAssignment>();

                        listP = (from tt in list select tt.Project).Distinct(projectEqualityComparer).ToList<Task>();

                        switch (oSorting)
                        {
                            case Sorting.DeadlineOrder:
                                var q = (from p in listP select new dtoTaskWithPortalComm(p, portalName));
                                listT = (from d in q orderby d.CommunityName, d.Deadline select d).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                break;

                            case Sorting.AlphabeticalOrder:
                                q = (from p in listP select new dtoTaskWithPortalComm(p, portalName));
                                listT = (from d in q orderby d.CommunityName, d.ProjectName select d).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                break;

                            default:
                                q = (from p in listP select new dtoTaskWithPortalComm(p, portalName));
                                listT = (from d in q orderby d.CommunityName, d.Deadline select d).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                break;
                        }

                        break;
                }

                //listP = (from tc in list select tc.Project).Distinct().Skip(PageSkip).Take(PageSize).ToList<Task>();
                olist = (from tp in listT
                         group tp by tp.CommunityId into g
                         select new dtoInvolvingProjectsWithRolesWithHeader(GetCommunity(g.Key),
                             (from tg in g select new dtoInvolvingProjectsWithRoles(tg, GetPermissionsOverTask(tg.ID, PersonId), GetRolesByProject(tg.ID, PersonId))).ToList<dtoInvolvingProjectsWithRoles>()
                             )).ToList<dtoInvolvingProjectsWithRolesWithHeader>();

                tx.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return olist;

        }


        public IList<TaskRole> GetRolesByProject(long ProjectId, int PersonId)
        {
            long oProjectId = ProjectId;
            IList<TaskRole> olistrole = null;
            IList<TaskAssignment> list = null;

            ITransaction tx = session.BeginTransaction();
            try
            {
                list = GetTaskAssignmentGeneric(tt => ((tt.AssignedUser.Id == PersonId) && (tt.Project.ID == ProjectId)));
                olistrole = (from prj in list select prj.TaskRole).Distinct().ToList<TaskRole>();
                tx.Commit();
            }
            catch (Exception ex)
            { }
            return olistrole;

        }
        #endregion "InvolvingProjects"

        //--------------------------------------------------------------------------------------------------------------

        #region "Tasks Management"
        //OK: Paginazione: Recupera il Count della lista di Project /Task di cui un untente è manager secondo filtri TaskFilter=allcommunities,portalpersonal,personal,etc ; ProjectOrderBy=active,future,completed; TaskType= Tasks o Projects
        public int GetTasksManagementCount(int PersonId, int CommunityId, TaskFilter TFilter, ProjectOrderBy PFilter, TaskManagedType TaskType)
        {
            IList<TaskAssignment> list = null;
            int taskcount = 0;
            ITransaction tx = session.BeginTransaction();
            try
            {
                switch (TaskType)
                {
                    case TaskManagedType.Projects:

                        switch (TFilter)
                        {
                            case TaskFilter.AllCommunities:
                                list = GetTaskAssignmentGeneric(tt => (
                                    ((PFilter == ProjectOrderBy.AllCompleted) && (tt.Task.Status == TaskStatus.completed))
                                    ||
                                    ((PFilter == ProjectOrderBy.AllActive) && (tt.Task.Status != TaskStatus.completed) && (tt.Task.StartDate <= DateTime.Now)) // && (tt.Task.Deadline >= DateTime.Now))
                                    ||
                                    ((PFilter == ProjectOrderBy.AllFuture) && (tt.Task.Status != TaskStatus.completed) && (tt.Task.StartDate > DateTime.Now)) // && (tt.Task.Deadline > DateTime.Now))
                                                            )
                                    && (tt.MetaInfo.isDeleted == false) && (tt.AssignedUser.Id == PersonId) && (tt.TaskRole == TaskRole.Manager || tt.TaskRole == TaskRole.ProjectOwner) && (tt.Task.Level == 0));

                                break;

                            case TaskFilter.CommunityPersonal:
                                list = GetTaskAssignmentGeneric(tt =>
                                    (((PFilter == ProjectOrderBy.AllCompleted) && (tt.Task.Status == TaskStatus.completed))
                                    ||
                                    ((PFilter == ProjectOrderBy.AllActive) && (tt.Task.Status != TaskStatus.completed) && (tt.Task.StartDate <= DateTime.Now)) // && (tt.Task.Deadline >= DateTime.Now))
                                    ||
                                    ((PFilter == ProjectOrderBy.AllFuture) && (tt.Task.Status != TaskStatus.completed) && (tt.Task.StartDate > DateTime.Now))) // && (tt.Task.Deadline > DateTime.Now)))

                                    && (tt.MetaInfo.isDeleted == false) && (tt.AssignedUser.Id == PersonId) && (tt.Task.Community.Id == CommunityId) && (tt.Task.isPersonal == true) && (tt.TaskRole == TaskRole.Manager || tt.TaskRole == TaskRole.ProjectOwner) && (tt.Task.Level == 0));
                                break;

                            case TaskFilter.CurrentCommunity:
                                list = GetTaskAssignmentGeneric(tt =>
                                    (((PFilter == ProjectOrderBy.AllCompleted) && (tt.Task.Status == TaskStatus.completed))
                                    ||
                                    ((PFilter == ProjectOrderBy.AllActive) && (tt.Task.Status != TaskStatus.completed) && (tt.Task.StartDate <= DateTime.Now)) // && (tt.Task.Deadline >= DateTime.Now))
                                    ||
                                    ((PFilter == ProjectOrderBy.AllFuture) && (tt.Task.Status != TaskStatus.completed) && (tt.Task.StartDate > DateTime.Now))) // && (tt.Task.Deadline > DateTime.Now)))

                                    && (tt.MetaInfo.isDeleted == false) && (tt.AssignedUser.Id == PersonId) && (tt.Task.Community.Id == CommunityId) && (tt.TaskRole == TaskRole.Manager || tt.TaskRole == TaskRole.ProjectOwner) && (tt.Task.Level == 0));
                                break;

                            case TaskFilter.PortalPersonal:
                                list = GetTaskAssignmentGeneric(tt =>
                                    ((PFilter == ProjectOrderBy.AllCompleted && tt.Task.Status == TaskStatus.completed)
                                    ||
                                    (PFilter == ProjectOrderBy.AllActive && tt.Task.Status != TaskStatus.completed && tt.Task.StartDate <= DateTime.Now) // && tt.Task.Deadline >= DateTime.Now)
                                    ||
                                    (PFilter == ProjectOrderBy.AllFuture && tt.Task.Status != TaskStatus.completed && tt.Task.StartDate > DateTime.Now)) // && tt.Task.Deadline > DateTime.Now))

                                    && (tt.MetaInfo.isDeleted == false) && (tt.AssignedUser.Id == PersonId) && (tt.Task.Community == null) && (tt.Task.isPersonal == true) && (tt.Task.isPortal == true) && (tt.TaskRole == TaskRole.Manager || tt.TaskRole == TaskRole.ProjectOwner) && (tt.Task.Level == 0));
                                break;

                            case TaskFilter.AllCommunitiesPersonal:
                                list = GetTaskAssignmentGeneric(tt =>
                                    ((PFilter == ProjectOrderBy.AllCompleted && tt.Task.Status == TaskStatus.completed)
                                    ||
                                    (PFilter == ProjectOrderBy.AllActive && tt.Task.Status != TaskStatus.completed && tt.Task.StartDate <= DateTime.Now) // && tt.Task.Deadline >= DateTime.Now)
                                    ||
                                    (PFilter == ProjectOrderBy.AllFuture && tt.Task.Status != TaskStatus.completed && tt.Task.StartDate > DateTime.Now)) // && tt.Task.Deadline > DateTime.Now))

                                    && (tt.MetaInfo.isDeleted == false) && (tt.AssignedUser.Id == PersonId) && (tt.Task.isPersonal == true) && (tt.Task.isPortal == false) && (tt.TaskRole == TaskRole.Manager || tt.TaskRole == TaskRole.ProjectOwner) && (tt.Task.Level == 0));
                                break;

                            default:
                                list = GetTaskAssignmentGeneric(tt => (
                                    (PFilter == ProjectOrderBy.AllCompleted && tt.Task.Status == TaskStatus.completed)
                                    ||
                                    (PFilter == ProjectOrderBy.AllActive && (tt.Task.Status != TaskStatus.completed) && (tt.Task.StartDate <= DateTime.Now)) // && (tt.Task.Deadline >= DateTime.Now))
                                    ||
                                    (PFilter == ProjectOrderBy.AllFuture && tt.Task.Status != TaskStatus.completed && tt.Task.StartDate > DateTime.Now) // && tt.Task.Deadline > DateTime.Now)
                                                            )
                                    && (tt.MetaInfo.isDeleted == false) && (tt.AssignedUser.Id == PersonId) && (tt.TaskRole == TaskRole.Manager || tt.TaskRole == TaskRole.ProjectOwner) && (tt.Task.Level == 0));
                                break;
                        }

                        break;

                    case TaskManagedType.Tasks:
                        switch (TFilter)
                        {
                            case TaskFilter.AllCommunities:
                                list = GetTaskAssignmentGeneric(tt => (
                                    ((PFilter == ProjectOrderBy.AllCompleted) && (tt.Task.Status == TaskStatus.completed))
                                    ||
                                    ((PFilter == ProjectOrderBy.AllActive) && (tt.Task.Status != TaskStatus.completed) && (tt.Task.StartDate <= DateTime.Now)) // && (tt.Task.Deadline >= DateTime.Now))
                                    ||
                                    ((PFilter == ProjectOrderBy.AllFuture) && (tt.Task.Status != TaskStatus.completed) && (tt.Task.StartDate > DateTime.Now)) // && (tt.Task.Deadline > DateTime.Now))
                                                            )
                                    && (tt.MetaInfo.isDeleted == false) && (tt.AssignedUser.Id == PersonId) && (tt.TaskRole == TaskRole.Manager || tt.TaskRole == TaskRole.ProjectOwner) && (tt.Task.Level != 0));

                                break;

                            case TaskFilter.CommunityPersonal:
                                list = GetTaskAssignmentGeneric(tt => (
                                    ((PFilter == ProjectOrderBy.AllCompleted) && (tt.Task.Status == TaskStatus.completed))
                                    ||
                                    ((PFilter == ProjectOrderBy.AllActive) && (tt.Task.Status != TaskStatus.completed) && (tt.Task.StartDate <= DateTime.Now)) // && (tt.Task.Deadline >= DateTime.Now))
                                    ||
                                    ((PFilter == ProjectOrderBy.AllFuture) && (tt.Task.Status != TaskStatus.completed) && (tt.Task.StartDate > DateTime.Now)) // && (tt.Task.Deadline > DateTime.Now))
                                            )
                                    && (tt.MetaInfo.isDeleted == false) && (tt.AssignedUser.Id == PersonId) && (tt.Task.Community.Id == CommunityId) && (tt.Task.isPersonal == true) && (tt.TaskRole == TaskRole.Manager || tt.TaskRole == TaskRole.ProjectOwner) && (tt.Task.Level != 0));
                                break;

                            case TaskFilter.CurrentCommunity:
                                list = GetTaskAssignmentGeneric(tt =>
                                    (((PFilter == ProjectOrderBy.AllCompleted) && (tt.Task.Status == TaskStatus.completed))
                                    ||
                                    ((PFilter == ProjectOrderBy.AllActive) && (tt.Task.Status != TaskStatus.completed) && (tt.Task.StartDate <= DateTime.Now)) // && (tt.Task.Deadline >= DateTime.Now))
                                    ||
                                    ((PFilter == ProjectOrderBy.AllFuture) && (tt.Task.Status != TaskStatus.completed) && (tt.Task.StartDate > DateTime.Now)))// && (tt.Task.Deadline > DateTime.Now)))

                                    && (tt.MetaInfo.isDeleted == false) && (tt.AssignedUser.Id == PersonId) && (tt.Task.Community.Id == CommunityId) && (tt.TaskRole == TaskRole.Manager || tt.TaskRole == TaskRole.ProjectOwner) && (tt.Task.Level != 0));
                                break;

                            case TaskFilter.PortalPersonal:
                                list = GetTaskAssignmentGeneric(tt =>
                                    ((PFilter == ProjectOrderBy.AllCompleted && tt.Task.Status == TaskStatus.completed)
                                    ||
                                    (PFilter == ProjectOrderBy.AllActive && tt.Task.Status != TaskStatus.completed && tt.Task.StartDate <= DateTime.Now) // && tt.Task.Deadline >= DateTime.Now)
                                    ||
                                    (PFilter == ProjectOrderBy.AllFuture && tt.Task.Status != TaskStatus.completed && tt.Task.StartDate > DateTime.Now)) // && tt.Task.Deadline > DateTime.Now))

                                    && (tt.MetaInfo.isDeleted == false) && (tt.AssignedUser.Id == PersonId) && (tt.Task.Community == null) && (tt.Task.isPersonal == true) && (tt.Task.isPortal == true) && (tt.TaskRole == TaskRole.Manager || tt.TaskRole == TaskRole.ProjectOwner) && (tt.Task.Level != 0));
                                break;

                            case TaskFilter.AllCommunitiesPersonal:
                                list = GetTaskAssignmentGeneric(tt =>
                                    ((PFilter == ProjectOrderBy.AllCompleted && tt.Task.Status == TaskStatus.completed)
                                    ||
                                    (PFilter == ProjectOrderBy.AllActive && tt.Task.Status != TaskStatus.completed && tt.Task.StartDate <= DateTime.Now) // && tt.Task.Deadline >= DateTime.Now)
                                    ||
                                    (PFilter == ProjectOrderBy.AllFuture && tt.Task.Status != TaskStatus.completed && tt.Task.StartDate > DateTime.Now)) // && tt.Task.Deadline > DateTime.Now))

                                    && (tt.MetaInfo.isDeleted == false) && (tt.AssignedUser.Id == PersonId) && (tt.Task.isPersonal == true) && (tt.Task.isPortal == false) && (tt.TaskRole == TaskRole.Manager || tt.TaskRole == TaskRole.ProjectOwner) && (tt.Task.Level != 0));
                                break;

                            default:
                                list = GetTaskAssignmentGeneric(tt => (
                                    (PFilter == ProjectOrderBy.AllCompleted && tt.Task.Status == TaskStatus.completed)
                                    ||
                                    (PFilter == ProjectOrderBy.AllActive && (tt.Task.Status != TaskStatus.completed) && (tt.Task.StartDate <= DateTime.Now)) // && (tt.Task.Deadline >= DateTime.Now))
                                    ||
                                    (PFilter == ProjectOrderBy.AllFuture && tt.Task.Status != TaskStatus.completed && tt.Task.StartDate > DateTime.Now) // && tt.Task.Deadline > DateTime.Now)
                                                            )
                                    && (tt.MetaInfo.isDeleted == false) && (tt.AssignedUser.Id == PersonId) && (tt.TaskRole == TaskRole.Manager || tt.TaskRole == TaskRole.ProjectOwner) && (tt.Task.Level != 0));
                                break;
                        }

                        break;
                }

                taskcount = list.Count;

                tx.Commit();
            }
            catch (Exception ex)
            {

            }

            return taskcount;
        }
        //--------------------------------------------------------------------------------------------------------------

        public IList<dtoAssignedTasksWithCommunityHeader> GetTasksManagementTasks(int PersonId, int CommunityId, int PageSize, int PageSkip, TaskFilter TFilter, ProjectOrderBy PFilter, TaskManagedType TaskType, Sorting oSorting, String portalName)
        {
            List<dtoAssignedTasksWithCommunityHeader> olist = new List<dtoAssignedTasksWithCommunityHeader>();
            IList<TaskAssignment> list = null;
            IList<dtoTaskWithPortalComm> listp = null;
            ITransaction tx = session.BeginTransaction();

            try
            {
                switch (TaskType)
                {
                    case TaskManagedType.Projects:

                        switch (TFilter)
                        {
                            case TaskFilter.AllCommunities:
                                list = GetTaskAssignmentGenericQuery(tt => (
                                    ((PFilter == ProjectOrderBy.AllCompleted) && (tt.Task.Status == TaskStatus.completed))
                                    ||
                                    ((PFilter == ProjectOrderBy.AllActive) && (tt.Task.Status != TaskStatus.completed) && (tt.Task.StartDate <= DateTime.Now)) // && (tt.Task.Deadline >= DateTime.Now))
                                    ||
                                    ((PFilter == ProjectOrderBy.AllFuture) && (tt.Task.Status != TaskStatus.completed) && (tt.Task.StartDate > DateTime.Now)) // && (tt.Task.Deadline > DateTime.Now))
                                                            )
                                    && (tt.MetaInfo.isDeleted == false) && (tt.AssignedUser.Id == PersonId) && (tt.TaskRole == TaskRole.Manager || tt.TaskRole == TaskRole.ProjectOwner) && (tt.Task.Level == 0)).ToList<TaskAssignment>();
                                //.OrderBy(tt => tt.Task.Community.Name).OrderBy(tt => tt.Task.EndDate).Skip(PageSkip).Take(PageSize).ToList<TaskAssignment>();
                                switch (oSorting)
                                {
                                    case Sorting.DeadlineOrder:
                                        listp = (from pl in list select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(tt => tt.CommunityName).ThenBy(tt => tt.Deadline).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                        break;

                                    case Sorting.AlphabeticalOrder:
                                        listp = (from pl in list select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(tt => tt.CommunityName).ThenBy(tt => tt.Name).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                        break;

                                    default:
                                        listp = (from pl in list select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(tt => tt.CommunityName).ThenBy(tt => tt.Deadline).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                        break;
                                }
                                break;

                            case TaskFilter.CommunityPersonal:
                                list = GetTaskAssignmentGenericQuery(tt =>
                                    (((PFilter == ProjectOrderBy.AllCompleted) && (tt.Task.Status == TaskStatus.completed))
                                    ||
                                    ((PFilter == ProjectOrderBy.AllActive) && (tt.Task.Status != TaskStatus.completed) && (tt.Task.StartDate <= DateTime.Now)) // && (tt.Task.Deadline >= DateTime.Now))
                                    ||
                                    ((PFilter == ProjectOrderBy.AllFuture) && (tt.Task.Status != TaskStatus.completed) && (tt.Task.StartDate > DateTime.Now))) // && (tt.Task.Deadline > DateTime.Now)))

                                    && (tt.MetaInfo.isDeleted == false) && (tt.AssignedUser.Id == PersonId) && (tt.Task.Community.Id == CommunityId) && (tt.Task.isPersonal == true) && (tt.TaskRole == TaskRole.Manager || tt.TaskRole == TaskRole.ProjectOwner) && (tt.Task.Level == 0)).ToList<TaskAssignment>();
                                //.OrderBy(tt => tt.Task.Community.Name).OrderBy(tt => tt.Task.EndDate).Skip(PageSkip).Take(PageSize).ToList<TaskAssignment>();
                                switch (oSorting)
                                {
                                    case Sorting.DeadlineOrder:
                                        listp = (from pl in list select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(tt => tt.CommunityName).ThenBy(tt => tt.Deadline).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                        break;

                                    case Sorting.AlphabeticalOrder:
                                        listp = (from pl in list select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(tt => tt.CommunityName).ThenBy(tt => tt.Name).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                        break;

                                    default:
                                        listp = (from pl in list select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(tt => tt.CommunityName).ThenBy(tt => tt.Deadline).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                        break;
                                }
                                break;

                            case TaskFilter.CurrentCommunity:
                                list = GetTaskAssignmentGenericQuery(tt =>
                                (((PFilter == ProjectOrderBy.AllCompleted) && (tt.Task.Status == TaskStatus.completed))
                                ||
                                ((PFilter == ProjectOrderBy.AllActive) && (tt.Task.Status != TaskStatus.completed) && (tt.Task.StartDate <= DateTime.Now)) // && (tt.Task.Deadline >= DateTime.Now))
                                ||
                                ((PFilter == ProjectOrderBy.AllFuture) && (tt.Task.Status != TaskStatus.completed) && (tt.Task.StartDate > DateTime.Now))) // && (tt.Task.Deadline > DateTime.Now)))

                                && (tt.MetaInfo.isDeleted == false) && (tt.AssignedUser.Id == PersonId) && (tt.Task.Community.Id == CommunityId) && (tt.TaskRole == TaskRole.Manager || tt.TaskRole == TaskRole.ProjectOwner) && (tt.Task.Level == 0)).ToList<TaskAssignment>();
                                //.OrderBy(tt => tt.Task.Community.Name).OrderBy(tt => tt.Task.EndDate).Skip(PageSkip).Take(PageSize).ToList<TaskAssignment>();
                                switch (oSorting)
                                {
                                    case Sorting.DeadlineOrder:
                                        listp = (from pl in list select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(tt => tt.CommunityName).ThenBy(tt => tt.Deadline).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                        break;

                                    case Sorting.AlphabeticalOrder:
                                        listp = (from pl in list select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(tt => tt.CommunityName).ThenBy(tt => tt.Name).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                        break;

                                    default:
                                        listp = (from pl in list select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(tt => tt.CommunityName).ThenBy(tt => tt.Deadline).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                        break;
                                }
                                break;

                            case TaskFilter.PortalPersonal:
                                list = GetTaskAssignmentGenericQuery(tt =>
                                ((PFilter == ProjectOrderBy.AllCompleted && tt.Task.Status == TaskStatus.completed)
                                ||
                                (PFilter == ProjectOrderBy.AllActive && tt.Task.Status != TaskStatus.completed && tt.Task.StartDate <= DateTime.Now) // && tt.Task.Deadline >= DateTime.Now)
                                ||
                                (PFilter == ProjectOrderBy.AllFuture && tt.Task.Status != TaskStatus.completed && tt.Task.StartDate > DateTime.Now)) // && tt.Task.Deadline > DateTime.Now))

                                && (tt.MetaInfo.isDeleted == false) && (tt.AssignedUser.Id == PersonId) && (tt.Task.Community == null) && (tt.Task.isPersonal == true) && (tt.Task.isPortal == true) && (tt.TaskRole == TaskRole.Manager || tt.TaskRole == TaskRole.ProjectOwner) && (tt.Task.Level == 0)).ToList<TaskAssignment>();

                                switch (oSorting)
                                {
                                    case Sorting.DeadlineOrder:
                                        listp = (from pl in list select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(tt => tt.CommunityName).ThenBy(tt => tt.Deadline).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                        break;

                                    case Sorting.AlphabeticalOrder:
                                        listp = (from pl in list select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(tt => tt.CommunityName).ThenBy(tt => tt.Name).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                        break;

                                    default:
                                        listp = (from pl in list select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(tt => tt.CommunityName).ThenBy(tt => tt.Deadline).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                        break;
                                }
                                break;


                            case TaskFilter.AllCommunitiesPersonal:
                                list = GetTaskAssignmentGenericQuery(tt =>
                                  ((PFilter == ProjectOrderBy.AllCompleted && tt.Task.Status == TaskStatus.completed)
                                  ||
                                  (PFilter == ProjectOrderBy.AllActive && tt.Task.Status != TaskStatus.completed && tt.Task.StartDate <= DateTime.Now) // && tt.Task.Deadline >= DateTime.Now)
                                  ||
                                  (PFilter == ProjectOrderBy.AllFuture && tt.Task.Status != TaskStatus.completed && tt.Task.StartDate > DateTime.Now)) // && tt.Task.Deadline > DateTime.Now))

                                  && (tt.MetaInfo.isDeleted == false) && (tt.AssignedUser.Id == PersonId) && (tt.Task.isPersonal == true) && (tt.Task.isPortal == false) && (tt.TaskRole == TaskRole.Manager || tt.TaskRole == TaskRole.ProjectOwner) && (tt.Task.Level == 0)).ToList<TaskAssignment>();

                                switch (oSorting)
                                {
                                    case Sorting.DeadlineOrder:
                                        listp = (from pl in list select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(tt => tt.CommunityName).ThenBy(tt => tt.Deadline).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                        break;

                                    case Sorting.AlphabeticalOrder:
                                        listp = (from pl in list select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(tt => tt.CommunityName).ThenBy(tt => tt.Name).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                        break;

                                    default:
                                        listp = (from pl in list select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(tt => tt.CommunityName).ThenBy(tt => tt.Deadline).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                        break;
                                }
                                break;

                            default:
                                list = GetTaskAssignmentGenericQuery(tt => (
                                (PFilter == ProjectOrderBy.AllCompleted && tt.Task.Status == TaskStatus.completed)
                                ||
                                (PFilter == ProjectOrderBy.AllActive && (tt.Task.Status != TaskStatus.completed) && (tt.Task.StartDate <= DateTime.Now)) // && (tt.Task.Deadline >= DateTime.Now))
                                ||
                                (PFilter == ProjectOrderBy.AllFuture && tt.Task.Status != TaskStatus.completed && tt.Task.StartDate > DateTime.Now) // && tt.Task.Deadline > DateTime.Now)
                                                        )
                                && (tt.MetaInfo.isDeleted == false) && (tt.AssignedUser.Id == PersonId) && (tt.TaskRole == TaskRole.Manager || tt.TaskRole == TaskRole.ProjectOwner) && (tt.Task.Level == 0)).ToList<TaskAssignment>();

                                switch (oSorting)
                                {
                                    case Sorting.DeadlineOrder:
                                        listp = (from pl in list select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(tt => tt.CommunityName).ThenBy(tt => tt.Deadline).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                        break;

                                    case Sorting.AlphabeticalOrder:
                                        listp = (from pl in list select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(tt => tt.CommunityName).ThenBy(tt => tt.Name).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                        break;

                                    default:
                                        listp = (from pl in list select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(tt => tt.CommunityName).ThenBy(tt => tt.Deadline).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                        break;
                                }
                                break;
                        }

                        break;

                    case TaskManagedType.Tasks:
                        switch (TFilter)
                        {
                            case TaskFilter.AllCommunities:
                                list = GetTaskAssignmentGenericQuery(tt => (
                                    ((PFilter == ProjectOrderBy.AllCompleted) && (tt.Task.Status == TaskStatus.completed))
                                    ||
                                    ((PFilter == ProjectOrderBy.AllActive) && (tt.Task.Status != TaskStatus.completed) && (tt.Task.StartDate <= DateTime.Now)) // && (tt.Task.Deadline >= DateTime.Now))
                                    ||
                                    ((PFilter == ProjectOrderBy.AllFuture) && (tt.Task.Status != TaskStatus.completed) && (tt.Task.StartDate > DateTime.Now)) // && (tt.Task.Deadline > DateTime.Now))
                                                            )
                                    && (tt.MetaInfo.isDeleted == false) && (tt.AssignedUser.Id == PersonId) && (tt.TaskRole == TaskRole.Manager || tt.TaskRole == TaskRole.ProjectOwner) && (tt.Task.Level != 0)).ToList<TaskAssignment>();
                                //.OrderBy(tt => tt.Task.Community.Name).OrderBy(tt => tt.Task.EndDate).Skip(PageSkip).Take(PageSize).ToList<TaskAssignment>();
                                switch (oSorting)
                                {
                                    case Sorting.DeadlineOrder:
                                        listp = (from pl in list select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(tt => tt.CommunityName).ThenBy(tt => tt.Deadline).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                        break;

                                    case Sorting.AlphabeticalOrder:
                                        listp = (from pl in list select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(tt => tt.CommunityName).ThenBy(tt => tt.Name).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                        break;

                                    default:
                                        listp = (from pl in list select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(tt => tt.CommunityName).ThenBy(tt => tt.Deadline).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                        break;
                                }
                                break;

                            case TaskFilter.CommunityPersonal:
                                list = GetTaskAssignmentGenericQuery(tt => (
                                ((PFilter == ProjectOrderBy.AllCompleted) && (tt.Task.Status == TaskStatus.completed))
                                ||
                                ((PFilter == ProjectOrderBy.AllActive) && (tt.Task.Status != TaskStatus.completed) && (tt.Task.StartDate <= DateTime.Now)) // && (tt.Task.Deadline >= DateTime.Now))
                                ||
                                ((PFilter == ProjectOrderBy.AllFuture) && (tt.Task.Status != TaskStatus.completed) && (tt.Task.StartDate > DateTime.Now)) // && (tt.Task.Deadline > DateTime.Now))
                                        )
                                && (tt.MetaInfo.isDeleted == false) && (tt.AssignedUser.Id == PersonId) && (tt.Task.Community.Id == CommunityId) && (tt.Task.isPersonal == true) && (tt.TaskRole == TaskRole.Manager || tt.TaskRole == TaskRole.ProjectOwner) && (tt.Task.Level != 0)).ToList<TaskAssignment>();
                                //.OrderBy(tt => tt.Task.Community.Name).OrderBy(tt => tt.Task.EndDate).Skip(PageSkip).Take(PageSize).ToList<TaskAssignment>();
                                switch (oSorting)
                                {
                                    case Sorting.DeadlineOrder:
                                        listp = (from pl in list select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(tt => tt.CommunityName).ThenBy(tt => tt.Deadline).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                        break;

                                    case Sorting.AlphabeticalOrder:
                                        listp = (from pl in list select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(tt => tt.CommunityName).ThenBy(tt => tt.Name).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                        break;

                                    default:
                                        listp = (from pl in list select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(tt => tt.CommunityName).ThenBy(tt => tt.Deadline).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                        break;
                                }
                                break;

                            case TaskFilter.CurrentCommunity:
                                list = GetTaskAssignmentGenericQuery(tt =>
                                (((PFilter == ProjectOrderBy.AllCompleted) && (tt.Task.Status == TaskStatus.completed))
                                ||
                                ((PFilter == ProjectOrderBy.AllActive) && (tt.Task.Status != TaskStatus.completed) && (tt.Task.StartDate <= DateTime.Now)) // && (tt.Task.Deadline >= DateTime.Now))
                                ||
                                ((PFilter == ProjectOrderBy.AllFuture) && (tt.Task.Status != TaskStatus.completed) && (tt.Task.StartDate > DateTime.Now))) // && (tt.Task.Deadline > DateTime.Now)))

                                && (tt.MetaInfo.isDeleted == false) && (tt.AssignedUser.Id == PersonId) && (tt.Task.Community.Id == CommunityId) && (tt.TaskRole == TaskRole.Manager || tt.TaskRole == TaskRole.ProjectOwner) && (tt.Task.Level != 0)).ToList<TaskAssignment>();

                                switch (oSorting)
                                {
                                    case Sorting.DeadlineOrder:
                                        listp = (from pl in list select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(tt => tt.CommunityName).ThenBy(tt => tt.Deadline).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                        break;

                                    case Sorting.AlphabeticalOrder:
                                        listp = (from pl in list select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(tt => tt.CommunityName).ThenBy(tt => tt.Name).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                        break;

                                    default:
                                        listp = (from pl in list select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(tt => tt.CommunityName).ThenBy(tt => tt.Deadline).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                        break;
                                } break;

                            case TaskFilter.PortalPersonal:
                                list = GetTaskAssignmentGenericQuery(tt =>
                                    ((PFilter == ProjectOrderBy.AllCompleted && tt.Task.Status == TaskStatus.completed)
                                   ||
                                    (PFilter == ProjectOrderBy.AllActive && tt.Task.Status != TaskStatus.completed && tt.Task.StartDate <= DateTime.Now) // && tt.Task.Deadline >= DateTime.Now)
                                    ||
                                    (PFilter == ProjectOrderBy.AllFuture && tt.Task.Status != TaskStatus.completed && tt.Task.StartDate > DateTime.Now)) // && tt.Task.Deadline > DateTime.Now))

                                    && (tt.MetaInfo.isDeleted == false) && (tt.AssignedUser.Id == PersonId) && (tt.Task.Community == null) && (tt.Task.isPersonal == true) && (tt.Task.isPortal == true) && (tt.TaskRole == TaskRole.Manager || tt.TaskRole == TaskRole.ProjectOwner) && (tt.Task.Level != 0)).ToList<TaskAssignment>();

                                switch (oSorting)
                                {
                                    case Sorting.DeadlineOrder:
                                        listp = (from pl in list select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(tt => tt.CommunityName).ThenBy(tt => tt.Deadline).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                        break;

                                    case Sorting.AlphabeticalOrder:
                                        listp = (from pl in list select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(tt => tt.CommunityName).ThenBy(tt => tt.Name).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                        break;

                                    default:
                                        listp = (from pl in list select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(tt => tt.CommunityName).ThenBy(tt => tt.Deadline).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                        break;
                                }
                                break;

                            case TaskFilter.AllCommunitiesPersonal:
                                list = GetTaskAssignmentGenericQuery(tt =>
                                ((PFilter == ProjectOrderBy.AllCompleted && tt.Task.Status == TaskStatus.completed)
                                ||
                                (PFilter == ProjectOrderBy.AllActive && tt.Task.Status != TaskStatus.completed && tt.Task.StartDate <= DateTime.Now) // && tt.Task.Deadline >= DateTime.Now)
                                ||
                                (PFilter == ProjectOrderBy.AllFuture && tt.Task.Status != TaskStatus.completed && tt.Task.StartDate > DateTime.Now)) // && tt.Task.Deadline > DateTime.Now))

                                && (tt.MetaInfo.isDeleted == false) && (tt.AssignedUser.Id == PersonId) && (tt.Task.isPersonal == true) && (tt.Task.isPortal == false) && (tt.TaskRole == TaskRole.Manager || tt.TaskRole == TaskRole.ProjectOwner) && (tt.Task.Level != 0)).ToList<TaskAssignment>();
                                switch (oSorting)
                                {
                                    case Sorting.DeadlineOrder:
                                        listp = (from pl in list select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(tt => tt.CommunityName).ThenBy(tt => tt.Deadline).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                        break;

                                    case Sorting.AlphabeticalOrder:
                                        listp = (from pl in list select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(tt => tt.CommunityName).ThenBy(tt => tt.Name).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                        break;

                                    default:
                                        listp = (from pl in list select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(tt => tt.CommunityName).ThenBy(tt => tt.Deadline).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                        break;
                                }
                                break;

                            default:
                                list = GetTaskAssignmentGenericQuery(tt => (
                                (PFilter == ProjectOrderBy.AllCompleted && tt.Task.Status == TaskStatus.completed)
                               ||
                                (PFilter == ProjectOrderBy.AllActive && (tt.Task.Status != TaskStatus.completed) && (tt.Task.StartDate <= DateTime.Now)) // && (tt.Task.Deadline >= DateTime.Now))
                                ||
                                (PFilter == ProjectOrderBy.AllFuture && tt.Task.Status != TaskStatus.completed && tt.Task.StartDate > DateTime.Now) // && tt.Task.Deadline > DateTime.Now)
                                                        )
                                && (tt.MetaInfo.isDeleted == false) && (tt.AssignedUser.Id == PersonId) && (tt.TaskRole == TaskRole.Manager || tt.TaskRole == TaskRole.ProjectOwner) && (tt.Task.Level != 0)).ToList<TaskAssignment>();
                                switch (oSorting)
                                {
                                    case Sorting.DeadlineOrder:
                                        listp = (from pl in list select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(tt => tt.CommunityName).ThenBy(tt => tt.Deadline).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                        break;

                                    case Sorting.AlphabeticalOrder:
                                        listp = (from pl in list select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(tt => tt.CommunityName).ThenBy(tt => tt.Name).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                        break;

                                    default:
                                        listp = (from pl in list select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(tt => tt.CommunityName).ThenBy(tt => tt.Deadline).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                        break;
                                }
                                break;
                        }

                        break;

                    default:
                        switch (TFilter)
                        {
                            case TaskFilter.AllCommunities:
                                list = GetTaskAssignmentGenericQuery(tt => (
                                    ((PFilter == ProjectOrderBy.AllCompleted) && (tt.Task.Status == TaskStatus.completed))
                                    ||
                                    ((PFilter == ProjectOrderBy.AllActive) && (tt.Task.Status != TaskStatus.completed) && (tt.Task.StartDate <= DateTime.Now)) // && (tt.Task.Deadline >= DateTime.Now))
                                    ||
                                    ((PFilter == ProjectOrderBy.AllFuture) && (tt.Task.Status != TaskStatus.completed) && (tt.Task.StartDate > DateTime.Now)) // && (tt.Task.Deadline > DateTime.Now))
                                                            )
                                    && (tt.MetaInfo.isDeleted == false) && (tt.AssignedUser.Id == PersonId) && (tt.TaskRole == TaskRole.Manager || tt.TaskRole == TaskRole.ProjectOwner) && (tt.Task.Level == 0)).ToList<TaskAssignment>();//.OrderBy(tt => tt.Task.Community.Name).OrderBy(tt => tt.Task.EndDate).Skip(PageSkip).Take(PageSize).ToList<TaskAssignment>();

                                switch (oSorting)
                                {
                                    case Sorting.DeadlineOrder:
                                        listp = (from pl in list select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(tt => tt.CommunityName).ThenBy(tt => tt.Deadline).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                        break;

                                    case Sorting.AlphabeticalOrder:
                                        listp = (from pl in list select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(tt => tt.CommunityName).ThenBy(tt => tt.Name).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                        break;

                                    default:
                                        listp = (from pl in list select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(tt => tt.CommunityName).ThenBy(tt => tt.Deadline).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                        break;
                                }
                                break;

                            case TaskFilter.CommunityPersonal:
                                list = GetTaskAssignmentGenericQuery(tt =>
                                    (((PFilter == ProjectOrderBy.AllCompleted) && (tt.Task.Status == TaskStatus.completed))
                                    ||
                                    ((PFilter == ProjectOrderBy.AllActive) && (tt.Task.Status != TaskStatus.completed) && (tt.Task.StartDate <= DateTime.Now)) // && (tt.Task.Deadline >= DateTime.Now))
                                    ||
                                    ((PFilter == ProjectOrderBy.AllFuture) && (tt.Task.Status != TaskStatus.completed) && (tt.Task.StartDate > DateTime.Now))) // && (tt.Task.Deadline > DateTime.Now)))

                                    && (tt.MetaInfo.isDeleted == false) && (tt.AssignedUser.Id == PersonId) && (tt.Task.Community.Id == CommunityId) && (tt.Task.isPersonal == true) && (tt.TaskRole == TaskRole.Manager || tt.TaskRole == TaskRole.ProjectOwner) && (tt.Task.Level == 0)).ToList<TaskAssignment>();
                                switch (oSorting)
                                {
                                    case Sorting.DeadlineOrder:
                                        listp = (from pl in list select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(tt => tt.CommunityName).ThenBy(tt => tt.Deadline).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                        break;

                                    case Sorting.AlphabeticalOrder:
                                        listp = (from pl in list select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(tt => tt.CommunityName).ThenBy(tt => tt.Name).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                        break;

                                    default:
                                        listp = (from pl in list select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(tt => tt.CommunityName).ThenBy(tt => tt.Deadline).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                        break;
                                }
                                break;

                            case TaskFilter.CurrentCommunity:
                                list = GetTaskAssignmentGenericQuery(tt =>
                                (((PFilter == ProjectOrderBy.AllCompleted) && (tt.Task.Status == TaskStatus.completed))
                                ||
                                ((PFilter == ProjectOrderBy.AllActive) && (tt.Task.Status != TaskStatus.completed) && (tt.Task.StartDate <= DateTime.Now)) // && (tt.Task.Deadline >= DateTime.Now))
                                ||
                                ((PFilter == ProjectOrderBy.AllFuture) && (tt.Task.Status != TaskStatus.completed) && (tt.Task.StartDate > DateTime.Now))) // && (tt.Task.Deadline > DateTime.Now)))

                                && (tt.MetaInfo.isDeleted == false) && (tt.AssignedUser.Id == PersonId) && (tt.Task.Community.Id == CommunityId) && (tt.TaskRole == TaskRole.Manager || tt.TaskRole == TaskRole.ProjectOwner) && (tt.Task.Level == 0)).ToList<TaskAssignment>();
                                switch (oSorting)
                                {
                                    case Sorting.DeadlineOrder:
                                        listp = (from pl in list select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(tt => tt.CommunityName).ThenBy(tt => tt.Deadline).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                        break;

                                    case Sorting.AlphabeticalOrder:
                                        listp = (from pl in list select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(tt => tt.CommunityName).ThenBy(tt => tt.Name).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                        break;

                                    default:
                                        listp = (from pl in list select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(tt => tt.CommunityName).ThenBy(tt => tt.Deadline).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                        break;
                                }
                                break;

                            case TaskFilter.PortalPersonal:
                                list = GetTaskAssignmentGenericQuery(tt =>
                                ((PFilter == ProjectOrderBy.AllCompleted && tt.Task.Status == TaskStatus.completed)
                                ||
                                (PFilter == ProjectOrderBy.AllActive && tt.Task.Status != TaskStatus.completed && tt.Task.StartDate <= DateTime.Now) // && tt.Task.Deadline >= DateTime.Now)
                                ||
                                (PFilter == ProjectOrderBy.AllFuture && tt.Task.Status != TaskStatus.completed && tt.Task.StartDate > DateTime.Now)) // && tt.Task.Deadline > DateTime.Now))

                                && (tt.MetaInfo.isDeleted == false) && (tt.AssignedUser.Id == PersonId) && (tt.Task.Community == null) && (tt.Task.isPersonal == true) && (tt.Task.isPortal == true) && (tt.TaskRole == TaskRole.Manager || tt.TaskRole == TaskRole.ProjectOwner) && (tt.Task.Level == 0)).ToList<TaskAssignment>();
                                //.OrderBy(tt => tt.Task.Community.Name).OrderBy(tt => tt.Task.EndDate).Skip(PageSkip).Take(PageSize).ToList<TaskAssignment>() ;
                                switch (oSorting)
                                {
                                    case Sorting.DeadlineOrder:
                                        listp = (from pl in list select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(tt => tt.CommunityName).ThenBy(tt => tt.Deadline).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                        break;

                                    case Sorting.AlphabeticalOrder:
                                        listp = (from pl in list select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(tt => tt.CommunityName).ThenBy(tt => tt.Name).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                        break;

                                    default:
                                        listp = (from pl in list select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(tt => tt.CommunityName).ThenBy(tt => tt.Deadline).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                        break;
                                }
                                break;

                            case TaskFilter.AllCommunitiesPersonal:
                                list = GetTaskAssignmentGenericQuery(tt =>
                                    ((PFilter == ProjectOrderBy.AllCompleted && tt.Task.Status == TaskStatus.completed)
                                    ||
                                    (PFilter == ProjectOrderBy.AllActive && tt.Task.Status != TaskStatus.completed && tt.Task.StartDate <= DateTime.Now) // && tt.Task.Deadline >= DateTime.Now)
                                    ||
                                    (PFilter == ProjectOrderBy.AllFuture && tt.Task.Status != TaskStatus.completed && tt.Task.StartDate > DateTime.Now)) // && tt.Task.Deadline > DateTime.Now))

                                    && (tt.MetaInfo.isDeleted == false) && (tt.AssignedUser.Id == PersonId) && (tt.Task.isPersonal == true) && (tt.Task.isPortal == false) && (tt.TaskRole == TaskRole.Manager || tt.TaskRole == TaskRole.ProjectOwner) && (tt.Task.Level == 0)).ToList<TaskAssignment>();
                                //.OrderBy(tt => tt.Task.Community.Name).OrderBy(tt => tt.Task.EndDate).Skip(PageSkip).Take(PageSize).ToList<TaskAssignment>();
                                switch (oSorting)
                                {
                                    case Sorting.DeadlineOrder:
                                        listp = (from pl in list select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(tt => tt.CommunityName).ThenBy(tt => tt.Deadline).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                        break;

                                    case Sorting.AlphabeticalOrder:
                                        listp = (from pl in list select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(tt => tt.CommunityName).ThenBy(tt => tt.Name).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                        break;

                                    default:
                                        listp = (from pl in list select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(tt => tt.CommunityName).ThenBy(tt => tt.Deadline).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                        break;
                                }
                                break;

                            default:
                                //list = GetTaskAssignmentGenericQuery(tt => (
                                list = GetTaskAssignmentGenericQuery(tt => (
                            (PFilter == ProjectOrderBy.AllCompleted && tt.Task.Status == TaskStatus.completed)
                            ||
                            (PFilter == ProjectOrderBy.AllActive && (tt.Task.Status != TaskStatus.completed) && (tt.Task.StartDate <= DateTime.Now)) // && (tt.Task.Deadline >= DateTime.Now))
                            ||
                            (PFilter == ProjectOrderBy.AllFuture && tt.Task.Status != TaskStatus.completed && tt.Task.StartDate > DateTime.Now) // && tt.Task.Deadline > DateTime.Now)
                                                    )
                            && (tt.MetaInfo.isDeleted == false) && (tt.AssignedUser.Id == PersonId) && (tt.TaskRole == TaskRole.Manager || tt.TaskRole == TaskRole.ProjectOwner) && (tt.Task.Level == 0)).ToList<TaskAssignment>();
                                //.OrderBy(tt => tt.Task.Community.Name).OrderBy(tt => tt.Task.EndDate).Skip(PageSkip).Take(PageSize).ToList<TaskAssignment>();

                                switch (oSorting)
                                {
                                    case Sorting.DeadlineOrder:
                                        listp = (from pl in list select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(tt => tt.CommunityName).ThenBy(tt => tt.Deadline).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                        break;

                                    case Sorting.AlphabeticalOrder:
                                        listp = (from pl in list select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(tt => tt.CommunityName).ThenBy(tt => tt.Name).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                        break;

                                    default:
                                        listp = (from pl in list select new dtoTaskWithPortalComm(pl, portalName)).OrderBy(tt => tt.CommunityName).ThenBy(tt => tt.Deadline).Skip(PageSkip).Take(PageSize).ToList<dtoTaskWithPortalComm>();
                                        break;
                                }
                                break;
                        }
                        break;
                }

                olist = (from tp in listp
                         group tp by tp.CommunityId into g
                         select new dtoAssignedTasksWithCommunityHeader(GetCommunity(g.Key),
                             (from tg in g select new dtoAssignedTasks(GetTask(tg.ID), GetPermissionsOverTask(tg.ID, PersonId))).ToList<dtoAssignedTasks>()
                             )).ToList<dtoAssignedTasksWithCommunityHeader>();

                tx.Commit();
            }
            catch (Exception ex)
            {

            }

            return olist;
        }
        //--------------------------------------------------------------------------------------------------------------

        #endregion "Tasks Management"

        //--------------------------------------------------------------------------------------------------------------

        #region "AssignUsersUC"

        //restituisce il Count della lista di dto Assign

        public int GetAssignedUsersCount(long TaskId)
        {
            int oCount = 0;
            long oTaskId = TaskId;
            IList<TaskAssignment> list = null;
            IList<dtoAssignUsers> olist = null;

            ILookup<Person, TaskAssignment> Look = null;


            ITransaction tx = session.BeginTransaction();
            try
            {
                list = GetTaskAssignmentGeneric(tt => ((tt.Task.ID == TaskId)));
                Look = list.ToLookup(x => x.AssignedUser, y => y);

                olist = new List<dtoAssignUsers>();

                olist = (from item in Look
                         select
                             new dtoAssignUsers(
                             item.Key,
                             (from TA in item select new dtoRolesPerTask(TA.TaskRole, TA.MetaInfo.isDeleted, TA.ID)).ToList<dtoRolesPerTask>(),
                             (from TA in item where TA.TaskRole == TaskRole.Resource select TA.Completeness).ToList<int>())
                         ).ToList<dtoAssignUsers>();

                oCount = olist.Count;

                tx.Commit();
            }
            catch (Exception ex)
            { }

            return oCount;

        }
        //--------------------------------------------------------------------------------------------------------------

        public IList<TaskRole> GetRolesByTask(long TaskId, int PersonId)
        {
            long oProjectId = TaskId;
            IList<TaskRole> olistrole = null;
            IList<TaskAssignment> list = null;

            ITransaction tx = session.BeginTransaction();
            try
            {
                list = GetTaskAssignmentGeneric(tt => ((tt.AssignedUser.Id == PersonId) && (tt.Task.ID == TaskId)));
                olistrole = (from tsk in list select tsk.TaskRole).ToList<TaskRole>();
                tx.Commit();
            }
            catch (Exception ex)
            { }
            return olistrole;

        }
        //--------------------------------------------------------------------------------------------------------------

        //restituisce un Bool che indica se esiste almeno una "Risorsa" assegnata al task in questione

        public Boolean IsThereAnyResource(long TaskId)
        {
            IList<TaskAssignment> list = null;
            Boolean IsThere = true;

            ITransaction tx = session.BeginTransaction();
            try
            {
                list = GetTaskAssignmentGeneric(tt => ((tt.TaskRole == TaskRole.Resource) && (tt.Task.ID == TaskId)));
                if ((list.Count == 0))
                {
                    IsThere = false;
                }

                else { IsThere = true; }
            }
            catch (Exception ex) { }

            return IsThere;
        }

        //--------------------------------------------------------------------------------------------------------------

        // restituisce una lista di dtoAssignUsers

        public IList<dtoAssignUsers> GetUsersTable(long TaskId, int PageSize, int PageSkip)
        {
            long oTaskId = TaskId;
            IList<TaskAssignment> list = null;
            IList<dtoAssignUsers> olist = null;

            ILookup<Person, TaskAssignment> Look = null;


            ITransaction tx = session.BeginTransaction();
            try
            {
                list = GetTaskAssignmentGeneric(tt => ((tt.Task.ID == TaskId)));
                Look = list.ToLookup(x => x.AssignedUser, y => y);

                olist = new List<dtoAssignUsers>();

                olist = (from item in Look
                         select
                             new dtoAssignUsers(
                             item.Key,
                             (from TA in item select new dtoRolesPerTask(TA.TaskRole, TA.MetaInfo.isDeleted, TA.ID)).ToList<dtoRolesPerTask>(),
                             (from TA in item where TA.TaskRole == TaskRole.Resource select TA.Completeness).ToList<int>())
                         ).Skip(PageSkip).Take(PageSize).ToList<dtoAssignUsers>();

                tx.Commit();
            }
            catch (Exception ex)
            { }
            return olist;

        }

        #endregion "AssignUsersUC"
        //--------------------------------------------------------------------------------------------------------------

        #region "TaskAdministration"


        //OK: Paginazione: Recupera il Count della lista dei Project di cui un untente è manager secondo filtri TaskFilter= SOLO allcommunities e currentcommunity ; ProjectOrderBy=active,future,completed;  
        public int GetAdministeredProjectsCount(IList<Int32> CommunityIdList, ProjectOrderBy ProjectOrder)
        {
            int taskcount = 0;
            List<TaskAssignment> list = new List<TaskAssignment>();
            IList<Task> listP = new List<Task>();

            ITransaction tx = session.BeginTransaction();
            try
            {
                foreach (var item in CommunityIdList)
                {
                    switch (ProjectOrder)
                    {

                        case ProjectOrderBy.AllActive:
                            list.AddRange(GetTaskAssignmentGeneric(tt => (tt.Task.Level == 0) && (tt.Task.Community.Id == item) && (tt.Task.Status != TaskStatus.completed) && (tt.Task.StartDate <= DateTime.Now)
                                && (tt.MetaInfo.isDeleted == false) && (tt.Task.isPersonal == false) && (tt.Task.isPortal == false)));
                            break;

                        case ProjectOrderBy.AllCompleted:
                            list.AddRange(GetTaskAssignmentGeneric(tt => (tt.Task.Level == 0) && (tt.Task.Community.Id == item) && (tt.Task.Status == TaskStatus.completed)
                                && (tt.MetaInfo.isDeleted == false) && (tt.Task.isPersonal == false) && (tt.Task.isPortal == false)));
                            break;

                        case ProjectOrderBy.AllFuture:
                            list.AddRange(GetTaskAssignmentGeneric(tt => ((tt.Task.Level == 0) && (tt.Task.Community.Id == item) && (tt.Task.Status != TaskStatus.completed) && (tt.Task.StartDate > DateTime.Now))
                                && (tt.MetaInfo.isDeleted == false) && (tt.Task.isPersonal == false) && (tt.Task.isPortal == false)));
                            break;
                    }

                }

                listP = (from tc in list select tc.Project).Distinct().ToList<Task>();
                taskcount = listP.Count;
                tx.Commit();
            }

            catch (Exception ex)
            {

            }

            return taskcount;
        }

        //--------------------------------------------------------------------------------------------------------------
        public class ProjectEqualityComparer : IEqualityComparer<Task>
        {
            public bool Equals(Task x, Task y)
            {
                if ((x.ID == y.ID))
                    return true;
                else
                    return false;
            }

            public int GetHashCode(Task obj)
            {

                return base.GetHashCode();

            }
        }


        // Recupero dei Task di tutte le comunità in cui sono amministratore, indipendentemente dall assegnamento
        public IList<dtoAdminProjectsWithCommunityHeader> GetAdministeredProjects(IList<int> CommunityIdList, int PersonId, int PageSize, int PageSkip, ProjectOrderBy ProjectOrder, Sorting oSorting)
        {
            List<dtoAdminProjectsWithCommunityHeader> oList = new List<dtoAdminProjectsWithCommunityHeader>();
            List<TaskAssignment> list = new List<TaskAssignment>();
            IList<Task> listP = new List<Task>();
            ProjectEqualityComparer projectEqualityComparer = new ProjectEqualityComparer();

            ITransaction tx = session.BeginTransaction();
            try
            {
                foreach (var item in CommunityIdList)
                {

                    switch (ProjectOrder)
                    {
                        case ProjectOrderBy.AllActive:
                            //foreach (var item in CommunityIdList) 
                            list.AddRange(GetTaskAssignmentGeneric(tt => (tt.Task.Level == 0) && (tt.Task.Community.Id == item) && (tt.Task.Status != TaskStatus.completed) && (tt.Task.StartDate <= DateTime.Now)
                            && (tt.MetaInfo.isDeleted == false) && (tt.Task.isPersonal == false) && (tt.Task.isPortal == false)));
                            break;

                        case ProjectOrderBy.AllCompleted:
                            //foreach (var item in CommunityIdList)
                            list.AddRange(GetTaskAssignmentGeneric(tt => (tt.Task.Level == 0) && (tt.Task.Community.Id == item) && (tt.Task.Status == TaskStatus.completed)
                                && (tt.MetaInfo.isDeleted == false) && (tt.Task.isPersonal == false) && (tt.Task.isPortal == false)));
                            break;

                        case ProjectOrderBy.AllFuture:
                            //foreach (var item in CommunityIdList) 
                            list.AddRange(GetTaskAssignmentGeneric(tt => ((tt.Task.Level == 0) && (tt.Task.Community.Id == item) && (tt.Task.Status != TaskStatus.completed) && (tt.Task.StartDate > DateTime.Now))
                                && (tt.MetaInfo.isDeleted == false) && (tt.Task.isPersonal == false) && (tt.Task.isPortal == false)));
                            break;
                    }

                }
                var p = (from tc in list select tc.Project).Distinct(projectEqualityComparer);
                switch (oSorting)
                {

                    case Sorting.DeadlineOrder:
                        listP = p.OrderBy(tc => tc.Community.Name).ThenBy(tc => tc.Deadline).Skip(PageSkip).Take(PageSize).ToList<Task>();
                        break;

                    case Sorting.AlphabeticalOrder:
                        listP = p.OrderBy(tc => tc.Community.Name).ThenBy(tc => tc.Name).Skip(PageSkip).Take(PageSize).ToList<Task>();
                        break;

                    default:
                        listP = p.OrderBy(tc => tc.Deadline).Skip(PageSkip).Take(PageSize).ToList<Task>();
                        break;
                }


                oList = (from tp in listP
                         group tp by tp.Community into g
                         select new dtoAdminProjectsWithCommunityHeader(g.Key,
                             (from tg in g select new dtoAdminProjects(tg)).ToList<dtoAdminProjects>()
                             )).ToList<dtoAdminProjectsWithCommunityHeader>();

                tx.Commit();
            }

            catch (Exception ex)
            {

            }

            return oList;

        }


        #endregion

        //--------------------------------------------------------------------------------------------------------------

        // restituisce l ID e il nome di tutti gli users che sono assegnati con qualsiasi ruolo in aualsiasi task, all interno di un progetto.

        public IList<dtoUsersOnQuickSelection> GetQuickSelectionUsers(long TaskID)
        {
            IList<dtoUsersOnQuickSelection> olist = new List<dtoUsersOnQuickSelection>();
            IList<TaskAssignment> tlist = new List<TaskAssignment>();
            IList<Person> plist = new List<Person>();

            tlist = GetTaskAssignmentGeneric(tt => (tt.Task.TaskParent.ID == TaskID) || (tt.Task.ID == TaskID)).ToList();
            plist = (from t in tlist select t.AssignedUser).Distinct().ToList();
            olist = (from p in plist select new dtoUsersOnQuickSelection(p)).ToList<dtoUsersOnQuickSelection>();
            return olist;
        }

        public IList<dtoUsers> GetQuickSelectionUsersFiltered(long TaskID, TaskRole Role)
        //Selezino tutti gli assegnamenti dell intero progetto e poi tolgo gli assegnamenti col ruolo selezionato al task su cui sto lavorando in mosdo da caricare tutti gli utenti 
        //appartenenti al 
        {
            IList<dtoUsers> olist = new List<dtoUsers>();
            IList<TaskAssignment> listaparziale = new List<TaskAssignment>();
            IList<Person> tuttiUser, filtrati = new List<Person>();
            IList<Person> usedlist = new List<Person>();
            IList<Person> blist = new List<Person>();
            Task oTask = GetTask(TaskID);
            IList<TaskAssignment> a = null; // new List<TaskAssignment>();
            IList<TaskAssignment> b = new List<TaskAssignment>();

            if (oTask.Level == 0)
            {
                a = GetTaskAssignmentGeneric(tt => (tt.Project.ID == TaskID));//.ToList<TaskAssignment> ;                 
            }
            else
            {
                a = GetTaskAssignmentGeneric(tt => (tt.Project.ID == oTask.Project.ID));//.ToList<TaskAssignment>;//.ToList();
            }


            b = (from ta in a where ((ta.Task.ID == TaskID) && (ta.TaskRole == Role)) select ta).ToList<TaskAssignment>(); //GetTaskAssignmentGeneric(tt => (tt.TaskRole == Role ) && (tt.Task.ID == TaskID));

            tuttiUser = (from ta in a group ta by ta.AssignedUser into g3 select g3.Key).ToList<Person>();

            filtrati = (from ta in b group ta by ta.AssignedUser into g2 select g2.Key).ToList<Person>();

            //listTA = (from t in a select t).Except(from p in b select p).ToList<TaskAssignment>();
            //plist = (from t in listTA select t.AssignedUser).Distinct().ToList();

            var d = (from t in a group t by t.AssignedUser into g select g.Key);//.ToList<Person>() ;    
            var e = (d.Except(filtrati)); //.ToList<>();
            olist = (from i in e select new dtoUsers(i)).ToList<dtoUsers>();
            return olist;
        }
        //--------------------------------------------------------------------------------------------------------------

        #region QuickUserSelection 'Tia

        public String GetProjectAllUsersString(long TaskID)
        {
            String oString = "";

            IList<Person> olist = new List<Person>();
            var query = GetTaskAssignmentGeneric(ta => (ta.Task.ID == TaskID) || (ta.Task.Project.ID == TaskID));
            var b = (from ta in query
                     select ta.AssignedUser).Distinct().ToList<Person>();
            foreach (Person operson in b)
            {
                oString = oString + operson.Surname + " ; ";
            }

            return oString;
        }

        //--------------------------------------------------------------------------------------------------------------------------------------------------------------

        public IList<dtoQuickUserSelection> GetQuickSelectionMap(Task InterestedTask, int PersonID, bool onlyActiveTask)
        {
            IList<dtoQuickUserSelection> olist = new List<dtoQuickUserSelection>();
            IList<dtoTaskMap> oDtoMap = GetTaskMap(InterestedTask, PersonID, onlyActiveTask);
            olist = (from o in oDtoMap select new dtoQuickUserSelection(o, this.GetProjectAllUsersString(o.TaskID))).ToList<dtoQuickUserSelection>();
            return olist; ;
        }
        //--------------------------------------------------------------------------------------------------------------------------------------------------------------

        //Dato un qualsiasi progetto o sototask, restituisce la lista dei task che lo compongono
        public IList<Task> GetAllSubTasks(Task SelectedTask)
        {
            IList<Task> olist = null;  //new List<Task>();
            //var q = new IQueryable();

            if (SelectedTask.Level == 0)
            {
                olist = GetIQuerableTaskGeneric(t => (t.Project.ID == SelectedTask.ID) || (t.ID == SelectedTask.ID)).ToList<Task>();
            }

            else
            {
                olist = GetIQuerableTaskGeneric(t => ((t.TaskWBSstring.StartsWith(SelectedTask.TaskWBSindex.ToString())) && (t.Project.ID == SelectedTask.Project.ID))).ToList<Task>(); //|| (t.ID == SelectedTask.ID)

            }

            return olist;
        }



        //Overload del metodo con TaskID invece che Task
        public IList<Task> GetAllSubTasks(long SelectedTaskID)
        {
            IList<Task> olist = null;  //new List<Task>();

            Task SelectedTask = GetTask(SelectedTaskID);

            if (SelectedTask.Level == 0)
            {
                olist = GetIQuerableTaskGeneric(t => (t.Project.ID == SelectedTask.ID) || (t.ID == SelectedTaskID)).ToList<Task>();
            }

            else
            {
                olist = GetIQuerableTaskGeneric(t => (t.TaskWBSstring.StartsWith(SelectedTask.TaskWBSindex.ToString())) && (t.Project.ID == SelectedTask.Project.ID)).ToList<Task>();

            }

            return olist;
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------------

        //Presa una certa sottostruttura del progetto o il progetto intero, calcolo tutti gli assegnamenti dei task componenti assegnati all utente passato

        public IList<TaskAssignment> GetAllSubTasksWithUser(long SelectedTaskID, Person oPerson)
        {
            IList<TaskAssignment> olist = null;  //new List<Task>();

            Task SelectedTask = GetTask(SelectedTaskID);

            if (SelectedTask.Level == 0)
            {
                olist = GetTaskAssignmentGenericIQuerable(ta => (ta.Project.ID == SelectedTask.ID) && (ta.AssignedUser.Id == oPerson.Id)).ToList<TaskAssignment>();
            }
            else
            {
                olist = GetTaskAssignmentGenericIQuerable(ta => (ta.Task.TaskWBSstring.StartsWith(SelectedTask.TaskWBSindex.ToString()) && (ta.Task.Project.ID == SelectedTask.Project.ID) && (ta.AssignedUser.Id == oPerson.Id))).ToList<TaskAssignment>();
            }
            return olist;
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------------

        public IList<Person> GetAllUsersAssignedToTaskSubStructure(long oTaskID)
        {
            IList<Person> olist = new List<Person>();
            IList<Person> tempUserList = new List<Person>();
            IList<TaskAssignment> TAlist = new List<TaskAssignment>();

            Task oTask = GetTask(oTaskID);
            var q = GetAllSubTasks(oTask);
            foreach (Task t in q)
            {
                IList<TaskAssignment> templist = GetTaskAssignmentGenericIQuerable(ta => (ta.Task.ID == t.ID)).ToList<TaskAssignment>();
                //tempUserList = (from ta in templist select new dtoUsersOnQuickSelection(ta.AssignedUser)).Distinct().ToList<dtoUsersOnQuickSelection>();
                tempUserList = (from ta in templist select (ta.AssignedUser)).ToList<Person>();
                olist = olist.Union(tempUserList).ToList();
            }
            olist = olist.Distinct().ToList<Person>();
            return olist;
        }

        //Overload del metodo con Task invece che TaskID

        //public IList<dtoUsersOnQuickSelection> GetAllUsersAssignedToTaskSubStructure(Task oTask)
        //{
        //    IList<dtoUsersOnQuickSelection> olist = new List<dtoUsersOnQuickSelection>();
        //    IList<int> bolist = new List<int>();
        //    IList<dtoUsersOnQuickSelection> tempUserList = new List<dtoUsersOnQuickSelection>();
        //    IList<int> otempUserList = new List<int>();
        //    IList<TaskAssignment> TAlist = new List<TaskAssignment>();
        //    dtoUsersEqualityComparer odtoUsersEqualityComparer = new dtoUsersEqualityComparer();
        //    var q = GetAllSubTasks(oTask);
        //    foreach (Task t in q)
        //    {
        //        IList<TaskAssignment> templist = GetTaskAssignmentGenericIQuerable(ta => (ta.Task.ID == t.ID)).ToList<TaskAssignment>();
        //        otempUserList = (from ta in templist select (ta.AssignedUser.Id)).Distinct().ToList<int>();
        //        bolist = bolist.Union(otempUserList).ToList();
        //    }

        //    olist = (from b in bolist select new dtoUsersOnQuickSelection(GetPerson(b))).ToList<dtoUsersOnQuickSelection>();
        //    return olist;
        //}

        //------------------------------------------------------------------------------------------------------------------------------------

        public IList<Person> GetAllUsersAssignedToTaskSubStructure(Task oTask)
        {
            IList<Person> olist = new List<Person>();
            IList<int> bolist = new List<int>();
            IList<Person> tempUserList = new List<Person>();
            IList<int> otempUserList = new List<int>();
            IList<TaskAssignment> TAlist = new List<TaskAssignment>();
            //dtoUsersEqualityComparer odtoUsersEqualityComparer = new dtoUsersEqualityComparer();
            var q = GetAllSubTasks(oTask);
            foreach (Task t in q)
            {
                IList<TaskAssignment> templist = GetTaskAssignmentGenericIQuerable(ta => (ta.Task.ID == t.ID)).ToList<TaskAssignment>();
                otempUserList = (from ta in templist select (ta.AssignedUser.Id)).Distinct().ToList<int>();
                bolist = bolist.Union(otempUserList).ToList();
            }

            //olist = (from b in bolist select new dtoUsersOnQuickSelection(GetPerson(b))).ToList<Person>();
            olist = (from b in bolist select GetPerson(b)).ToList<Person>();
            return olist;
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------

        //Da una lista di TA caricare la lista dei ruoli ricoperti IList<dtoUsersOnQuickSelection>

        public IList<dtoUsersOnQuickSelection> GetPoweredUserDetail(Task oTask)
        {
            IList<Person> olist = new List<Person>();
            List<dtoUsersOnQuickSelection> dtolist = new List<dtoUsersOnQuickSelection>();
            String oRoles = "";
            int Completeness = 0;
            dtoUsersOnQuickSelection odtoQU = new dtoUsersOnQuickSelection();
            IList<TaskAssignment> TAlist = new List<TaskAssignment>();
            //dtoUsersEqualityComparer odtoUsersEqualityComparer = new dtoUsersEqualityComparer();
            var q = GetAllSubTasks(oTask);
            olist = GetAllUsersAssignedToTaskSubStructure(oTask);

            foreach (Person p in olist)
            {
                IList<TaskAssignment> templist = GetAllSubTasksWithUser(oTask.ID, p);
                oRoles = GetRolesStringFromAssignments(p.Id, templist);
                Completeness = GetCompleteness(p.Id, templist);
                odtoQU = new dtoUsersOnQuickSelection(p, oRoles, Completeness);
                dtolist.Add(odtoQU);
            }
            dtolist = (from b in dtolist select b).OrderBy(b => b.Name).ToList();
            return dtolist;

        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------

        //dato un utente e una lista di Task assignment trovo la lista dei ruoli in cui esso è impiegato all interno dell intero gruppo di TA

        public String GetRolesStringFromAssignments(int UserID, IList<TaskAssignment> TAlist)
        {
            IList<TaskRole> rolist = new List<TaskRole>();
            String ostring = "";
            var Roleslist = (from ta in TAlist where ta.AssignedUser.Id == UserID select ta.TaskRole);
            rolist = Roleslist.Distinct<TaskRole>().ToList<TaskRole>();
            foreach (TaskRole t in rolist)
            {
                ostring = ostring + t.ToString() + " ; ";
            }
            return ostring;
        }


        public int GetCompleteness(int UserID, IList<TaskAssignment> TAlist)
        {
            int tot = 0;
            int S = 0;
            var q = (from ta in TAlist where ta.TaskRole == TaskRole.Resource select ta);

            if (q.Count() != 0)
            {
                foreach (TaskAssignment t in q)
                {
                    S = S + t.Completeness;
                }
                tot = (S / q.Count());
            }

            else

                tot = -1;

            return tot;
        }


        //Da verificare che funzioni o che serva
        public class dtoUsersEqualityComparer : IEqualityComparer<dtoUsersOnQuickSelection>
        {
            public bool Equals(dtoUsersOnQuickSelection x, dtoUsersOnQuickSelection y)
            {

                if (x.Id == y.Id) //&& (x.BlogUri == y.BlogUri))

                    return true;

                else

                    return false;

            }

            public int GetHashCode(dtoUsersOnQuickSelection obj)
            {

                return base.GetHashCode();

            }
        }


        #endregion

        //--------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------

        //--------------------------------------------------------------------------------------------------------------

        //--------------------------------------------------------------------------------------------------------------

        //--------------------------------------------------------------------------------------------------------------

        //--------------------------------------------------------------------------------------------------------------

        //--------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------



        #endregion MATTIA
        //--------------------------------------------------------------------------------------------------------------


        #region TaskMap

        public int GetMaxLevel(List<dtoTaskMap> ListOfTask)
        {
            int MaxLevel = 0;

            try
            {
                MaxLevel = (from t in ListOfTask select t.Level).Max();
            }
            catch (Exception)
            {

                MaxLevel = 0;
            }

            return MaxLevel;
        }


        public IList<dtoTaskMap> GetTaskMap(long InterestedTaskID, int PersonID, bool onlyActiveTask)
        {
            Task InterestedTask = null;

            InterestedTask = GetTask(InterestedTaskID);
            return GetTaskMap(InterestedTask, PersonID, onlyActiveTask);
        }

        public IList<dtoTaskMap> GetTaskMap(Task InterestedTask, int PersonID, bool onlyActiveTask)
        {
            IList<dtoTaskMap> ListDtoTaskMap = new List<dtoTaskMap>();
            IList<Task> ListOfTasks = new List<Task>();
            IList<TaskAssignment> ListTA;
            long ProjectID;


            if (InterestedTask.Level == 0) //campo project non settato
            {
                ProjectID = InterestedTask.ID;
            }
            else
            {
                ProjectID = InterestedTask.Project.ID;
            }
            ListTA = GetTaskAssignments(PersonID, ProjectID, false);
            ListOfTasks.Add(InterestedTask);
            GetChildForTaskMap(InterestedTask, ListOfTasks, onlyActiveTask);
            int isProjectOwner = (from ta in ListTA where ta.TaskRole == TaskRole.ProjectOwner select ta).Count();

            if (isProjectOwner == 1)//sbagliato x livelli intermedi!!! devo vedere TaskAssignment
            {//sono ProjectOwner allora ho tutti i permessi su tutti i task
                TaskPermissionEnum Permission = GetRolePermissions(TaskRole.ProjectOwner);
                foreach (Task t in ListOfTasks)
                {
                    ListDtoTaskMap.Add(new dtoTaskMap(t, Permission));//x ora tutti sullo stesso livello

                }
            }
            else
            {
                TaskPermissionEnum PermissionOverTask;
                foreach (Task t in ListOfTasks)
                {
                    PermissionOverTask = GetPermissionsOverTask(t, ListTA);
                    ListDtoTaskMap.Add(new dtoTaskMap(t, PermissionOverTask));
                }
            }
            Task oFirstElement = ListOfTasks.ElementAt(0);
            if (oFirstElement.Level > 0)
            {
                dtoTaskMap dtoProject = new dtoTaskMap(oFirstElement.Project, GetPermissionsOverTask(oFirstElement.Project.ID, PersonID));
                dtoProject.TaskName = "...";
                ListDtoTaskMap.Insert(0, dtoProject);
            }


            return ListDtoTaskMap;
        }

        //--------------------------------------------------------------------------------------------------------------------------------------------------------------
        public int GetMaxLevel(List<dtoQuickUserSelection> ListOfTask)
        {
            int MaxLevel = 0;

            try
            {
                MaxLevel = (from t in ListOfTask select t.dtoSwitch.Level).Max();
            }
            catch (Exception)
            {

                MaxLevel = 0;
            }

            return MaxLevel;
        }

        private IList<TaskAssignment> GetTaskAssignments(int PersonID, long ProjectID, bool isDeleted)
        {
            return GetTaskAssignmentGeneric(ta => ta.Project.ID == ProjectID
                                                && ta.AssignedUser.Id == PersonID
                                                && ta.MetaInfo.isDeleted == isDeleted);

        }


        //restituisce una lista di task (t1,t1.1,t1.2,t2,t2.1,t2.2,t2.3,t3...)
        private void GetChildForTaskMap(Task Task, IList<Task> Tasks, bool onlyActiveTask)
        {
            IList<Task> OrderChild;
            OrderChild = (from Task t in Task.TaskChildren where t.TaskWBSindex > 0 orderby t.TaskWBSindex select t).ToList();
            if (!onlyActiveTask)
            {
                IList<Task> temp = (from Task t in Task.TaskChildren where t.TaskWBSindex == 0 orderby t.TaskWBSindex select t).ToList();

                if (temp.Count > 0)
                {
                    OrderChild = OrderChild.Concat<Task>(temp).ToList();
                }
            }

            foreach (Task t in OrderChild)
            {
                Tasks.Add(t);

                GetChildForTaskMap(t, Tasks, onlyActiveTask);
            }

        }
        #endregion


        #region Gantt




        public ProjectForGanttXML GetProjectForGantXML(long ProjectID, string BaseUrl)
        {

            ProjectForGanttXML oProject = new ProjectForGanttXML();
            Task oProjectTask = GetTask(ProjectID);

            oProject.Items = (from t in GetAllProject(t => (t.Project.ID == ProjectID || t.ID == ProjectID)
                && !t.MetaInfo.isDeleted)
                              select new TaskForGanttXML(t, GetNumberOfChildren(t.ID, false) == 0, BaseUrl)).ToList();

            oProject.Items = oProject.Items.OrderBy(t => t.pName).ToList();
            oProject.Items.Insert(0, oProject.Items.Last());
            oProject.Items.RemoveAt(oProject.Items.Count - 1);

            return oProject;
        }

        private IList<Task> GetAllProject(Expression<Func<Task, bool>> condition)
        {
            IList<Task> list;
            list = (from Task t in session.Linq<Task>() select t).Where(condition).ToList<Task>();
            list = (from t in list orderby t.WBS ascending select t).ToList();
            return list;
        }

        #endregion


        #region ManageAssignment

        public int GetTaskCommunity(long TaskID)
        {
            int CommunityID = 0;
            Task oTask = GetTask(TaskID);
            if (!oTask.isPortal)
            {
                CommunityID = oTask.Community.Id;
            }

            return CommunityID;
        }


        public TaskRole GetTaskRole(long TaskID, int PersonID)
        {
            TaskRole Role;
            IQueryable<TaskAssignment> listTA = GetTaskAssignmentGenericQuery(t => (t.AssignedUser.Id == PersonID) && (t.Task.ID == TaskID) && (!t.MetaInfo.isDeleted));
            // IList<TaskAssignment> listT = GetTaskAssignmentGeneric(t => (t.AssignedUser.Id == PersonID) && (t.Task.ID == TaskID) && (!t.MetaInfo.isDeleted));
            // (from t in listT select t.TaskRole).First<TaskRole>();//.Min<TaskRole>;

            try
            {
                Role = (from t in listTA select t.TaskRole).Min<TaskRole>();
            }
            catch (Exception e)
            {

                Role = TaskRole.None;
            }
            return Role;
        }

        public TaskPermissionEnum GetPermissionsOverTask(long TaskID, int PersonID)
        {

            TaskPermissionEnum PermissionEnum = TaskPermissionEnum.None;
            Task oTask = GetTask(TaskID);
            try
            {
                IQueryable<TaskAssignment> listTA = GetTaskAssignmentGenericQuery(t => (t.AssignedUser.Id == PersonID) &&
                                                                                 (t.Task.ID == TaskID) && (!t.MetaInfo.isDeleted));
                IList<TaskRole> ListRole = (from t in listTA select t.TaskRole).ToList<TaskRole>();
                if (oTask.Level > 0)
                {
                    if (oTask.Project.MetaInfo.CreatedBy.Id == PersonID)
                    {
                        ListRole.Add(TaskRole.ProjectOwner);
                    }
                }
                foreach (TaskRole item in ListRole)
                {

                    PermissionEnum |= GetRolePermissions(item);
                }
            }
            catch (Exception)
            {


            }

            return PermissionEnum;
        }


        public TaskPermissionEnum GetPermissionOverAllProject(long ProjectID, int PersonID)
        {
            TaskPermissionEnum PermissionEnum = TaskPermissionEnum.None;
            IQueryable<TaskAssignment> QueryableTA = GetTaskAssignmentGenericIQuerable(ta => ta.AssignedUser.Id == PersonID && (ta.Project.ID == ProjectID || ta.Task.ID == ProjectID)
                && ta.MetaInfo.isDeleted == false);
            IList<TaskRole> ListTA = (from ta in QueryableTA select ta.TaskRole).Distinct().ToList<TaskRole>();

            foreach (TaskRole role in ListTA)
            {

                PermissionEnum |= GetRolePermissions(role);
            }
            return PermissionEnum;
        }



        private TaskPermissionEnum GetPermissionsOverTask(Task oTask, IList<TaskAssignment> ListOfAssignment)
        {

            TaskPermissionEnum PermissionEnum = TaskPermissionEnum.None;
            IList<TaskRole> ListRole = (from TaskAssignment ta in ListOfAssignment where ta.Task.ID == oTask.ID select ta.TaskRole).ToList<TaskRole>();
            foreach (TaskRole item in ListRole)
            {
                PermissionEnum |= GetRolePermissions(item);
            }
            return PermissionEnum;
        }

        #endregion

        #region SwichTaskMap

        public int GetMaxLevel(List<dtoSwichTask> ListOfTask)
        {
            int MaxLevel = 0;

            try
            {
                MaxLevel = (from t in ListOfTask select t.Level).Max();
            }
            catch (Exception)
            {

                MaxLevel = 0;
            }

            return MaxLevel;
        }

        public IList<dtoSwichTask> GetSwichTaskMap(long InterestedTaskID)
        {
            Task InterestedTask = null;

            InterestedTask = GetTask(InterestedTaskID);
            return GetSwichTaskMap(InterestedTask);
        }

        public IList<dtoSwichTask> GetSwichTaskMap(Task InterestedTask)
        {
            IList<dtoSwichTask> ListDtoSwichTask = new List<dtoSwichTask>();
            ListDtoSwichTask.Add(new dtoSwichTask(InterestedTask, true, true));
            GetChildForSwichTaskMap(InterestedTask, ListDtoSwichTask);

            return ListDtoSwichTask;
        }


        public bool CanSwichTaskWBSPosition(long ProjectID, int PersonID)
        {
            int n = (from t in session.Linq<TaskAssignment>() where t.AssignedUser.Id == PersonID && t.Task.ID == ProjectID && (t.TaskRole == TaskRole.Manager || t.TaskRole == TaskRole.ProjectOwner) select t.ID).Count();
            if (n > 0)
            {
                n = (from t in session.Linq<Task>() where t.Project.ID == ProjectID && t.MetaInfo.isDeleted == false select t).Count();
                if (n > 0)
                {
                    return true;
                }

            }
            return false;
        }


        private void GetChildForSwichTaskMap(Task Task, IList<dtoSwichTask> ListDtoSwichTask)
        {
            IList<Task> OrderChild = (from Task t in Task.TaskChildren where t.TaskWBSindex > 0 orderby t.TaskWBSindex select t).ToList();
            Task oTask;
            bool isFirst, isLast;
            for (int i = 0; i < OrderChild.Count; i++)
            {
                if ((i == 0) && (0 == (OrderChild.Count - 1)))
                {
                    isFirst = true;
                    isLast = true;
                }
                else if (i == 0)
                {
                    isFirst = true;
                    isLast = false;
                }
                else if (i == (OrderChild.Count - 1))
                {
                    isFirst = false;
                    isLast = true;
                }
                else
                {
                    isFirst = false;
                    isLast = false;
                }
                oTask = OrderChild.ElementAt<Task>(i);
                ListDtoSwichTask.Add(new dtoSwichTask(oTask, isFirst, isLast));
                GetChildForSwichTaskMap(oTask, ListDtoSwichTask);
            }
        }


        #endregion

        //--------------------------------------------------------------------------------------------------------------------------------------------------------------




        #region TaskDetail


        public dtoTaskDetail GetTaskDetail(long TaskID)
        {
            dtoTaskDetail Detail;
            Task oTask = GetTask(TaskID);
            Detail = new dtoTaskDetail(oTask);
            return Detail;
        }

        //MD
        public dtoTaskDetailWithPermission GetTaskDetailWithAdminPermission(long TaskID)
        {
            Task oTask = GetTask(TaskID);
            //IList<TaskAssignment> ListTA;
            dtoTaskDetailWithPermission TaskDetailWithPermission = null;
            dtoTaskDetail TaskDetail = null;
            TaskPermissionEnum Permission;
            Permission = GetRolePermissions(TaskRole.ProjectOwner);
            TaskDetail = new dtoTaskDetail(oTask);
            TaskDetailWithPermission = new dtoTaskDetailWithPermission(TaskDetail, Permission);

            return TaskDetailWithPermission;

        }


        public dtoTaskDetailWithPermission GetTaskDetailWithPermission(long TaskID, int PersonID)
        {
            Task oTask = GetTask(TaskID);
            IList<TaskAssignment> ListTA;
            // IList<dtoTaskDetailWithPermission> ListTaskDetail = new List<dtoTaskDetailWithPermission>();
            dtoTaskDetailWithPermission TaskDetailWithPermission = null;
            dtoTaskDetail TaskDetail = null;
            TaskPermissionEnum Permission;
            Boolean isProjectOwner = false, isResource = false, isManager = false, isVisitor = false;

            if (oTask.Level != 0)
            {
                if (PersonID == oTask.Project.MetaInfo.CreatedBy.Id) //sono ProjectOwner
                {
                    isProjectOwner = true;
                }
            }
            //recupero tutti gli assegnamenti
            ListTA = GetTaskAssignmentGeneric(ta => ta.Task.ID == TaskID &&
                                                ta.AssignedUser.Id == PersonID &&
                                                ta.MetaInfo.isDeleted == false);

            if (ListTA.Count != 0)//verifico se ho ricevuto assegnamenti
            {
                foreach (TaskAssignment item in ListTA)
                {
                    if (item.TaskRole == TaskRole.Resource || item.TaskRole == TaskRole.Customized_Resource)
                    {
                        isResource = true;
                        TaskDetail = new dtoTaskDetail(item);
                    }
                    else if (item.TaskRole == TaskRole.Manager)
                    {
                        isManager = true;
                    }
                    else if (item.TaskRole == TaskRole.ProjectOwner)
                    {
                        isProjectOwner = true;
                    }
                    else if (item.TaskRole == TaskRole.Visitor)
                    {
                        isVisitor = true;
                    }
                }

                if (isProjectOwner)//sono  Project Owner, ma ho altri assegnamenti
                {
                    Permission = GetRolePermissions(TaskRole.ProjectOwner);
                    if (isResource)
                    {
                        Permission |= TaskPermissionEnum.TaskSetCompleteness;
                        TaskDetailWithPermission = new dtoTaskDetailWithPermission(TaskDetail, Permission);
                    }
                    else
                    {
                        TaskDetail = new dtoTaskDetail(oTask);
                        TaskDetailWithPermission = new dtoTaskDetailWithPermission(TaskDetail, Permission);
                    }

                }
                else //non sono ProjectOwner ma sono stato assegnato
                {
                    if (isProjectOwner)
                    {
                        Permission = GetRolePermissions(TaskRole.ProjectOwner);
                        if (isResource)
                        {
                            Permission |= TaskPermissionEnum.TaskSetCompleteness;
                            TaskDetailWithPermission = new dtoTaskDetailWithPermission(TaskDetail, Permission);
                        }
                        else
                        {
                            TaskDetail = new dtoTaskDetail(oTask);
                            TaskDetailWithPermission = new dtoTaskDetailWithPermission(TaskDetail, Permission);
                        }
                    }
                    else if (isManager)
                    {
                        Permission = GetRolePermissions(TaskRole.Manager);
                        if (isResource)
                        {
                            Permission |= TaskPermissionEnum.TaskSetCompleteness;
                            TaskDetailWithPermission = new dtoTaskDetailWithPermission(TaskDetail, Permission);
                        }
                        else
                        {
                            TaskDetail = new dtoTaskDetail(oTask);
                            TaskDetailWithPermission = new dtoTaskDetailWithPermission(TaskDetail, Permission);
                        }
                    }
                    else if (isResource)
                    {
                        Permission = GetRolePermissions(TaskRole.Resource);
                        TaskDetailWithPermission = new dtoTaskDetailWithPermission(TaskDetail, Permission);
                    }
                    else if (isVisitor)
                    {
                        Permission = GetRolePermissions(TaskRole.Visitor);
                        TaskDetailWithPermission = new dtoTaskDetailWithPermission(new dtoTaskDetail(oTask), Permission);
                    }
                }


            }
            else //nn sono stato assegnato
            {
                if (isProjectOwner)//sono solo Project Owner
                {
                    Permission = GetRolePermissions(TaskRole.ProjectOwner);
                    TaskDetail = new dtoTaskDetail(oTask);
                    TaskDetailWithPermission = new dtoTaskDetailWithPermission(TaskDetail, Permission);
                }
                else //nessun assegnamento
                {
                    TaskDetailWithPermission = new dtoTaskDetailWithPermission(new dtoTaskDetail(oTask), TaskPermissionEnum.None);
                }

            }

            return TaskDetailWithPermission;
        }

        //-------------------------------------------------------------------------------------------------------------------------------------
        //public dtoTaskDetailWithPermission GetTaskDetailWithPermissionAdminMode(TaskRole Role, IList<ModuleCommunityPermission<lm.Comol.Modules.TaskList.ModuleTasklist>> ComPermission)
        //    {
        //        //int count =  (from c in ComPermission where c.Permissions.Administratio).Count;
        //        if ((Role == TaskRole.ProjectOwner) && (from c in ComPermission where c.Permissions.Administration select c).Count() > 0)

        //        {

        //        }

        //        return 0;
        //    }

        #endregion


        #region For UC Assigned User

        public IList<dtoTaskAssignment> GetPagedTaskAssignment(long TaskID, bool onlyActiveAssignment, int PageSize, int PageSkip)
        {
            IList<dtoTaskAssignment> ListDto = null;
            IList<TaskAssignment> ListTA = null;
            int taskSkip = PageSize * PageSkip;
            if (onlyActiveAssignment)
            {
                ListTA = GetTaskAssignmentGenericQuery(t => t.Task.ID == TaskID && t.MetaInfo.isDeleted == false)
                                                    .Skip(taskSkip).Take(PageSize).ToList<TaskAssignment>();
            }
            else
            {
                ListTA = GetTaskAssignmentGenericQuery(t => t.Task.ID == TaskID)
                                                    .Skip(taskSkip).Take(PageSize).ToList<TaskAssignment>();
            }

            ListDto = (from ta in ListTA select new dtoTaskAssignment(ta)).ToList<dtoTaskAssignment>();


            return ListDto;
        }

        public IList<dtoTaskAssignment> GetPagedTaskAssignment(long TaskID, TaskRole Role, int PageSize, int PageSkip, bool onlyActiveAssignment)
        {
            IList<dtoTaskAssignment> ListDto = null;
            IList<TaskAssignment> ListTA = null;
            int taskSkip = PageSize * PageSkip;
            if (onlyActiveAssignment)
            {
                ListTA = GetTaskAssignmentGenericQuery(t => t.Task.ID == TaskID && t.TaskRole == Role && t.MetaInfo.isDeleted == false)
                                                    .Skip(taskSkip).Take(PageSize).ToList<TaskAssignment>();
            }
            else
            {
                ListTA = GetTaskAssignmentGenericQuery(t => t.Task.ID == TaskID && t.TaskRole == Role)
                                                    .Skip(taskSkip).Take(PageSize).ToList<TaskAssignment>();
            }

            ListDto = (from ta in ListTA select new dtoTaskAssignment(ta)).ToList<dtoTaskAssignment>();


            return ListDto;
        }

        public IList<dtoTaskAssignment> GetPagedTaskAssignment(long TaskID, TaskRole Role, bool isDeleted, int PageSize, int PageSkip)
        {
            IList<dtoTaskAssignment> ListDto = null;
            IList<TaskAssignment> ListTA = null;
            int taskSkip = PageSize * PageSkip;

            ListTA = GetTaskAssignmentGenericQuery(t => t.Task.ID == TaskID && t.MetaInfo.isDeleted == isDeleted && t.TaskRole == Role)
                                                    .Skip(taskSkip).Take(PageSize).ToList();


            ListDto = (from ta in ListTA select new dtoTaskAssignment(ta)).ToList<dtoTaskAssignment>();


            return ListDto;
        }

        public IList<dtoTaskAssignment> GetPagedTaskAssignment(long TaskID, TaskRole Role, int PageSize, int PageSkip)
        {
            IList<dtoTaskAssignment> ListDto = null;
            IList<TaskAssignment> ListTA = null;
            int taskSkip = PageSize * PageSkip;

            ListTA = GetTaskAssignmentGenericQuery(t => t.Task.ID == TaskID && t.TaskRole == Role)
                                                    .Skip(taskSkip).Take(PageSize).ToList();


            ListDto = (from ta in ListTA select new dtoTaskAssignment(ta)).ToList<dtoTaskAssignment>();


            return ListDto;
        }
        public int GetTaskAssignmentCount(long TaskID)
        {
            int taskcount = 0;

            taskcount = GetTaskAssignmentGenericCount(tt => (tt.MetaInfo.isDeleted == false) &&
                                                    (tt.Task.ID == TaskID));
            return taskcount;
        }

        public int GetTaskAssignmentCount(long TaskID, TaskRole Role, bool isDeleted)
        {
            int taskcount;

            taskcount = GetTaskAssignmentGenericCount(tt => (tt.MetaInfo.isDeleted == isDeleted) &&
                                                    (tt.Task.ID == TaskID) && (tt.TaskRole == Role));


            return taskcount;
        }

        public int GetTaskAssignmentCount(long TaskID, TaskRole Role)
        {
            int taskcount;

            taskcount = GetTaskAssignmentGenericCount(tt => (tt.Task.ID == TaskID) && (tt.TaskRole == Role));


            return taskcount;
        }


        public int GetTasksAssignmentCount(int PersonId, TaskFilter Filter, int CommunityId)
        {
            int taskcount = 0;
            switch (Filter)
            {
                case TaskFilter.AllCommunities:
                    taskcount = GetTaskAssignmentGenericCount(tt => (tt.MetaInfo.isDeleted == false) && (tt.TaskRole == TaskRole.Resource) && (tt.AssignedUser.Id == PersonId));
                    break;

                case TaskFilter.CommunityPersonal:
                    taskcount = GetTaskAssignmentGenericCount(tt => (tt.MetaInfo.isDeleted == false) && (tt.TaskRole == TaskRole.Resource) && (tt.AssignedUser.Id == PersonId) && (tt.Task.Community.Id == CommunityId) && (tt.Task.isPersonal == true));
                    break;
                case TaskFilter.CurrentCommunity:
                    taskcount = GetTaskAssignmentGenericCount(tt => (tt.MetaInfo.isDeleted == false) && (tt.TaskRole == TaskRole.Resource) && (tt.AssignedUser.Id == PersonId) && (tt.Task.Community.Id == CommunityId));
                    break;
                case TaskFilter.PortalPersonal:
                    taskcount = GetTaskAssignmentGenericCount(tt => (tt.MetaInfo.isDeleted == false) && (tt.TaskRole == TaskRole.Resource) && (tt.AssignedUser.Id == PersonId) && (tt.Task.Community == null) && (tt.Task.isPersonal == true));
                    break;

                default:
                    taskcount = GetTaskAssignmentGenericCount(tt => (tt.MetaInfo.isDeleted == false) && (tt.TaskRole == TaskRole.Resource) && (tt.AssignedUser.Id == PersonId));
                    break;
            }

            return taskcount;
        }
        #endregion

        #region TaskWBS

        private int GetMaxWBSindex(int Level, long TaskParentID)
        {
            int maxIndex;
            IQueryable<Task> ListOfTask = GetIQuerableTaskGeneric(t => (t.TaskParent.ID == TaskParentID) && (t.Level == Level)
                                                               && (!t.MetaInfo.isDeleted));
            try
            {
                maxIndex = (from Task t in ListOfTask select t.TaskWBSindex).Max();
            }
            catch (Exception)
            {
                maxIndex = 0;
            }
            return maxIndex;
        }

        private void SetWBSforNewTask(Task oTask)
        {
            switch (oTask.Level)
            {
                case 0:
                    oTask.TaskWBSstring = "";
                    oTask.TaskWBSindex = 0;
                    break;

                case 1:
                    oTask.TaskWBSstring = "";
                    oTask.TaskWBSindex = GetMaxWBSindex(1, oTask.TaskParent.ID) + 1;
                    break;
                case 2:
                    oTask.TaskWBSstring = oTask.TaskParent.TaskWBSindex + ".";
                    oTask.TaskWBSindex = GetMaxWBSindex(oTask.Level, oTask.TaskParent.ID) + 1;
                    break;
                default:
                    oTask.TaskWBSstring = oTask.TaskParent.TaskWBSstring + oTask.TaskParent.TaskWBSindex + ".";
                    oTask.TaskWBSindex = GetMaxWBSindex(oTask.Level, oTask.TaskParent.ID) + 1;
                    break;
            }
        }


        private void UpdateWBSforMeAndChildren(Task oTask, String WBSstring, int valueToUpdateWBSindex)
        {
            if (oTask.Level > 0)
            {
                oTask.TaskWBSstring = WBSstring;

                oTask.TaskWBSindex += valueToUpdateWBSindex;

                SaveOrUpdateTask(oTask);

                String WBSforChildren = "";

                switch (oTask.Level)
                {
                    case 1:
                        WBSforChildren = oTask.TaskWBSindex + ".";
                        break;

                    default:
                        WBSforChildren = oTask.TaskWBSstring + oTask.TaskWBSindex + ".";
                        break;
                }


                foreach (Task child in oTask.TaskChildren)
                {
                    //if (!child.MetaInfo.isDeleted)
                    //{
                    UpdateWBSforMeAndChildren(child, WBSforChildren, 0);
                    //}
                }
            }


        }

        private void UpdateWBSforBrothers(Task oTask)
        {
            IList<Task> ListOfBrother;
            ListOfBrother = GetTaskGeneric(t => (t.TaskParent.ID == oTask.TaskParent.ID) && (t.Level == oTask.Level) &&
                                           (!t.MetaInfo.isDeleted) && (t.TaskWBSindex > oTask.TaskWBSindex));
            oTask.TaskWBSindex = 0;
            foreach (Task brother in ListOfBrother)
            {

                UpdateWBSforMeAndChildren(brother, brother.TaskWBSstring, -1);//-1 perchè la funzione viene chiamata quando cancello il task precedente
            }

        }

        public bool SwitchTasksWBSposition(Task oNewTaskPrevius, Task oNewTaskNext)
        {
            bool isTranctionConclused = true;
            ITransaction tx = session.BeginTransaction();
            try
            {
                UpdateWBSforMeAndChildren(oNewTaskPrevius, oNewTaskPrevius.TaskWBSstring, -1);
                UpdateWBSforMeAndChildren(oNewTaskNext, oNewTaskNext.TaskWBSstring, +1);
                tx.Commit();
            }
            catch (Exception ex)
            {
                isTranctionConclused = false;
                Console.WriteLine(ex);
                if (tx != null && tx.IsActive)
                {
                    tx.Rollback();
                }
            }
            return isTranctionConclused;
        }

        public bool MoveTaskWbsPrevius(long TaskID)
        {
            Task oNewTaskPrevius, oNewTaskNext;
            oNewTaskPrevius = GetTask(TaskID);
            int newWbsIndex = oNewTaskPrevius.TaskWBSindex - 1;

            try
            {
                oNewTaskNext = GetIQuerableTaskGeneric(t => (t.TaskParent.ID == oNewTaskPrevius.TaskParent.ID) && (t.TaskWBSindex == newWbsIndex)).First();

            }
            catch (Exception e)
            {
                Console.WriteLine("MoveTaskWbsPrevius: " + e);
                return false;
            }

            return SwitchTasksWBSposition(oNewTaskPrevius, oNewTaskNext);
        }

        public bool MoveTaskWbsNext(long TaskID)
        {
            Task oNewTaskPrevius, oNewTaskNext;
            oNewTaskNext = GetTask(TaskID);
            int newWbsIndex = oNewTaskNext.TaskWBSindex + 1;

            try
            {
                oNewTaskPrevius = GetIQuerableTaskGeneric(t => ((t.TaskWBSindex == newWbsIndex) && (t.TaskParent.ID == oNewTaskNext.TaskParent.ID))).First();

            }
            catch (Exception e)
            {
                Console.WriteLine("MoveTaskWbsNext: " + e);
                return false;
            }

            return SwitchTasksWBSposition(oNewTaskPrevius, oNewTaskNext);
        }


        #endregion


        #region selectTask

        public int GetMaxLevel(List<dtoSelectTask> ListOfTask)
        {
            int MaxLevel = 0;

            try
            {
                MaxLevel = (from t in ListOfTask select t.Level).Max();
            }
            catch (Exception)
            {

                MaxLevel = 0;
            }

            return MaxLevel;
        }

        public IList<dtoSelectTask> GetSelectTaskMap(long InterestedTaskID, int PersonID)
        {
            Task InterestedTask = null;

            InterestedTask = GetTask(InterestedTaskID);
            return GetSelectTaskMap(InterestedTask, PersonID);
        }

        public IList<dtoSelectTask> GetSelectTaskMap(Task InterestedTask, int PersonID)
        {
            IList<dtoSelectTask> ListDtoSelectTask = new List<dtoSelectTask>();
            TaskPermissionEnum Permission = GetPermissionsOverTask(InterestedTask.ID, PersonID);
            ListDtoSelectTask.Add(new dtoSelectTask(InterestedTask, Permission));
            GetChildForSelectTaskMap(InterestedTask, PersonID, ListDtoSelectTask);


            return ListDtoSelectTask;
        }

        private void GetChildForSelectTaskMap(Task Task, int PersonID, IList<dtoSelectTask> ListDtoSwichTask)
        {
            IList<Task> OrderChild = (from Task t in Task.TaskChildren where t.TaskWBSindex > 0 orderby t.TaskWBSindex select t).ToList();
            Task oTask;
            TaskPermissionEnum Permission;
            for (int i = 0; i < OrderChild.Count; i++)
            {
                oTask = OrderChild.ElementAt<Task>(i);
                Permission = GetPermissionsOverTask(oTask.ID, PersonID);
                ListDtoSwichTask.Add(new dtoSelectTask(oTask, Permission));
                GetChildForSelectTaskMap(oTask, PersonID, ListDtoSwichTask);
            }
        }

        #endregion

        //controllo effettuato affinche non si cancelli l ultimo manager di un task
        public bool CanDeleteManagers(long TaskID)
        {
            int n = GetTaskAssignmentGenericCount(ta => ta.Task.ID == TaskID && !ta.MetaInfo.isDeleted && ta.TaskRole == TaskRole.Manager);

            if (n == 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        #region Permessi in base al ruolo
        public TaskPermissionEnum GetRolePermissions(TaskRole Role)
        {
            TaskPermissionEnum Permission;
            switch (Role)
            {

                case TaskRole.ProjectOwner:
                    Permission = TaskPermissionEnum.AddFile | TaskPermissionEnum.ManagementUser | TaskPermissionEnum.ProjectDelete
                        | TaskPermissionEnum.TaskCreate | TaskPermissionEnum.TaskDelete | TaskPermissionEnum.TaskSetCategory
                        | TaskPermissionEnum.TaskSetDeadline | TaskPermissionEnum.TaskSetEndDate | TaskPermissionEnum.TaskSetPriority | TaskPermissionEnum.TaskSetStartDate
                        | TaskPermissionEnum.TaskSetStatus | TaskPermissionEnum.TaskView | TaskPermissionEnum.TreeVisibility;//TaskPermissionEnum.TaskDelete | TaskPermissionEnum.TaskAddFile | TaskPermissionEnum.TaskSetDeadline | TaskPermissionEnum.TaskSetCategory | TaskPermissionEnum.TaskSetPriority | TaskPermissionEnum.TaskSetEndDate | TaskPermissionEnum.TaskSetStartDate | TaskPermissionEnum.TaskSetStatus | TaskPermissionEnum.TaskSetCompleteness | TaskPermissionEnum.TaskUpdate | TaskPermissionEnum.TaskDelete | TaskPermissionEnum.TaskCreate; //da finire           
                    break;
                case TaskRole.Manager:
                    Permission = TaskPermissionEnum.AddFile | TaskPermissionEnum.ManagementUser
                       | TaskPermissionEnum.TaskCreate | TaskPermissionEnum.TaskDelete | TaskPermissionEnum.TaskSetCategory
                       | TaskPermissionEnum.TaskSetDeadline | TaskPermissionEnum.TaskSetEndDate | TaskPermissionEnum.TaskSetPriority | TaskPermissionEnum.TaskSetStartDate
                       | TaskPermissionEnum.TaskSetStatus | TaskPermissionEnum.TaskView | TaskPermissionEnum.TreeVisibility;
                    break;
                case TaskRole.Resource:
                    Permission = TaskPermissionEnum.AddFile | TaskPermissionEnum.TaskSetCompleteness
                       | TaskPermissionEnum.TaskView | TaskPermissionEnum.TreeVisibility;
                    break;
                case TaskRole.Visitor: //---->>>Aggiungere anche ViewFile!?!?!?!?!?!?!
                    Permission = TaskPermissionEnum.TaskView;
                    break;
                default:
                    Permission = TaskPermissionEnum.None;
                    break;
            }
            return Permission;
        }

        #endregion

    }
}


