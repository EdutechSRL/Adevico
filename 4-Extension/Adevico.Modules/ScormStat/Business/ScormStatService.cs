using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Adevico.APIconnection.Core;
using Adevico.Modules.ScormStat.Domain;
using lm.Comol.Core.BaseModules.FileRepository.Business;
using lm.Comol.Core.Business;
using lm.Comol.Core.DomainModel;

using lm.Comol.Core.FileRepository.Domain;
using NHibernate.Hql.Ast.ANTLR;
using NHibernate.Mapping;
using NHibernate.Transform;


namespace Adevico.Modules.ScormStat
{
    public class ScormStatService : BaseCoreServices
    {
          #region initClass

        protected iApplicationContext _Context;

        public ScormStatService()
        {
            
        }

        public ScormStatService(iApplicationContext oContext)
            : base(oContext.DataContext)
        {
            _Context = oContext;
            Manager = new BaseModuleManager(oContext.DataContext);
            UC = oContext.UserContext;
            RepService = new ServiceRepository(oContext);
        }

        public ScormStatService(iDataContext oDC)
            : base(oDC)
        {
            Manager = new BaseModuleManager(oDC);
            _Context = new ApplicationContext { DataContext = oDC };

        }
        
        private ServiceRepository RepService;
        #endregion

        /// <summary>
        /// SOLO file SCORM!
        /// SOLO comunità!
        /// ToDo: Generalizzare!
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// In futuro: generalizzare...
        /// </remarks>
        public dtoScormFileListResponse GetCommunityScormFiles()

        {
            dtoScormFileListResponse response = new dtoScormFileListResponse();
            response.Success = false;

            int communityId = (base.UC.CurrentCommunityID > 0)
                ? base.UC.CurrentCommunityID
                : base.UC.WorkingCommunityID; 
            int personId = base.UC.CurrentUserID;

            
            //da \lm.Comol.Core.BaseModules\FileRepository\Presentation\Repository\Presentation\RepositoryPresenter.vb
            //IList<Domain.dtoFile> files = new List<dtoFile>();

            lm.Comol.Core.FileRepository.Domain.RepositoryType type = RepositoryType.Community;

            if (communityId <= 0)
            {
                response.Files = null;
                response.ErrorInfo = GenericError.InvalidDataInput;
                return response;
            }

            ModuleRepository module = RepService.GetPermissions(type, communityId, personId);   //--> Old version error.

            if (module.ViewRepository)
            {
                Boolean isAdmin = module.ManageItems || module.Administration; //Boh, forse non serve...
                //int userTypeId = Manager.GetIdProfileType(personId);
                       

                int userRoleId = Manager.GetSubscriptionIdRole(personId, communityId, true);

                IList<Int64> frItemIds = (from liteItemAssignments a in Manager.GetIQ<liteItemAssignments>()
                    where (a.IdCommunity == communityId
                           && (
                               ((a.Type == AssignmentType.role && a.IdRole == userRoleId)
                                || (a.Type == AssignmentType.person && a.IdPerson == personId)
                                || a.Type == AssignmentType.community)
                               && a.Permissions > 0))
                    select a.IdItem).Distinct().ToList();
                    
                try
                {
                    response.Files =  (from RepositoryItem r in
                                 Manager.GetAll<RepositoryItem>(ri =>
                                     ri.IsFile
                                     && frItemIds.Contains(ri.Id)
                                     && ri.Type == ItemType.ScormPackage
                                     && ri.Deleted == BaseStatusDeleted.None
                                     && (isAdmin || ri.IsVisible)
                                     )
                             select new dtoFile
                             {
                                 Id = r.Id,
                                 LastUpdate = r.ModifiedOn.ToString(),
                                 LastVersionId = r.IdVersion,
                                 Name =  r.Name,
                                 Extension = r.Extension,
                                 TotalDownload = r.Downloaded,
                                 Path = "--",
                                 TotalPlay = GetFilePlay(r.Id, communityId)
                             }
                    ).ToList();
                }
                catch (Exception ex)
                {
                    response.Files = null;
                    response.ErrorInfo = GenericError.Internal;
                    response.ServiceErrorCode = 100;
                    
                    string exception = ex.ToString(); //debug only
                }

                response.Success = true;
                response.ErrorInfo = GenericError.None;
                return response;

                //AssignmentType.person
                //AssignmentType.role
                //AssignmentType.community

                //List<dtoDisplayRepositoryItem> items = RepService.GetAvailableRepositoryItems(
                //    settings, 
                //    idCurrentUser, 
                //    type, 
                //    idCommunity, 
                //    View.GetUnknownUserName(), 
                //    View.GetFolderTypeTranslation(), 
                //    View.GetTypesTranslations(), 
                //    module, 
                //    admin, 
                //    admin);

                //// =>

                //List<dtoRepositoryItem> fItems = RepService.GetFullRepository(identifier, unknownUser, useCache);

                //var query = Manager.GetIQ<liteRepositoryItem>();
                //query =
                //    query.Where(
                //        i => !i.IsInternal && !i.IsVirtual && i.Repository != null);

                //query =
                //    query.Where(
                //        i => i.Repository.Type == RepositoryType.Community && i.Repository.IdCommunity == communityId);

                //List<dtoRepositoryItem> results = 
                //    items.Where(i => (father == null && i.IdFolder == 0) || (father != null && father.Id == i.IdFolder))
                //    .OrderBy(i => i.IsFile)
                //    .ThenBy(i => i.Name)
                //    .Select(i => new dtoRepositoryItem(
                //            i, 
                //            father, 
                //            users.Where(o => o.Id == i.IdOwner).FirstOrDefault(), 
                //            users.Where(o => o.Id == i.IdModifiedBy).FirstOrDefault(), 
                //            unknownUser)
                //        {
                //            Path = (father == null) ? "/" : father.Path + father.Name + "/"
                //        })
                //    .ToList();

                //results.Where(
                //    i => i.Type == ItemType.Folder)
                //        .ToList()
                //        .ForEach(i => i.Children = CreateNodes(items, i, users, unknownUser));

            }
            else
            {
                response.Files = null;
                response.ErrorInfo = GenericError.NoServicePermission;
                return response;
            }

            //return null;
        }

        /// <summary>
        /// SOLO file SCORM!
        /// SOLO comunità!
        /// ToDo: Generalizzare!
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// In futuro: generalizzare...
        /// </remarks>
        public dtoScormFileListResponse GetModuleLinkStatistics(long LinkId, ItemType fileType = ItemType.None)
        {
            dtoScormFileListResponse response = new dtoScormFileListResponse();
            response.Success = false;

            if(LinkId <= 0)
                return response;


            ModuleLink link = Manager.Get<ModuleLink>(LinkId);


            if(link == null || link.DestinationItem == null || link.DestinationItem.ObjectLongID <= 0)
                return response;


            //liteRepositoryItemVersion version = null;
            
            //ToDo: rivedere permission
            //bool hasPermissions = RepService.HasPermissionToSeeItem(UC.CurrentUserID, item, version, ModuleRepository.ActionType.PlayFile);

            //if (link.DestinationItem != null && link.DestinationItem.ObjectIdVersion > 0)
            //    version = RepService.ItemGetVersion(link.DestinationItem.ObjectLongID, link.DestinationItem.ObjectIdVersion);

            //if(version == null)
            //    return response;


            //ModuleObject obj = ModuleObject.CloneObject(link.DestinationItem);
            //obj.ObjectIdVersion = (version != null ? version.Id : obj.ObjectIdVersion);

            bool hasPermissions = true;
            //View.HasPermissionForLink(UserContext.CurrentUserID, idLink, obj, item.Type, link.SourceItem.ServiceID, link.SourceItem.ServiceCode);
            //View.ItemIdCommunity = link.SourceItem.CommunityID;

            //RepositoryItem
            //ToDo!


            try
            {
                var query = from RepositoryItem r in
                    Manager.GetAll<RepositoryItem>(ri =>
                        ri.IsFile
                        && ri.Id == link.DestinationItem.ObjectLongID)
                            select r;

                if (fileType != ItemType.None)
                {
                    query = query.Where(ri => ri.Type == fileType);
                }

                response.Files = (
                                    from RepositoryItem r in query
                                  select new dtoFile
                                  {
                                      Id = r.Id,
                                      LastUpdate = r.ModifiedOn.ToString(),
                                      LastVersionId = r.IdVersion,
                                      Name = r.Name,
                                      Extension = r.Extension,
                                      TotalDownload = r.Downloaded,
                                      Path = "--",
                                      TotalPlay = GetFilePlay(r.Id, link.DestinationItem.CommunityID)
                                  }
                ).ToList();

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Files = null;
                response.ErrorInfo = GenericError.Internal;
                response.ServiceErrorCode = 100;

                string exception = ex.ToString(); //debug only
            }

            return response;
        }

        //public dtoScormFileListResponse GetModuleScormFiles(long ModuleId)
        //{
        //    //ToDo!
        //    return null;
        //}

        //public dtoScormFileListResponse GetModuleScormFiles(string ModuleCode)
        //{
        //    //ToDo!
        //    return null;
        //}


        //ToDo: ottimizzare. GroupBy e Dictionary<IdItem, totalplay>?
        private int GetFilePlay(Int64 itemId, int communityId)
        {
            int play = 0;
            try
            {
                play = (from p in Manager.GetIQ<PlayStatistics>()
                            where p.IdItem == itemId
                            && p.IdCommunity == communityId
                            select p.Id)
                            .Count();
            }
            catch (Exception)
            {
                
            }
            


            return play;
        }


        public dtoScormPlayResponse GetPlayResponse(
            IList<int> usersIds, 
            IList<Int64> filesId,
            BehaviorCode behavior)
        {
            dtoScormPlayResponse response = new dtoScormPlayResponse();

            if (usersIds == null || !usersIds.Any()
                || filesId == null || !filesId.Any())
            {
                response.ErrorInfo = GenericError.InvalidDataInput;
                return response;
            }

            //Se tutti i dati di play, SOLO per il primo utente/file
            //if (behavior == BehaviorCode.All)
            //{
            //    usersIds = usersIds.Skip(0).Take(1).ToList();
            //    filesId = filesId.Skip(0).Take(1).ToList();
            //}

            IList<ScormStatPlay> statPlays = new List<ScormStatPlay>();

            int counterUsr = usersIds.Count;
            int counterFile = filesId.Count;

            //if (counterFile > 10)
            //{
            //    if (counter > 200)
            //    {
            int blockFile = (counterFile / 10) + 1;
            int blockUsr = (counterUsr / 200) + 1;

            for (int curblockFile = 0; curblockFile < blockFile; curblockFile++)
            {
                IList<Int64> currentFileId = filesId.Skip(10 * curblockFile).Take(10).ToList();

                for (int curblockUsr = 0; curblockUsr < blockUsr; curblockUsr++)
                {
                    IList<int> currentUsrId = usersIds.Skip(200 * curblockUsr).Take(200).ToList();

                    IList<ScormStatPlay> curStatPlays = new List<ScormStatPlay>();

                    try
                    {
                        curStatPlays = Manager.GetAll<ScormStatPlay>(p =>
                         currentUsrId.Contains(p.PersonId)
                         && currentFileId.Contains(p.FileId)
                         && p.IsCalculated);
                    }
                    catch { }

                    if (curStatPlays != null && curStatPlays.Any())
                    {
                        statPlays = statPlays.Union(curStatPlays).ToList();
                    }
                }
            }

            statPlays = statPlays.Distinct().ToList();
            //}

            //file

            //} else
            //{
            //    statPlays = Manager.GetAll<ScormStatPlay>(p =>
            //        usersIds.Contains(p.PersonId)
            //        && filesId.Contains(p.FileId)
            //        && p.IsCalculated);
            //}


            //Recupero info su Version
            IList<Int64> versionsIds = (from sp in statPlays
                                        select sp.VersionId).Distinct().ToList();

            IDictionary<long, long> versions = new Dictionary<long, long>();

            if (versionsIds.Any())
            {
                versions = (from RepositoryItemVersion ver in Manager.GetIQ<RepositoryItemVersion>()
                            where versionsIds.Contains(ver.Id)
                            && ver.Number > 0
                            select new { id = ver.Id, num = ver.Number })
                    .Distinct()
                    .ToDictionary(x => x.id, x => x.num);
            }

            try
            {
                response.Plays = (from ScormStatPlay p in statPlays
                    select new dtoScormStatPlay(p)
                    {
                        VersionNumber = versions.ContainsKey(p.VersionId)? versions[p.VersionId] : 0,
                    })
                    .ToList();
            }
            catch (Exception)
            {
                response.ErrorInfo = GenericError.Internal;
                response.Success = false;
                response.ServiceErrorCode = 100;
                return response;
            }
            
            
            if (behavior == BehaviorCode.LastPlay || behavior == BehaviorCode.LastCompleted)
            {

                IList<dtoScormStatPlay> lastPlays = new List<dtoScormStatPlay>();


                var filegroup = from dtoScormStatPlay p in response.Plays
                    group p by p.FileId
                    into f
                    select new { file = f.Key, play = f};

                foreach (var play in filegroup)
                {
                    var usergroup = from p in play.play
                                    group p by p.PersonId
                                        into f
                                        select new { user = f.Key, play = f};

                    foreach (var userplays in usergroup)
                    {
                        dtoScormStatPlay lastplay = null;
                        
                        if (behavior == BehaviorCode.LastCompleted)
                        {
                            lastplay = (from p in userplays.play
                                where
                                    p.Status == PackageStatus.completed
                                    || p.Status == PackageStatus.completedpassed
                                select p).OrderByDescending(p => p.EndPlayOn).FirstOrDefault();
                        }

                        if (lastplay == null || behavior == BehaviorCode.LastPlay)
                        {
                            lastplay = (from p in userplays.play
                                        select p).OrderByDescending(p => p.EndPlayOn).FirstOrDefault();
                        }

                        if(lastplay != null)
                            lastPlays.Add(lastplay);
                    }

                }

                response.Plays = lastPlays;
            }

            response.Success = true;
            response.ErrorInfo = GenericError.None;
            response.ServiceErrorCode = 0;

            return response;

        }

        
    }

    /// <summary>
    /// Indica quali play esporre
    /// </summary>
    public enum BehaviorCode
    {
        /// <summary>
        /// TUTTI i play: solo per 1 utente, 1 file
        /// </summary>
        All = -1,
        /// <summary>
        /// Ultimo play dell'utente sul file, se ci sono più play
        /// </summary>
        LastPlay = 1,
        /// <summary>
        /// Ultimo play COMPLETATO dall'utente sul file, se esiste, altrimenti solo ultimo play
        /// </summary>
        LastCompleted = 16
    }
}
