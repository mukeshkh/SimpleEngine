using SimpleRuleEngine.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleRuleEngine.Interface
{
    public interface IReader
    {
        IList<Data> GetData(string filePath);

        IList<RuleInfo> GetRules(string filePath);
    }
}
