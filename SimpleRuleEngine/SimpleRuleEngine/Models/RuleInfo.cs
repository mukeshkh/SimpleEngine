using System;
using System.Linq;

namespace SimpleRuleEngine.Models
{
    public class RuleInfo
    {
        public static int _ids=0;

        public int Id { get; set; }

        public string Signal { get; set; }

        public ValueTypes ValueType { get; set; }

        public string[] Value { get; set; }

        public Operator Operator { get; set; }

        public RuleInfo()
        {
            Id = ++_ids;
        }

        public override string ToString() => Id + ", " +
                                             Signal.ToString() + ", " +
                                             Operator + ", " +
                                             "[" + string.Join(" & ", Value) + "]";
                
        // override object.Equals
        public override bool Equals(object obj)
        {
            //       
            // See the full list of guidelines at
            //   http://go.microsoft.com/fwlink/?LinkID=85237  
            // and also the guidance for operator== at
            //   http://go.microsoft.com/fwlink/?LinkId=85238
            //

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            var rule = (RuleInfo)obj;
            // TODO: write your implementation of Equals() here
            return Id == rule.Id &&
                    ValueType == rule.ValueType &&
                    Signal == rule.Signal &&
                    Operator == rule.Operator &&
                    Value == rule.Value;
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return Tuple.Create(Id, Signal, ValueType, Operator, Value).GetHashCode();
        }
    }
}
