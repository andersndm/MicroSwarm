namespace Mss.Types
{
    public class MssKeyType(string identifier) : MssType(identifier)
    {
        public override bool IsSameType(MssType type)
        {
            if (type is MssKeyType key)
            {
                return ToString() == type.ToString();
            }

            return false;
        }

        public override bool IsPkFkPair(MssType type)
        {
            if (type is MssKeyType keyType)
            {
                if ((ToString() == "PK" && keyType.ToString() == "FK") ||
                    (ToString() == "FK" && keyType.ToString() == "PK"))
                {
                    return true;
                }
            }
            return false;
        }
    }
}