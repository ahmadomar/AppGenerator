using System;
using System.Collections.Generic;
using System.Data.Entity.Design;
using System.Data.Metadata.Edm;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;

namespace AppsGenerator.Classes.Generator
{
    public class EdmMapping
    {
        public EdmMapping(EntityModelSchemaGenerator mcGenerator, StoreItemCollection store)
        {
            //DebugCheck.NotNull(mcGenerator);
            //DebugCheck.NotNull(store);

            // Pull mapping xml out
            var mappingDoc = new XmlDocument();
            var mappingXml = new StringBuilder();

            using (var textWriter = new StringWriter(mappingXml))
            {
                mcGenerator.WriteStorageMapping(new XmlTextWriter(textWriter));
            }

            mappingDoc.LoadXml(mappingXml.ToString());

            var entitySets = mcGenerator.EntityContainer.BaseEntitySets.OfType<EntitySet>();
            var associationSets = mcGenerator.EntityContainer.BaseEntitySets.OfType<AssociationSet>();
            var tableSets = store.GetItems<EntityContainer>().Single().BaseEntitySets.OfType<EntitySet>();

            this.EntityMappings = BuildEntityMappings(mappingDoc, entitySets, tableSets);
            this.ManyToManyMappings = BuildManyToManyMappings(mappingDoc, associationSets, tableSets);
        }

        public Dictionary<EntityType, Tuple<EntitySet, Dictionary<EdmProperty, EdmProperty>>> EntityMappings { get; set; }

        public Dictionary<AssociationType, Tuple<EntitySet, Dictionary<RelationshipEndMember, Dictionary<EdmMember, string>>>> ManyToManyMappings { get; set; }

        private static Dictionary<AssociationType, Tuple<EntitySet, Dictionary<RelationshipEndMember, Dictionary<EdmMember, string>>>> BuildManyToManyMappings(XmlDocument mappingDoc, IEnumerable<AssociationSet> associationSets, IEnumerable<EntitySet> tableSets)
        {
            //DebugCheck.NotNull(mappingDoc);
            //DebugCheck.NotNull(associationSets);
            //DebugCheck.NotNull(tableSets);

            // Build mapping for each association
            var mappings = new Dictionary<AssociationType, Tuple<EntitySet, Dictionary<RelationshipEndMember, Dictionary<EdmMember, string>>>>();
            var namespaceManager = new XmlNamespaceManager(mappingDoc.NameTable);
            namespaceManager.AddNamespace("ef", mappingDoc.ChildNodes[0].NamespaceURI);
            foreach (var associationSet in associationSets.Where(a => !a.ElementType.AssociationEndMembers.Where(e => e.RelationshipMultiplicity != RelationshipMultiplicity.Many).Any()))
            {
                var setMapping = mappingDoc.SelectSingleNode(string.Format("//ef:AssociationSetMapping[@Name=\"{0}\"]", associationSet.Name), namespaceManager);
                var tableName = setMapping.Attributes["StoreEntitySet"].Value;
                var tableSet = tableSets.Single(s => s.Name == tableName);

                var endMappings = new Dictionary<RelationshipEndMember, Dictionary<EdmMember, string>>();
                foreach (var end in associationSet.AssociationSetEnds)
                {
                    var propertyToColumnMappings = new Dictionary<EdmMember, string>();
                    var endMapping = setMapping.SelectSingleNode(string.Format("./ef:EndProperty[@Name=\"{0}\"]", end.Name), namespaceManager);
                    foreach (XmlNode fk in endMapping.ChildNodes)
                    {
                        var propertyName = fk.Attributes["Name"].Value;
                        var property = end.EntitySet.ElementType.Properties[propertyName];
                        var columnName = fk.Attributes["ColumnName"].Value;
                        propertyToColumnMappings.Add(property, columnName);
                    }

                    endMappings.Add(end.CorrespondingAssociationEndMember, propertyToColumnMappings);
                }

                mappings.Add(associationSet.ElementType, Tuple.Create(tableSet, endMappings));
            }

            return mappings;
        }

        private static Dictionary<EntityType, Tuple<EntitySet, Dictionary<EdmProperty, EdmProperty>>> BuildEntityMappings(XmlDocument mappingDoc, IEnumerable<EntitySet> entitySets, IEnumerable<EntitySet> tableSets)
        {
            //DebugCheck.NotNull(mappingDoc);
            //DebugCheck.NotNull(entitySets);
            //DebugCheck.NotNull(tableSets);

            // Build mapping for each type
            var mappings = new Dictionary<EntityType, Tuple<EntitySet, Dictionary<EdmProperty, EdmProperty>>>();
            var namespaceManager = new XmlNamespaceManager(mappingDoc.NameTable);
            namespaceManager.AddNamespace("ef", mappingDoc.ChildNodes[0].NamespaceURI);
            foreach (var entitySet in entitySets)
            {
                // Post VS2010 builds use a different structure for mapping
                var setMapping = mappingDoc.ChildNodes[0].NamespaceURI == "http://schemas.microsoft.com/ado/2009/11/mapping/cs"
                    ? mappingDoc.SelectSingleNode(string.Format("//ef:EntitySetMapping[@Name=\"{0}\"]/ef:EntityTypeMapping/ef:MappingFragment", entitySet.Name), namespaceManager)
                    : mappingDoc.SelectSingleNode(string.Format("//ef:EntitySetMapping[@Name=\"{0}\"]", entitySet.Name), namespaceManager);

                var tableName = setMapping.Attributes["StoreEntitySet"].Value;
                var tableSet = tableSets.Single(s => s.Name == tableName);

                var propertyMappings = new Dictionary<EdmProperty, EdmProperty>();
                foreach (var prop in entitySet.ElementType.Properties)
                {
                    var propMapping = setMapping.SelectSingleNode(string.Format("./ef:ScalarProperty[@Name=\"{0}\"]", prop.Name), namespaceManager);
                    var columnName = propMapping.Attributes["ColumnName"].Value;
                    var columnProp = tableSet.ElementType.Properties[columnName];

                    propertyMappings.Add(prop, columnProp);
                }

                mappings.Add(entitySet.ElementType, Tuple.Create(tableSet, propertyMappings));
            }

            return mappings;
        }
    }
}