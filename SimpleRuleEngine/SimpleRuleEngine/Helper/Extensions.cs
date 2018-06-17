using SimpleRuleEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SimpleRuleEngine.Helper
{
    public static class Extensions
    {
        public static readonly Dictionary<ValueTypes, string> TypeToConvert =
            new Dictionary<ValueTypes, string>()
            {
                {ValueTypes.DateTime, "ToDateTime" },
                {ValueTypes.Integer, "ToSingle" },
                {ValueTypes.String, "ToString" }
            };

        public static readonly Dictionary<ValueTypes, Type> TypeToType =
            new Dictionary<ValueTypes, Type>()
            {
                {ValueTypes.DateTime, typeof(DateTime) },
                {ValueTypes.Integer, typeof(float) },
                {ValueTypes.String, typeof(string) }
            };

        public static bool IsBandRule(RuleInfo rule)
        {
            switch(rule.Operator)
            {
                case Operator.InBand:
                case Operator.OutBand:
                    return true;
            }
            return false;
        }

        public static int MaxExprType => 
            Enum.GetValues(typeof(ExpressionType)).Cast<int>().Max();

        public static string FileName = "FaileData.json";

        //public static Func<Data, string> BuildExpr(this RuleInfo rule)
        //{
        //    Type propType = TypeToType[rule.ValueType];
        //    var convertMethod = TypeToConvert[rule.ValueType];

        //    var paramValue = Expression.Parameter(typeof(Data));

        //    var methodInfo = typeof(Convert).GetMethod(
        //                                    convertMethod,
        //                                    new Type[] { typeof(string) },
        //                                    null);

        //    var left = Expression.Property(paramValue, nameof(Data.Value));

        //    MethodCallExpression convertedLeft = Expression.Call(methodInfo, left);

        //    Func<Data, bool> func = (data) => false;
        //    // if its .net defined operator
        //    if ((int)rule.Operator <= MaxExprType)
        //    {
        //        if (Enum.TryParse(rule.Operator.ToString(), out ExpressionType expressionType))
        //        {
        //            var right = Expression.Constant(Convert.ChangeType(rule.Value, propType));
        //            func =
        //                Expression.Lambda<Func<Data, bool>>
        //                                    (
        //                                        Expression.MakeBinary(expressionType, convertedLeft, right), paramValue
        //                                    ).Compile();
        //        }
        //    }
        //    //new operator
        //    else if (rule.Operator == Operator.IsFuture)
        //    {
        //        var right = Expression.Constant(
        //                            Convert.ChangeType(rule.Value, typeof(DateTime))
        //                            );
        //        func = (data) =>
        //              DateTime.Parse(data.Value) >= DateTime.Now;
        //    }
        //    return (data) => func.Invoke(data) == true ? data.Id + " "+ rule.ToString() : string.Empty;
        //    //throw new InvalidOperationException("Invalid Operator");
        //}
    }

}
