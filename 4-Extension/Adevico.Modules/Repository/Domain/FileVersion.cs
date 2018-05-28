using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Adevico.Modules.Repository.Domain
{
    /// <summary>
    /// There are no comments for FileVersion in the schema.
    /// </summary>
    public class FileVersion
    {
        public FileVersion()
        {
            this.IsActive = false;
            this.Status = 1;
            this.Deleted = 0;
        }

        public virtual long Id { get; set; }
        public virtual System.Nullable<long> IdPerson { get; set; }
        public virtual System.Nullable<long> IdItem { get; set; }
        public virtual System.Nullable<long> IdCommunity { get; set; }
        public virtual string Name { get; set; }
        public virtual string Url { get; set; }
        public virtual string Extension { get; set; }
        public virtual System.Nullable<short> ItemType { get; set; }
        public virtual string Description { get; set; }
        public virtual string ContentType { get; set; }
        public virtual long Size { get; set; }
        public virtual long Number { get; set; }
        public virtual System.Nullable<long> Downloaded { get; set; }
        public virtual System.Nullable<bool> IsActive { get; set; }
        public virtual System.Nullable<short> Status { get; set; }
        public virtual System.Nullable<short> Deleted { get; set; }
        public virtual System.Nullable<System.DateTime> CreatedOn { get; set; }
        public virtual System.Nullable<System.DateTime> ModifiedOn { get; set; }
        public virtual FileRepository File { get; set; }
    }
}
