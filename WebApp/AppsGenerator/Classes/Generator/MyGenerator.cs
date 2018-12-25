using AppsGenerator.Classes.Generator;
using AppsGenerator.CSharpGenerator.Web.Me;
using AppsGenerator.CSharpGenerator.Web.MvcView;
using AppsGenerator.CSharpGenerator.Web.ReverseEngineerCodeFirst;
using Microsoft.AspNet.Scaffolding.Core.Metadata;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data.Entity.Design;
using System.Data.Entity.Design.PluralizationServices;
using System.Data.Metadata.Edm;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Resources;
using System.Web;
using AppsGenerator.CSharpGenerator.Web.MvcControllerWithContext;
using AppsGenerator.Classes.Mains;

namespace AppsGenerator.Classes.Generator
{
    public class MyGenerator : ApplicationMain
    {
        public IEnumerable<EntityType> DatabaseTables { get; set; }

        private EntityStoreSchemaGenerator entityStoreSchemaGenerator { set; get; }
        private EntityModelSchemaGenerator entityModelSchemaGenerator { set; get; }

        public MyGenerator(string ConnectionString, String ApplicationName, string AppPath = null)
            : base(ConnectionString, ApplicationName, AppPath)
        {
            entityStoreSchemaGenerator = GetEntityStoreSchemaGenerator();
            entityModelSchemaGenerator = GetEntityModelSchemaGenerator();

            DatabaseTables = GetTablesAsEntityType();
        }

        private static readonly IEnumerable<EntityStoreSchemaFilterEntry> _storeMetadataFilters = new[]
            {
                new EntityStoreSchemaFilterEntry(null, null, "EdmMetadata", EntityStoreSchemaFilterObjectTypes.Table, EntityStoreSchemaFilterEffect.Exclude),
                new EntityStoreSchemaFilterEntry(null, null, "__MigrationHistory", EntityStoreSchemaFilterObjectTypes.Table, EntityStoreSchemaFilterEffect.Exclude)
            };


        private EntityStoreSchemaGenerator GetEntityStoreSchemaGenerator()
        {
            EntityStoreSchemaGenerator storeGenerator =
               new EntityStoreSchemaGenerator("System.Data.SqlClient", ConnectionString, ApplicationName);

            storeGenerator.GenerateForeignKeyProperties = true;

            IList<EdmSchemaError> ssdlErrors = storeGenerator.GenerateStoreMetadata(_storeMetadataFilters);

            if (ssdlErrors.Count > 0)
                return null;
            return storeGenerator;
        }

        private EntityModelSchemaGenerator GetEntityModelSchemaGenerator()
        {
            if (entityStoreSchemaGenerator == null)
                return null;
            var modelGenerator = new EntityModelSchemaGenerator(entityStoreSchemaGenerator.EntityContainer, ApplicationName, ApplicationName);
            //modelGenerator.PluralizationService = PluralizationService.CreateService(new CultureInfo("en"));
            modelGenerator.GenerateForeignKeyProperties = true;

            modelGenerator.GenerateMetadata();
            return modelGenerator;
        }

        public IEnumerable<EntitySet> GetTablesAsEntitySet()
        {
            if (entityModelSchemaGenerator == null)
                return null;
            var tables = entityModelSchemaGenerator.EntityContainer.BaseEntitySets.OfType<EntitySet>();
            return tables;
        }


        private IEnumerable<EntityType> GetTablesAsEntityType()
        {
            if (entityModelSchemaGenerator == null)
                return null;

            //// Pull out info about types to be generated
            var entityTypes = entityModelSchemaGenerator.EdmItemCollection.OfType<EntityType>().ToArray();
            return entityTypes;
        }


        private ReadOnlyMetadataCollection<EdmProperty> GetProperties(EntityType table)
        {
            ReadOnlyMetadataCollection<EdmProperty> properties;

            properties = table.Properties;
            return properties;
        }


        private List<EdmMember> GetMemberKeys(EntityType table)
        {
            List<EdmMember> keys = table.KeyMembers.ToList();
            return keys;
        }


        private ModelMetadata GetModelMetadata(EntityType entityType, ReadOnlyMetadataCollection<EdmProperty> properties, string primaryKey)
        {
            var modelMetadata = new ModelMetadata();
            var props = new List<PropertyMetadata>();
            var primaryKeys = new List<PropertyMetadata>();

            var foreignKeys = GetForeignKeys(entityType);

            foreach (var ce in entityType.Properties)
            {
                PropertyMetadata property = new PropertyMetadata();

                property.PropertyName = ce.Name;
                property.TypeName = ce.TypeUsage.EdmType.FullName;
                property.DefaultValue = ce.Name;
                property.IsEnum = false;
                property.IsForeignKey = false;

                foreach (var item in foreignKeys)
                {
                    if (ce.Name.Equals(item.Name))
                    {
                        property.IsForeignKey = true;
                    }
                }

                if (primaryKey.Equals(property.PropertyName))
                {
                    primaryKeys.Add(property);
                    property.IsPrimaryKey = true;
                }
                property.Scaffold = true;
                property.ShortTypeName = ce.TypeUsage.EdmType.Name;
                property.IsReadOnly = false;
                props.Add(property);

            }


            modelMetadata.EntitySetName = entityType.Name;
            modelMetadata.PrimaryKeys = primaryKeys.ToArray();
            modelMetadata.Properties = props.ToArray();

            if (foreignKeys.Count == 0)
                modelMetadata.RelatedEntities = new RelatedModelMetadata[] { };
            else
                modelMetadata.RelatedEntities = GetRelatedModelMetadata(entityType);

            return modelMetadata;
        }

        List<EdmProperty> GetForeignKeys(EntityType entityType)
        {
            //Read foreign keys
            List<EdmProperty> foreignKeys = new List<EdmProperty>();
            foreach (var entityMember in entityType.NavigationProperties)
                foreach (System.Data.Metadata.Edm.EdmProperty foreignKey in entityMember.GetDependentProperties())
                {
                    //... use foreignKey
                    foreignKeys.Add(foreignKey);
                }
            return foreignKeys;
        }

        RelatedModelMetadata[] GetRelatedModelMetadata(EntityType entityType)
        {
            var foreignKeys = GetForeignKeys(entityType);
            List<RelatedModelMetadata> relatedModelMetadata = new List<RelatedModelMetadata>();

            for (int x = 0; x < foreignKeys.Count; x++)
            {
                RelatedModelMetadata relatedModel = new RelatedModelMetadata();
                foreach (var navigationProperty in entityType.NavigationProperties)
                {
                    EntityType fromEntity = navigationProperty.FromEndMember.GetEntityType();
                    EntityType toEntity = navigationProperty.ToEndMember.GetEntityType();

                    var relatedEntity = relatedModelMetadata.Where(re => re.AssociationPropertyName == toEntity.Name).Count();
                    if (relatedEntity != 0)
                        continue;

                    RelationshipType relationshipType = navigationProperty.RelationshipType;
                    relatedModel.AssociationPropertyName = navigationProperty.Name; //((AssociationType)(relationshipType)).ReferentialConstraints[0].ToProperties[0].Name;

                    //if (fromEntity.Properties.Count > 1)
                    //    relatedModel.DisplayPropertyName = toEntity.Properties[1].Name;
                    //else
                        relatedModel.DisplayPropertyName = toEntity.Properties[0].Name;
                    relatedModel.EntitySetName = toEntity.Name;//navigationProperty.Name;

                    relatedModel.PrimaryKeyNames = new string[] { fromEntity.KeyMembers[0].Name };
                    relatedModel.ShortTypeName = fromEntity.Name;
                    relatedModel.TypeName = fromEntity.FullName;

                    if (!relatedModelMetadata.Contains(relatedModel))
                        relatedModelMetadata.Add(relatedModel);

                    string AssociationName = ((AssociationType)(relationshipType)).ReferentialConstraints[0].ToProperties[0].Name;
                    if (AssociationName == foreignKeys[x].Name)
                        break;
                }

                relatedModel.ForeignKeyPropertyNames = new string[] { foreignKeys[x].Name };
            }

            return relatedModelMetadata.ToArray();
        }

        public ModelMetadata GetModelMetadata(EntityType table)
        {
            return GetModelMetadata(table, GetProperties(table), GetMemberKeys(table).First().Name);
        }


        public void GenerateCode()
        {
            var entityTypes = DatabaseTables;//GetTablesAsEntityType();
            var mappings = new EdmMapping(entityModelSchemaGenerator, entityStoreSchemaGenerator.StoreItemCollection);

            // Generate Entity Classes and Mappings
            var modelsNamespace = ApplicationName + ".Models";
            var modelsDirectory = Path.Combine(AppPath, "Models");
            var mappingNamespace = modelsNamespace + ".Mapping";
            var mappingDirectory = Path.Combine(modelsDirectory, "Mapping");
            //var entityFrameworkVersion = GetEntityFrameworkVersion(references);

            foreach (var entityType in entityTypes)
            {
                // Generate the code file
                var entity = new Entity
                {
                    entityType = entityType,
                    Namespace = modelsNamespace,
                    EntityFrameworkVersion = new Version(4, 4)
                    //TableSet = mappings.EntityMappings[entityType].Item1
                };
                string code = entity.TransformText();
                var filePath = Path.Combine(modelsDirectory, entityType.Name + ".cs");
                File.AppendAllText(filePath, code);

                var mapping = new Mapping
                {
                    entityType = entityType,
                    MappingNamespace = mappingNamespace,
                    ModelsNamespace = modelsNamespace,
                    EntityFrameworkVersion = new Version(4, 4),
                    entitySet = mappings.EntityMappings[entityType].Item1,
                    PropertyToColumnMappings = mappings.EntityMappings[entityType].Item2,
                    ManyToManyMappings = mappings.ManyToManyMappings
                };
                var mappingContents = mapping.TransformText();

                var mappingFilePath = Path.Combine(mappingDirectory, entityType.Name + "Map.cs");
                File.AppendAllText(mappingFilePath, mappingContents);
            }

            CreateClassesMetadata(entityTypes.ToList(), modelsNamespace, modelsDirectory);
        }

        private void CreateClassesMetadata(List<EntityType> EntityTypes, string ModelsNamespace, string ModelsDirectory)
        {
            //Generate Classes Metadata
            ClassesMetadata classesMetadata = new ClassesMetadata();
            classesMetadata.Namespace = ModelsNamespace;
            classesMetadata.Tables = EntityTypes.ToList();
            var metadataFilePath = Path.Combine(ModelsDirectory, "ModelsMetadata.cs");
            File.AppendAllText(metadataFilePath, classesMetadata.TransformText());
        }
    }
}