using System.Linq.Expressions;

namespace System
{
    public class PropertyHelper
    {
        /// <summary>
        /// 返回一个属性的名称
        /// </summary>
        /// <typeparam name="P">属性所在的类的类型</typeparam>
        /// <typeparam name="T">属性类型</typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static string GetPropertyName<P, T>(Expression<Func<P, T>> expression)
        {
            if (expression.Body is UnaryExpression)
            {
                UnaryExpression unex = (UnaryExpression)expression.Body;
                if (unex.NodeType == ExpressionType.Convert)
                {
                    Expression ex = unex.Operand;
                    MemberExpression mex = (MemberExpression)ex;
                    return mex.Member.Name;
                }
            }

            MemberExpression memberExpression = (MemberExpression)expression.Body;
            return memberExpression.Member.Name;
        }

        /// <summary>
        /// 属性辅助类
        /// </summary>
        /// <typeparam name="Class"></typeparam>
        public class Property<Class>
        {
            /// <summary>
            /// 返回一个属性的名称
            /// 调用：string PropertyName =  PropertyHelper.Property<System.Web.UI.Page>.GetPropertyName(p => p.Request);
            /// 返回属性Request的名称：Request
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="Field"></param>
            /// <returns></returns>
            public static string GetPropertyName<T>(Expression<Func<Class, T>> Field)
            {
                return PropertyHelper.GetPropertyName(Field);
            }
        }

    }
}
