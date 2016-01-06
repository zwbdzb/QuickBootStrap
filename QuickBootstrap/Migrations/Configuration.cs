using System.Collections.Generic;
using QuickBootstrap.Entities;

namespace QuickBootstrap.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<QuickBootstrap.Entities.DefaultDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(QuickBootstrap.Entities.DefaultDbContext context)
        {
            // context.Database.ExecuteSqlCommand("CREATE INDEX IX_OrderNO ON SalesDatas (O_cd)");

            //new List<User>
            //{
            //    new User
            //    {
            //        UserName="mr.wangya@qq.com",
            //        UserPwd= "670b14728ad9902aecba32e22fa4f6bd", //000000
            //        CreateTime = DateTime.Now,
            //        Nick = "王亚",
            //    },
            //    new User{
            //         UserName="hiAladdin@163.com",
            //        UserPwd= "670b14728ad9902aecba32e22fa4f6bd", //000000
            //        CreateTime = DateTime.Now,
            //        Nick = "王军",
            //    }
            //}.ForEach(m => context.User.Add(m));
        }
    }
}
