using System.Data.Entity.Migrations;

namespace HierarchyParentChild.Api.Migrations
{
    public partial class order : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AttributeMetadatas", "Order", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AttributeMetadatas", "Order");
        }
    }
}
