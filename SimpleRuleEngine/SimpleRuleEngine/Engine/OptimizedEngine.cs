using SimpleRuleEngine.Helper;
using SimpleRuleEngine.Interface;
using SimpleRuleEngine.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;

namespace SimpleRuleEngine
{
    public class OptimizedEngine : IRuleEngine
    {
        private readonly ConcurrentDictionary<string, IRuleEngine> _ruleSet = 
                                            new ConcurrentDictionary<string, IRuleEngine>();

        private readonly ConcurrentBag<string> _failedResultBag;

        private readonly ConcurrentDictionary<Data, ConcurrentBag<RuleInfo>> _failedResultSet;

        public OptimizedEngine()
        {
            _failedResultBag = new ConcurrentBag<string>();
            _failedResultSet = new ConcurrentDictionary<Data, ConcurrentBag<RuleInfo>>();
            _ruleSet = new ConcurrentDictionary<string, IRuleEngine>();
        }

        public void Enqueue(RuleInfo rule)
        {
            if (!_ruleSet.TryGetValue(rule.Signal, out IRuleEngine ruleEngine))
            {
                _ruleSet[rule.Signal] = new RuleEngine(_failedResultBag, _failedResultSet);
            }
            _ruleSet[rule.Signal].Enqueue(rule);
        }

        public /*Dictionary<string, List<string>>*/void Eval(IEnumerable<Data> data)
        {
            //ConcurrentDictionary<string, ConcurrentBag<string>> failedRules
            //    = new ConcurrentDictionary<string, ConcurrentBag<string>>();

            var dataGroups = data.GroupBy(x => x.Signal, x => x);
            foreach (var items in dataGroups)
            {
                if (!_ruleSet.ContainsKey(items.Key))
                {
                    //Rules not configured for given data

                    Array.ForEach(items.ToArray(),
                                    (item) =>
                                        _failedResultBag.Add(item.Id + " rule not found"));
                    
                }
                else
                {
                    //var result = 
                    _ruleSet[items.Key].Eval(items.ToList());
                        //.SelectMany(x => x.Value);

                    //failedRules[items.Key.ToString()] = new ConcurrentBag<string>(result);
                }
            }
            //return failedRules.ToDictionary(x => x.Key, y => y.Value.ToList());
        }

        public Dictionary<int, IEnumerable<string>> FailedResults()
        {
            var resultArray = _failedResultBag.ToArray();

            var list = resultArray.Select(x =>
            {
                var values = x.Split(" ");

                return new 
                {
                    key = int.Parse(values[0]),
                    val = x.Substring(values[0].Length)
                };
            });

            var groups = list.GroupBy(x => x.key).OrderBy(x => x.Key);
            return groups.ToDictionary(x => x.Key, x => x.Select(z=>z.val));
        }

        public void Export(FileFormat format)
        {
            InstanceFactory.GetExporter(format).Export(_failedResultSet);
        }

        public void ClearResult()
        {
            _failedResultBag.Clear();
            _failedResultSet.Clear();
        }

        public void ClearRules()
        {
            Parallel.ForEach(_ruleSet,(rule)=>rule.Value.ClearRules());
        }

        public void ReplaceRules(IEnumerable<RuleInfo> rules)
        {
            ClearRules();
            AppendRules(rules);
        }

        public void AppendRules(IEnumerable<RuleInfo> rules)
        {
            Parallel.ForEach(rules, (rule) => Enqueue(rule));
        }
    }
}
