using MicroSwarm.FileSystem;
using MicroSwarm.Templates;
using Mss;
using Mss.Types;
using System.Diagnostics;

namespace MssBuilder
{
    public class MssEntityClassFile : MssCSharpFile
    {
        private readonly MssEntity _entity;
        private readonly string _serviceName;

        public bool UsesValueTypes { get => _usesValueTypes; }
        private bool _usesValueTypes = false;

        public MssEntityClassFile(MssEntity entity, IEnumerable<MssRelation> relations, SwarmDir dir, string serviceName)
            : base(MssEntity.GetName(entity, serviceName), dir)
        {
            _serviceName = serviceName;
            _entity = entity;
            string nameSpace = _serviceName + "." + dir.Name;

            List<MssField> fieldsWithRelation = [];
            List<MssField> fieldsWithoutRelation = [];
            foreach (var field in entity.Fields)
            {
                if (relations.Any(r => r.ContainsField(field)))
                {
                    fieldsWithRelation.Add(field);
                }
                else
                {
                    fieldsWithoutRelation.Add(field);
                }
            }

            AppendLine(EntityTemplate.RenderHeader(nameSpace, MssEntity.GetName(entity, _serviceName)));
            const int PROPERTY_INDENTATION_LEVEL = 2;
            Indent(PROPERTY_INDENTATION_LEVEL);

            AddPropertiesWithRelations(fieldsWithRelation, relations);
            _usesValueTypes = AddPropertiesWithoutRelations(fieldsWithoutRelation);

            ClearIndentation();

            Append(EntityTemplate.RenderFooter());

            if (_usesValueTypes)
            {
                PrependLine(UsingTemplate.Render(MssValueTypeProject.ProjectName) + "\n");
            }
        }

        private void AddPropertiesWithRelations(IEnumerable<MssField> fields, IEnumerable<MssRelation> relations)
        {
            foreach (var field in fields)
            {
                // primary key won't be added if part of a relation, so must be manually added here
                if (field.Type is MssKeyType && field.Type.ToString() == "PK")
                {
                    // Todo could add a to c# string
                    AppendLine(PropertyTemplate.Render(field.Name, "int"));
                    continue;
                }

                var rels = relations.Where(r => r.ContainsField(field)).ToList();
                Debug.Assert(rels.Count > 0);
                foreach (var rel in rels)
                {
                    var toField = rels[0].GetOppositeField(field)!;
                    var toEntity = rels[0].GetOppositeEntity(_entity)!;
                    var fromRel = rels[0].GetRelation(field)!;
                    var toRel = rels[0].GetRelation(toField)!;

                    string toEntityName = MssEntity.GetName(toEntity, _serviceName);
                    Debug.Assert(field.Type.IsPkFkPair(toField.Type));

                    if (toRel.IsMany)
                    {
                        AppendLine(PropertyTemplate.Render(toEntityName, $"List<{toEntityName}>"));
                    }
                    else
                    {
                        AppendLine(PropertyTemplate.Render(toEntityName, toEntityName));
                    }
                }
            }
        }

        private bool AddPropertiesWithoutRelations(IEnumerable<MssField> fields)
        {
            bool usesValueTypes = false;
            foreach (var field in fields)
            {
                var typeStr = field.Type.ToString();
                if (field.Type is MssClassType)
                {
                    usesValueTypes = true;
                }
                else if (field.Type is MssKeyType)
                {
                    typeStr = "int";
                }
                AppendLine(PropertyTemplate.Render(field.Name, typeStr));
            }
            return usesValueTypes;
        }
    }
}