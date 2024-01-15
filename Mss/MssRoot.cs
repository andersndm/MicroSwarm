namespace Mss
{
    public class MssRoot(IEnumerable<MssField> fields)
    {
        public readonly IEnumerable<MssField> Fields = fields;

        public MssField? GetField(string name)
        {
            return Fields.FirstOrDefault(f => f.Name == name);
        }
    }
}