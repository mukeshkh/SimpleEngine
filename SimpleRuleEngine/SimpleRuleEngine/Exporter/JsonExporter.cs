using Newtonsoft.Json;
using SimpleRuleEngine.Interface;
using SimpleRuleEngine.Models;
using System;
using System.Collections.Concurrent;
using System.IO;
using static SimpleRuleEngine.Helper.Extensions;

namespace SimpleRuleEngine.Exporter
{
    public class JsonExporter : IExport
    {
        public void Export(ConcurrentDictionary<Data, ConcurrentBag<RuleInfo>> results)
        {
            var path = Directory.GetCurrentDirectory();
            Console.WriteLine($"Exporting to {(path+FileName)}...");
            using (StreamWriter file = File.CreateText(path + FileName))
            using (JsonTextWriter writer = new JsonTextWriter(file))
            {
                writer.Formatting = Formatting.Indented;
                writer.WriteStartArray();

                foreach (var result in results)
                {
                    writer.WriteStartObject();
                    writer.WritePropertyName("DId");
                    writer.WriteValue(result.Key.Id);

                    writer.WritePropertyName("Signal");
                    writer.WriteValue(result.Key.Signal);

                    writer.WritePropertyName("Rules");

                    writer.WriteStartArray();

                    foreach (var item in result.Value)
                    {
                        //writer.WriteStartObject();
                        //writer.WritePropertyName("Rule");
                        writer.WriteValue("Rule: "+item.ToString());
                        //writer.WriteEndObject();
                    }
                    writer.WriteEndArray();

                    writer.WriteEndObject();
                }
                writer.WriteEndArray();
            }
        }
    }
}
