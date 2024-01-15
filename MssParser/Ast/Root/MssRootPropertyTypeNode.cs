using Irony.Ast;
using Irony.Parsing;
using Mss.Ast.Visitor;
using Mss.Types;

namespace Mss.Ast
{
    public class MssRootPropertyTypeNode : MssNode
    {
        public MssBuiltInTypeNode? BuiltInType { get; set; } = null;
        public MssListTypeNode? ListType { get; set; } = null;
        public MssType? Type { get; set; } = null;

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            if (Children.Count > 0)
            {
                const int EXPECTED_CHILD_COUNT = 1;
                if (Children.Count == EXPECTED_CHILD_COUNT)
                {
                    if (Children[0] is MssBuiltInTypeNode builtin)
                    {
                        BuiltInType = builtin;
                    }
                    else if (Children[0] is MssListTypeNode list)
                    {
                        ListType = list;
                    }
                    else
                    {
                        throw new InvalidChildTypeException();
                    }
                }
                else
                {
                    throw new InvalidChildCountException(EXPECTED_CHILD_COUNT, Children.Count);
                }
            }
            else
            {
                Token = treeNode.ChildNodes[0].Token;
                AsString = "RootPropertyType: " + Token.Text;
            }
        }

        public override void Accept(IMssAstVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}