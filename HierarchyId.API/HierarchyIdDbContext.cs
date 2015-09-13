using System.Data.Entity;

namespace HierarchyId.API
{


    public class HierarchyIdDbContext : DbContext
    {
        public HierarchyIdDbContext()
            : base("HierarchyIdDbConnection")
        {
        }

        public virtual DbSet<Block> Blocks { get; set; }

        public virtual DbSet<Subservice> Subservices { get; set; }

        public virtual DbSet<SubserviceVersion> SubserviceVersions { get; set; }


        public virtual DbSet<AttributeMetadata> AttributeMetadatas { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

        }
    }
}
