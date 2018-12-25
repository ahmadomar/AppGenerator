using Microsoft.AspNet.Scaffolding.Core.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppsGenerator.CSharpGenerator.Web.MvcControllerWithContext
{
    public class Testing
    {
        // These functionalities needs to move to core.
        private ModelMetadata modelMetadata { set; get; }
        public Testing(ModelMetadata modelMetadata)
        {
            this.modelMetadata = modelMetadata;
        }
        public RelatedModelMetadata GetRelatedModelMetadata(PropertyMetadata property)
        {
            RelatedModelMetadata propertyModel;
            IDictionary<string, RelatedModelMetadata> relatedProperties;
            if (modelMetadata.RelatedEntities != null)
            {
                relatedProperties = modelMetadata.RelatedEntities.ToDictionary(item => item.AssociationPropertyName);
            }
            else
            {
                relatedProperties = new Dictionary<string, RelatedModelMetadata>();
            }
            relatedProperties.TryGetValue(property.PropertyName, out propertyModel);

            return propertyModel;
        }

        // A foreign key, e.g. CategoryID, will have an association name of Category
        public string GetAssociationName(PropertyMetadata property)
        {
            RelatedModelMetadata propertyModel = GetRelatedModelMetadata(property);
            return propertyModel != null ? propertyModel.AssociationPropertyName : property.PropertyName;
        }

        // A foreign key, e.g. CategoryID, will have a value expression of Category.CategoryID
        public string GetValueExpression(PropertyMetadata property)
        {
            RelatedModelMetadata propertyModel = GetRelatedModelMetadata(property);
            return propertyModel != null ? propertyModel.AssociationPropertyName + "." + propertyModel.DisplayPropertyName : property.PropertyName;
        }

        // This will return the primary key property name, if and only if there is exactly
        // one primary key. Returns null if there is no PK, or the PK is composite.
        public string GetPrimaryKeyName()
        {
            return (modelMetadata.PrimaryKeys != null && modelMetadata.PrimaryKeys.Count() == 1) ? modelMetadata.PrimaryKeys[0].PropertyName : null;
        }

        public bool IsPropertyGuid(PropertyMetadata property)
        {
            return String.Equals("System.Guid", property.TypeName, StringComparison.OrdinalIgnoreCase);
        }
    }
}