using System;

using TextualDB.Components.Operations.Exceptions;

namespace TextualDB.Components.Operations
{
    public class TextualWhereCondition
    {
        public WhereOperation WhereOperation { get; private set; }

        public string Column { get; private set; }
        public object Value { get; private set; }

        public TextualWhereCondition(WhereOperation op, string column, object value)
        {
            WhereOperation = op;
            Column = column;
            Value = value;
        }

        public bool Check(TextualOperation op, TextualRow row)
        {
            var val = row.GetValue(Column);

            if (WhereOperation != WhereOperation.Equal && WhereOperation != WhereOperation.NotEqual
                && WhereOperation != WhereOperation.Any && WhereOperation != WhereOperation.Contains)
            {
                try
                {
                    val = Convert.ToDouble(val);
                    Value = Convert.ToDouble(Value);
                }
                catch
                {
                    throw new IncorrectValueFormatException(op, row.ParentTable);
                }
            }

            switch (WhereOperation)
            {
                case WhereOperation.Any:
                    return true;
                case WhereOperation.Contains:
                    return val.ToString().Contains(Value.ToString());
                case WhereOperation.Equal:
                    return val.ToString() == Value.ToString();
                case WhereOperation.GreaterThan:
                    return (double)val > (double)Value;
                case WhereOperation.GreaterThanOrEqual:
                    return (double)val >= (double)Value;
                case WhereOperation.LesserThan:
                    return (double)val < (double)Value;
                case WhereOperation.LesserThanOrEqual:
                    return (double)val <= (double)Value;
                case WhereOperation.NotEqual:
                    return val.ToString() != Value.ToString();
                default:
                    throw new UnknownWhereOperationException(op, (int)WhereOperation);
            }
        }

        public static WhereOperation ParseWhereOperation(TextualOperation operation, string op)
        {
            switch (op)
            {
                case "*":
                    return WhereOperation.Any;
                case "CONTAINS":
                    return WhereOperation.Contains;
                case "=":
                    return WhereOperation.Equal;
                case ">":
                    return WhereOperation.GreaterThan;
                case ">=":
                    return WhereOperation.GreaterThanOrEqual;
                case "<":
                    return WhereOperation.LesserThan;
                case "<=":
                    return WhereOperation.LesserThanOrEqual;
                case "!=":
                    return WhereOperation.NotEqual;
                default:
                    throw new UnknownWhereOperationException(operation, op);
            }
        }
    }

    public enum WhereOperation
    {
        Any,
        Contains,
        Equal,
        GreaterThan,
        GreaterThanOrEqual,
        LesserThan,
        LesserThanOrEqual,
        NotEqual
    }
}
