using System.Data.Entity.Migrations;

namespace HierarchyParentChild.Api.Migrations
{
    public partial class subservice : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AttributeMetadatas", "ElementId", "dbo.Elements");
            DropForeignKey("dbo.TreeElements", "ParentId", "dbo.Elements");
            DropForeignKey("dbo.Elements", "SubserviceVersionId", "dbo.SubserviceVersions");
            CreateTable(
                "dbo.Subservices",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Namespace = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.SubserviceVersions", "SubserviceId", c => c.Guid(nullable: false));
            CreateIndex("dbo.SubserviceVersions", "SubserviceId");
            AddForeignKey("dbo.SubserviceVersions", "SubserviceId", "dbo.Subservices", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AttributeMetadatas", "ElementId", "dbo.Elements", "Id");
            AddForeignKey("dbo.TreeElements", "ParentId", "dbo.Elements", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Elements", "SubserviceVersionId", "dbo.SubserviceVersions", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Elements", "SubserviceVersionId", "dbo.SubserviceVersions");
            DropForeignKey("dbo.TreeElements", "ParentId", "dbo.Elements");
            DropForeignKey("dbo.AttributeMetadatas", "ElementId", "dbo.Elements");
            DropForeignKey("dbo.SubserviceVersions", "SubserviceId", "dbo.Subservices");
            DropIndex("dbo.SubserviceVersions", new[] { "SubserviceId" });
            DropColumn("dbo.SubserviceVersions", "SubserviceId");
            DropTable("dbo.Subservices");
            AddForeignKey("dbo.Elements", "SubserviceVersionId", "dbo.SubserviceVersions", "Id");
            AddForeignKey("dbo.TreeElements", "ParentId", "dbo.Elements", "Id");
            AddForeignKey("dbo.AttributeMetadatas", "ElementId", "dbo.Elements", "Id", cascadeDelete: true);
        }
    }
}
