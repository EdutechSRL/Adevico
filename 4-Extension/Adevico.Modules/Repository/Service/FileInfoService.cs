using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Adevico.APIconnection.Core;
using Adevico.APIconnection.Core.Business;
using Adevico.APIconnection.Core.dto;
using Adevico.Modules.ScormStat.Domain;
using lm.Comol.Core.BaseModules.FileRepository.Business;
using lm.Comol.Core.Business;
using lm.Comol.Core.DomainModel;

using lm.Comol.Core.FileRepository.Domain;
using NHibernate.Hql.Ast.ANTLR;
using NHibernate.Mapping;
using NHibernate.Transform;
using Adevico.Modules.Repository.Domain;
using lm.Comol.Core.BaseModules.FileRepository.Presentation.Domain;


namespace Adevico.Modules.Repository.Service
{
    public class FileInfoService : BaseCoreServices
    {
        protected iApplicationContext _Context;

        public FileInfoService()
        {

        }

        public FileInfoService(iApplicationContext oContext)
            : base(oContext.DataContext)
        {
            _Context = oContext;
            Manager = new BaseModuleManager(oContext.DataContext);
            UC = oContext.UserContext;
            repositoryService = new ServiceRepository(oContext);
            coreApiService = new CoreAPIService(oContext);
        }

        public FileInfoService(iDataContext oDC)
            : base(oDC)
        {
            Manager = new BaseModuleManager(oDC);
            _Context = new ApplicationContext { DataContext = oDC };
            repositoryService = new ServiceRepository(oDC);
            coreApiService = new CoreAPIService(oDC);
        }

        private ServiceRepository repositoryService;
        private CoreAPIService coreApiService;

        /// <summary>
        /// SOLO file repositori nei materiali!
        /// SOLO comunità!
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// In futuro: generalizzare...
        /// </remarks>
        public dtoFileInfoResponse GetCommunityFilesRepo(Int64 communityId)
        {
            dtoFileInfoResponse response = new dtoFileInfoResponse();
            response.Success = false;

            if (communityId <= 0)
            {
                response.Files = null;
                response.ErrorInfo = GenericError.InvalidDataInput;
                return response;
            }


            //repositoryService.GetAvailableRepositoryItems()


            IList<FileRepository> ListFileRepo = (from FileRepository f in Manager.GetIQ<FileRepository>()
                                      where f.IdCommunity == communityId && 
                                            (f.ModuleCode == null || f.ModuleCode == "" ||  f.ModuleCode == "SRVMATER") &&
                                            f.IsFile == true && f.IsPersonal == false && f.IsVisible == true &&
                                            f.IsVirtual == false && f.IsDownloadable == true && f.Deleted == (int)BaseStatusDeleted.None &&
                                            f.ItemType == (short)lm.Comol.Core.DomainModel.Repository.RepositoryItemType.FileStandard
                                      select f).ToList();

            response.Files = ListFileRepo;
            response.Success = true;
            response.ErrorInfo = GenericError.None;

            return response;
        }
        public dtoFileDownloadsResponse GetCommunityFilesRepoDownloads(Int64 communityId, Int64 ItemId, Int64 VersionId)
        {
            dtoFileDownloadsResponse response = new dtoFileDownloadsResponse();
            response.Success = false;

            if (communityId <= 0)
            {
                response.Downloads = null;
                response.ErrorInfo = GenericError.InvalidDataInput;
                return response;
            }

            IList<FileRepoDownloads> ListFileRepo = (from FileRepoDownloads f in Manager.GetIQ<FileRepoDownloads>()
                                                  where f.IdCommunity == communityId && f.IdItem == ItemId && f.IdVersion == VersionId
                                                  select f).ToList();

            response.Downloads = ListFileRepo;
            response.Success = true;
            response.ErrorInfo = GenericError.None;

            return response;
        }

        public dtoPersonsResponse GetUsersFileAccess(int communityId, Int64 ItemId, Int64 VersionId, dtoPersonsResponse response, bool hasStatPermission, bool getAll)
        {
            IList<int> usersId = new List<int>();

            //Creatore file e versioni
            liteRepositoryItem item = repositoryService.ItemGet(ItemId);
            if(item == null)
                return new dtoPersonsResponse(); // List<Adevico.APIconnection.Core.dto.dtoPerson>();

            //Todo: CHECK - in teoria NON SERVE!!!

            //if (item.HasVersions)
            //{
            //    UsersId = 
            //        (from liteRepositoryItemVersion version in
            //            repositoryService.GetQueryVersions(v => v.IdItem == item.Id)
            //        select version.IdCreatedBy).Distinct().ToList();
            //}

            //UsersId.Add(item.IdCreatedBy);

            //Assegnati direttamente

            List<dtoDisplayAssignment> assignments = repositoryService.GetAssignments(ItemId);


            if (getAll || !assignments.Any(a => a.Denyed && a.IdCommunity == communityId && a.IdPerson == 0 && a.IdRole == 0))
            {
                //Recupera tutti!!!
                usersId = (from Subscription sub in Manager.GetIQ<Subscription>()
                                                     where sub.Community != null
                                                           && sub.Community.Id == communityId
                                                           && sub.Person != null
                                                           && sub.Accepted
                                                           && sub.Role.Id > -3       //Tolgo -3 passante e -4 non autenticato.
                                                     select sub.Person.Id).Distinct().ToList();
            }
            else
            {
                List<int> asgnPrsnId = (from dtoDisplayAssignment ass in assignments
                                        where ass.Denyed == false && ass.IdPerson != 0
                                        select ass.IdPerson).Distinct().ToList();

                //Assegnazione ruoli
                List<int> asgnRolesId = (from dtoDisplayAssignment ass in assignments
                                         where ass.Denyed == false && ass.IdRole != 0
                                         select ass.IdRole).Distinct().ToList();

                ////Ruoli admin da comunità
                //List<int> communityRolesId = coreApiService.RoleGetByRepositoryCommuintyShowFile(communityId);

                ////Tutti i ruoli
                //List<int> allRolesId = asgnRolesId.Union(communityRolesId).Distinct().ToList();


                List<int> asgnPrsnByRoles = coreApiService.PersonGetIdByRole(asgnRolesId, communityId);

                usersId = usersId.Union(asgnPrsnId).Union(asgnPrsnByRoles).Distinct().ToList();

                if (!hasStatPermission)
                {
                    usersId = usersId.Where(uId => uId == base.UC.CurrentUserID).ToList();
                }

                
            }
            response.Persons = coreApiService.PersonDtoGetListById(usersId);

            return response;
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
            Valid = 1
        }
    }
}
