using SimpleRuleEngine.Interface;
using SimpleRuleEngine.Models;
using System;

namespace SimpleRuleEngine.RuleEvalutors
{
    public class NullRule : IRule
    {
        public RuleInfo RuleInfo { get; private set; }

        private Func<Data, bool> Rule;

        public NullRule(RuleInfo rule)
        {
            RuleInfo = rule;

            Rule = (data) => false;
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
