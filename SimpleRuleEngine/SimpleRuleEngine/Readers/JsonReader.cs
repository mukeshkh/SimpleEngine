using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SimpleRuleEngine.Interface;
using SimpleRuleEngine.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SimpleRuleEngine.Readers
{
    public class JsonReader : IReader
    {
        public IList<Data> GetData(string filePath)
        {
            return GetValues<Data>(filePath);
        }

        public IList<RuleInfo> GetRules(string filePath)
        {
            return GetValues<RuleInfo>(filePath);
        }

        private IList<T> GetValues<T>(string filePath)
        {
            IList<T> data = new List<T>();
            using (StreamReader file = File.OpenText(filePath))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                var array = JArray.Load(reader);
                data = array.Select(x => x.ToObject<T>()).ToList();
            }
            return data;
        }
    }
}
