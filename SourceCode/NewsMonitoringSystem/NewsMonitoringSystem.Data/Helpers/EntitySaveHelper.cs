using System.Collections.Generic;
using NewsMonitoringSystem.Data.Entities.Generated;

namespace NewsMonitoringSystem.Data.Helpers
{
    public class EntitySaveHelper
    {
        public static int CountItemsToSavePerOneContext { get; set; } = 100;

        private static int countSavedItems = 0;

        public static void SaveChanges(List<Document> itemsToSave)
        {
            var dbContext = new NewsContext();
            dbContext.Configuration.AutoDetectChangesEnabled = false;
            countSavedItems = 0;

            foreach (var item in itemsToSave)
            {
                SaveItem(dbContext, item);
            }
        }

        private static void SaveItem(NewsContext dbContext, Document item)
        {
            dbContext.Documents.Add(item);

            countSavedItems++;
            var recreateContext = countSavedItems == CountItemsToSavePerOneContext;

            if (recreateContext)
            {
                dbContext.SaveChanges();
                dbContext = new NewsContext();
                dbContext.Configuration.AutoDetectChangesEnabled = false;
            }
        }
    }
}
