using System.Data.Entity.Migrations;

namespace HierarchyParentChild.Api.Migrations
{
    public partial class _true : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AttributeMetadatas", "ElementId", "dbo.Elements");
            DropForeignKey("dbo.TreeElements", "ParentId", "dbo.Elements");
            AddForeignKey("dbo.AttributeMetadatas", "ElementId", "dbo.Elements", "Id", cascadeDelete: true);
            AddForeignKey("dbo.TreeElements", "ParentId", "dbo.Elements", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TreeElements", "ParentId", "dbo.Elements");
            DropForeignKey("dbo.AttributeMetadatas", "ElementId", "dbo.Elements");
            AddForeignKey("dbo.TreeElements", "ParentId", "dbo.Elements", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AttributeMetadatas", "ElementId", "dbo.Elements", "Id");
        }
    }
}
