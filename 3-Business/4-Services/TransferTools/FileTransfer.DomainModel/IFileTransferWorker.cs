using FileTransfer.DomainModel.Configuration;
using lm.Comol.Core.FileRepository.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileTransfer.DomainModel
{
    public interface IFileTransferWorker
    {
        Config CurrentConfiguration { get; set; }
        Platform CurrentPlatform { get; set; }

        void TransferFile(FileTransferBase f, Platform platform);

        void DirFile(String[] files, Platform platform);
    }
}
