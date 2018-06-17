using SimpleRuleEngine.Interface;
using SimpleRuleEngine.Models;
using System;
using System.Linq.Expressions;
using static SimpleRuleEngine.Helper.Extensions;


namespace SimpleRuleEngine.RuleEvalutors
{
    public class BandRule : IRule
    {
        public RuleInfo RuleInfo { get; private set; }

        private Func<Data, bool> Rule;

        public BandRule(RuleInfo rule)
        {
            RuleInfo = rule;

            Func<Data, bool> func = (data) => false;

            // if its .net defined operator
            if (rule.Operator == Operator.InBand)
            {
                GetConditions(rule, ExpressionType.GreaterThanOrEqual, ExpressionType.LessThanOrEqual, 
                              out Func<Data, bool> firstCondition, out Func<Data, bool> secondCondition);

                func = (data) => firstCondition(data) && secondCondition(data);
            }

            else if (rule.Operator == Operator.OutBand)
            {
                GetConditions(rule, ExpressionType.LessThan, ExpressionType.GreaterThan, 
                              out Func<Data, bool> firstCondition, out Func<Data, bool> secondCondition);

                func = (data) => firstCondition(data) || secondCondition(data);
            }
            Rule = func;
        }

        private void GetConditions(RuleInfo rule, ExpressionType firstOperator, ExpressionType secondOperator,
                                    out Func<Data, bool> firstCondition, 
                                    out Func<Data, bool> secondCondition)
        {
            var val1 = rule.Value[0];
            var val2 = rule.Value[1];

            firstCondition =
                GetRule(val1, firstOperator);// Expression.Constant(Convert.ChangeType(val1, propType)));

            secondCondition =
                GetRule(val2, secondOperator);
                //GetExpression(secondOperator, left, Expression.Constant(Convert.ChangeType(val2, propType)));
        }

        public bool Eval(Data data, out string result)
        {
            result = string.Empty;
            if (Rule.Invoke(data))
                return true;
            result = data.Id + " " + RuleInfo.ToString();
            return false;
        }

        private Func<Data, bool> GetRule(string rightValue,ExpressionType expressionType)
        {
            Type propType = TypeToType[RuleInfo.ValueType];
            var convertMethod = TypeToConvert[RuleInfo.ValueType];

            var paramValue = Expression.Parameter(typeof(Data));

            var methodInfo = typeof(Convert).GetMethod(
                                            convertMethod,
                                            new Type[] { typeof(string) },
                                            null);

            var left = Expression.Property(paramValue, nameof(Data.Value));

            MethodCallExpression convertedLeft = Expression.Call(methodInfo, left);

            Func<Data, bool> func = (data) => false;
            // if its .net defined operator

            var right = Expression.Constant(Convert.ChangeType(rightValue, propType));
            func =
                Expression.Lambda<Func<Data, bool>>
                                    (
                                        Expression.MakeBinary(expressionType, convertedLeft, right), paramValue
                                    ).Compile();

            return func;
        }

        private Func<Data,bool> GetExpression(ExpressionType binaryOperator, Expression left, Expression right)
        {
            var paramValue = Expression.Parameter(typeof(Data));

            return Expression.Lambda<Func<Data, bool>>
                                                   (
                                                       Expression.MakeBinary(binaryOperator, left, right), paramValue
                                                   ).Compile();
        }
    }
}
