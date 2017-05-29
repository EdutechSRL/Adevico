using System;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), Flags(), CLSCompliant(true)]
	public enum BaseStatusDeleted //specifica come è stato cancellato il file
	{
		None = 0,
		Manual = 1,
		Automatic = 2,
		Cascade = 4
	}

	[Serializable(), CLSCompliant(true)]
	public class DomainBaseObject<T> : iDomainBaseObject<T>
	{
        public virtual T Id { get; set; }
        public virtual BaseStatusDeleted Deleted { get; set; }
        public virtual byte[] TimeStamp { get; set; }

        public DomainBaseObject() {
            Deleted = BaseStatusDeleted.None;
        }
	}
}