using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Models;

namespace Api.DataAccess
{
    public static class User
    {
        public static Core.Models.User Add(Core.Models.User entity)
        {
            try
            {
                using (var ctx = new Data.AmstramgramContext())
                {
                    var userRepo = new Data.Repositories.UserRepository(ctx, null);
                    var result = userRepo.Add(entity);
                    if (!userRepo.SaveChanges())
                        return null;
                    return result;
                }
            }
            catch
            {
                return null;
            }
        }

        public static bool AddRange(IEnumerable<Core.Models.User> entities)
        {
            try
            {
                using (var ctx = new Data.AmstramgramContext())
                {
                    var userRepo = new Data.Repositories.UserRepository(ctx, null);
                    userRepo.AddRange(entities);
                    return userRepo.SaveChanges();
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool Delete(Core.Models.User entity)
        {
            try
            {
                using (var ctx = new Data.AmstramgramContext())
                {
                    var userRepo = new Data.Repositories.UserRepository(ctx, null);
                    userRepo.Delete(entity);
                    return userRepo.SaveChanges();
                }
            }
            catch
            {
                return false;
            }
        }

        public static async Task<Core.Models.User> Get(long id)
        {
            try
            {
                using (var ctx = new Data.AmstramgramContext())
                {
                    var userRepo = new Data.Repositories.UserRepository(ctx, null);
                    var result = await userRepo.Get(id);
                    return result;
                }
            }
            catch
            {
                return null;
            }
        }

        public static async Task<Core.Models.User> Get(long id, string include)
        {
            try
            {
                using (var ctx = new Data.AmstramgramContext())
                {
                    var userRepo = new Data.Repositories.UserRepository(ctx, null);
                    var result = await userRepo.Get(id, include);
                    return result;
                }
            }
            catch
            {
                return null;
            }
        }

        public static async Task<Core.Models.User> Get(long id, IEnumerable<string> includes)
        {
            try
            {
                using (var ctx = new Data.AmstramgramContext())
                {
                    var userRepo = new Data.Repositories.UserRepository(ctx, null);
                    var result = await userRepo.Get(id, includes);
                    return result;
                }
            }
            catch
            {
                return null;
            }
        }

        public static async Task<List<Core.Models.User>> GetAll()
        {
            try
            {
                using (var ctx = new Data.AmstramgramContext())
                {
                    var userRepo = new Data.Repositories.UserRepository(ctx, null);
                    var result = await userRepo.GetAll();
                    return result;
                }
            }
            catch
            {
                return new List<Core.Models.User>();
            }
        }

        public static async Task<List<Core.Models.User>> GetAll(string include)
        {
            try
            {
                using (var ctx = new Data.AmstramgramContext())
                {
                    var userRepo = new Data.Repositories.UserRepository(ctx, null);
                    var result = await userRepo.GetAll(include);
                    return result;
                }
            }
            catch
            {
                return new List<Core.Models.User>();
            }
        }

        public static async Task<List<Core.Models.User>> GetAll(IEnumerable<string> includes)
        {
            try
            {
                using (var ctx = new Data.AmstramgramContext())
                {
                    var userRepo = new Data.Repositories.UserRepository(ctx, null);
                    var result = await userRepo.GetAll(includes);
                    return result;
                }
            }
            catch
            {
                return new List<Core.Models.User>();
            }
        }

        public static async Task<ICollection<Core.Models.UserFollower>> GetFollowers(long id)
        {
            try
            {
                using (var ctx = new Data.AmstramgramContext())
                {
                    var userRepo = new Data.Repositories.UserRepository(ctx, null);
                    var result = await userRepo.GetFollowers(id);
                    return result;
                }
            }
            catch
            {
                return new List<Core.Models.UserFollower>();
            }
        }

        public static async Task<ICollection<Core.Models.UserFollower>> GetFollowing(long id)
        {
            try
            {
                using (var ctx = new Data.AmstramgramContext())
                {
                    var userRepo = new Data.Repositories.UserRepository(ctx, null);
                    var result = await userRepo.GetFollowing(id);
                    return result;
                }
            }
            catch
            {
                return new List<Core.Models.UserFollower>();
            }
        }

        public static async Task<Core.Models.User> GetFromAccessToken(string accessToken)
        {
            try
            {
                using (var ctx = new Data.AmstramgramContext())
                {
                    var userRepo = new Data.Repositories.UserRepository(ctx, null);
                    var result = await userRepo.GetFromAccessToken(accessToken);
                    return result;
                }
            }
            catch
            {
                return null;
            }
        }

        public static async Task<Core.Models.User> GetFromEmail(string email)
        {
            try
            {
                using (var ctx = new Data.AmstramgramContext())
                {
                    var userRepo = new Data.Repositories.UserRepository(ctx, null);
                    var result = await userRepo.GetFromEmail(email);
                    return result;
                }
            }
            catch
            {
                return null;
            }
        }

        public static async Task<ICollection<Core.Models.Picture>> GetPictures(long id)
        {
            try
            {
                using (var ctx = new Data.AmstramgramContext())
                {
                    var userRepo = new Data.Repositories.UserRepository(ctx, null);
                    var result = await userRepo.GetPictures(id);
                    return result;
                }
            }
            catch
            {
                return new List<Core.Models.Picture>();
            }
        }

        public static async Task<Core.Models.User> SignInUser(Core.Models.User user)
        {
            try
            {
                using (var ctx = new Data.AmstramgramContext())
                {
                    var userRepo = new Data.Repositories.UserRepository(ctx, null);
                    var result = await userRepo.SignInUser(user);
                    return result;
                }
            }
            catch
            {
                return null;
            }
        }

        public static bool Update(Core.Models.User entity)
        {
            try
            {
                using (var ctx = new Data.AmstramgramContext())
                {
                    var userRepo = new Data.Repositories.UserRepository(ctx, null);
                    userRepo.Update(entity);
                    return userRepo.SaveChanges();
                }
            }
            catch
            {
                return false;
            }
        }
    }
}