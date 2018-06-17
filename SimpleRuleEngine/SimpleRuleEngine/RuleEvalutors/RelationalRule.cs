using SimpleRuleEngine.Interface;
using SimpleRuleEngine.Models;
using System;
using System.Linq.Expressions;
using static SimpleRuleEngine.Helper.Extensions;

namespace SimpleRuleEngine.RuleEvalutors
{
    public class RelationalRule : IRule
    {
        public RuleInfo RuleInfo { get; private set; }

        private Func<Data, bool> Rule;

        public RelationalRule(RuleInfo rule)
        {
            RuleInfo = rule;
            Rule = GetRule(rule);
        }

        private Func<Data,bool> GetRule(RuleInfo rule)
        {
            Type propType = TypeToType[rule.ValueType];
            var convertMethod = TypeToConvert[rule.ValueType];

            var paramValue = Expression.Parameter(typeof(Data));

            var methodInfo = typeof(Convert).GetMethod(
                                            convertMethod,
                                            new Type[] { typeof(string) },
                                            null);

            var left = Expression.Property(paramValue, nameof(Data.Value));

            MethodCallExpression convertedLeft = Expression.Call(methodInfo, left);

            Func<Data, bool> func = (data) => false;
            // if its .net defined operator
            if ((int)rule.Operator <= MaxExprType)
            {
                if (Enum.TryParse(rule.Operator.ToString(), out ExpressionType expressionType))
                {
                    var right = Expression.Constant(Convert.ChangeType(rule.Value[0], propType));
                    func =
                        Expression.Lambda<Func<Data, bool>>
                                            (
                                                Expression.MakeBinary(expressionType, convertedLeft, right), paramValue
                                            ).Compile();
                }
            }
            return func;
        }

        public bool Eval(Data data, out string result)
        {
            result = string.Empty;
            if (Rule.Invoke(data))
                return true;
            result = data.Id + " " + RuleInfo.ToString();
            return false;
        }
    }
}
