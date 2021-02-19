using Amazon.DynamoDBv2.DocumentModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Expression = System.Linq.Expressions.Expression;

namespace LearningPlan.DataAccess.Implementation
{
    public static class DynamoExtensions
    {
        public async static IAsyncEnumerable<Document> WhereDynamo<TSource>(this Table table, string partitionKey)
        {
            var search = table.Query(partitionKey, new Amazon.DynamoDBv2.DocumentModel.Expression());
            do
            {
                foreach (var set in await search.GetNextSetAsync())
                {
                    yield return set;
                }
            } while (!search.IsDone);
        }

        public async static IAsyncEnumerable<Document> WhereDynamo<TSource>(this Table table, Expression<Func<TSource, bool>> filter)
        {
            ScanOperationConfig config = new ScanOperationConfig 
            {
                Filter = new ScanFilter()
            };

            ScanFilter scanFilter = config.Filter;

            if (filter != null)
            {
                AddCondition(scanFilter, (BinaryExpression)filter.Body);

                void AddCondition(ScanFilter scanFilter, BinaryExpression filter)
                {
                    ExpressionType nodeType = filter.NodeType;

                    ScanOperator? queryOperator = null;
                    bool isComplex = false;
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
                        case ExpressionType.AndAlso:
                        case ExpressionType.And:
                            Expression leftAnd = filter.Left;
                            Expression rightAnd = filter.Right;
                            AddCondition(scanFilter, (BinaryExpression)leftAnd);
                            AddCondition(scanFilter, (BinaryExpression)rightAnd);
                            isComplex = true;
                            break;
                        case ExpressionType.Or:
                        case ExpressionType.OrElse:
                            config.ConditionalOperator = ConditionalOperatorValues.Or;
                            Expression leftOr = filter.Left;
                            Expression rigthOr = filter.Right;
                            AddCondition(scanFilter, (BinaryExpression)leftOr);
                            AddCondition(scanFilter, (BinaryExpression)rigthOr);
                            isComplex = true;
                            break;
                    }

                    if (!isComplex) 
                    {
                        Expression left = filter.Left is MemberExpression ? ((MemberExpression)filter.Left).Expression.NodeType == ExpressionType.Parameter ? filter.Left : filter.Right : filter.Left;
                        Expression rigth = filter.Right is MemberExpression ? ((MemberExpression)filter.Right).Expression.NodeType == ExpressionType.Constant || ((MemberExpression)filter.Right).Expression.NodeType == ExpressionType.MemberAccess ? filter.Right : filter.Left : filter.Right;
                        MemberExpression member = left as MemberExpression;
                        var leftValue = member.Member.Name;
                        MemberExpression rmember = rigth as MemberExpression;
                        var rigthValue = rmember?.Member?.Name;
                        object val2 = null;

                        ConstantExpression member2 = rigth as ConstantExpression;
                        if (member2 != null)
                        {
                            val2 = member2.Value;
                        }
                        else if (rigth is MemberExpression)
                        {
                            val2 = GetValue(rigth as MemberExpression);
                        }

                        if (val2 == null && queryOperator.Value == ScanOperator.NotEqual)
                        {
                            scanFilter.AddCondition(leftValue, ScanOperator.IsNotNull);
                        }
                        else if (val2 == null && queryOperator.Value == ScanOperator.Equal)
                        {
                            scanFilter.AddCondition(leftValue, ScanOperator.IsNull);
                        }
                        else
                        {
                            scanFilter.AddCondition(leftValue, queryOperator.Value, val2?.GetType().GetProperty(rigthValue)?.GetValue(val2).ToString() ?? val2?.ToString());
                        }
                    }
                } 
               
            }

            var scan = table.Scan(config);

            do
            {
                foreach(var set in await scan.GetNextSetAsync()) 
                {
                    yield return set;
                }
            } while (!scan.IsDone);
        }

        private static object GetValue(MemberExpression exp)
        {
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
