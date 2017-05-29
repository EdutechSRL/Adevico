using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Linq;
using lm.Comol.Core.DomainModel;
using System.Linq.Expressions;

namespace lm.Comol.Core.DataLayer
{
    public class DatabaseContext : NHibernateContext
    {
        public DatabaseContext(ISession session)
            : base(session)
        {
            if (session==null)
                 throw new Exception("DatabaseContext create");
        }

        public IOrderedQueryable<Person> Persons
        {
            get { return Session.Linq<Person>(); }
        }

        public IOrderedQueryable<Community> Communities
        {
            get { return Session.Linq<Community>(); }
        }

        public IOrderedQueryable<CommunityType> CommunityTypes
        {
            get { return Session.Linq<CommunityType>(); }
        }

        public IOrderedQueryable<Role> Roles
        {
            get { return Session.Linq<Role>(); }
        }

        public IOrderedQueryable<Subscription> Subscriptions
        {
            get { return Session.Linq<Subscription>(); }
        }

        public IOrderedQueryable<ModuleDefinition> Modules
        {
            get { return Session.Linq<ModuleDefinition>(); }
        }

        //public IOrderedQueryable<RoleCommunityAssociation> RoleCommunityAssociations
        //{
        //    get { return Session.Linq<RoleCommunityAssociation>(); }
        //}

        //public IOrderedQueryable<RoleCommunityTemplateAssociation> RoleCommunityTemplateAssociations
        //{
        //    get { return Session.Linq<RoleCommunityTemplateAssociation>(); }
        //}

        //public IOrderedQueryable<RoleSubscriptionAssociation> RoleSubscribtionAssociations
        //{
        //    get { return Session.Linq<RoleSubscriptionAssociation>(); }
        //}

        //public IOrderedQueryable<Permission> Permissions
        //{
        //    get { return Session.Linq<Permission>(); }
        //}

        public IOrderedQueryable<T> Generic<T>()
        {
            return Session.Linq<T>();
        }

        public IOrderedQueryable<CommunityFile> FileObjects
        {
            get { return Session.Linq<CommunityFile>(); }
        }

        public IOrderedQueryable<T> FileObjectsFFF<T>()
        {
             return Session.Linq<T>();
        }



      


    }
}
