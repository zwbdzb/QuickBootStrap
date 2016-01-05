namespace QuickBootstrap.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBlogUrl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SalesDatas", "Sales", c => c.String());
            AddColumn("dbo.SalesDatas", "Commission", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.SalesDatas", "Stat_code", c => c.Int());
            AddColumn("dbo.SalesDatas", "Stat_desc", c => c.String());
            AddColumn("dbo.SalesDatas", "Cancel_comment", c => c.String());
            AddColumn("dbo.SalesDatas", "Bill_yyyymmdd", c => c.DateTime());
            DropColumn("dbo.SalesDatas", "Stat");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SalesDatas", "Stat", c => c.Int());
            DropColumn("dbo.SalesDatas", "Bill_yyyymmdd");
            DropColumn("dbo.SalesDatas", "Cancel_comment");
            DropColumn("dbo.SalesDatas", "Stat_desc");
            DropColumn("dbo.SalesDatas", "Stat_code");
            DropColumn("dbo.SalesDatas", "Commission");
            DropColumn("dbo.SalesDatas", "Sales");
        }
    }
}
