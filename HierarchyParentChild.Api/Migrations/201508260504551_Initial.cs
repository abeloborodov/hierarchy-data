using System.Data.Entity.Migrations;

namespace HierarchyParentChild.Api.Migrations
{
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AttributeMetadatas",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                        Placeholder = c.String(),
                        ElementId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Elements", t => t.ElementId, cascadeDelete: true)
                .Index(t => t.ElementId);
            
            CreateTable(
                "dbo.Elements",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        EnglishName = c.String(nullable: false, maxLength: 100, fixedLength: true),
                        RussianName = c.String(nullable: false, maxLength: 100, fixedLength: true),
                        IsChoice = c.Boolean(nullable: false),
                        Order = c.Int(nullable: false),
                        SubserviceVersionId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SubserviceVersions", t => t.SubserviceVersionId)
                .Index(t => t.SubserviceVersionId);
            
            CreateTable(
                "dbo.TreeElements",
                c => new
                    {
                        ParentId = c.Guid(nullable: false),
                        ChildId = c.Guid(nullable: false),
                        Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.ParentId, t.ChildId })
                .ForeignKey("dbo.Elements", t => t.ParentId)
                .ForeignKey("dbo.Elements", t => t.ChildId)
                .Index(t => t.ParentId)
                .Index(t => t.ChildId);
            
            CreateTable(
                "dbo.SubserviceVersions",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Version = c.String(nullable: false, maxLength: 5),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IncidentDatas",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        IncidentId = c.Guid(nullable: false),
                        Value = c.String(),
                        AttributeMetadateId = c.Guid(nullable: false),
                        SimpleAttributeId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Incident", t => t.IncidentId)
                .ForeignKey("dbo.SimpleAttributes", t => t.SimpleAttributeId)
                .ForeignKey("dbo.AttributeMetadatas", t => t.AttributeMetadateId)
                .Index(t => t.IncidentId)
                .Index(t => t.AttributeMetadateId)
                .Index(t => t.SimpleAttributeId);
            
            CreateTable(
                "dbo.Incident",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        TicketNumber = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SimpleAttributes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Restriction = c.String(nullable: false, maxLength: 10, fixedLength: true),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.IncidentDatas", "AttributeMetadateId", "dbo.AttributeMetadatas");
            DropForeignKey("dbo.IncidentDatas", "SimpleAttributeId", "dbo.SimpleAttributes");
            DropForeignKey("dbo.IncidentDatas", "IncidentId", "dbo.Incident");
            DropForeignKey("dbo.Elements", "SubserviceVersionId", "dbo.SubserviceVersions");
            DropForeignKey("dbo.TreeElements", "ChildId", "dbo.Elements");
            DropForeignKey("dbo.TreeElements", "ParentId", "dbo.Elements");
            DropForeignKey("dbo.AttributeMetadatas", "ElementId", "dbo.Elements");
            DropIndex("dbo.IncidentDatas", new[] { "SimpleAttributeId" });
            DropIndex("dbo.IncidentDatas", new[] { "AttributeMetadateId" });
            DropIndex("dbo.IncidentDatas", new[] { "IncidentId" });
            DropIndex("dbo.TreeElements", new[] { "ChildId" });
            DropIndex("dbo.TreeElements", new[] { "ParentId" });
            DropIndex("dbo.Elements", new[] { "SubserviceVersionId" });
            DropIndex("dbo.AttributeMetadatas", new[] { "ElementId" });
            DropTable("dbo.SimpleAttributes");
            DropTable("dbo.Incident");
            DropTable("dbo.IncidentDatas");
            DropTable("dbo.SubserviceVersions");
            DropTable("dbo.TreeElements");
            DropTable("dbo.Elements");
            DropTable("dbo.AttributeMetadatas");
        }
    }
}
