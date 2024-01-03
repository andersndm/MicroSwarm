namespace CLI.Templates
{
    public static class PropertyTemplate
    {
        public static string Render(string propertyName, string typeName)
        {
            return
$$"""
        public {{typeName}} {{propertyName}} { get; set; }
""";
        }
    }
}