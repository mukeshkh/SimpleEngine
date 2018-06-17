using SimpleRuleEngine.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace SimpleRuleEngine.Interface
{
    public interface IExport
    {
        void Export(ConcurrentDictionary<Data, ConcurrentBag<RuleInfo>> failedResultSet);
    }
}
