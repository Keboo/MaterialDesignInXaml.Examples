using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExpressionTest
{
    class Program
    {
        static void Main(string[] args)
        {
            ValueExpression<int> a = new ValueExpression<int>(5);
            ValueExpression<int> b = new ValueExpression<int>(6);
            object c = a + b;

        }
    }
}
