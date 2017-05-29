using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Repository;
using lm.Comol.Core.BaseModules.FileStatistics.Domain;
using lm.Comol.Core.Business;

namespace lm.Comol.Core.BaseModules.Scorm.Business
{
    public class ServiceFileStatistics :CoreServices, iLinkedService
    {
        private iApplicationContext _Context;

        #region iLinkedService Members
        //Effettivamente utilizzati
       

        public void SaveActionExecution(ModuleLink link, Boolean isStarted, Boolean isPassed, short Completion, Boolean isCompleted, Int16 mark, Int32 idUser, Dictionary<String, long> moduleUserLong = null, Dictionary<String, String> moduleUserString = null)
        {
            //throw new NotImplementedException();
        }
        public void SaveActionsExecution(List<dtoItemEvaluation<ModuleLink>> evaluatedLinks, Int32 idUser, Dictionary<String, long> moduleUserLong = null, Dictionary<String, String> moduleUserString = null)
        {
            //throw new NotImplementedException();
        }

        //Non utilizzati... ??
        public bool AllowActionExecution(ModuleLink link, Int32 idUser, Int32 idCommunity, Int32 idRole, Dictionary<String, long> moduleUserLong = null, Dictionary<String, String> moduleUserString = null)
        {
            //throw new NotImplementedException();
            return true;
        }
        public List<StandardActionType> GetAllowedStandardAction(ModuleObject source, ModuleObject destination, Int32 idUser, Int32 idRole, Int32 idCommunity, Dictionary<String, long> moduleUserLong = null, Dictionary<String, String> moduleUserString = null)
        {
            return new List<StandardActionType>();
        }
        public bool AllowStandardAction(StandardActionType actionType, ModuleObject source, ModuleObject destination, Int32 idUser, Int32 idRole, Dictionary<String, long> moduleUserLong = null, Dictionary<String, String> moduleUserString = null)
        {
            //throw new NotImplementedException();
            return true;
        }
        public void PhisicalDeleteCommunity(int idCommunity, int idUser, string baseFilePath, string baseThumbnailPath)
        {

        }
        public void PhisicalDeleteRepositoryItem(long idFileItem, Int32 idCommunity, Int32 idUser, Dictionary<String, long> moduleUserLong = null, Dictionary<String, String> moduleUserString = null)
        {
            //throw new NotImplementedException();
        }
        public StatTreeNode<StatFileTreeLeaf> GetObjectItemFilesForStatistics(long objectId, Int32 objectTypeId, Dictionary<Int32, string> translations, Int32 idCommunity, Int32 idUser, Dictionary<String, long> moduleUserLong = null, Dictionary<String, String> moduleUserString = null)
        {
            return new StatTreeNode<StatFileTreeLeaf>();
        }
        #endregion

       

        public IDictionary<String, Int32> GetDonwloadStat(IList<Int32> UsersIds, IList<Int64> FilesIds)
        {
            //Qui ho TUTTI i DOWNLOADINFO...
            //dovrei raggrupparli e contarli
            //List<FileDownloadInfo> AllPermFDIs = 
            //    (from f in Manager.GetIQ<FileDownloadInfo>()
            //     where FilesIds.Contains(f.File.Id) && UsersIds.Contains(f.Downloader.Id)
            //     select f)
            //     .ToList();


            //var x = (from f in Manager.GetIQ<FileDownloadInfo>()
            //         where FilesIds.Contains(f.File.Id) && UsersIds.Contains(f.Downloader.Id)
            //         group f by () into di
            //         select new { Key = di, NumDownload = di.Count() })
            //         ;

            
            var x = (from f in Manager.GetAll<FileDownloadInfo>(f => (FilesIds.Contains(f.File.Id) && UsersIds.Contains(f.Downloader.Id)))
                     select new { Key = f.Downloader.Id.ToString() + "-" + f.File.Id.ToString() })
                     ;

            //var xl = x.ToList();

            var z = from f in x
                    group f by f.Key into di
                    select new { MyKey = di.Key, NumDown = di.Count() };

            //var y = z.ToList();


            return z.ToDictionary(k => k.MyKey, k => k.NumDown);
            //return null;
        }

        public IDictionary<String, Int32> GetDonwloadStat(Int32 UserId, IList<Int64> FilesIds)
        {
            var x = (from f in Manager.GetAll<FileDownloadInfo>(f => (FilesIds.Contains(f.File.Id) && (UserId == f.Downloader.Id)))
                     select new { Key = f.Downloader.Id.ToString() + "-" + f.File.Id.ToString() })
                     ;

            //var xl = x.ToList();

            var z = from f in x
                    group f by f.Key into di
                    select new { MyKey = di.Key, NumDown = di.Count() };

            //var y = z.ToList();


            return z.ToDictionary(k => k.MyKey, k => k.NumDown);
        }


        #region initClass
        public ServiceFileStatistics() { }
            public ServiceFileStatistics(iApplicationContext oContext)
            {
                throw new NullReferenceException("No nHibernate session!!!");
               
                this.Manager = new BaseModuleManager(oContext.DataContext);
                this.UC = oContext.UserContext;
                //_Context = oContext;
                //this.UC = oContext.UserContext;
            }
            public ServiceFileStatistics(iApplicationContext oContext, NHibernate.ISession ComolSession) {
                _Context = oContext;
                this.Manager = new BaseModuleManager(oContext.DataContext);

                this.UC = oContext.UserContext;
            }
            //public ServiceFileStatistics(iDataContext oDC)
            //{
            //    this.Manager = new BaseModuleManager(oDC);
            //    _Context = new ApplicationContext();
            //    _Context.DataContext = oDC;
            //    this.UC = null;
            //}

        #endregion
        
      
        #region iLinkedService Members

        /// <summary>
        /// Mirco: Che è sta roba?!?!?!?!??!--> STATISTICHE INDIPENDENTI DALLA SORGENTE , DA MOSTRARE A VIDEO IN UN ALBERO DOVE OGNI PADRE RAPPRESENTA L'OGGETTO PROPRIETARIO DEL FILE....
        /// </summary>
        /// <param name="IdCommunity"></param>
        /// <param name="UserID"></param>
        /// <param name="objectId"></param>
        /// <param name="objectTypeId"></param>
        /// <returns></returns>
        public StatTreeNode<StatFileTreeLeaf> GetObjectItemFilesForStatistics(int IdCommunity, int UserID, long objectId, int objectTypeId)
        {
            return new StatTreeNode<StatFileTreeLeaf>();
        }

        #endregion

        #region Detailed Stats
        
        public dtoFileDetail GetDetailFile(IList<Int32> UsersId, Int64 FileId, IDictionary<String, String> translateServiceCode)
        {
            dtoFileDetail FileInfo = new dtoFileDetail();
            
            // 1. Col_Manager -> ColBaseCommunityFile (per le info sul file).
            //      La proprietà IsInternal mi dice se appartiene ad un servizio o alla comunità.
            BaseCommunityFile File = Manager.Get<BaseCommunityFile>(FileId);


            FileInfo.FileName = File.Name;
            
            if (File.IsInternal)
            {
                FileInfo.ComService = "Service";
                FileInfo.Path = "";
            } else {
                FileInfo.ComService = File.CommunityOwner.Name;
                FileInfo.Path = File.FilePath;
            }

            FileInfo.Size = File.Size;
            FileInfo.LoadedBy = File.Owner.SurnameAndName;
            FileInfo.LoadedOn = File.CreatedOn;
            FileInfo.Downloads = File.Downloads;

            FileInfo.isInternal = File.IsInternal;

            
            // 2. da filelink recupero le info sui download (riferiti ad un singolo file)
            //

            //var x = (from f in Manager.GetAll<FileDownloadInfo>(f => (FilesIds.Contains(f.File.Id) && UsersIds.Contains(f.Downloader.Id)))
            //         select new { Key = f.Downloader.Id.ToString() + "-" + f.File.Id.ToString() });

            IList<Person> Users = Manager.GetAll<Person>(p => (UsersId.Contains(p.Id))).ToList();
            FileInfo.DownDetails = new List<dtoUserDownInfo>();

            foreach(Person prsn in Users)
            {
                dtoUserDownInfo UDI = new dtoUserDownInfo();
                UDI.downBy = prsn.SurnameAndName;
                UDI.downOnList = GetdtoDownInfos(prsn.Id, FileId, translateServiceCode);
                if (UDI.downOnList == null)
                {
                    UDI.downOnList = new List<dtoDownInfo>();
                }
                UDI.TotalDownload = UDI.downOnList.Count(); //Eventualmente metterlo "fuori" o toglierlo del tutto...
                FileInfo.DownDetails.Add(UDI);
            }

            

            // 3. Prendere dalla View un iDictionary(String, String) -> Dictionary(Service.Code, ServiceTranslateName) 
            //      per tradurre i nomi dei servizi.
            //

            return FileInfo;
        }

        private IList<dtoDownInfo> GetdtoDownInfos(Int32 UserId, Int64 FileId, IDictionary<String, String> ServTranslation)
        {
            //IList<dtoDownInfo> dtoDIs = 
              return (from FileDownloadInfo f in (Manager.GetAll<FileDownloadInfo>(fdi => ((fdi.File.Id == FileId) && (fdi.Downloader.Id == UserId))))
                     select new dtoDownInfo{ downDate = f.CreatedOn, downService = ServTranslation[f.ServiceCode] }).ToList<dtoDownInfo>();
                                            
            //    new List<dtoDownInfo>();
            //IList<FileDownloadInfo> FDIs = ;
            
            //foreach (FileDownloadInfo fdi in FDIs)
            //{
            //    dtoDownInfo dDI = new dtoDownInfo();
            //    dDI.downDate = fdi.CreatedOn;
            //    dDI.downService = ServTranslation[fdi.ServiceCode];
            //    dtoDIs.Add(dDI);
            //}
            
            //return dtoDIs;
        }

        #endregion

        public String GetXMLStringExport(
            IList<StatFileTreeLeaf> StatTreeLeafs, 
            IList<Int32> LearnersIds, 
            Int32 CurrentLearner)
        {
            return "";
        }

        public List<dtoItemEvaluation<long>> EvaluateModuleLinks(List<ModuleLink> links, int idUser, Dictionary<string, long> moduleUserLong = null, Dictionary<string, string> moduleUserString = null)
        {
            return null;
        }

        public dtoEvaluation EvaluateModuleLink(ModuleLink link, int idUser, Dictionary<string, long> moduleUserLong = null, Dictionary<string, string> moduleUserString = null)
        {
            return null;
        }
    }

}
