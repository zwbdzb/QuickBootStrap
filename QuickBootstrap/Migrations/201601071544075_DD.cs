namespace QuickBootstrap.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DD : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SalesDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Yyyymmdd = c.String(maxLength: 30),
                        Hhmiss = c.String(maxLength: 30),
                        GenerationTime = c.DateTime(),
                        O_cd = c.String(nullable: false, maxLength: 30),
                        M_id = c.String(maxLength: 30),
                        Mbr_id = c.String(maxLength: 30),
                        Comm = c.Decimal(nullable: false, precision: 18, scale: 4),
                        U_id = c.String(maxLength: 30),
                        P_cd = c.String(nullable: false, maxLength: 30),
                        It_cnt = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, storeType: "money"),
                        C_cd = c.String(maxLength: 30),
                        AddTime = c.DateTime(),
                        Sales = c.Decimal(nullable: false, storeType: "money"),
                        Commission = c.Decimal(nullable: false, storeType: "money"),
                        Stat_code = c.Int(),
                        Stat_desc = c.String(maxLength: 50),
                        Affiliate_id = c.String(maxLength: 30),
                        Cancel_comment = c.String(),
                        Bill_yyyymmdd = c.String(),
                        UpdateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserName = c.String(nullable: false, maxLength: 50),
                        UserPwd = c.String(nullable: false, maxLength: 32),
                        Nick = c.String(nullable: false, maxLength: 50),
                        IsEnable = c.Boolean(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserName);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Users");
            DropTable("dbo.SalesDatas");
        }
    }
}
