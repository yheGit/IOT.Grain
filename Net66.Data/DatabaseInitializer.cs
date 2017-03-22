using Net66.Data.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Text;

namespace Net66.Data
{
    public class DatabaseInitializer
    {
        public static void Initialize()
        {
            using (var db = new GrainContext())
            {
                var objectContext = ((IObjectContextAdapter)db).ObjectContext;
                var mappingItemCollection = (StorageMappingItemCollection)objectContext.ObjectStateManager.MetadataWorkspace.GetItemCollection(DataSpace.CSSpace);
                mappingItemCollection.GenerateViews(new List<EdmSchemaError>());
            }
        }
    }
}
