using System;
using System.Linq.Expressions;
using System.Reflection;

namespace AttributeRouting.Helpers
{
    public class ExpressionHelper
    {
        public static MethodInfo GetMethodInfo<TType, TMethod>(Expression<Func<TType, TMethod>> expression)
        {
            var methodCall = expression.Body as MethodCallExpression;
            if (methodCall != null)
                return methodCall.Method;

            throw new ArgumentException("expression must be a MethodCallExpression");
        }
    }
}
