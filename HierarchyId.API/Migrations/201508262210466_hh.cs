namespace HierarchyId.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class hh : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Blocks", "BlockName", c => c.String(nullable: false));
            AlterColumn("dbo.Blocks", "Path", c => c.HierarchyId(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Blocks", "Path", c => c.HierarchyId());
            AlterColumn("dbo.Blocks", "BlockName", c => c.String());
        }
    }
}
