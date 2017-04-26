using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickBootstrap.Entities
{
    /// <summary>
    /// 数据库初始化种子, 顺便完成 数据库不存在则创建数据库,数据库存在就使用原有数据库
    /// </summary>
    public sealed class InitDataBase : CreateDatabaseIfNotExists<DefaultDbContext>
    {
        protected override void Seed(DefaultDbContext context)
        {
          
        }
    }
}

/*
   CreateDatabaseIfNotExists：默认的策略:如果数据库不存在，那么就创建数据库。但是如果数据库存在了，而且实体发生了变化，就会出现异常。

   DropCreateDatabaseIfModelChanges：此策略表明:如果模型变化了，数据库就会被重新创建，原来的数据库被删除掉了。

   DropCreateDatabaseAlways：此策略表示:每次运行程序都会重新创建数据库，这在开发和调试的时候非常有用。
     
     */
