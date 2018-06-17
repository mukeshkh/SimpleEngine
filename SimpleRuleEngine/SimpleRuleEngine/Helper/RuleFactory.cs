using SimpleRuleEngine.Interface;
using SimpleRuleEngine.Models;
using SimpleRuleEngine.RuleEvalutors;
using static SimpleRuleEngine.Helper.Extensions;

namespace SimpleRuleEngine.Helper
{
    public class RuleFactory : IRuleFactory
    {
        public static IRuleFactory Instance => new RuleFactory();

        private RuleFactory()
        {

        }

        public IRule GetRule(RuleInfo ruleInfo)
        {
            IRule rule = new NullRule(ruleInfo);

            if (RelationOperatorRule(ruleInfo))
            {
                rule = new RelationalRule(ruleInfo);
            }
            else if (ruleInfo.ValueType == ValueTypes.DateTime && ruleInfo.Operator == Operator.IsFuture)
            {
                rule = new DateTimeRule(ruleInfo);
            }
            else if (IsBandRule(ruleInfo) && ruleInfo.ValueType!=ValueTypes.String)
            {
                rule = new BandRule(ruleInfo);
            }
            return rule;
        }

        private bool RelationOperatorRule(RuleInfo rule)
        {
            switch(rule.ValueType)
            {
                case ValueTypes.DateTime:
                case ValueTypes.Integer:
                    return (int)rule.Operator <= MaxExprType;
                case ValueTypes.String:
                    return rule.Operator == Operator.EqualTo ||
                        rule.Operator == Operator.NotEqual;
            }
            return false;
        }
    }
}
