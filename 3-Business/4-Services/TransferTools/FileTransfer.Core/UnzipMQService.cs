using FileTransfer.Data;
using FileTransfer.DomainModel.Configuration;
using FileTransfer.FileUnzip;
using FileTransfer.MediaAnalyzer;
using lm.Comol.Core.FileRepository.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace FileTransfer.Core
{
    public class UnzipMQService : IUnzipMQService
    {
        private Config Cfg = ConfigurationLoader.Configuration;

         
        public void FileUnzipNotification(string platform, Guid fileId)
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
                    FilesUnzip(pl, fileId);
                }
                else
                {
                    
                }
            }
            catch (Exception ex)
            {

                
                //throw ex;
            }
        }

        public void FilesUnzip(Platform platform, Guid fileId)
        {
            try
            {
                

                Manager mgr = new Manager(platform);

                IList<FileTransferBase> toUnzip = mgr.GetFilesToUnzip(fileId);
                //toUnzip = mgr.GetFiles(); //just for test

                //IList<FileTransferBase> toCopy = mgr.GetFilesToTransfer();

                if (toUnzip.Count > 0)
                {

                    foreach (var file in toUnzip)
                    {
                        try
                        {                            
                            file.Status = TransferStatus.Unzipping;
                            mgr.SaveFileTransaction(file);

                            FileUnzipManager fum = new FileUnzipManager();
                            String newpath = platform.RemoteFilePath; //default, unmanaged file

                            if (file.Discriminator == FileTransferType.Multimedia)
                            {
                                newpath = platform.MultimediaFilePath;
                            }
                            else if (file.Discriminator == FileTransferType.Scorm)
                            {
                                newpath = platform.ScormFilePath;
                            }

                            string filepath = "";

                            switch (platform.ActiveTransferProtocol)
                            {
                                case TransferProtocolType.none:
                                    break;                                
                                case TransferProtocolType.WCF:
                                    filepath = platform.WCF.PhysicalPath;
                                    break;
                                default:
                                    break;
                            }

                            file.Status = TransferStatus.Unzipping;
                            mgr.SaveFileTransaction(file);

                            fum.Unzip(file, filepath, newpath);

                            file.Status = (file.Status == TransferStatus.Unzipped && file.Discriminator == FileTransferType.Unmanaged) ? TransferStatus.Completed : TransferStatus.ReadyToAnalyze;
                            mgr.SaveFileTransaction(file);

                            switch (file.Discriminator)
                            {
                                case FileTransferType.Unmanaged:
                                    break;
                                case FileTransferType.Scorm:
                                   
                                case FileTransferType.Multimedia:
                                    try
                                    {

                                        if (file.CloneOf != Guid.Empty)
                                        {
                                            FileTransferMultimedia ftm = file as FileTransferMultimedia;

                                            newpath = Path.Combine(newpath, ftm.UniqueIdVersion.ToString());

                                            FileTransferMultimedia originalfile = (mgr.GetFile(ftm.CloneOf, ftm.CloneOfVersion) as FileTransferMultimedia);
                                            
                                            MultimediaAnalyzer ma = new MultimediaAnalyzer(platform, newpath);

                                            ma.CloneMultimediaIndexes(ftm, originalfile);

                                            file.Status = TransferStatus.Analyzed;

                                            mgr.SaveFileTransaction(file);
                                            
                                        }
                                        else
                                        {

                                            FileTransferMultimedia ftm = file as FileTransferMultimedia;

                                            newpath = Path.Combine(newpath, ftm.UniqueIdVersion.ToString());

                                            MultimediaAnalyzer ma = new MultimediaAnalyzer(platform, newpath);

                                            IList<String> all = ma.AnalyzeAll();
                                            IList<KeyValue> candidates = ma.AnalyzeCandidates();
                                            

                                            

                                            ma.SaveAnalyzeResult(all, candidates, ftm);

                                            if (candidates.Count() == 0)
                                            {
                                                file.Status = TransferStatus.Multimedia_NoCandidates;
                                            }
                                            else
                                            {
                                                file.Status = TransferStatus.Analyzed;
                                            }

                                            mgr.SaveFileTransaction(file);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        file.Status = TransferStatus.Multimedia_AnalyzeError;
                                        
                                    }
                                    break;
                                case FileTransferType.VideoStreaming:
                                    break;
                                default:
                                    break;
                            }

                            //if (file.Discriminator == FileTransferType.Multimedia)
                            //{


                            //}
                            

                            if (file.Discriminator != FileTransferType.Unmanaged)
                            {
                                if (file.Status == TransferStatus.Analyzed)
                                {
                                    file.Status = TransferStatus.Completed;                                    
                                }
                            }

                            mgr.SaveFileTransaction(file);
                        }
                        catch (Exception ex)
                        {
                            
                            file.Status = TransferStatus.UnableToUnzip;
                            mgr.SaveFileTransaction(file);
                        }

                    }

                }

            }
            catch (Exception ex)
            {
                
                throw ex;
            }

        }
    }
}
