using FileTransfer.Core;
using FileTransfer.DomainModel.Configuration;
using FileTransfer.WCFService.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FileTransfer.WCFService
{
    public class WCFFileTransfer : IFileTransferService
    {
        private Config Cfg = ConfigurationLoader.Configuration;
        

        public void Upload(Models.FileTransferRequest request)
        {

            ImpersonateHelper imp = new ImpersonateHelper(Cfg);

            imp.BeginImpersonate();

            string fileName = request.FileName;

            //if (ConfigurationManager.AppSettings["UploadPath"] == null)
            //{
            //    throw new ApplicationException("Missing upload path");
            //}

            Platform platform = Cfg.PlatformsDict[request.Platform];

            string uploadPath = platform.RemoteFilePath;
            string filePath = Path.Combine(Path.GetFullPath(uploadPath), fileName);

            FileStream fs = null;
            try
            {
                

                string dir = System.IO.Path.GetDirectoryName(filePath);                
                if (!Directory.Exists(dir))
                {
                    
                    DirectoryInfo di = Directory.CreateDirectory(dir);
                    
                }
                else
                {
                    
                }
                fs = File.Create(filePath);
                byte[] buffer = new byte[4 * 1024];
                int read = 0;
                while ((read = request.Data.Read(buffer, 0, buffer.Length)) != 0)
                {
                    fs.Write(buffer, 0, read);
                }
                
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }
                
                if (request.Data != null)
                {
                    request.Data.Close();
                    request.Data.Dispose();
                }
                

                imp.EndImpersonate();
                
            }
        }


        public void Dir(string[] files)
        {
            
            
            
        }
    }
}
