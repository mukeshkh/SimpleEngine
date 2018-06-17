using SimpleRuleEngine.Models;

namespace SimpleRuleEngine.Interface
{
    public interface IRuleFactory
    {
        IRule GetRule(RuleInfo ruleInfo);
    }
}
