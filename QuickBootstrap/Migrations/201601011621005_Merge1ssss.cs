namespace QuickBootstrap.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Merge1ssss : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SalesDatas", "GenerationTime", c => c.DateTime());
            DropColumn("dbo.SalesDatas", "Yyyymmdd");
            DropColumn("dbo.SalesDatas", "Hhmiss");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SalesDatas", "Hhmiss", c => c.String(nullable: false, maxLength: 32));
            AddColumn("dbo.SalesDatas", "Yyyymmdd", c => c.String());
            DropColumn("dbo.SalesDatas", "GenerationTime");
        }
    }
}
