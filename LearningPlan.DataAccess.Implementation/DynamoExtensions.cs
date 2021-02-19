using Amazon.DynamoDBv2.DocumentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LearningPlan.DataAccess.Implementation
{
    public static class DynamoExtensions
    {
        public static List<Document> WhereDynamo<TSource>(this Table table, Expression<Func<TSource, bool>> propertyLambda)
        {
            Type type = typeof(TSource);

            // QueryFilter filter = new QueryFilter("Id", QueryOperator.Equal, forumName + "#" + threadName);

            ExpressionType nodeType = propertyLambda.Body.NodeType;

            ScanOperator? queryOperator = null;
            switch (nodeType)
            {
                case ExpressionType.Equal:
                    queryOperator = ScanOperator.Equal;
                    break;
                case ExpressionType.GreaterThan:
                    queryOperator = ScanOperator.GreaterThan;
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    queryOperator = ScanOperator.GreaterThanOrEqual;
                    break;
                case ExpressionType.LessThan:
                    queryOperator = ScanOperator.LessThan;
                    break;
                case ExpressionType.LessThanOrEqual:
                    queryOperator = ScanOperator.LessThanOrEqual;
                    break;
                case ExpressionType.NotEqual:
                    queryOperator = ScanOperator.NotEqual;
                    break;
                case ExpressionType.And:
                    break;
                case ExpressionType.Or:
                    break;
                
            }

            System.Linq.Expressions.Expression left = ((BinaryExpression)propertyLambda.Body).Left;
            System.Linq.Expressions.Expression rigth = ((BinaryExpression)propertyLambda.Body).Right;
            MemberExpression member = left as MemberExpression;
            var leftValue = member.Member.Name;
            MemberExpression rmember = rigth as MemberExpression;
            var rigthValue = rmember.Member.Name;
            object val2 = "";

            ConstantExpression member2 = rigth as ConstantExpression;
            if (member2 != null)
            {
                val2 = member2.Value;
            }
            else if (rigth is MemberExpression)
            {
                val2 = GetValue(rigth as MemberExpression);

            }

            ScanFilter scanFilter = new ScanFilter();
            scanFilter.AddCondition(leftValue, queryOperator.Value, val2.GetType().GetProperty(rigthValue)?.GetValue(val2).ToString() ?? val2.ToString());

            //QueryFilter filter = new QueryFilter(leftValue, queryOperator.Value, val2.GetType().GetProperty(leftValue)?.GetValue(val2).ToString() ?? val2.ToString());

            var docs = table.Scan(scanFilter).GetNextSetAsync();
            return docs.GetAwaiter().GetResult();
        }

        private static object GetValue(MemberExpression exp)
        {
            // expression is ConstantExpression or FieldExpression
            if (exp.Expression is ConstantExpression)
            {
                return (((ConstantExpression)exp.Expression).Value)
                        .GetType()
                        .GetField(exp.Member.Name)
                        .GetValue(((ConstantExpression)exp.Expression).Value);
            }
            else if (exp.Expression is MemberExpression)
            {
                return GetValue((MemberExpression)exp.Expression);
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
