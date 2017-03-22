using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Net66.Data.Base
{
    public static class EfUtils
    {
        public static string GetContext_ConneString
        {
            get
            {
                string result;
                try
                {
                    result = ConfigurationManager.ConnectionStrings["SSEContext"].ConnectionString;
                }
                catch
                {
                    result = "";
                }
                return result;
            }
        }
       
        public static Expression<Func<T, bool>> True<T>()
        {
            return (T f) => true;
        }
        public static Expression<Func<T, bool>> False<T>()
        {
            return (T f) => false;
        }
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> exp_left, Expression<Func<T, bool>> exp_right)
        {
            ParameterExpression parameterExpression = Expression.Parameter(typeof(T), "candidate");
            ParameterReplacer expr_1B = new ParameterReplacer(parameterExpression);
            Expression left = expr_1B.Replace(exp_left.Body);
            Expression right = expr_1B.Replace(exp_right.Body);
            return Expression.Lambda<Func<T, bool>>(Expression.Or(left, right), new ParameterExpression[]
            {
                parameterExpression
            });
        }
        public static Expression<Func<T, bool>> AndOr<T>(string[] keys, string orKey, T t)
        {
            if (keys == null)
            {
                return null;
            }
            Expression<Func<T, bool>> expression = EfUtils.And<T>(keys, t);
            if (!string.IsNullOrEmpty(orKey))
            {
                Expression<Func<T, bool>> exp_right = EfUtils.And<T>(new string[]
                {
                    orKey
                }, t);
                expression = expression.Or(exp_right);
            }
            return expression;
        }
        public static Expression<Func<T, bool>> And<T>(string[] keys, T t)
        {
            if (keys == null)
            {
                return null;
            }
            Expression<Func<T, bool>> expression = EfUtils.True<T>();
            for (int i = 0; i < keys.Length; i++)
            {
                string text = keys[i];
                PropertyInfo expr_28 = t.GetType().GetProperty(text);
                string val = expr_28.GetValue(t, null).ToString();
                string type = expr_28.ToString();
                if (text.ToLower().Equals("datetime") || text.ToLower().Equals("clientdatetime"))
                {
                    expression = expression.And(EfUtils.AndIndexOf<T>(type, text, val));
                }
                else
                {
                    expression = expression.And(EfUtils.And<T>(type, text, val));
                }
            }
            return expression;
        }
        public static Expression<Func<T, bool>> And<T>(string type, string key, string val)
        {
            ParameterExpression parameterExpression = Expression.Parameter(typeof(T), "candidate");
            Expression arg_65_0 = Expression.Property(parameterExpression, typeof(T).GetProperty(key));
            Expression right;
            if (type.ToLower().IndexOf("system.nullable`1[system.int32]") >= 0)
            {
                right = Expression.Constant(Convert.ToInt32(val), typeof(int?));
            }
            else
            {
                right = Expression.Constant(val);
            }
            return Expression.Lambda<Func<T, bool>>(Expression.Equal(arg_65_0, right), new ParameterExpression[]
            {
                parameterExpression
            });
        }
        public static Expression<Func<T, bool>> AndIndexOf<T>(string type, string key, string val)
        {
            ParameterExpression parameterExpression = Expression.Parameter(typeof(T), "candidate");
            Expression arg_52_0 = Expression.Property(parameterExpression, typeof(T).GetProperty(key));
            MethodInfo method = typeof(string).GetMethod("Contains");
            ConstantExpression constantExpression = Expression.Constant(val);
            return Expression.Lambda<Func<T, bool>>(Expression.Call(arg_52_0, method, new Expression[]
            {
                constantExpression
            }), new ParameterExpression[]
            {
                parameterExpression
            });
        }
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> exp_left, Expression<Func<T, bool>> exp_right)
        {
            ParameterExpression parameterExpression = Expression.Parameter(typeof(T), "candidate");
            ParameterReplacer expr_1B = new ParameterReplacer(parameterExpression);
            Expression left = expr_1B.Replace(exp_left.Body);
            Expression right = expr_1B.Replace(exp_right.Body);
            return Expression.Lambda<Func<T, bool>>(Expression.And(left, right), new ParameterExpression[]
            {
                parameterExpression
            });
        }
    }

    internal class ParameterReplacer : ExpressionVisitor
    {
        public ParameterExpression ParameterExpression
        {
            get;
            private set;
        }
        public ParameterReplacer(ParameterExpression paramExpr)
        {
            this.ParameterExpression = paramExpr;
        }
        public Expression Replace(Expression expr)
        {
            return this.Visit(expr);
        }
        protected override Expression VisitParameter(ParameterExpression p)
        {
            return this.ParameterExpression;
        }
    }

}
