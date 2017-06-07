using System;
using System.Linq;
using NewsMonitoringSystem.Data.Entities.Generated;

namespace NewsMonitoringSystem.Data
{
    using System.Data.Entity;

    public class NewsContext : DbContext
    {
        public NewsContext()
            : base("name=NewsContext")
        {
        }

        public virtual DbSet<Document> Documents { get; set; }
        //public virtual DbSet<DocumentCategory> DocumentCategories { get; set; }
        //public virtual DbSet<DocumentDestination> DocumentDestinations { get; set; }

        #region Overrided methods

        public override int SaveChanges()
        {
            var entities = ChangeTracker.Entries()
                                        .Where(e => e.State == EntityState.Modified
                                                    || e.State == EntityState.Added);
            
            foreach (var dbEntityEntry in entities)
            {
                var entity = dbEntityEntry.Entity as EntityBase;
                if (entity != null)
                {
                    SetAuditProperties(entity, 
                                       dbEntityEntry.State);
                }
            }

            return base.SaveChanges();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Set audit properties for entity,
        /// if new entity - "Inserted" datetime and "InsertedBy"
        /// if existing entity - "LastModified" datetime and "ModifiedBy"
        /// </summary>
        public void SetAuditProperties(EntityBase entity, 
                                       EntityState state)
        {
            switch (state)
            {
                case EntityState.Added:
                    entity.Inserted = DateTime.Now;
                    entity.InsertedBy = null; //ToDo: Get current db user 
                    goto case EntityState.Modified;
                case EntityState.Modified:
                    entity.LastModified = DateTime.Now;
                    entity.ModifiedBy = null; //ToDo: Get current db user 
                    break;
            }
        }

        #endregion
    }

}