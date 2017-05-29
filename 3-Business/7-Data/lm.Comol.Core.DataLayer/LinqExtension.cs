using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Linq;
using System.Linq.Expressions;

namespace lm.Comol.Core.DataLayer
{
    public static class LinqExtension
    {

        public static INHibernateQueryable<T> Expand<T>(this INHibernateQueryable<T> q, Expression<Func<T, object>> action)
        {
            return (INHibernateQueryable<T>)q.Expand(PropertyName<T>(action));
        }

        public static INHibernateQueryable<T> ExpandPath<T>(this INHibernateQueryable<T> q, Expression<Func<T, object>> action)
        {
            return (INHibernateQueryable<T>)q.Expand(StronglyTypeHelper.PropertyPath<T, object>(action));
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


        public static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> predicate1, Expression<Func<T, bool>> predicate2)
        {
            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(predicate1, predicate2));
        }
        public static Expression<Func<T, bool>> OrElse<T>(this Expression<Func<T, bool>> predicate1, Expression<Func<T, bool>> predicate2)
        {
            return Expression.Lambda<Func<T, bool>>(Expression.OrElse(predicate1, predicate2));
        }
    }
}
