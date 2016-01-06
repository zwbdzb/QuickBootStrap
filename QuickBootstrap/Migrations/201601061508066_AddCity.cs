namespace QuickBootstrap.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SalesDatas", "Affiliate_id", c => c.String(maxLength: 30));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SalesDatas", "Affiliate_id");
        }
    }
}
