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
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(QuickBootstrap.Entities.DefaultDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            new List<User>
            {
                new User
                {
                    UserName="mr.wangya@qq.com", 
                    UserPwd= "670b14728ad9902aecba32e22fa4f6bd", //000000
                    CreateTime = DateTime.Now,
                    Nick = "ÍõÑÇ",
                }
            }.ForEach(m => context.User.Add(m));
            
        }

     

    }
}
