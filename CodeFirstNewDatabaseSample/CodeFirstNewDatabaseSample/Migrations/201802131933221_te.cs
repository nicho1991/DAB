namespace CodeFirstNewDatabaseSample.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class te : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Users", "Organization_OrganizationId", "dbo.Organizations");
            DropIndex("dbo.Users", new[] { "Organization_OrganizationId" });
            RenameColumn(table: "dbo.Users", name: "Organization_OrganizationId", newName: "Organization_OrganizationName");
            DropPrimaryKey("dbo.Organizations");
            AlterColumn("dbo.Organizations", "OrganizationName", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Users", "Organization_OrganizationName", c => c.String(maxLength: 128));
            AddPrimaryKey("dbo.Organizations", "OrganizationName");
            CreateIndex("dbo.Users", "Organization_OrganizationName");
            AddForeignKey("dbo.Users", "Organization_OrganizationName", "dbo.Organizations", "OrganizationName");
            DropColumn("dbo.Organizations", "OrganizationId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Organizations", "OrganizationId", c => c.Int(nullable: false, identity: true));
            DropForeignKey("dbo.Users", "Organization_OrganizationName", "dbo.Organizations");
            DropIndex("dbo.Users", new[] { "Organization_OrganizationName" });
            DropPrimaryKey("dbo.Organizations");
            AlterColumn("dbo.Users", "Organization_OrganizationName", c => c.Int());
            AlterColumn("dbo.Organizations", "OrganizationName", c => c.String());
            AddPrimaryKey("dbo.Organizations", "OrganizationId");
            RenameColumn(table: "dbo.Users", name: "Organization_OrganizationName", newName: "Organization_OrganizationId");
            CreateIndex("dbo.Users", "Organization_OrganizationId");
            AddForeignKey("dbo.Users", "Organization_OrganizationId", "dbo.Organizations", "OrganizationId");
        }
    }
}
