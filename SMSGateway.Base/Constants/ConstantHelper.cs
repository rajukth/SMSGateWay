using System.Reflection;

namespace SMSGateway.Base.Constants;

public class ConstantHelper
{
    private static readonly Dictionary<(Type, char), string> Cache = new Dictionary<(Type, char), string>();

    public static string GetName<T>(char value) where T : class
    {
        var key = (typeof(T), value);
        if (Cache.TryGetValue(key, out var name))
        {
            return name;
        }

        var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(f => f.IsLiteral && !f.IsInitOnly && f.FieldType == typeof(char));

        foreach (var field in fields)
        {
            if ((char)field.GetValue(null) == value)
            {
                name = field.Name;
                Cache[key] = name;
                return name;
            }
        }

        return "Unknown"; // Or throw an exception or handle it as per your requirements
    }
}
