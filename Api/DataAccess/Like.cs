using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Data;
using Core.Models;

namespace Api.DataAccess
{
    public static class Like
    {
        public static Core.Models.Like Add(Core.Models.Like entity)
        {
            try
            {
                using (var ctx = new Data.AmstramgramContext())
                {
                    var LikeRepo = new Data.Repositories.LikeRepository(ctx, null);
                    var result = LikeRepo.Add(entity);
                    if (!LikeRepo.SaveChanges())
                        return null;
                    return result;
                }
            }
            catch
            {
                return null;
            }
        }

        public static bool AddRange(IEnumerable<Core.Models.Like> entities)
        {
            try
            {
                using (var ctx = new Data.AmstramgramContext())
                {
                    var LikeRepo = new Data.Repositories.LikeRepository(ctx, null);
                    LikeRepo.AddRange(entities);
                    return LikeRepo.SaveChanges();
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool Delete(Core.Models.Like entity)
        {
            try
            {
                using (var ctx = new Data.AmstramgramContext())
                {
                    var LikeRepo = new Data.Repositories.LikeRepository(ctx, null);
                    LikeRepo.Delete(entity);
                    return LikeRepo.SaveChanges();
                }
            }
            catch
            {
                return false;
            }
        }

        public static Core.Models.Like Find(long userId, long pictureId)
        {
            try
            {
                using (var ctx = new Data.AmstramgramContext())
                {
                    var LikeRepo = new Data.Repositories.LikeRepository(ctx, null);
                    return LikeRepo.Find(userId, pictureId);
                }
            }
            catch
            {
                return null;
            }
        }

        public static async Task<Core.Models.Like> Get(long id)
        {
            try
            {
                using (var ctx = new Data.AmstramgramContext())
                {
                    var LikeRepo = new Data.Repositories.LikeRepository(ctx, null);
                    return await LikeRepo.Get(id);
                }
            }
            catch
            {
                return null;
            }
        }

        public static async Task<Core.Models.Like> Get(long id, string include)
        {
            try
            {
                using (var ctx = new Data.AmstramgramContext())
                {
                    var LikeRepo = new Data.Repositories.LikeRepository(ctx, null);
                    return await LikeRepo.Get(id, include);
                }
            }
            catch
            {
                return null;
            }
        }

        public static async Task<Core.Models.Like> Get(long id, IEnumerable<string> includes)
        {
            try
            {
                using (var ctx = new Data.AmstramgramContext())
                {
                    var LikeRepo = new Data.Repositories.LikeRepository(ctx, null);
                    return await LikeRepo.Get(id, includes);
                }
            }
            catch
            {
                return null;
            }
        }

        public static async Task<List<Core.Models.Like>> GetAll()
        {
            try
            {
                using (var ctx = new Data.AmstramgramContext())
                {
                    var LikeRepo = new Data.Repositories.LikeRepository(ctx, null);
                    return await LikeRepo.GetAll();
                }
            }
            catch
            {
                return new List<Core.Models.Like>();
            }
        }

        public static async Task<List<Core.Models.Like>> GetAll(string include)
        {
            try
            {
                using (var ctx = new Data.AmstramgramContext())
                {
                    var LikeRepo = new Data.Repositories.LikeRepository(ctx, null);
                    return await LikeRepo.GetAll(include);
                }
            }
            catch
            {
                return new List<Core.Models.Like>();
            }
        }

        public static async Task<List<Core.Models.Like>> GetAll(IEnumerable<string> includes)
        {
            try
            {
                using (var ctx = new Data.AmstramgramContext())
                {
                    var LikeRepo = new Data.Repositories.LikeRepository(ctx, null);
                    return await LikeRepo.GetAll(includes);
                }
            }
            catch
            {
                return new List<Core.Models.Like>();
            }
        }

        public static async Task<Core.Models.User> GetUser(long id)
        {
            try
            {
                using (var ctx = new Data.AmstramgramContext())
                {
                    var LikeRepo = new Data.Repositories.LikeRepository(ctx, null);
                    return await LikeRepo.GetUser(id);
                }
            }
            catch
            {
                return null;
            }
        }

        public static bool Update(Core.Models.Like entity)
        {
            try
            {
                using (var ctx = new Data.AmstramgramContext())
                {
                    var LikeRepo = new Data.Repositories.LikeRepository(ctx, null);
                    LikeRepo.Update(entity);
                    return LikeRepo.SaveChanges();
                }
            }
            catch
            {
                return false;
            }
        }
    }
}