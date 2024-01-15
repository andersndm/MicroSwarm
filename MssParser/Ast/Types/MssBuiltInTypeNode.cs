using Irony.Ast;
using Irony.Parsing;
using Mss.Ast.Visitor;
using Mss.Types;

namespace Mss.Ast
{
    public class MssBuiltInTypeNode : MssNode
    {
        public string TypeString { get; set; } = "";
        public MssType? Type { get; set; } = null;
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            Token = treeNode.ChildNodes[0].Token;
            TypeString = Token.Text;
            AsString = "BuiltInType: " + TypeString;
        }

        public override void Accept(IMssAstVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

}