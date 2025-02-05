namespace PermissionPOC.attributes;

[AttributeUsage(AttributeTargets.Property)]
public class PermissionAttribute : Attribute
{
    public List<string> Values { get; }

    // Constructor to accept a list of permission keys
    public PermissionAttribute(params string[] values)
    {
        Values = values.ToList();
    }
}