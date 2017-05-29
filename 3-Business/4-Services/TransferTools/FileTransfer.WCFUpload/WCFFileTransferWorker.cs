using FileTransfer.DomainModel;
using FileTransfer.DomainModel.Configuration;
using FileTransfer.WCFUpload.ServiceReference1;
using lm.Comol.Core.FileRepository.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FileTransfer.WCFUpload
{
    public class WCFFileTransferWorker : FileTransferWorkerBase
    {

        
        public override void TransferFile(FileTransferBase f, Platform platform)
        {
            
            try
            {

                String sourcepath = TemporarySanitizePath(f.Path);
                String sourcefilepath = Path.Combine(sourcepath, f.Filename);
                String destinationfilepath = "";

                

                if (!platform.KeepFileStructure)
                {
                    destinationfilepath = Path.Combine(CurrentPlatform.WCF.PhysicalPath, f.Filename);
                    
                }
                else
                {
                    
                    String dirguid = f.Path.Split(Path.VolumeSeparatorChar)[0];
                    String structure = f.Path.Substring (Path.GetPathRoot(f.Path).Length)+"\\";
                    destinationfilepath = Path.Combine(CurrentPlatform.WCF.PhysicalPath, dirguid, structure, f.Filename);
                    
                }

                if (File.Exists(sourcefilepath))
                {

                    FileInfo fi = new FileInfo(sourcefilepath);

                    

                    FileTransferServiceClient stc = new FileTransferServiceClient();

                    //stc.Endpoint.Address = new System.ServiceModel.EndpointAddress("net.tcp://#IP_SCORM_SERVER#:9999/");

                    using (FileStream fs = new FileStream(sourcefilepath, FileMode.Open,FileAccess.Read,FileShare.Read))
                    {

                        //Console.WriteLine(DateTime.Now.ToLongTimeString());                        
                        //FileTransferServiceClient proxy = new FileTransferServiceClient();

                        

                        stc.Upload(destinationfilepath, platform.Name, fs);
                        stc.Close();
                    }

                    


                    f.Status = f.Decompress ? TransferStatus.ReadyToUnzip : TransferStatus.Completed;
                    //return f;
                }
                else
                {
                    
                    f.Status = TransferStatus.UploadFileNotFound;
                   // return f;
                }
            }
            catch (Exception ex)
            {
                
                f.Status = TransferStatus.Error;
                //return f;
                //throw ex;
            }
        }



        public override void DirFile(string[] files, Platform platform)
        {
            
            FileTransferServiceClient stc = new FileTransferServiceClient();
            stc.Endpoint.Address = new System.ServiceModel.EndpointAddress("net.tcp://#IP_SCORM_SERVER#:9999/");
            stc.Dir(files);   
        }
    }
}
