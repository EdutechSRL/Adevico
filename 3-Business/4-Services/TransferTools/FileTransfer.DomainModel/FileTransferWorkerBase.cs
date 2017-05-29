using FileTransfer.DomainModel.Configuration;
using lm.Comol.Core.FileRepository.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileTransfer.DomainModel
{
    public abstract class FileTransferWorkerBase: IFileTransferWorker
    {
        public Configuration.Config CurrentConfiguration { get; set; }

        public Configuration.Platform CurrentPlatform { get; set; }

        public abstract void TransferFile(FileTransferBase f, Platform platform);

        public String TemporarySanitizePath(String path)
        {
            return path;
        }


        public abstract void DirFile(string[] files, Platform platform);
    }
}
