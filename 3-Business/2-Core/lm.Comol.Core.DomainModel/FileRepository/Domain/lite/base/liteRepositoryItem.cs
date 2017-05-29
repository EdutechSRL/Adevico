﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain
{
    [Serializable]
    public class liteRepositoryItem : liteBaseRepositoryItem
    {
        public virtual Int32 IdCommunity { get; set; }
        public virtual long IdFolder { get; set; }
        public virtual System.Guid UniqueId { get; set; }
        public virtual String Name { get; set; }
        public virtual String Extension { get; set; }
        public virtual String Url { get; set; }
        public virtual String DisplayName
        {
            get
            {
                switch (Type)
                {
                    case ItemType.File:
                        return Name + Extension;
                    case ItemType.Multimedia:
                    case ItemType.ScormPackage:
                    case ItemType.Folder:
                        return Name;
                    case ItemType.Link:
                        return (String.IsNullOrWhiteSpace(Name) ? Url : Name);
                    default:
                        return Name;
                }
            }
        }

        public virtual String DownloadFullName
        {
            get
            {
                switch (Type)
                {
                    case ItemType.Folder:
                        return Name;
                    case ItemType.Link:
                        return (String.IsNullOrWhiteSpace(Name) ? Url : Name);
                    default:
                        return Name + Extension;
                }
            }
        }
        public virtual long VersionsSize { get; set; }
        public virtual long DeletedSize { get; set; }
        public virtual long SizeWithVersions { get { return Size + VersionsSize; } }
        public virtual String ContentType { get; set; }
        public virtual Boolean IsFile { get; set; }
        public virtual Boolean HasVersions { get; set; }
        public virtual long IdVersion { get; set; }
        public virtual System.Guid UniqueIdVersion { get; set; }
        public virtual String Tags { get; set; }
        public virtual long CloneOf { get; set; }
        public virtual System.Guid CloneOfUniqueId { get; set; }
        
        public virtual Boolean IsDownloadable { get; set; }

        public virtual Boolean IsInternal { get; set; }
        public virtual Boolean IsPersonal { get; set; }
        
        public virtual ItemModuleSettings Module { get; set; }
        public virtual Boolean IsVirtual { get; set; }
        public virtual Boolean IsVisible { get; set; }
        public virtual long DisplayOrder { get; set; }
        public virtual long RevisionsNumber { get; set; }
        public virtual long IdPlayer { get; set; }
        
        public virtual Boolean AllowUpload { get; set; }
        public virtual Boolean HasThumbnail {get{ return !String.IsNullOrWhiteSpace(Thumbnail);}}
        public liteRepositoryItem()
        {
            Repository = new RepositoryIdentifier();
        }

        public virtual String ToString()
        {
            return Id.ToString() + "-" + DisplayName +"-" + Type.ToString() + "-" + Deleted.ToString();
        }
    }
}