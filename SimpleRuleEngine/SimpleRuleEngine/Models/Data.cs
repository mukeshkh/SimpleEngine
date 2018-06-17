namespace SimpleRuleEngine.Models
{
    public class Data
    {
        private static int _ids = 0;

        public int Id { get; set; }

        public string Signal { get; set; }

        public ValueTypes ValueType { get; set; }

        public string Value { get; set; }

        public Data()
        {
            Id = ++_ids;
        }
    }
}
