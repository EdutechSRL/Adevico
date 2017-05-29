using FileTransfer.Data;
using FileTransfer.DomainModel;
using FileTransfer.DomainModel.Configuration;
using FileTransfer.WCFUpload;
using lm.Comol.Core.FileRepository.Domain;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace FileTransfer.Core
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "FileMQService" in both code and config file together.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single)]
    public class FileMQService : IFileMQService
    {
        private Config Cfg = ConfigurationLoader.Configuration;

         

        public void FileTransferNotification(String platform)
        {
            try
            {
                
                Platform pl = null;
                if (Cfg.PlatformsDict.Keys.Contains(platform))
                {
                    pl = Cfg.PlatformsDict[platform];
                }
                if (pl != null)
                {
                    FilesTransfer(pl);
                }
                else
                {
                    
                }
            }
            catch (Exception ex)
            {

                
                throw ex;
            }
        }

        public void FilesTransfer(Platform platform)
        {
            try
            {                
                

                Manager mgr = new Manager(platform);

                IList<FileTransferBase> toTransfer = mgr.GetFilesToTransfer();
                //toTransfer = mgr.GetFiles(); //just for test
                
                //IList<FileTransferBase> toCopy = mgr.GetFilesToTransfer();

                if (toTransfer.Count > 0) {

                    TransferAllFiles(platform, toTransfer, mgr);

                }
                
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
            
        }

        private void TransferAllFiles(Platform platform, IList<FileTransferBase> toTransfer, Manager mgr)
        {            

            IFileTransferWorker ftw = null;

            
            switch (platform.ActiveTransferProtocol)
            {
                case TransferProtocolType.none:
                    
                    break;
                case TransferProtocolType.WCF:
                    ftw = new WCFFileTransferWorker();
                    break;
            }

            if (ftw != null)
            {

                ImpersonateHelper imp = new ImpersonateHelper(Cfg);

                imp.BeginImpersonate();

                ftw.CurrentConfiguration = Cfg;
                ftw.CurrentPlatform = platform;

                foreach (var file in toTransfer)
                {
                    try
                    {
                        file.Status = TransferStatus.Copying;
                        //mgr.BeginTransaction();
                        mgr.SaveFileTransaction(file);
                        //mgr.Commit();                    

                        ftw.TransferFile(file, platform);

                        //file.Status = TransferStatus.Completed;
                        //mgr.SaveFile(file);

                        //mgr.BeginTransaction();
                        mgr.SaveFileTransaction(file);

                        if (file.Status == TransferStatus.ReadyToUnzip)
                        {
                            ServiceReferenceUnzip.UnzipMQServiceClient serviceunzip = new ServiceReferenceUnzip.UnzipMQServiceClient();
                            serviceunzip.FileUnzipNotification(platform.Name, file.UniqueIdVersion);
                        }
                    }
                    catch (Exception ex)
                    {
                        
                        //throw ex;
                    }                                        
                    
                    //mgr.Commit();   
                }

                

                imp.EndImpersonate();
            }
            else
            {
                
            }
        }

        private void CopyAllFiles(Platform platform, IList<FileTransferBase> toCopy) { }
        private void DeleteAllFiles(Platform platform, IList<FileTransferBase> toDelete) { }        
        public void TransferAllFilesDirect(String Platform, List<FileTransferBase> toTransfer)
        {

            Platform platform = null;
            if (Cfg.PlatformsDict.Keys.Contains(Platform))
            {
                platform = Cfg.PlatformsDict[Platform];
            }
            if (platform != null)
            {
                

                IFileTransferWorker ftw = null;

                
                switch (platform.ActiveTransferProtocol)
                {
                    case TransferProtocolType.none:
                        
                        break;                    
                    case TransferProtocolType.WCF:
                        ftw = new WCFFileTransferWorker();
                        break;
                }

                if (ftw != null)
                {

                    ImpersonateHelper imp = new ImpersonateHelper(Cfg);

                    imp.BeginImpersonate();

                    ftw.CurrentConfiguration = Cfg;
                    ftw.CurrentPlatform = platform;

                    foreach (var file in toTransfer)
                    {
                        try
                        {


                            file.Status = TransferStatus.Copying;
                            //mgr.BeginTransaction();
                            //mgr.SaveFileTransaction(file);
                            //mgr.Commit();                    

                            ftw.TransferFile(file, platform);

                            //file.Status = TransferStatus.Completed;
                            //mgr.SaveFile(file);

                            //mgr.BeginTransaction();
                            //mgr.SaveFileTransaction(file);

                            if (file.Status == TransferStatus.ReadyToUnzip)
                            {
                                ServiceReferenceUnzip.UnzipMQServiceClient serviceunzip = new ServiceReferenceUnzip.UnzipMQServiceClient();
                                serviceunzip.FileUnzipNotification(platform.Name, file.UniqueIdVersion);
                            }
                        }
                        catch (Exception ex)
                        {
                            
                            //throw ex;
                        }

                        //mgr.Commit();   
                    }

                    

                    imp.EndImpersonate();
                }
                else
                {
                    
                }
            }
            else
            {
                
            }


            
        }


        public void DirAllFilesDirect(string Platform, string[] Files)
        {
            Platform platform = null;
            if (Cfg.PlatformsDict.Keys.Contains(Platform))
            {
                platform = Cfg.PlatformsDict[Platform];
            }
            if (platform != null)
            {
                IFileTransferWorker ftw = null;

                
                switch (platform.ActiveTransferProtocol)
                {
                    case TransferProtocolType.none:
                        
                        break;                    
                    case TransferProtocolType.WCF:
                        ftw = new WCFFileTransferWorker();
                        break;
                }

                if (ftw != null)
                {
                    ftw.DirFile(Files, platform);
                }
            }
        }
    }
}
