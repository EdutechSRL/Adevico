using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Linq;
using lm.Notification.Core.Domain;

namespace lm.Notification.Core.DataLayer
{
    public class DatabaseContext : NHibernateContext
    {
        public DatabaseContext(ISession session)
            : base(session)
        {
        }

        public IOrderedQueryable<TemplateMessage> Templates
        {
            get { return Session.Linq<TemplateMessage>(); }
        }

        public IOrderedQueryable<NotificationSummary> PersonSummaries
        {
            get { return Session.Linq<NotificationSummary>(); }
        }


        public IOrderedQueryable<T> Generic<T>()
        {
            return Session.Linq<T>();
        }


    }
}
