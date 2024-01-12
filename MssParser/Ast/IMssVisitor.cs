namespace Mss.Ast.Visitor
{
    public interface IMssAstVisitor
    {
        void Visit(MssSpecNode node);
        void Visit(MssSpecListNode node);

        void Visit(MssServiceNode node);
        void Visit(MssRootNode node);
        void Visit(MssRootPropertyNode node);
        void Visit(MssRootPropertyListNode node);
        void Visit(MssRootPropertyTypeNode node);
        void Visit(MssIdentifierNode node);

        void Visit(MssBuiltInTypeNode node);
        void Visit(MssListTypeNode node);
    }
}