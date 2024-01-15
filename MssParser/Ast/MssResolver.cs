using Irony.Parsing;
using Mss.Parsing;
using Mss.Ast;
using Mss.Ast.Visitor;
using Mss.Types;
using System.Diagnostics;

namespace Mss.Resolver
{
    public class MssResolver(string filename) : IMssAstVisitor
    {
        private readonly string _filename = filename;

        public readonly List<MssError> Errors = [];

        private readonly MssInvalidType _invalidType = new();
        private readonly List<MssBuiltInType> _builtInTypes =
        [
            new MssBuiltInType("int"),
            new MssBuiltInType("float"),
            new MssBuiltInType("bool"),
            new MssBuiltInType("string"),
            new MssBuiltInType("EK")
        ];
        private readonly List<MssListType> _listTypes = [];

        private readonly List<MssService> _services = [];
        public IEnumerable<MssService> Services => _services;

        private const string ERROR_IDENTIFIER = "ErrorIdentifier";

        public MssSpec GetSpec()
        {
            return new MssSpec(_services, _filename);
        }

        public IEnumerable<MssType> GetTypes()
        {
            List<MssType> types = [];
            types.AddRange(_builtInTypes);
            types.AddRange(_listTypes);
            return types;
        }

        private string VerifyName(string name, SourceLocation location,
                                  string builtinMsg, string serviceMsg)
        {
            if (name != ERROR_IDENTIFIER)
            {
                if (_builtInTypes.Select(t => t.ToString()).Contains(name))
                {
                    Errors.Add(new(builtinMsg, _filename, location));
                    return ERROR_IDENTIFIER;
                }

                var serviceNames = Services.Select(s => s.Name);
                if (serviceNames.Contains(name))
                {
                    Errors.Add(new(serviceMsg, _filename, location));
                    return ERROR_IDENTIFIER;
                }
            }
            return name;
        }

        private string VerifyName(string name, IEnumerable<MssField> fields, SourceLocation location)
        {
            if (fields.Select(f => f.Name).Contains(name))
            {
                Errors.Add(new("Duplicate field name", _filename, location));
                return ERROR_IDENTIFIER;
            }
            return name;
        }

        private string VerifyServiceName(MssServiceNode node)
        {
            var name = node.Identifier;
            return VerifyName(name, node.Location,
                    "Cannot give a service a built-in type name: " + name,
                    "A service already exists with the name: " + name);
        }

        private MssListType GetListType(MssType subType)
        {
            foreach (var listType in _listTypes)
            {
                if (listType.SubType.IsSameType(subType))
                {
                    return listType;
                }
            }

            var newListType = new MssListType(subType);
            _listTypes.Add(newListType);
            return newListType;
        }

        private MssType ResolveType(string typeName)
        {
            return (MssType?)_builtInTypes.FirstOrDefault(t => t.ToString() == typeName) ?? _invalidType;
        }

        private MssListType ResolveType(MssListTypeNode node)
        {
            return GetListType(node.ContainedTypeNode.Type!);
        }

        private void VisitChildren(MssNode node)
        {
            foreach (var child in node.Children)
            {
                child.Accept(this);
            }
        }

        public void Visit(MssSpecNode node)
        {
            VisitChildren(node);
        }

        public void Visit(MssSpecListNode node)
        {
            VisitChildren(node);
        }

        public void Visit(MssServiceNode node)
        {
            // catch the number of errors before parsing the spec
            var errorCount = Errors.Count;

            VisitChildren(node);

            string serviceName = VerifyServiceName(node);
            MssRoot root = new(node.Root.Fields);

            // if this spec caused errors, don't add it to services
            if (Errors.Count > errorCount) { return; }

            _services.Add(new MssService(serviceName, root));
        }

        public void Visit(MssRootNode node)
        {
            VisitChildren(node);
            node.Fields = node.PropertyList.Fields;
        }

        public void Visit(MssRootPropertyListNode node)
        {
            VisitChildren(node);
            foreach (var prop in node.Properties)
            {
                var identifier = VerifyName(prop.Identifier, node.Fields, prop.Location);
                node.Fields.Add(new(identifier, prop.Type!));
            }
        }

        public void Visit(MssRootPropertyNode node)
        {
            VisitChildren(node);
            node.Type = node.PropertyTypeNode.Type;
        }

        public void Visit(MssRootPropertyTypeNode node)
        {
            VisitChildren(node);
            if (node.BuiltInType != null)
            {
                node.Type = node.BuiltInType.Type;
            }
            else
            {
                Debug.Assert(node.ListType != null);
                node.Type = ResolveType(node.ListType);
            }
        }

        public void Visit(MssListTypeNode node)
        {
            VisitChildren(node);
            node.Type = node.ContainedTypeNode.Type;
        }

        public void Visit(MssBuiltInTypeNode node)
        {
            VisitChildren(node);

            node.Type = ResolveType(node.TypeString);
        }

        public void Visit(MssIdentifierNode node)
        {
            VisitChildren(node);
        }
    }
}