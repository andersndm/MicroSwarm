using System.Text;

namespace MicroSwarm.Templates
{
    public static class DbContextTemplate
    {
        public static string RenderHeader(string solutionName, string serviceName, string className)
        {
            return
$$"""
using {{solutionName}}Core.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace {{serviceName}}
{
    public class {{className}} : DbContext
    {
""";
        }

        public static string RenderDbSet(string name)
        {
            return
$$"""
        public DbSet<{{name}}> {{name}} { get; set; }
""";
        }

        public static string RenderDbSets(IEnumerable<string> names)
        {
            StringBuilder builder = new();
            foreach (var name in names)
            {
                builder.AppendLine(RenderDbSet(name));
            }
            return builder.ToString();
        }

        public static string RenderConstructor(string serviceName, string className)
        {
            return
$$"""
        public {{className}}()
        {
            try
            {
                _ = Database.EnsureCreated();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public {{className}}(DbContextOptions<{{className}}> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("{{serviceName}}DB");
        }
""";
        }

        public static string RenderOnModelCreatingHeader()
        {
            return
"""
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
""";
        }

        public static string RenderOnModelCreatingFooter()
        {
            return
"""
        }
""";
        }

        public static string RenderFooter()
        {
            return
    """
    }
}
""";
        }

        public static string RenderEntitySpecHeader(string entityName)
        {
            return
$$"""
modelBuilder.Entity<{{entityName}}>(entity =>
{
""";
        }

        public static string RenderEntitySpecFooter()
        {
            return
"""
});
""";
        }

        public static string RenderEntityPrimaryKey(string name)
        {
            return
$"""
entity.HasKey(e => e.{name});
""";
        }

        public static string RenderEntityOwns(string name)
        {
            return
$"""
entity.OwnsOne(e => e.{name});
""";
        }
    }
}