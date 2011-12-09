using System;
using System.Linq.Expressions;

namespace AttributeRouting.Extensions
{
    internal static class ObjectExtensions
    {
        public static TResult SafeGet<T, TResult>(this T obj, Expression<Func<T, TResult>> memberExpression)
        {
            return SafeGet(obj, memberExpression, default(TResult));
        }

        public static TResult SafeGet<T, TResult>(this T obj, Expression<Func<T, TResult>> memberExpression, TResult defaultValue)
        {
            try
            {
                var result = memberExpression.Compile().Invoke(obj);
                return result;
            }
            catch (NullReferenceException)
            {
                return defaultValue;
            }
        }
    }
}
