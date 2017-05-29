using lm.Comol.Core.FileRepository.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ionic.Zip;
using System.IO;

namespace FileTransfer.FileUnzip
{
    public class FileUnzipManager
    {
        
        public void Unzip(FileTransferBase file, String filepath, String newpath)
        {
            try
            {
                
                
                Ionic.Zip.ZipFile f = new ZipFile(Path.Combine(filepath, file.Filename));

                //string folder="";

                f.ExtractAll(Path.Combine(newpath, file.UniqueIdVersion.ToString()), ExtractExistingFileAction.DoNotOverwrite);

                //file.Status = TransferStatus.Unzipped;    
            }
            catch (Exception ex)
            {
                file.Status = TransferStatus.UnableToUnzip;
                throw;
            }
            


        }

        public void Unzip(String filename, String folder)
        {
            try
            {

                Ionic.Zip.ZipFile f = new ZipFile(filename);

                f.ExtractAll(folder);

                f.ZipError += f_ZipError;
                f.ExtractProgress += f_ExtractProgress;
                
                
            }
            catch (Exception ex)
            {             
                throw ex;
            }
        }

        void f_ExtractProgress(object sender, ExtractProgressEventArgs e)
        {            

            throw new NotImplementedException();

        }

        void f_ZipError(object sender, ZipErrorEventArgs e)
        {
            throw new NotImplementedException();
        }

    }
}
