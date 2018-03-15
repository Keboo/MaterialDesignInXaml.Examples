using Microsoft.DwayneNeed.Reactive.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace Microsoft.DwayneNeed.Reactive
{
    public class ObservableExpression<T> : IObservableValue<T>
    {
        public ObservableExpression(Expression<Func<T>> expression)
        {
            Expression body = expression.Body;
            if (body.NodeType != ExpressionType.Call)
            {
                throw new ArgumentException("The Expression must be a method call.");
            }

            MethodCallExpression methodCall = body as MethodCallExpression;
        }

        public T GetValue()
        {
	        throw new NotImplementedException();
        }


        public IObservable<T> Changed
        {
            get { throw new NotImplementedException(); }
        }
    }
}
