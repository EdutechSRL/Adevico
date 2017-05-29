using lm.Comol.Core.FileRepository.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileTransfer.DomainModel
{
    public static class Extentsions
    {
        public static Int32 CountScorm(this IList<FileTransferBase> list)
        {
            return list.CountType(FileTransferType.Scorm);
        }

        public static Int32 CountMultimedia(this IList<FileTransferBase> list)
        {
            return list.CountType(FileTransferType.Multimedia);
        }

        public static Int32 CountUnmanaged(this IList<FileTransferBase> list)
        {
            return list.CountType(FileTransferType.Unmanaged);
        }

        public static Int32 CountType(this IList<FileTransferBase> list, FileTransferType type)
        {
            return (from item in list where item.Discriminator == type select item).Count();
        }
    }
}
