namespace HierarchyId.API.BreadthFirst.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompositeIndex : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Blocks", new[] { "Level", "Path" }, unique: true, name: "IX_BreadthFirst");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Blocks", "IX_BreadthFirst");
        }
    }
}
