namespace HierarchyId.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Subservice : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Subservices",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Namespace = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SubserviceVersions",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        SubserviceId = c.Guid(nullable: false),
                        Version = c.String(nullable: false, maxLength: 5),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Subservices", t => t.SubserviceId, cascadeDelete: true)
                .Index(t => t.SubserviceId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SubserviceVersions", "SubserviceId", "dbo.Subservices");
            DropIndex("dbo.SubserviceVersions", new[] { "SubserviceId" });
            DropTable("dbo.SubserviceVersions");
            DropTable("dbo.Subservices");
        }
    }
}
