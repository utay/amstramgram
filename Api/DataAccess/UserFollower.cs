using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Data;
using Core.Models;

namespace Api.DataAccess
{
    public static class UserFollower
    {
        public static Core.Models.UserFollower Add(Core.Models.UserFollower entity)
        {
            try
            {
                using (var ctx = new Data.AmstramgramContext())
                {
                    var UserFollowerRepo = new Data.Repositories.UserFollowerRepository(ctx, null);
                    var result = UserFollowerRepo.Add(entity);
                    if (!UserFollowerRepo.SaveChanges())
                        return null;
                    return result;
                }
            }
            catch
            {
                return null;
            }
        }

        public static bool AddRange(IEnumerable<Core.Models.UserFollower> entities)
        {
            try
            {
                using (var ctx = new Data.AmstramgramContext())
                {
                    var UserFollowerRepo = new Data.Repositories.UserFollowerRepository(ctx, null);
                    UserFollowerRepo.AddRange(entities);
                    return UserFollowerRepo.SaveChanges();
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool Delete(Core.Models.UserFollower entity)
        {
            try
            {
                using (var ctx = new Data.AmstramgramContext())
                {
                    var UserFollowerRepo = new Data.Repositories.UserFollowerRepository(ctx, null);
                    UserFollowerRepo.Delete(entity);
                    return UserFollowerRepo.SaveChanges();
                }
            }
            catch
            {
                return false;
            }
        }

        public static Core.Models.UserFollower Find(long userId, long followerId)
        {
            try
            {
                using (var ctx = new Data.AmstramgramContext())
                {
                    var UserFollowerRepo = new Data.Repositories.UserFollowerRepository(ctx, null);
                    return UserFollowerRepo.Find(userId, followerId);
                }
            }
            catch
            {
                return null;
            }
        }

        public static async Task<Core.Models.UserFollower> Get(long id)
        {
            try
            {
                using (var ctx = new Data.AmstramgramContext())
                {
                    var UserFollowerRepo = new Data.Repositories.UserFollowerRepository(ctx, null);
                    var result = await UserFollowerRepo.Get(id);
                    return result;
                }
            }
            catch
            {
                return null;
            }
        }

        public static async Task<Core.Models.UserFollower> Get(long id, string include)
        {
            try
            {
                using (var ctx = new Data.AmstramgramContext())
                {
                    var UserFollowerRepo = new Data.Repositories.UserFollowerRepository(ctx, null);
                    var result = await UserFollowerRepo.Get(id, include);
                    return result;
                }
            }
            catch
            {
                return null;
            }
        }

        public static async Task<Core.Models.UserFollower> Get(long id, IEnumerable<string> includes)
        {
            try
            {
                using (var ctx = new Data.AmstramgramContext())
                {
                    var UserFollowerRepo = new Data.Repositories.UserFollowerRepository(ctx, null);
                    var result = await UserFollowerRepo.Get(id, includes);
                    return result;
                }
            }
            catch
            {
                return null;
            }
        }

        public static async Task<List<Core.Models.UserFollower>> GetAll()
        {
            try
            {
                using (var ctx = new Data.AmstramgramContext())
                {
                    var UserFollowerRepo = new Data.Repositories.UserFollowerRepository(ctx, null);
                    return await UserFollowerRepo.GetAll();
                }
            }
            catch
            {
                return new List<Core.Models.UserFollower>();
            }
        }

        public static async Task<List<Core.Models.UserFollower>> GetAll(string include)
        {
            try
            {
                using (var ctx = new Data.AmstramgramContext())
                {
                    var UserFollowerRepo = new Data.Repositories.UserFollowerRepository(ctx, null);
                    return await UserFollowerRepo.GetAll(include);
                }
            }
            catch
            {
                return new List<Core.Models.UserFollower>();
            }
        }

        public static async Task<List<Core.Models.UserFollower>> GetAll(IEnumerable<string> includes)
        {
            try
            {
                using (var ctx = new Data.AmstramgramContext())
                {
                    var UserFollowerRepo = new Data.Repositories.UserFollowerRepository(ctx, null);
                    return await UserFollowerRepo.GetAll(includes);
                }
            }
            catch
            {
                return new List<Core.Models.UserFollower>();
            }
        }

        public static async Task<Core.Models.User> GetFollower(long id)
        {
            try
            {
                using (var ctx = new Data.AmstramgramContext())
                {
                    var UserFollowerRepo = new Data.Repositories.UserFollowerRepository(ctx, null);
                    return await UserFollowerRepo.GetFollower(id);
                }
            }
            catch
            {
                return null;
            }
        }

        public static async Task<Core.Models.User> GetUser(long id)
        {
            try
            {
                using (var ctx = new Data.AmstramgramContext())
                {
                    var UserFollowerRepo = new Data.Repositories.UserFollowerRepository(ctx, null);
                    return await UserFollowerRepo.GetUser(id);
                }
            }
            catch
            {
                return null;
            }
        }

        public static bool Update(Core.Models.UserFollower entity)
        {
            try
            {
                using (var ctx = new Data.AmstramgramContext())
                {
                    var UserFollowerRepo = new Data.Repositories.UserFollowerRepository(ctx, null);
                    UserFollowerRepo.Update(entity);
                    return UserFollowerRepo.SaveChanges();
                }
            }
            catch
            {
                return false;
            }
        }
    }
}