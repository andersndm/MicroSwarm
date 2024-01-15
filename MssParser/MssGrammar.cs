using Irony.Parsing;
using Mss.Ast;

namespace Mss.Parsing
{
    /*
       See Outline.md for grammar definition
    */
    public class MssGrammar : Grammar
    {
        public MssGrammar() : base(caseSensitive: true)
        {
            // Non-Terminals
            var spec = new NonTerminal("Spec", typeof(MssSpecNode));
            var specList = new NonTerminal("SpecList", typeof(MssSpecListNode));

            var service = new NonTerminal("Service", typeof(MssServiceNode));
            var root = new NonTerminal("Root", typeof(MssRootNode));
            var rootProperty = new NonTerminal("RootProperty", typeof(MssRootPropertyNode));
            var rootPropertyList = new NonTerminal("RootPropertyList", typeof(MssRootPropertyListNode));
            var rootPropertyType = new NonTerminal("RootPropertyType", typeof(MssRootPropertyTypeNode));

            var listType = new NonTerminal("ListType", typeof(MssListTypeNode));
            var builtInType = new NonTerminal("BuiltInType", typeof(MssBuiltInTypeNode));

            // Terminals
            var identifier = new IdentifierTerminal("Identifier");
            identifier.AstConfig.NodeType = typeof(MssIdentifierNode);

            var intType = ToTerm("int");
            var floatType = ToTerm("float");
            var stringType = ToTerm("string");
            var boolType = ToTerm("bool");
            var externalKey = ToTerm("EK");

            // Comment Terminals
            var lineComment = new CommentTerminal("LineComment", "//", "\n", "\r\n");
            var multilineComment = new CommentTerminal("MultilineComment", "/*", "*/");

            NonGrammarTerminals.Add(lineComment);
            NonGrammarTerminals.Add(multilineComment);

            spec.Rule = specList;
            specList.Rule = MakePlusRule(specList, service);

            service.Rule = ToTerm("service") + identifier + "{" + root + "}";
            root.Rule = ToTerm("root") + "{" + rootPropertyList + "}";

            rootPropertyList.Rule = MakePlusRule(rootPropertyList, rootProperty);
            rootProperty.Rule = identifier + ":" + rootPropertyType + ";";
            rootPropertyType.Rule = builtInType | listType;

            listType.Rule = ToTerm("List") + "<" + builtInType + ">";

            builtInType.Rule = intType | floatType | stringType | boolType | externalKey;

            this.Root = spec;
            this.LanguageFlags = LanguageFlags.CreateAst;
        }
    }
}