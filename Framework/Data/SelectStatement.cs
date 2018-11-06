// <copyright filename="SelectStatement.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Text;

    using Database.Attributes;

    internal class SelectStatement<T> : ISelectStatement<T>
    {
        private Expression<Func<T, bool>> whereClause;

        public string GetStatement()
        {
            var builder = new StringBuilder();
            var type = typeof(T);

            var columnNames = GetColumnNames(type);
            string tableName = $@" ""{GetTableName(type)}"" ";
            string where = GetWhereClause();

            builder.Append(@" SELECT ");
            builder.Append(string.Join(@", ", columnNames));
            builder.Append(@" FROM ");
            builder.Append(tableName);
            builder.Append(where);

            return builder.ToString();
        }

        private IEnumerable<string> GetColumnNames(Type type)
        {
            foreach (var property in type.GetProperties())
            {
                var attr = property.GetCustomAttribute<FieldNameAttribute>();
                if (attr != null)
                {
                    yield return $@"U_{attr.FieldName}";
                }
                else
                {
                    yield return $@"U_{property.Name}";
                }
            }
        }

        public void Where(Expression<Func<T, bool>> predicate)
        {
            whereClause = predicate;
        }

        private string GetOperation(BinaryExpression operation)
        {
            if (operation.NodeType == ExpressionType.Equal)
            {
                return @" = ";
            }

            throw new ArgumentException(@"Unsupported expression type.");
        }

        private string GetTableName(Type type)
        {
            var sysTableAttr = type.GetCustomAttribute<SystemTableAttribute>();
            var usrTableAttr = type.GetCustomAttribute<UserTableAttribute>();
            if (!(sysTableAttr == null ^ usrTableAttr == null))
            {
                throw new ArgumentException($"'{type}' is either missing or has both system and user table attributes.");
            }

            if (sysTableAttr != null)
            {
                return sysTableAttr.TableName;
            }

            return usrTableAttr.TableName;
        }

        private object GetValue(Expression right)
        {
            if (right is ConstantExpression expression)
            {
                return expression.Value;
            }

            if (right is MemberExpression memberExpression)
            {
                return GetValue(memberExpression);
            }

            return null;
        }


        private object GetValue(MemberExpression member)
        {
            var objectMember = Expression.Convert(member, typeof(object));

            var getterLambda = Expression.Lambda<Func<object>>(objectMember);

            var getter = getterLambda.Compile();

            return getter();
        }

        private string GetWhereClause()
        {
            if (whereClause == null)
            {
                return string.Empty;
            }

            var operation = (BinaryExpression) whereClause.Body;
            var left = (MemberExpression) operation.Left;
            var value = GetValue(operation.Right);

            var builder = new StringBuilder();
            builder.Append(@" WHERE ");
            builder.Append($@" ""{left.Member.Name}"" ");
            builder.Append($@" {GetOperation(operation)} ");
            builder.Append($@" N'{value}' ");

            return builder.ToString();
        }
    }
}