﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 17.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace AppsGenerator.CSharpGenerator.Web.ReverseEngineerCodeFirst
{
    using System.Linq;
    using System.Data.Metadata.Edm;
    using AppsGenerator.Classes.Code;
    using System.Collections.Generic;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "C:\Users\Ahmed\Source\Repos\AppGenerator\WebApp\AppsGenerator\CSharpGenerator\Web\ReverseEngineerCodeFirst\Mapping.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "17.0.0.0")]
    public partial class Mapping : MappingBase
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public virtual string TransformText()
        {
            
            #line 1 "C:\Users\Ahmed\Source\Repos\AppGenerator\WebApp\AppsGenerator\CSharpGenerator\Web\ReverseEngineerCodeFirst\Mapping.tt"

// Simplifying assumptions based on reverse engineer rules
//  - No complex types
//  - One entity container
//  - No inheritance
//  - Always have two navigation properties
//  - All associations expose FKs (except many:many)

            
            #line default
            #line hidden
            
            #line 14 "C:\Users\Ahmed\Source\Repos\AppGenerator\WebApp\AppsGenerator\CSharpGenerator\Web\ReverseEngineerCodeFirst\Mapping.tt"

    var code = new CodeGenerator.CodeGenerationTools(this);

	if (EntityFrameworkVersion >= new Version(4, 4))
	{

            
            #line default
            #line hidden
            this.Write("using System.ComponentModel.DataAnnotations.Schema;\r\n");
            
            #line 21 "C:\Users\Ahmed\Source\Repos\AppGenerator\WebApp\AppsGenerator\CSharpGenerator\Web\ReverseEngineerCodeFirst\Mapping.tt"

	}
	else
	{

            
            #line default
            #line hidden
            this.Write("using System.ComponentModel.DataAnnotations;\r\n");
            
            #line 27 "C:\Users\Ahmed\Source\Repos\AppGenerator\WebApp\AppsGenerator\CSharpGenerator\Web\ReverseEngineerCodeFirst\Mapping.tt"

	}

            
            #line default
            #line hidden
            this.Write("using System.Data.Entity.ModelConfiguration;\r\nusing ");
            
            #line 31 "C:\Users\Ahmed\Source\Repos\AppGenerator\WebApp\AppsGenerator\CSharpGenerator\Web\ReverseEngineerCodeFirst\Mapping.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ModelsNamespace));
            
            #line default
            #line hidden
            this.Write(";\r\n\r\nnamespace ");
            
            #line 33 "C:\Users\Ahmed\Source\Repos\AppGenerator\WebApp\AppsGenerator\CSharpGenerator\Web\ReverseEngineerCodeFirst\Mapping.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(code.EscapeNamespace(MappingNamespace)));
            
            #line default
            #line hidden
            this.Write("\r\n{\r\n    public class ");
            
            #line 35 "C:\Users\Ahmed\Source\Repos\AppGenerator\WebApp\AppsGenerator\CSharpGenerator\Web\ReverseEngineerCodeFirst\Mapping.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(entityType.Name));
            
            #line default
            #line hidden
            this.Write("Map : EntityTypeConfiguration<");
            
            #line 35 "C:\Users\Ahmed\Source\Repos\AppGenerator\WebApp\AppsGenerator\CSharpGenerator\Web\ReverseEngineerCodeFirst\Mapping.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(entityType.Name));
            
            #line default
            #line hidden
            this.Write(">\r\n    {\r\n        public ");
            
            #line 37 "C:\Users\Ahmed\Source\Repos\AppGenerator\WebApp\AppsGenerator\CSharpGenerator\Web\ReverseEngineerCodeFirst\Mapping.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(entityType.Name));
            
            #line default
            #line hidden
            this.Write("Map()\r\n        {\r\n            // Primary Key\r\n");
            
            #line 40 "C:\Users\Ahmed\Source\Repos\AppGenerator\WebApp\AppsGenerator\CSharpGenerator\Web\ReverseEngineerCodeFirst\Mapping.tt"

    if (entityType.KeyMembers.Count() == 1)
    {

            
            #line default
            #line hidden
            this.Write("            this.HasKey(t => t.");
            
            #line 44 "C:\Users\Ahmed\Source\Repos\AppGenerator\WebApp\AppsGenerator\CSharpGenerator\Web\ReverseEngineerCodeFirst\Mapping.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(entityType.KeyMembers.Single().Name));
            
            #line default
            #line hidden
            this.Write(");\r\n");
            
            #line 45 "C:\Users\Ahmed\Source\Repos\AppGenerator\WebApp\AppsGenerator\CSharpGenerator\Web\ReverseEngineerCodeFirst\Mapping.tt"

    }
    else
    {

            
            #line default
            #line hidden
            this.Write("            this.HasKey(t => new { ");
            
            #line 50 "C:\Users\Ahmed\Source\Repos\AppGenerator\WebApp\AppsGenerator\CSharpGenerator\Web\ReverseEngineerCodeFirst\Mapping.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(string.Join(", ", entityType.KeyMembers.Select(m => "t." + m.Name))));
            
            #line default
            #line hidden
            this.Write(" });\r\n");
            
            #line 51 "C:\Users\Ahmed\Source\Repos\AppGenerator\WebApp\AppsGenerator\CSharpGenerator\Web\ReverseEngineerCodeFirst\Mapping.tt"

    }

            
            #line default
            #line hidden
            this.Write("\r\n            // Properties\r\n");
            
            #line 56 "C:\Users\Ahmed\Source\Repos\AppGenerator\WebApp\AppsGenerator\CSharpGenerator\Web\ReverseEngineerCodeFirst\Mapping.tt"

    foreach (var prop in entityType.Properties)
    {
        var type = (PrimitiveType)prop.TypeUsage.EdmType;
        var isKey = entityType.KeyMembers.Contains(prop);
        var storeProp = PropertyToColumnMappings[prop];
        var sgpFacet = storeProp.TypeUsage.Facets.SingleOrDefault(f => f.Name == "StoreGeneratedPattern");
        var storeGeneratedPattern = sgpFacet == null
            ? StoreGeneratedPattern.None
            : (StoreGeneratedPattern)sgpFacet.Value;
            
        var configLines = new List<string>();
             
        if (type.ClrEquivalentType == typeof(int)
            || type.ClrEquivalentType == typeof(decimal)
            || type.ClrEquivalentType == typeof(short)
            || type.ClrEquivalentType == typeof(long))
        {
            if (isKey && storeGeneratedPattern != StoreGeneratedPattern.Identity)
            {
                configLines.Add(".HasDatabaseGeneratedOption(DatabaseGeneratedOption.None)");
            }
            else if ((!isKey || entityType.KeyMembers.Count > 1) && storeGeneratedPattern == StoreGeneratedPattern.Identity)
            {
                configLines.Add(".HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)");
            }
        }
                    
        if (type.ClrEquivalentType == typeof(string)
            || type.ClrEquivalentType == typeof(byte[]))
        {
            if (!prop.Nullable)
            {
                configLines.Add(".IsRequired()");
            }
                
            var unicodeFacet = (Facet)prop.TypeUsage.Facets.SingleOrDefault(f => f.Name == "IsUnicode");
            if(unicodeFacet != null && !(bool)unicodeFacet.Value)
            {
                configLines.Add(".IsUnicode(false)");
            }
                
            var fixedLengthFacet = (Facet)prop.TypeUsage.Facets.SingleOrDefault(f => f.Name == "FixedLength");
            if (fixedLengthFacet != null && (bool)fixedLengthFacet.Value)
            {
                configLines.Add(".IsFixedLength()");
            }
                
            var maxLengthFacet = (Facet)prop.TypeUsage.Facets.SingleOrDefault(f => f.Name == "MaxLength");
            if (maxLengthFacet != null && !maxLengthFacet.IsUnbounded)
            {
                configLines.Add(string.Format(".HasMaxLength({0})", maxLengthFacet.Value));

                if (storeGeneratedPattern == StoreGeneratedPattern.Computed
                    && type.ClrEquivalentType == typeof(byte[])
                    && (int)maxLengthFacet.Value == 8)
                {
                    configLines.Add(".IsRowVersion()");
                }
            }
        }
            
        if(configLines.Any())
        {

            
            #line default
            #line hidden
            this.Write("            this.Property(t => t.");
            
            #line 121 "C:\Users\Ahmed\Source\Repos\AppGenerator\WebApp\AppsGenerator\CSharpGenerator\Web\ReverseEngineerCodeFirst\Mapping.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(prop.Name));
            
            #line default
            #line hidden
            this.Write(")\r\n                ");
            
            #line 122 "C:\Users\Ahmed\Source\Repos\AppGenerator\WebApp\AppsGenerator\CSharpGenerator\Web\ReverseEngineerCodeFirst\Mapping.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(string.Join("\r\n                ", configLines)));
            
            #line default
            #line hidden
            this.Write(";\r\n\r\n");
            
            #line 124 "C:\Users\Ahmed\Source\Repos\AppGenerator\WebApp\AppsGenerator\CSharpGenerator\Web\ReverseEngineerCodeFirst\Mapping.tt"

        }
    }

    var tableSet = entitySet;
    var tableName = (string)tableSet.MetadataProperties["Table"].Value
        ?? tableSet.Name;
    var schemaName = (string)tableSet.MetadataProperties["Schema"].Value;

            
            #line default
            #line hidden
            this.Write("            // Table & Column Mappings\r\n");
            
            #line 134 "C:\Users\Ahmed\Source\Repos\AppGenerator\WebApp\AppsGenerator\CSharpGenerator\Web\ReverseEngineerCodeFirst\Mapping.tt"

    if (schemaName == "dbo" || string.IsNullOrWhiteSpace(schemaName))
    {

            
            #line default
            #line hidden
            this.Write("            this.ToTable(\"");
            
            #line 138 "C:\Users\Ahmed\Source\Repos\AppGenerator\WebApp\AppsGenerator\CSharpGenerator\Web\ReverseEngineerCodeFirst\Mapping.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(tableName));
            
            #line default
            #line hidden
            this.Write("\");\r\n");
            
            #line 139 "C:\Users\Ahmed\Source\Repos\AppGenerator\WebApp\AppsGenerator\CSharpGenerator\Web\ReverseEngineerCodeFirst\Mapping.tt"

    }
    else
    {

            
            #line default
            #line hidden
            this.Write("            this.ToTable(\"");
            
            #line 144 "C:\Users\Ahmed\Source\Repos\AppGenerator\WebApp\AppsGenerator\CSharpGenerator\Web\ReverseEngineerCodeFirst\Mapping.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(tableName));
            
            #line default
            #line hidden
            this.Write("\", \"");
            
            #line 144 "C:\Users\Ahmed\Source\Repos\AppGenerator\WebApp\AppsGenerator\CSharpGenerator\Web\ReverseEngineerCodeFirst\Mapping.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(schemaName));
            
            #line default
            #line hidden
            this.Write("\");\r\n");
            
            #line 145 "C:\Users\Ahmed\Source\Repos\AppGenerator\WebApp\AppsGenerator\CSharpGenerator\Web\ReverseEngineerCodeFirst\Mapping.tt"

    }

    foreach (var property in entityType.Properties)
    {

            
            #line default
            #line hidden
            this.Write("            this.Property(t => t.");
            
            #line 151 "C:\Users\Ahmed\Source\Repos\AppGenerator\WebApp\AppsGenerator\CSharpGenerator\Web\ReverseEngineerCodeFirst\Mapping.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(property.Name));
            
            #line default
            #line hidden
            this.Write(").HasColumnName(\"");
            
            #line 151 "C:\Users\Ahmed\Source\Repos\AppGenerator\WebApp\AppsGenerator\CSharpGenerator\Web\ReverseEngineerCodeFirst\Mapping.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(PropertyToColumnMappings[property].Name));
            
            #line default
            #line hidden
            this.Write("\");\r\n");
            
            #line 152 "C:\Users\Ahmed\Source\Repos\AppGenerator\WebApp\AppsGenerator\CSharpGenerator\Web\ReverseEngineerCodeFirst\Mapping.tt"

    }
        
    // Find m:m relationshipsto configure 
    var manyManyRelationships = entityType.NavigationProperties
        .Where(np => np.DeclaringType == entityType
            && np.ToEndMember.RelationshipMultiplicity == RelationshipMultiplicity.Many
            && np.FromEndMember.RelationshipMultiplicity == RelationshipMultiplicity.Many
            && np.RelationshipType.RelationshipEndMembers.First() == np.FromEndMember); // <- ensures we only configure from one end
        
    // Find FK relationships that this entity is the dependent of
    var fkRelationships = entityType.NavigationProperties
        .Where(np => np.DeclaringType == entityType
            && ((AssociationType)np.RelationshipType).IsForeignKey
            && ((AssociationType)np.RelationshipType).ReferentialConstraints.Single().ToRole == np.FromEndMember);
        
    if(manyManyRelationships.Any() || fkRelationships.Any())
    {

            
            #line default
            #line hidden
            this.Write("\r\n            // Relationships\r\n");
            
            #line 173 "C:\Users\Ahmed\Source\Repos\AppGenerator\WebApp\AppsGenerator\CSharpGenerator\Web\ReverseEngineerCodeFirst\Mapping.tt"

        foreach (var navProperty in manyManyRelationships)
        {
            var otherNavProperty = navProperty.ToEndMember.GetEntityType().NavigationProperties.Where(n => n.RelationshipType == navProperty.RelationshipType && n != navProperty).Single();
            var association = (AssociationType)navProperty.RelationshipType;
            var mapping = ManyToManyMappings[association];
            var item1 = mapping.Item1;
            var mappingTableName = (string)mapping.Item1.MetadataProperties["Table"].Value
                ?? item1.Name;
            var mappingSchemaName = (string)item1.MetadataProperties["Schema"].Value;

            // Need to ensure that FKs are decalred in the same order as the PK properties on each principal type
            var leftType = (EntityType)navProperty.DeclaringType;
            var leftKeyMappings = mapping.Item2[navProperty.FromEndMember];
            var leftColumns = string.Join(", ", leftType.KeyMembers.Select(m => "\"" + leftKeyMappings[m] + "\""));
            var rightType = (EntityType)otherNavProperty.DeclaringType;
            var rightKeyMappings = mapping.Item2[otherNavProperty.FromEndMember];
            var rightColumns = string.Join(", ", rightType.KeyMembers.Select(m => "\"" + rightKeyMappings[m] + "\""));

            
            #line default
            #line hidden
            this.Write("            this.HasMany(t => t.");
            
            #line 192 "C:\Users\Ahmed\Source\Repos\AppGenerator\WebApp\AppsGenerator\CSharpGenerator\Web\ReverseEngineerCodeFirst\Mapping.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(code.Escape(navProperty)));
            
            #line default
            #line hidden
            this.Write(")\r\n                .WithMany(t => t.");
            
            #line 193 "C:\Users\Ahmed\Source\Repos\AppGenerator\WebApp\AppsGenerator\CSharpGenerator\Web\ReverseEngineerCodeFirst\Mapping.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(code.Escape(otherNavProperty)));
            
            #line default
            #line hidden
            this.Write(")\r\n                .Map(m =>\r\n                    {\r\n");
            
            #line 196 "C:\Users\Ahmed\Source\Repos\AppGenerator\WebApp\AppsGenerator\CSharpGenerator\Web\ReverseEngineerCodeFirst\Mapping.tt"

            if (mappingSchemaName == "dbo" || string.IsNullOrWhiteSpace(mappingSchemaName))
            {

            
            #line default
            #line hidden
            this.Write("                        m.ToTable(\"");
            
            #line 200 "C:\Users\Ahmed\Source\Repos\AppGenerator\WebApp\AppsGenerator\CSharpGenerator\Web\ReverseEngineerCodeFirst\Mapping.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(mappingTableName));
            
            #line default
            #line hidden
            this.Write("\");\r\n");
            
            #line 201 "C:\Users\Ahmed\Source\Repos\AppGenerator\WebApp\AppsGenerator\CSharpGenerator\Web\ReverseEngineerCodeFirst\Mapping.tt"

            }
            else
            {

            
            #line default
            #line hidden
            this.Write("                        m.ToTable(\"");
            
            #line 206 "C:\Users\Ahmed\Source\Repos\AppGenerator\WebApp\AppsGenerator\CSharpGenerator\Web\ReverseEngineerCodeFirst\Mapping.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(mappingTableName));
            
            #line default
            #line hidden
            this.Write("\", \"");
            
            #line 206 "C:\Users\Ahmed\Source\Repos\AppGenerator\WebApp\AppsGenerator\CSharpGenerator\Web\ReverseEngineerCodeFirst\Mapping.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(mappingSchemaName));
            
            #line default
            #line hidden
            this.Write("\");\r\n");
            
            #line 207 "C:\Users\Ahmed\Source\Repos\AppGenerator\WebApp\AppsGenerator\CSharpGenerator\Web\ReverseEngineerCodeFirst\Mapping.tt"

            }

            
            #line default
            #line hidden
            this.Write("                        m.MapLeftKey(");
            
            #line 210 "C:\Users\Ahmed\Source\Repos\AppGenerator\WebApp\AppsGenerator\CSharpGenerator\Web\ReverseEngineerCodeFirst\Mapping.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(leftColumns));
            
            #line default
            #line hidden
            this.Write(");\r\n                        m.MapRightKey(");
            
            #line 211 "C:\Users\Ahmed\Source\Repos\AppGenerator\WebApp\AppsGenerator\CSharpGenerator\Web\ReverseEngineerCodeFirst\Mapping.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(rightColumns));
            
            #line default
            #line hidden
            this.Write(");\r\n                    });\r\n\r\n");
            
            #line 214 "C:\Users\Ahmed\Source\Repos\AppGenerator\WebApp\AppsGenerator\CSharpGenerator\Web\ReverseEngineerCodeFirst\Mapping.tt"

        }
            
        foreach (var navProperty in fkRelationships)
        {
            var otherNavProperty = navProperty.ToEndMember.GetEntityType().NavigationProperties.Where(n => n.RelationshipType == navProperty.RelationshipType && n != navProperty).Single();
            var association = (AssociationType)navProperty.RelationshipType;
                
            if (navProperty.ToEndMember.RelationshipMultiplicity == RelationshipMultiplicity.One)
            {

            
            #line default
            #line hidden
            this.Write("            this.HasRequired(t => t.");
            
            #line 225 "C:\Users\Ahmed\Source\Repos\AppGenerator\WebApp\AppsGenerator\CSharpGenerator\Web\ReverseEngineerCodeFirst\Mapping.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(code.Escape(navProperty)));
            
            #line default
            #line hidden
            this.Write(")\r\n");
            
            #line 226 "C:\Users\Ahmed\Source\Repos\AppGenerator\WebApp\AppsGenerator\CSharpGenerator\Web\ReverseEngineerCodeFirst\Mapping.tt"

            }
            else
            {

            
            #line default
            #line hidden
            this.Write("            this.HasOptional(t => t.");
            
            #line 231 "C:\Users\Ahmed\Source\Repos\AppGenerator\WebApp\AppsGenerator\CSharpGenerator\Web\ReverseEngineerCodeFirst\Mapping.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(code.Escape(navProperty)));
            
            #line default
            #line hidden
            this.Write(")\r\n");
            
            #line 232 "C:\Users\Ahmed\Source\Repos\AppGenerator\WebApp\AppsGenerator\CSharpGenerator\Web\ReverseEngineerCodeFirst\Mapping.tt"

            }
                
            if(navProperty.FromEndMember.RelationshipMultiplicity == RelationshipMultiplicity.Many)
            {

            
            #line default
            #line hidden
            this.Write("                .WithMany(t => t.");
            
            #line 238 "C:\Users\Ahmed\Source\Repos\AppGenerator\WebApp\AppsGenerator\CSharpGenerator\Web\ReverseEngineerCodeFirst\Mapping.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(code.Escape(otherNavProperty)));
            
            #line default
            #line hidden
            this.Write(")\r\n");
            
            #line 239 "C:\Users\Ahmed\Source\Repos\AppGenerator\WebApp\AppsGenerator\CSharpGenerator\Web\ReverseEngineerCodeFirst\Mapping.tt"

                if(association.ReferentialConstraints.Single().ToProperties.Count == 1)
                {

            
            #line default
            #line hidden
            this.Write("                .HasForeignKey(d => d.");
            
            #line 243 "C:\Users\Ahmed\Source\Repos\AppGenerator\WebApp\AppsGenerator\CSharpGenerator\Web\ReverseEngineerCodeFirst\Mapping.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(association.ReferentialConstraints.Single().ToProperties.Single().Name));
            
            #line default
            #line hidden
            this.Write(");\r\n");
            
            #line 244 "C:\Users\Ahmed\Source\Repos\AppGenerator\WebApp\AppsGenerator\CSharpGenerator\Web\ReverseEngineerCodeFirst\Mapping.tt"

                }
                else
                {

            
            #line default
            #line hidden
            this.Write("                .HasForeignKey(d => new { ");
            
            #line 249 "C:\Users\Ahmed\Source\Repos\AppGenerator\WebApp\AppsGenerator\CSharpGenerator\Web\ReverseEngineerCodeFirst\Mapping.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(string.Join(", ", association.ReferentialConstraints.Single().ToProperties.Select(p => "d." + p.Name))));
            
            #line default
            #line hidden
            this.Write(" });\r\n");
            
            #line 250 "C:\Users\Ahmed\Source\Repos\AppGenerator\WebApp\AppsGenerator\CSharpGenerator\Web\ReverseEngineerCodeFirst\Mapping.tt"

                }
            }
            else
            {
                // NOTE: We can assume that this is a required:optional relationship 
                //       as EDMGen will never create an optional:optional relationship
                // 		 because everything is one:many except PK-PK relationships which must be required

            
            #line default
            #line hidden
            this.Write("                .WithOptional(t => t.");
            
            #line 259 "C:\Users\Ahmed\Source\Repos\AppGenerator\WebApp\AppsGenerator\CSharpGenerator\Web\ReverseEngineerCodeFirst\Mapping.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(code.Escape(otherNavProperty)));
            
            #line default
            #line hidden
            this.Write(");\r\n");
            
            #line 260 "C:\Users\Ahmed\Source\Repos\AppGenerator\WebApp\AppsGenerator\CSharpGenerator\Web\ReverseEngineerCodeFirst\Mapping.tt"
	
                }
            }

            
            #line default
            #line hidden
            this.Write("\r\n");
            
            #line 265 "C:\Users\Ahmed\Source\Repos\AppGenerator\WebApp\AppsGenerator\CSharpGenerator\Web\ReverseEngineerCodeFirst\Mapping.tt"

        }

            
            #line default
            #line hidden
            this.Write("        }\r\n    }\r\n}\r\n\r\n\r\n\r\n");
            return this.GenerationEnvironment.ToString();
        }
        private global::Microsoft.VisualStudio.TextTemplating.ITextTemplatingEngineHost hostValue;
        /// <summary>
        /// The current host for the text templating engine
        /// </summary>
        public virtual global::Microsoft.VisualStudio.TextTemplating.ITextTemplatingEngineHost Host
        {
            get
            {
                return this.hostValue;
            }
            set
            {
                this.hostValue = value;
            }
        }
        
        #line 274 "C:\Users\Ahmed\Source\Repos\AppGenerator\WebApp\AppsGenerator\CSharpGenerator\Web\ReverseEngineerCodeFirst\Mapping.tt"

    public EntityType entityType{set;get;}
    public EntitySet entitySet{set;get;}
    public Version EntityFrameworkVersion{set;get;}
    public string MappingNamespace{set;get;}
    public string ModelsNamespace{set;get;}

    public Dictionary<EdmProperty, EdmProperty> PropertyToColumnMappings { get; set; }
    public Dictionary<AssociationType, Tuple<EntitySet, Dictionary<RelationshipEndMember, Dictionary<EdmMember, string>>>> ManyToManyMappings { get; set; }
 
        
        #line default
        #line hidden
    }
    
    #line default
    #line hidden
    #region Base class
    /// <summary>
    /// Base class for this transformation
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "17.0.0.0")]
    public class MappingBase
    {
        #region Fields
        private global::System.Text.StringBuilder generationEnvironmentField;
        private global::System.CodeDom.Compiler.CompilerErrorCollection errorsField;
        private global::System.Collections.Generic.List<int> indentLengthsField;
        private string currentIndentField = "";
        private bool endsWithNewline;
        private global::System.Collections.Generic.IDictionary<string, object> sessionField;
        #endregion
        #region Properties
        /// <summary>
        /// The string builder that generation-time code is using to assemble generated output
        /// </summary>
        protected System.Text.StringBuilder GenerationEnvironment
        {
            get
            {
                if ((this.generationEnvironmentField == null))
                {
                    this.generationEnvironmentField = new global::System.Text.StringBuilder();
                }
                return this.generationEnvironmentField;
            }
            set
            {
                this.generationEnvironmentField = value;
            }
        }
        /// <summary>
        /// The error collection for the generation process
        /// </summary>
        public System.CodeDom.Compiler.CompilerErrorCollection Errors
        {
            get
            {
                if ((this.errorsField == null))
                {
                    this.errorsField = new global::System.CodeDom.Compiler.CompilerErrorCollection();
                }
                return this.errorsField;
            }
        }
        /// <summary>
        /// A list of the lengths of each indent that was added with PushIndent
        /// </summary>
        private System.Collections.Generic.List<int> indentLengths
        {
            get
            {
                if ((this.indentLengthsField == null))
                {
                    this.indentLengthsField = new global::System.Collections.Generic.List<int>();
                }
                return this.indentLengthsField;
            }
        }
        /// <summary>
        /// Gets the current indent we use when adding lines to the output
        /// </summary>
        public string CurrentIndent
        {
            get
            {
                return this.currentIndentField;
            }
        }
        /// <summary>
        /// Current transformation session
        /// </summary>
        public virtual global::System.Collections.Generic.IDictionary<string, object> Session
        {
            get
            {
                return this.sessionField;
            }
            set
            {
                this.sessionField = value;
            }
        }
        #endregion
        #region Transform-time helpers
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void Write(string textToAppend)
        {
            if (string.IsNullOrEmpty(textToAppend))
            {
                return;
            }
            // If we're starting off, or if the previous text ended with a newline,
            // we have to append the current indent first.
            if (((this.GenerationEnvironment.Length == 0) 
                        || this.endsWithNewline))
            {
                this.GenerationEnvironment.Append(this.currentIndentField);
                this.endsWithNewline = false;
            }
            // Check if the current text ends with a newline
            if (textToAppend.EndsWith(global::System.Environment.NewLine, global::System.StringComparison.CurrentCulture))
            {
                this.endsWithNewline = true;
            }
            // This is an optimization. If the current indent is "", then we don't have to do any
            // of the more complex stuff further down.
            if ((this.currentIndentField.Length == 0))
            {
                this.GenerationEnvironment.Append(textToAppend);
                return;
            }
            // Everywhere there is a newline in the text, add an indent after it
            textToAppend = textToAppend.Replace(global::System.Environment.NewLine, (global::System.Environment.NewLine + this.currentIndentField));
            // If the text ends with a newline, then we should strip off the indent added at the very end
            // because the appropriate indent will be added when the next time Write() is called
            if (this.endsWithNewline)
            {
                this.GenerationEnvironment.Append(textToAppend, 0, (textToAppend.Length - this.currentIndentField.Length));
            }
            else
            {
                this.GenerationEnvironment.Append(textToAppend);
            }
        }
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void WriteLine(string textToAppend)
        {
            this.Write(textToAppend);
            this.GenerationEnvironment.AppendLine();
            this.endsWithNewline = true;
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void Write(string format, params object[] args)
        {
            this.Write(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void WriteLine(string format, params object[] args)
        {
            this.WriteLine(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Raise an error
        /// </summary>
        public void Error(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Raise a warning
        /// </summary>
        public void Warning(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            error.IsWarning = true;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Increase the indent
        /// </summary>
        public void PushIndent(string indent)
        {
            if ((indent == null))
            {
                throw new global::System.ArgumentNullException("indent");
            }
            this.currentIndentField = (this.currentIndentField + indent);
            this.indentLengths.Add(indent.Length);
        }
        /// <summary>
        /// Remove the last indent that was added with PushIndent
        /// </summary>
        public string PopIndent()
        {
            string returnValue = "";
            if ((this.indentLengths.Count > 0))
            {
                int indentLength = this.indentLengths[(this.indentLengths.Count - 1)];
                this.indentLengths.RemoveAt((this.indentLengths.Count - 1));
                if ((indentLength > 0))
                {
                    returnValue = this.currentIndentField.Substring((this.currentIndentField.Length - indentLength));
                    this.currentIndentField = this.currentIndentField.Remove((this.currentIndentField.Length - indentLength));
                }
            }
            return returnValue;
        }
        /// <summary>
        /// Remove any indentation
        /// </summary>
        public void ClearIndent()
        {
            this.indentLengths.Clear();
            this.currentIndentField = "";
        }
        #endregion
        #region ToString Helpers
        /// <summary>
        /// Utility class to produce culture-oriented representation of an object as a string.
        /// </summary>
        public class ToStringInstanceHelper
        {
            private System.IFormatProvider formatProviderField  = global::System.Globalization.CultureInfo.InvariantCulture;
            /// <summary>
            /// Gets or sets format provider to be used by ToStringWithCulture method.
            /// </summary>
            public System.IFormatProvider FormatProvider
            {
                get
                {
                    return this.formatProviderField ;
                }
                set
                {
                    if ((value != null))
                    {
                        this.formatProviderField  = value;
                    }
                }
            }
            /// <summary>
            /// This is called from the compile/run appdomain to convert objects within an expression block to a string
            /// </summary>
            public string ToStringWithCulture(object objectToConvert)
            {
                if ((objectToConvert == null))
                {
                    throw new global::System.ArgumentNullException("objectToConvert");
                }
                System.Type t = objectToConvert.GetType();
                System.Reflection.MethodInfo method = t.GetMethod("ToString", new System.Type[] {
                            typeof(System.IFormatProvider)});
                if ((method == null))
                {
                    return objectToConvert.ToString();
                }
                else
                {
                    return ((string)(method.Invoke(objectToConvert, new object[] {
                                this.formatProviderField })));
                }
            }
        }
        private ToStringInstanceHelper toStringHelperField = new ToStringInstanceHelper();
        /// <summary>
        /// Helper to produce culture-oriented representation of an object as a string
        /// </summary>
        public ToStringInstanceHelper ToStringHelper
        {
            get
            {
                return this.toStringHelperField;
            }
        }
        #endregion
    }
    #endregion
}
