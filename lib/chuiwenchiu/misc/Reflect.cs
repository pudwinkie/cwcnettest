/*
var info = Reflect.GetMethod<DateTime>(dt => dt.ToShortDateString());
Console.WriteLine(info.Name);

var dayProperty = Reflect.GetProperty<DateTime>(dt => dt.Day);
Console.WriteLine(dayProperty.Name);

 */
public static class Reflect
{
    public static MethodInfo GetMethod<TClass>(Expression<Action<TClass>> expression)
    {
        var methodCall = expression.Body as MethodCallExpression;
        if(methodCall == null)
        {
            throw new ArgumentException("Expected method call");
        }
        return methodCall.Method;
    }

    public static PropertyInfo GetProperty<TClass>(Expression<Func<TClass, object>> expression)
    {
        MemberExpression memberExpression;
        var unary = expression.Body as UnaryExpression;
        if (unary != null)
        {
            memberExpression = unary.Operand as MemberExpression;
        }
        else
        {
            memberExpression = expression.Body as MemberExpression;
        }
        if (memberExpression == null || !(memberExpression.Member is PropertyInfo))
        {
            throw new ArgumentException("Expected property expression");
        }
        return (PropertyInfo) memberExpression.Member;
    }
}