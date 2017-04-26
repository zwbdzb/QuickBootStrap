using System;
using System.Data.Entity.Migrations;
using System.Collections.Generic;
using QuickBootstrap.Entities;

namespace QuickBootstrap.Migrations
{
    /// <summary>
    /// 本配置专为数据迁移
    /// </summary>
    internal sealed class Configuration : DbMigrationsConfiguration<DefaultDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(DefaultDbContext context)
        {
            /*  This method will be called after migrating to the latest version.
              You can use the DbSet<T>.AddOrUpdate() helper extension method 
              to avoid creating duplicate seed data. E.g.
            */

            context.User.Add(new User
            {
                UserName = "hiAladdin@163.com",
                UserPwd = "670b14728ad9902aecba32e22fa4f6bd",       //000000
                CreateTime = DateTime.Now,
                Nick = "huangjun",
            });
        }
    }
}
