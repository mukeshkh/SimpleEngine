using SimpleRuleEngine.Interface;
using SimpleRuleEngine.Models;
using SimpleRuleEngine.RuleEvalutors;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleRuleEngine
{
    public class RuleEngine : IRuleEngine , IDispose
    {
        private readonly ConcurrentDictionary<ValueTypes, ConcurrentBag<IRule>> Rules;

        private readonly ConcurrentDictionary<Data, ConcurrentBag<RuleInfo>> _failedResultSet;

        private readonly ConcurrentBag<string> _failedResultBag;


        public IRuleFactory RuleFactory { get; private set; }
        
        public RuleEngine(ConcurrentBag<string> resultBag, ConcurrentDictionary<Data, ConcurrentBag<RuleInfo>> failedResultSet)
        {
            RuleFactory = Helper.RuleFactory.Instance;

            _failedResultBag = resultBag;
            _failedResultSet = failedResultSet;
            Rules = new ConcurrentDictionary<ValueTypes, ConcurrentBag<IRule>>();
        }

        public void Enqueue(RuleInfo ruleInfo)
        {
            var rule = RuleFactory.GetRule(ruleInfo);
            if (!(rule is NullRule))
            {
                Rules.AddOrUpdate(ruleInfo.ValueType,
                                    new ConcurrentBag<IRule>(new[] { rule }),
                                    (k, v) => { v.Add(rule); return v; });
            }
            else
            {
                //rule not supported yet
            }
        }

        public /*Dictionary<string,List<string>>*/void Eval(IEnumerable<Data> dataCollection)
        {
            //ConcurrentDictionary<string,ConcurrentBag<string>> failedRules 
            //    = new ConcurrentDictionary<string, ConcurrentBag<string>>();

            Parallel.ForEach(dataCollection, (data) =>
            //Parallel.For(0, dataCollection.Count, i =>
            {
                //var data = dataCollection[i];
                if (Rules.TryGetValue(data.ValueType, out ConcurrentBag<IRule> rules))
                {
                    var z = rules.Select(x => x).ToList();

                    Parallel.For(0, z.Count, j =>
                                                {
                                                    var result = z[j].Eval(data, out string failedInfo);
                                                    if (result)
                                                    {
                                                        _failedResultBag.Add(failedInfo);

                                                        _failedResultSet.AddOrUpdate(
                                                            data, 
                                                            new ConcurrentBag<RuleInfo>(
                                                            new[] { z[j].RuleInfo }), 
                                                            (k, v) => {
                                                                v.Add(z[j].RuleInfo); return v;
                                                            });

                                                        //failedRules.AddOrUpdate(
                                                        //    data.ValueType.ToString(),
                                                        //    new ConcurrentBag<string>(new[] {
                                                        //        result
                                                        //    }),
                                                        //    (k, v) => {
                                                        //        v.Add(result); return v;
                                                        //    });
                                                    }
                                                });
                }
            });
            //return failedRules.ToDictionary(x=>x.Key,y=>y.Value.ToList());
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
            return groups.ToDictionary(x => x.Key, x => x.Select(z => z.val));
        }

        public void ClearResult()
        {
            _failedResultBag.Clear();
            _failedResultSet.Clear();
        }

        public void ClearRules()
        {
            Rules.Clear();
            ClearResult();
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
