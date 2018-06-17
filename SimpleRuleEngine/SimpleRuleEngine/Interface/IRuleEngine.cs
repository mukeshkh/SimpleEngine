using SimpleRuleEngine.Models;
using System.Collections.Generic;

namespace SimpleRuleEngine.Interface
{
    public interface IRuleEngine
    {
        void Eval(IEnumerable<Data> data);

        void Enqueue(RuleInfo rule);

        Dictionary<int, IEnumerable<string>> FailedResults();

        void ClearResult();

        void ClearRules();

        void ReplaceRules(IEnumerable<RuleInfo> rules);

        void AppendRules(IEnumerable<RuleInfo> rules);
    }
}