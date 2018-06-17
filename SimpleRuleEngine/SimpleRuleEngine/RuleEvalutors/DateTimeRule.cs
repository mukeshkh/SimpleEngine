using SimpleRuleEngine.Interface;
using SimpleRuleEngine.Models;
using System;

namespace SimpleRuleEngine.RuleEvalutors
{
    public class DateTimeRule : IRule
    {
        public RuleInfo RuleInfo { get; private set; }

        private Func<Data, bool> Rule;

        public DateTimeRule(RuleInfo rule)
        {
            RuleInfo = rule;
            Rule = GetIsFtureRule(rule);
        }

        public bool Eval(Data data, out string result)
        {
            result = string.Empty;
            if (Rule.Invoke(data))
                return true;
            result = data.Id + " " + RuleInfo.ToString();
            return false;
        }
        private static Func<Data, bool> GetIsFtureRule(RuleInfo rule)
        {
            Func<Data, bool> func = (data) => false;
            if (rule.Operator == Operator.IsFuture)
            {
                func = (data) =>
                      DateTime.Parse(data.Value) >= DateTime.Now;
            }

            return func;
        }
    }
}
