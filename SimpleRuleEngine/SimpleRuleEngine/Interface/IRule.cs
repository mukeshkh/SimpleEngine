using SimpleRuleEngine.Models;
using System;

namespace SimpleRuleEngine.Interface
{
    public interface IRule
    {
        RuleInfo RuleInfo { get; }

        //Func<Data,string> Rule { get; }

        bool Eval(Data data, out string result);
    }
}
