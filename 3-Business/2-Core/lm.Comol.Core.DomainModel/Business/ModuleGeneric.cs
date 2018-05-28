using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel.Business
{
    [Serializable(), CLSCompliant(true)]
    public class ModuleGeneric
    {
        #region "properties"
        public Boolean Read { get; set; }            //Lettura              ( 0 - 1)
        public Boolean Write { get; set; }           //Scrittura            ( 1 - 2)
        public Boolean Modify { get; set; }          //Modifica             ( 2 - 4)
        public Boolean Delete { get; set; }          //Cancellazione        ( 3 - 8)
        public Boolean Moderate { get; set; }        //Moderate             ( 4 - 16)
        public Boolean Permission { get; set; }      //Gestione permessi    ( 5 - 32)
        public Boolean Admin { get; set; }           //Admin                ( 6 - 64)
        public Boolean Send { get; set; }            //Send                 ( 7 - 128)
        public Boolean Receive { get; set; }         //Receive              ( 8 - 256)
        public Boolean Synchronize { get; set; }     //Synchronize          ( 9 - 512)
        public Boolean Browse { get; set; }          //Browse               ( 10 - 1024)
        public Boolean Print { get; set; }           //Print                ( 11 - 2048)
        public Boolean ChangeOwner { get; set; }     //ChangeOwner          ( 12 - 4096)
        public Boolean Add { get; set; }             //Aggiungi             ( 13 - 8192)
        public Boolean ChangeStatus { get; set; }    //Change Status        ( 14 - 16384)
        public Boolean Download { get; set; }        //Download             ( 15 - 32768)


        private string _code = "unknown";
        
        /// <summary>
        /// Codice servizio
        /// </summary>
        public string Code
        {
            get { return _code; }
        }
        #endregion

        /// <summary>
        /// Inizializzazione servizio vuoto
        /// </summary>
        /// <param name="ServiceCode">Codice del servizio</param>
        public ModuleGeneric(string ServiceCode) {
            _code = ServiceCode;
        }
        /// <summary>
        /// Inizializzazione con permessi
        /// </summary>
        /// <param name="ServiceCode"></param>
        /// <param name="permission"></param>
        public ModuleGeneric(string ServiceCode, long permission)
        {
            _code = ServiceCode;

            Read = PermissionHelper.CheckPermissionSoft((long)Base2Permission.Read, permission);
            Write = PermissionHelper.CheckPermissionSoft((long)Base2Permission.Write, permission);
            Modify = PermissionHelper.CheckPermissionSoft((long)Base2Permission.Modify, permission);
            Delete = PermissionHelper.CheckPermissionSoft((long)Base2Permission.Delete, permission);
            Moderate = PermissionHelper.CheckPermissionSoft((long)Base2Permission.Moderate, permission);
            Permission = PermissionHelper.CheckPermissionSoft((long)Base2Permission.Permission, permission);
            Admin = PermissionHelper.CheckPermissionSoft((long)Base2Permission.Admin, permission);
            Send = PermissionHelper.CheckPermissionSoft((long)Base2Permission.Send, permission);
            Receive = PermissionHelper.CheckPermissionSoft((long)Base2Permission.Receive, permission);
            Synchronize = PermissionHelper.CheckPermissionSoft((long)Base2Permission.Synchronize, permission);
            Browse = PermissionHelper.CheckPermissionSoft((long)Base2Permission.Browse, permission);
            Print = PermissionHelper.CheckPermissionSoft((long)Base2Permission.Print, permission);
            ChangeOwner = PermissionHelper.CheckPermissionSoft((long)Base2Permission.ChangeOwner, permission);
            Add = PermissionHelper.CheckPermissionSoft((long)Base2Permission.Add, permission);
            ChangeStatus = PermissionHelper.CheckPermissionSoft((long)Base2Permission.ChangeStatus, permission);
            Download = PermissionHelper.CheckPermissionSoft((long)Base2Permission.Download, permission);

        }

        /// <summary>
        /// Creazione modulo portale
        /// </summary>
        /// <param name="ServiceCode"></param>
        /// <param name="idProfileType"></param>
        /// <returns></returns>
        public static ModuleGeneric CreatePortalmodule(string ServiceCode, int idProfileType)
        {
            Boolean admin = (idProfileType == (int)UserTypeStandard.SysAdmin || idProfileType == (int)UserTypeStandard.Administrator);
            Boolean baseAdmin = (admin || idProfileType == (int)UserTypeStandard.Administrative);
            Boolean isGenericUser = (idProfileType == (int)UserTypeStandard.Guest || idProfileType == (int)UserTypeStandard.PublicUser);
            ModuleGeneric module = new ModuleGeneric(ServiceCode);

            module.Read = isGenericUser || baseAdmin || admin;
            module.Write = baseAdmin || admin;
            module.Modify = baseAdmin || admin;
            module.Delete = baseAdmin || admin;
            module.Moderate = baseAdmin || admin;
            module.Permission = admin;
            module.Admin = admin;
            module.Send = baseAdmin || admin;
            module.Receive = baseAdmin || admin;
            module.Synchronize = baseAdmin || admin;
            module.Browse = isGenericUser || baseAdmin || admin;
            module.Print = baseAdmin || admin;
            module.ChangeOwner = admin;
            module.Add = baseAdmin || admin;
            module.ChangeStatus = baseAdmin || admin;
            module.Download = isGenericUser || baseAdmin || admin;

            return module;
        }

      
        /// <summary>
        /// Enum Base2Permission (1:1 con sistema)
        /// </summary>
        [Flags(), Serializable()]
        public enum Base2Permission : long
        {
            /// <summary>
            /// Lettura
            /// </summary>
            Read  = 1, 
            /// <summary>
            /// Scrittura
            /// </summary>
            Write = 1 << 1,
            /// <summary>
            /// Modifica
            /// </summary>
            Modify = 1 << 2,
            /// <summary>
            /// Cancellazione
            /// </summary>
            Delete = 1 << 3,
            /// <summary>
            /// Moderazione
            /// </summary>
            Moderate = 1 << 4,
            /// <summary>
            /// Modifica permessi
            /// </summary>
            Permission = 1 << 5,
            /// <summary>
            /// Amministrazione
            /// </summary>
            Admin = 1 << 6,
            /// <summary>
            /// Invio
            /// </summary>
            Send = 1 << 7,
            /// <summary>
            /// Ricevi
            /// </summary>
            Receive = 1 << 8,
            /// <summary>
            /// Sincronizza
            /// </summary>
            Synchronize = 1 << 9,
            /// <summary>
            /// Esplora
            /// </summary>
            Browse = 1 << 10,
            /// <summary>
            /// Stampa
            /// </summary>
            Print = 1 << 11,
            /// <summary>
            /// Modifica proprietario
            /// </summary>
            ChangeOwner = 1 << 12,
            /// <summary>
            /// Aggiungi
            /// </summary>
            Add = 1 << 13,
            /// <summary>
            /// Modifica stato
            /// </summary>
            ChangeStatus = 1 << 14,
            /// <summary>
            /// Download
            /// </summary>
            Download = 1 << 15
        }
    }
}
