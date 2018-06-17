using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SimpleRuleEngine.Helper;
using SimpleRuleEngine.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace SimpleRuleEngine
{
    public class Program
    {
        static string ruleFile = @".\rules.json";
        static string dataFile = @".\raw_data.json";

        static readonly string exitingProgram = "existing program...";

        public static void Main(string[] args)
        {
            if(args.Length>0)
            {
                ruleFile = args[0];
            }
            if(args.Length>1)
            {
                dataFile = args[1];
            }

            if(!File.Exists(dataFile))
            {
                Console.WriteLine("data file not found");
                Console.WriteLine(exitingProgram);
                return;
            }


            OptimizedEngine engine = new OptimizedEngine();

            var reader = InstanceFactory.GetReader(FileFormat.json);

            IList<RuleInfo> ruleList=null;
            IList<Data> dataList = null; ;

            int option = 0;
            while(option != 8)
            {
                Console.WriteLine("\n1. Append new Rules (existing rules stays)");
                Console.WriteLine("2. Add new Rules (existing rules get replaced)");
                Console.WriteLine("3. Run through data (existing results stays)");
                Console.WriteLine("4. Display Results ");
                Console.WriteLine("5. Export Results");
                Console.WriteLine("6. Clear Rules (+existing result get cleared)");
                Console.WriteLine("7. Clear Results ");

                Console.WriteLine("8. Exit");

                Console.Write("Chose option (1-7): ");

                if(int.TryParse(Console.ReadLine(), out option))
                {
                    switch(option)
                    {
                        case 1:
                            Console.Write("full rule file Path ");
                            ruleFile = Console.ReadLine();

                            if (CheckForFile(ruleFile))
                            {
                                ruleList = reader.GetRules(ruleFile);

                                engine.AppendRules(ruleList);
                            }
                            break;

                        case 2:
                            Console.Write("full rule file Path (json): ");
                            ruleFile = Console.ReadLine();
                            if (CheckForFile(ruleFile))
                            {
                                ruleList = reader.GetRules(ruleFile);

                                engine.ReplaceRules(ruleList);
                            }
                            break;
                        case 3:
                            Console.Write("full data file Path (json): ");

                            dataFile = Console.ReadLine();
                            if (CheckForFile(dataFile))
                            {
                                dataList = reader.GetData(dataFile);

                                engine.Eval(dataList);
                            }
                            break;

                        case 4:
                            Console.WriteLine("Data falls into configured rules: ");
                            var groups = engine.FailedResults();
                            foreach (var group in groups)
                            {
                                Console.WriteLine("DataId " + group.Key);
                                foreach (var item in group.Value)
                                {
                                    Console.WriteLine("\tRule " + item);
                                }
                                Console.WriteLine();
                            }
                            break;

                        case 5:
                            engine.Export(FileFormat.json);
                            break;

                        case 6:
                            engine.ClearRules();
                            break;

                        case 7:
                            engine.ClearResult();
                            break;
                            
                        case 8:
                            return;
                    }
                }
            }

            Console.Write("press any key to exit");
            Console.ReadKey();
        }

        private static bool CheckForFile(string path)
        {
            if (!File.Exists(path))
            {
                Console.WriteLine("file "+path+" not found...");
                return false;
            }
            return true;
        }
    }
}
