namespace CodeFirstNewDatabaseSample.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HomeLands : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Organizations", "HomeLand_CountryId", "dbo.Countries");
            DropIndex("dbo.Organizations", new[] { "HomeLand_CountryId" });
            CreateTable(
                "dbo.OrganizationCountries",
                c => new
                    {
                        Organization_OrganizationName = c.String(nullable: false, maxLength: 128),
                        Country_CountryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Organization_OrganizationName, t.Country_CountryId })
                .ForeignKey("dbo.Organizations", t => t.Organization_OrganizationName, cascadeDelete: true)
                .ForeignKey("dbo.Countries", t => t.Country_CountryId, cascadeDelete: true)
                .Index(t => t.Organization_OrganizationName)
                .Index(t => t.Country_CountryId);
            
            DropColumn("dbo.Organizations", "HomeLand_CountryId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Organizations", "HomeLand_CountryId", c => c.Int());
            DropForeignKey("dbo.OrganizationCountries", "Country_CountryId", "dbo.Countries");
            DropForeignKey("dbo.OrganizationCountries", "Organization_OrganizationName", "dbo.Organizations");
            DropIndex("dbo.OrganizationCountries", new[] { "Country_CountryId" });
            DropIndex("dbo.OrganizationCountries", new[] { "Organization_OrganizationName" });
            DropTable("dbo.OrganizationCountries");
            CreateIndex("dbo.Organizations", "HomeLand_CountryId");
            AddForeignKey("dbo.Organizations", "HomeLand_CountryId", "dbo.Countries", "CountryId");
        }
    }
}
