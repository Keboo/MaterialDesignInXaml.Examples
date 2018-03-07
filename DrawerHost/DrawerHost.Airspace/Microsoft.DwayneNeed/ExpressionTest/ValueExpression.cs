using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExpressionTest
{
    // Eventually consider a family of classes/interfaces:
    //
    // IValueExpression (GetValue).
    // IChangeableValueExpression (Changed event)
    // I

    class ValueExpression<T>
    {
        public ValueExpression(T value)
        {
            _value = value;
        }

        public static implicit operator T(ValueExpression<T> valueExpression)
        {
            return valueExpression._value;
        }

        private T _value;
    }
}
