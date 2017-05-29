using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace lm.Notification.Core.Domain
{
    [Serializable]
    public class CachedItem<T>
    {
        public virtual T Item { get; set; }
        public virtual DateTime InsertedDate { get; set; }

        public CachedItem(){
            InsertedDate = DateTime.Now;
        }
        public CachedItem(T items){
            this.InsertedDate = DateTime.Now;
            this.Item = items;
        }
    }
}
