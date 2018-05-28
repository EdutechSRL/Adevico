using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Adevico.Modules.Repository.Domain
{
    public class FileRepoDownloads
    {

        public FileRepoDownloads()
        {
        }
        public virtual long Id { get; set; }
        public virtual System.Nullable<long> IdCommunity { get; set; }
        public virtual System.Nullable<long> IdPerson { get; set; }
        public virtual System.Nullable<long> IdItem { get; set; }
        public virtual System.Nullable<long> IdVersion { get; set; }
        public virtual System.Nullable<short> ItemType { get; set; }
        public virtual System.Nullable<System.DateTime> CreatedOn { get; set; }
    }
}
