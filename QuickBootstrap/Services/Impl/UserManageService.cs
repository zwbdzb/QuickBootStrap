using System;
using System.Data.Entity;
using System.Linq;
using QuickBootstrap.Entities;
using QuickBootstrap.Services.Util;

namespace QuickBootstrap.Services.Impl
{
    // 用户管理服务类
    public sealed class UserManageService : ServiceContext, IUserManageService
    {
        public PagedResult<User> GetAll()
        {
            var result = new PagedResult<User>
            {
                PageIndex = 0,
                PageSize = 10,
                SizeCount = DbContext.User.Count(),
                Result = DbContext.User.OrderByDescending(p => p.CreateTime).ToList()
            };

            return result;
        }

        public bool Create(User model)
        {
            DbContext.User.Add(model);
            return DbContext.SaveChanges() > 0;
        }

        public bool ExistsUser(string username)
        {
            return DbContext.User.FirstOrDefault(p => p.UserName.Equals(username,StringComparison.OrdinalIgnoreCase)) != null;
        }

        public User Get(string username = "")
        {
            return DbContext.User.FirstOrDefault(p => p.UserName.Equals(username, StringComparison.OrdinalIgnoreCase) );
        }

        public bool Edit(User model)
        {
            DbContext.Entry(model).State = EntityState.Modified;
            return DbContext.SaveChanges() > 0;
        }

        public bool Delete(string username)
        {
            var model = Get(username);
            if (model == null)
            {
                return false;   
            }
            DbContext.Entry(model).State = EntityState.Deleted;
            return DbContext.SaveChanges() > 0;
        }
    }
}