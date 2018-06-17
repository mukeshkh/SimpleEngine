using System.Linq.Expressions;

namespace SimpleRuleEngine.Models
{
    public enum ValueTypes
    {
        Integer,
        String,
        DateTime
    }

    public enum FileFormat
    {
        json
    }

    public enum Operator
    {
        /// <summary>
        /// A "Equal to" comparison, such as (a == b).
        /// </summary>
        EqualTo = ExpressionType.Equal,

        /// <summary>
        /// A "greater than" comparison, such as (a > b).
        /// </summary>
        GreaterThan = ExpressionType.GreaterThan,

        /// <summary>
        /// A "greater than or equal to" comparison, such as (a >= b).
        /// </summary>
        GreaterThanOrEqual = ExpressionType.GreaterThanOrEqual,

        /// <summary>
        /// A "less than" comparison, such as (a < b).
        /// </summary>
        LessThan = ExpressionType.LessThan,

        /// <summary>
        /// A "less than or equal to" comparison, such as (a <= b).
        /// </summary>
        LessThanOrEqual = ExpressionType.LessThanOrEqual,

        /// <summary>
        /// An inequality comparison, such as (a != b).
        /// </summary>
        NotEqual = ExpressionType.NotEqual,

        /// <summary>
        /// 
        /// </summary>
        IsFuture = 100,

        /// <summary>
        /// An band comparison within range (by 2 numbers), such as (a within [min to max]).
        /// </summary>
        InBand,

        /// <summary>
        /// An band comparison outside range (by 2 numbers) , such as (a out of [min to max] ).
        /// </summary>
        OutBand,
    }
}
