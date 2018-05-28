using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Adevico.Modules.Repository.Domain
{
    public partial class FileRepository
    {
        public FileRepository()
        {
            this.IsFile = false;
            this.Status = 1;
            this.DisplayMode = 0;
            this.HasVersions = false;
            this.IsVisible = true;
            this.IsInternal = false;
            this.IsVirtual = false;
            this.IsPersonal = false;
            this.Directories = 0;
            this.Deleted = 0;
            this.FileVersions = new List<FileVersion>();
        }
        public virtual long Id { get; set; }
        public virtual System.Nullable<long> IdFolder { get; set; }
        public virtual System.Nullable<long> IdPerson { get; set; }
        public virtual System.Nullable<long> IdCommunity { get; set; }
        public virtual string Name { get; set; }
        public virtual string Url { get; set; }
        public virtual string Extension { get; set; }
        public virtual string Description { get; set; }
        public virtual string ContentType { get; set; }
        public virtual string ModuleCode { get; set; }
        public virtual System.Nullable<long> Size { get; set; }
        public virtual System.Nullable<long> Downloaded { get; set; }
        public virtual System.Nullable<bool> IsFile { get; set; }
        public virtual System.Nullable<short> ItemType { get; set; }
        public virtual System.Nullable<long> IdVersion { get; set; }
        public virtual System.Nullable<short> Availability { get; set; }
        public virtual System.Nullable<short> Status { get; set; }
        public virtual System.Nullable<short> DisplayMode { get; set; }
        public virtual string Tags { get; set; }
        public virtual System.Nullable<bool> HasVersions { get; set; }
        public virtual System.Nullable<bool> IsVisible { get; set; }
        public virtual System.Nullable<bool> IsDownloadable { get; set; }
        public virtual System.Nullable<bool> IsInternal { get; set; }
        public virtual System.Nullable<bool> IsVirtual { get; set; }
        public virtual System.Nullable<bool> IsPersonal { get; set; }
        public virtual System.Nullable<long> Directories { get; set; }
        public virtual System.Nullable<short> Deleted { get; set; }
        public virtual IList<FileVersion> FileVersions { get; set; }
    }

}
