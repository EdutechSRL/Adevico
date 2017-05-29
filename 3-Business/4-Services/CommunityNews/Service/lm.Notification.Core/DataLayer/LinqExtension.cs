using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Linq;
using System.Linq.Expressions;

namespace lm.Notification.Core.DataLayer
{
    public static class LinqExtension
    {

        public static INHibernateQueryable<T> Expand<T>(this INHibernateQueryable<T> q, Expression<Func<T, object>> action)
        {
            return (INHibernateQueryable<T>)q.Expand(PropertyName<T>(action));
        }

        public static String PropertyName<T>(Expression<Func<T, object>> action)
        {
            return GetMemberInfo(action).Member.Name;
        }

        private static MemberExpression GetMemberInfo(Expression method)
        {
            LambdaExpression lambda = method as LambdaExpression;
            if (lambda == null)
                throw new ArgumentNullException("method");

            MemberExpression memberExpr = null;

            if (lambda.Body.NodeType == ExpressionType.Convert)
            {
                memberExpr = ((UnaryExpression)lambda.Body).Operand as MemberExpression;
            }
            else if (lambda.Body.NodeType == ExpressionType.MemberAccess)
            {
                memberExpr = lambda.Body as MemberExpression;
            }

            if (memberExpr == null)
                throw new ArgumentException("method");

            return memberExpr;
        }
    }
}
