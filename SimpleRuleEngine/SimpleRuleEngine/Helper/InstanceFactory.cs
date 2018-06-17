using SimpleRuleEngine.Exporter;
using SimpleRuleEngine.Interface;
using SimpleRuleEngine.Models;
using SimpleRuleEngine.Readers;

namespace SimpleRuleEngine.Helper
{
    public class InstanceFactory
    {
        public static IExport GetExporter(FileFormat format)
        {
            switch(format)
            {
                case FileFormat.json:
                    return new JsonExporter();
            }
            return null;
        }

        public static IReader GetReader(FileFormat format)
        {
            switch (format)
            {
                case FileFormat.json:
                    return new JsonReader();
            }
            return null;
        }
    }
}
