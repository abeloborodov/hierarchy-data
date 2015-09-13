namespace HierarchyId.API.BreadthFirst.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AttributeMetadatas",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        BlockId = c.Guid(nullable: false),
                        AttributeName = c.String(),
                        Placeholder = c.String(),
                        Order = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Blocks", t => t.BlockId, cascadeDelete: true)
                .Index(t => t.BlockId);
            
            CreateTable(
                "dbo.Blocks",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        BlockName = c.String(nullable: false),
                        Path = c.HierarchyId(nullable: false),
                        IsChoice = c.Boolean(nullable: false),
                        Order = c.Int(nullable: false),
                        Level = c.Short(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
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
            DropForeignKey("dbo.AttributeMetadatas", "BlockId", "dbo.Blocks");
            DropIndex("dbo.SubserviceVersions", new[] { "SubserviceId" });
            DropIndex("dbo.AttributeMetadatas", new[] { "BlockId" });
            DropTable("dbo.SubserviceVersions");
            DropTable("dbo.Subservices");
            DropTable("dbo.Blocks");
            DropTable("dbo.AttributeMetadatas");
        }
    }
}
