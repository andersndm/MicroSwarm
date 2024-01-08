namespace MicroSwarm.Templates
{
    public static class QueryFilterBaseClassTemplate
    {
        public static string Render(string serviceName)
        {
            return
$$"""
namespace {{serviceName}}Core.Filtering
{
    public abstract class QueryFilter<T>
    {
        public QueryFilter() { }
        public abstract QueryFilter<T> FromJson(string jsonString);

        public virtual Func<T, bool> CreateFilter() => (t) => true;
    }
}
""";
        }
    }
}