using System;
namespace lm.Comol.Core.DomainModel
{
	[Serializable()]
	public class DomainBaseObjectLiteMetaInfo<T> : DomainBaseObject<T>
	{
        public virtual litePerson CreatedBy { get; set; }
        public virtual DateTime? CreatedOn { get; set; }
        public virtual String CreatorProxyIpAddress { get; set; }
        public virtual String CreatorIpAddress { get; set; }
        public virtual litePerson ModifiedBy { get; set; }
        public virtual DateTime? ModifiedOn { get; set; }
        public virtual String ModifiedProxyIpAddress { get; set; }
        public virtual String ModifiedIpAddress { get; set; }

        public DomainBaseObjectLiteMetaInfo()
        {
            Deleted = DomainModel.BaseStatusDeleted.None;
        }
        public virtual void CreateMetaInfo(litePerson person)
        {
            CreatedBy = person;
            CreatedOn = DateTime.Now;
            UpdateMetaInfo(person, BaseStatusDeleted.None);
        }
        public virtual void CreateMetaInfo(litePerson person, String IpAddress, String ProxyIpAddress, DateTime? date = null)
        {
            CreatedBy = person;
            CreatedOn = (date.HasValue)? date:DateTime.Now;
            CreatorIpAddress = IpAddress;
            CreatorProxyIpAddress = ProxyIpAddress;
            UpdateMetaInfo(person, BaseStatusDeleted.None);
        }
        public virtual void UpdateMetaInfo(litePerson user, DateTime? date = null) 
        {
            ModifiedBy = user;
            ModifiedOn = (date.HasValue) ? date.Value : DateTime.Now;
        }
        public virtual void UpdateMetaInfo(litePerson user, BaseStatusDeleted delete, DateTime? date = null)
        {
            UpdateMetaInfo(user);
            Deleted = delete;
        }

        public virtual void UpdateMetaInfo(litePerson user, string IpAddress, string ProxyIpAddress)
        {
            UpdateMetaInfo(user);
            ModifiedIpAddress = IpAddress;
            ModifiedProxyIpAddress = ProxyIpAddress;
        }
        public virtual void UpdateMetaInfo(litePerson user, string IpAddress, string ProxyIpAddress,DateTime date)
        {
            UpdateMetaInfo(user, date);
            ModifiedIpAddress = IpAddress;
            ModifiedProxyIpAddress = ProxyIpAddress;
        }
        public virtual void SetDeleteMetaInfo(litePerson person, String IpAddress, String ProxyIpAddress, DateTime? date = null)
        {
            ModifiedIpAddress = IpAddress;
            ModifiedProxyIpAddress = ProxyIpAddress;
            UpdateMetaInfo(person, BaseStatusDeleted.Manual, date);
        }
        public virtual void RecoverMetaInfo(litePerson person, String IpAddress, String ProxyIpAddress, DateTime? date = null )
        {
            ModifiedIpAddress = IpAddress;
            ModifiedProxyIpAddress = ProxyIpAddress;
            UpdateMetaInfo(person, BaseStatusDeleted.None, date);
        }
    }
}