using FileTransfer.DomainModel.Configuration;
using lm.Comol.Core.FileRepository.Domain;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.SessionUtility;

namespace FileTransfer.Data
{
    public class Manager
    {        
        

        protected ISession session = null;
        protected Repository rx = null;

        public Manager(Platform platform)
        {
            try
            {
                this.session = SessionDispatcher.NewSession(platform.ConnectionString);
                this.rx = new Repository(this.session);
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
            
        }

        public IQueryable<T> Query<T>()
        {
            return this.rx.Query<T>();
        }

        public FileTransferBase GetFile(long id)
        {
            return this.rx.Get<FileTransferBase>(id);
        }

        public FileTransferBase GetFile(Guid id, Guid idversion)
        {
            return (from item in Query<FileTransferBase>() where item.UniqueIdItem == id && item.UniqueIdVersion == idversion select item).Skip(0).Take(1).FirstOrDefault();
        }

        public IList<FileTransferBase> GetFiles()
        {
            try
            {
                return rx.GetAll<FileTransferBase>();
            }
            catch (Exception ex)
            {
                
                
                throw ex;
            }
        }

        public IList<FileTransferBase> GetFilesToTransfer()
        {
            try
            {
                //return (from item in rx.Query<FileTransferBase>() where item.Status == TransferStatus.ReadyForTransfer && item.Deleted == lm.Comol.Core.DomainModel.BaseStatusDeleted.None select item).ToList();
                return (from item in rx.Query<FileTransferBase>() where item.Status == TransferStatus.ReadyForTransfer && item.Deleted == lm.Comol.Core.DomainModel.BaseStatusDeleted.None select item).ToList();
            }
            catch (Exception ex)
            {

                
                throw ex;
            }
        }

        public void DeleteList<T>(IEnumerable<T> list)
        {
            try
            {
                foreach (var item in list)
                {
                    rx.Delete(item);
                }
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        public void SaveList<T>(IEnumerable<T> list)
        {
            try
            {
                foreach (var item in list)
                {
                    rx.Save(item);
                }
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        public IList<FileTransferBase> GetFilesToUnzip(Guid id)
        {
            try
            {
                //return (from item in rx.Query<FileTransferBase>() where item.Status == TransferStatus.ReadyToUnzip && item.Deleted == lm.Comol.Core.DomainModel.BaseStatusDeleted.None select item).ToList();
                return (from item in rx.Query<FileTransferBase>() where item.Status == TransferStatus.ReadyToUnzip && item.UniqueIdVersion == id && item.Deleted == lm.Comol.Core.DomainModel.BaseStatusDeleted.None select item).ToList();
            }
            catch (Exception ex)
            {

                
                throw ex;
            }
        }

        public IList<FileTransferBase> GetFilesToCopy()
        {
            try
            {
                //return (from item in rx.Query<FileTransferBase>() where item.Status == TransferStatus.ReadyForTransfer && item.Deleted == lm.Comol.Core.DomainModel.BaseStatusDeleted.None select item).ToList();
                return (from item in rx.Query<FileTransferBase>() where item.Status == TransferStatus.ReadyForTransfer && item.Deleted == lm.Comol.Core.DomainModel.BaseStatusDeleted.None select item).ToList();
            }
            catch (Exception ex)
            {

                
                throw ex;
            }
        }

        public IList<FileTransferBase> GetFilesToDelete()
        {
            try
            {
                //return (from item in rx.Query<FileTransferBase>() where item.Status == TransferStatus.ReadyToDelete && item.Deleted == lm.Comol.Core.DomainModel.BaseStatusDeleted.None select item).ToList();
                return (from item in rx.Query<FileTransferBase>() where item.Status == TransferStatus.ReadyToDelete && item.Deleted == lm.Comol.Core.DomainModel.BaseStatusDeleted.None select item).ToList();
            }
            catch (Exception ex)
            {

                
                throw ex;
            }
        }

        public void SaveFile(FileTransferBase file)
        {
            rx.Save(file);
        }

        public RepositoryItem GetRepositoryItem(FileTransferBase file)
        {
            try
            {

                List<RepositoryItem> items = (from item in rx.Query<RepositoryItem>() where item.Id == file.IdItem && item.IdVersion == file.IdVersion select item).Skip(0).Take(1).ToList();
                if (items.Count() == 1)
                {
                    return items.FirstOrDefault();
                }                
                return null;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        public RepositoryItemVersion GetRepositoryItemVersion(FileTransferBase file)
        {
            try
            {

                return rx.Get<RepositoryItemVersion>(file.IdVersion);
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        public void SaveFileTransaction(FileTransferBase file)
        {
            try
            {
                rx.BeginTransaction();
                file.isCompleted = (file.Status == TransferStatus.Completed);

                RepositoryItem ri = GetRepositoryItem(file);
                if (ri != null)
                {
                    ri.Availability = StatusToAvailability(file, ri.Availability);
                    rx.Save(ri);
                }

                RepositoryItemVersion riv = GetRepositoryItemVersion(file);
                if (riv != null)
                {
                    riv.Availability = StatusToAvailability(file, riv.Availability);
                    rx.Save(riv);
                }

                rx.Save(file);
                rx.Commit();
            }
            catch (Exception ex)
            {
                rx.Rollback();
                
                throw ex;
            }
            
        }

        private ItemAvailability StatusToAvailability(TransferStatus status,ItemAvailability current)
        {
            ItemAvailability avail = current;

            switch (status)
            {
                case TransferStatus.Multimedia_AnalyzeError:
                    avail = ItemAvailability.witherrors;
                    break;
                case TransferStatus.Scorm_AnalyzeError:
                    avail = ItemAvailability.witherrors;
                    break;
                case TransferStatus.FileTypeError:
                    avail = ItemAvailability.witherrors;
                    break;
                case TransferStatus.UnableToDeleteAfterUnzip:
                    avail = ItemAvailability.witherrors;
                    break;
                case TransferStatus.UnzipFileNotFound:
                    avail = ItemAvailability.witherrors;
                    break;
                case TransferStatus.UploadFileNotFound:
                    avail = ItemAvailability.witherrors;
                    break;
                case TransferStatus.UnableToUnzip:
                    avail = ItemAvailability.unabletounzip;
                    break;
                case TransferStatus.Error:
                    avail = ItemAvailability.witherrors;
                    break;
                case TransferStatus.ReadyForTransfer:
                    avail = ItemAvailability.transfer;
                    break;
                case TransferStatus.Copying:
                    avail = ItemAvailability.transfer;
                    break;
                case TransferStatus.ReadyToUnzip:
                    avail = ItemAvailability.transfer;
                    break;
                case TransferStatus.Unzipping:
                    avail = ItemAvailability.transfer;
                    break;
                case TransferStatus.Unzipped:
                    //current
                    break;
                case TransferStatus.ReadyToDelete:
                    //current
                    break;
                case TransferStatus.Completed:
                    avail = ItemAvailability.available;
                    break;
                case TransferStatus.Deleting:
                    //current
                    break;
                case TransferStatus.Deleted:
                    //current
                    break;
                case TransferStatus.ReadyToAnalyze:
                    avail = ItemAvailability.analyzing;
                    break;
                case TransferStatus.Analyzed:
                    //current
                    break;
                case TransferStatus.Multimedia_NoCandidates :
                    avail = ItemAvailability.waitingsettings;
                    break;
                default:
                    //current
                    break;
            }

            return avail;
        }

        private ItemAvailability StatusToAvailability(FileTransferBase file, ItemAvailability current)
        {
            return StatusToAvailability(file.Status, current);
        }

        public void BeginTransaction()
        {
            rx.BeginTransaction();
        }

        public void Commit()
        {
            rx.Commit();
        }

        public void Rollback()
        {
            rx.Rollback();
        }

        //public IList<FileTransferBase> GetFilesToCopy()
        //{
        //    try
        //    {
        //        return (from item in rx.Query<FileTransferBase>() where item.Status == TransferStatus.Copying select item).ToList();
        //    }
        //    catch (Exception ex)
        //    {

        //        
        //        throw ex;
        //    }
        //}
    }
}
