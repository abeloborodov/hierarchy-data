using System.Data.Entity;

namespace HierarchyParentChild.Api.EF
{
    public partial class HierarchyParentChildContext : DbContext
    {
        public HierarchyParentChildContext()
            : base("Smev_ClientTest")
        {
        }

        public virtual DbSet<Element> Elements { get; set; }
        public virtual DbSet<TreeElement> TreeElements { get; set; }
        
        public virtual DbSet<AttributeMetadata> AttributeMetadatas { get; set; }
        
        public virtual DbSet<Incident> Incidents { get; set; }
        public virtual DbSet<IncidentData> IncidentDatas { get; set; }
        public virtual DbSet<SimpleAttribute> SimpleAttributes { get; set; }
        public virtual DbSet<SubserviceVersion> SubserviceVersions { get; set; }
        public virtual DbSet<Subservice> Subservices { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Element>()
                .Property(e => e.EnglishName)
                .IsFixedLength();

            modelBuilder.Entity<Element>()
                .Property(e => e.RussianName)
                .IsFixedLength();


            

            modelBuilder.Entity<Element>()
                .HasMany(p => p.AttributeMetadatas)
                .WithRequired(d => d.Element)
                .HasForeignKey(d => d.ElementId)
                .WillCascadeOnDelete(true);//false
            //has many ancestors
            modelBuilder.Entity<Element>()
                .HasMany(p => p.Parents)
                .WithRequired(d => d.Child)
                .HasForeignKey(d => d.ChildId)
                .WillCascadeOnDelete(false);//false

            // has many offspring
            modelBuilder.Entity<Element>()
                            .HasMany(p => p.Children)
                            .WithRequired(d => d.Parent)
                            .HasForeignKey(d => d.ParentId)
                            .WillCascadeOnDelete(false);//false

            modelBuilder.Entity<TreeElement>()
                        .HasKey(p => new
                        {
                            p.ParentId,
                            p.ChildId
                        });




            modelBuilder.Entity<AttributeMetadata>()
                .HasMany(e => e.IncidentDatas)
                .WithRequired(e => e.AttributeMetadata)
                .HasForeignKey(e => e.AttributeMetadateId)
                .WillCascadeOnDelete(false);


            

            modelBuilder.Entity<Incident>()
                .HasMany(e => e.IncidentDatas)
                .WithRequired(e => e.Incident)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SimpleAttribute>()
                .Property(e => e.Restriction)
                .IsFixedLength();

            modelBuilder.Entity<Subservice>()
                .HasMany(e => e.SubserviceVersions)
                .WithRequired(e => e.Subservice)
                .WillCascadeOnDelete(true);
            modelBuilder.Entity<SubserviceVersion>()
                .HasMany(e => e.Elements)
                .WithRequired(e => e.SubserviceVersion)
                .WillCascadeOnDelete(true);
        }
    }
}
