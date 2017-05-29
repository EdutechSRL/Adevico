using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace lm.Comol.Core.DataLayer
{
    /// <summary>
    /// Helper class to get property paths and values from a model.
    /// </summary>
    public static class StronglyTypeHelper
    {
        /// <summary>
        /// Gets the value of a model property.
        /// </summary>
        /// <typeparam name="TModel">Accepts the type of model.</typeparam>
        /// <typeparam name="TProperty">Accepts the type of property.</typeparam>
        /// <param name="propertyExpression">Accepts a lambda expression representing the property to get the value on.</param>
        /// <param name="model">Accepts the model.</param>
        /// <returns>A string property value.</returns>
        public static string GetValue<TModel, TProperty>
            (
                this Expression<Func<TModel, TProperty>> propertyExpression,
                TModel model
            ) where TModel : class
        {
            if (model != default(TModel))
            {
                return propertyExpression.Compile().Invoke(model).ToString();
            }
            return string.Empty;
        }
        /// <summary>
        /// Gets the path of a property path including indexes on lists.
        /// </summary>
        /// <typeparam name="TModel">Accepts the type of model.</typeparam>
        /// <typeparam name="TProperty">Accepts the type of property.</typeparam>
        /// <param name="propertyExpression">Accepts a lambda expression representing the property to get the property path on.</param>
        /// <param name="model">Accepts the model.</param>
        /// <returns>A string property path.</returns>
        public static string GetPropertyPath<TModel, TProperty>
            (
                this Expression<Func<TModel, TProperty>> propertyExpression
            )
        {
            List<string> pathItems = new List<string>();
            BuildGraph(propertyExpression.Body, pathItems);
            return BuildProperty(pathItems);
        }

        public static string RenderTextBoxFor<TModel, TProperty>
            (
                TModel model,
                Expression<Func<TModel, TProperty>> property
            ) where TModel : class
        {
            var propertyName = property.GetPropertyPath();
            var propertyValue = property.GetValue(model);
            var inputString = string.Format("{0}:{1}", propertyName, propertyValue);
            return inputString;
        }

        public static string PropertyPath<TModel, TProperty>(Expression<Func<TModel, TProperty>> property)
        {
            var propertyName = property.GetPropertyPath();
            return propertyName;
        }

        private static string BuildProperty(List<string> pathItems)
        {
            StringBuilder sb = new StringBuilder();
            pathItems.Reverse();
            foreach (var item in pathItems)
            {
                if (sb.Length > 0)
                {
                    sb.Append(".");
                }
                sb.Append(item);
            }
            return sb.ToString();
        }
        private static void BuildGraph(Expression expression, IList<string> pathItems)
        {
            if (expression is MemberExpression)
            {
                BuildMemberExpressionProperty(expression, pathItems);
            }
            if (expression is MethodCallExpression)
            {
                BuildMethodCallExpressionProperty(expression, pathItems);
            }
        }
        private static void BuildMethodCallExpressionProperty(Expression expression, IList<string> pathItems)
        {
            AddMethodCallExpressionPropertyName(expression, pathItems);
            if (MethodCallExpressionParent(expression) as ParameterExpression == null)
            {
                BuildGraph(MethodCallExpressionParent(expression), pathItems);
            }
        }
        private static Expression MethodCallExpressionParent(Expression expression)
        {
            return ((MemberExpression)(MemberExpression)((MethodCallExpression)expression).Object).Expression;
        }
        private static void BuildMemberExpressionProperty(Expression expression, IList<string> pathItems)
        {
            AddMemberExpressionPropertyName(expression, pathItems);
            if (MemberExpressionParent(expression) as ParameterExpression == null)
            {
                BuildGraph(MemberExpressionParent(expression), pathItems);
            }
        }
        private static Expression MemberExpressionParent(Expression expression)
        {
            return ((MemberExpression)expression).Expression;
        }
        private static void AddMethodCallExpressionPropertyName(Expression expression, IList<string> pathItems)
        {
            pathItems.Add(MethodCallExpressionPropertyWithIndex(expression));
        }
        private static string MethodCallExpressionPropertyWithIndex(Expression expression)
        {
            return MethodCallExpressionProperty(expression) + GetIndex((MethodCallExpression)expression);
        }
        private static string MethodCallExpressionProperty(Expression expression)
        {
            return ((MemberExpression)((MethodCallExpression)expression).Object).Member.Name;
        }
        private static void AddMemberExpressionPropertyName(Expression expression, IList<string> pathItems)
        {
            pathItems.Add(((MemberExpression)expression).Member.Name);
        }
        private static string GetIndex(MethodCallExpression method)
        {
            MemberExpression args = (MemberExpression)method.Arguments[0];
            object argValue = ((ConstantExpression)args.Expression).Value;
            FieldInfo field = args.Member.DeclaringType.GetField(args.Member.Name);
            int value = (int)field.GetValue(argValue);
            return string.Format(CultureInfo.CurrentCulture, "[{0}]", value);
        }
    }
}
