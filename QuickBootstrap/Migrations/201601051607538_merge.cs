namespace QuickBootstrap.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class merge : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SalesDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GenerationTime = c.DateTime(),
                        O_cd = c.String(nullable: false),
                        M_id = c.String(),
                        Mbr_id = c.String(),
                        Comm = c.Decimal(nullable: false, precision: 18, scale: 2),
                        U_id = c.String(),
                        P_cd = c.String(nullable: false),
                        It_cnt = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        C_cd = c.String(),
                        AddTime = c.DateTime(),
                        Sales = c.String(),
                        Commission = c.Decimal(precision: 18, scale: 2),
                        Stat_code = c.Int(),
                        Stat_desc = c.String(),
                        Cancel_comment = c.String(),
                        Bill_yyyymmdd = c.DateTime(),
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
