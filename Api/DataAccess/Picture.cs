using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Models;

namespace Api.DataAccess
{
    public static class Picture
    {
        public static Core.Models.Picture Add(Core.Models.Picture entity)
        {
            try
            {
                using (var ctx = new Data.AmstramgramContext())
                {
                    var pictureRepo = new Data.Repositories.PictureRepository(ctx, null);
                    var result = pictureRepo.Add(entity);
                    if (!pictureRepo.SaveChanges())
                        return null;
                    return result;
                }
            }
            catch
            {
                return null;
            }
        }

        public static bool AddRange(IEnumerable<Core.Models.Picture> entities)
        {
            try
            {
                using (var ctx = new Data.AmstramgramContext())
                {
                    var pictureRepo = new Data.Repositories.PictureRepository(ctx, null);
                    pictureRepo.AddRange(entities);
                    return pictureRepo.SaveChanges();
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool Delete(Core.Models.Picture entity)
        {
            try
            {
                using (var ctx = new Data.AmstramgramContext())
                {
                    var pictureRepo = new Data.Repositories.PictureRepository(ctx, null);
                    pictureRepo.Delete(entity);
                    return pictureRepo.SaveChanges();
                }
            }
            catch
            {
                return false;
            }
        }

        public static async Task<Core.Models.Picture> Get(long id)
        {
            try
            {
                using (var ctx = new Data.AmstramgramContext())
                {
                    var pictureRepo = new Data.Repositories.PictureRepository(ctx, null);
                    var result = await pictureRepo.Get(id);
                    return result;
                }
            }
            catch
            {
                return null;
            }
        }

        public static async Task<Core.Models.Picture> Get(long id, string include)
        {
            try
            {
                using (var ctx = new Data.AmstramgramContext())
                {
                    var pictureRepo = new Data.Repositories.PictureRepository(ctx, null);
                    var result = await pictureRepo.Get(id, include);
                    return result;
                }
            }
            catch
            {
                return null;
            }
        }

        public static async Task<Core.Models.Picture> Get(long id, IEnumerable<string> includes)
        {
            try
            {
                using (var ctx = new Data.AmstramgramContext())
                {
                    var pictureRepo = new Data.Repositories.PictureRepository(ctx, null);
                    var result = await pictureRepo.Get(id, includes);
                    return result;
                }
            }
            catch
            {
                return null;
            }
        }

        public static async Task<List<Core.Models.Picture>> GetAll()
        {
            try
            {
                using (var ctx = new Data.AmstramgramContext())
                {
                    var pictureRepo = new Data.Repositories.PictureRepository(ctx, null);
                    return await pictureRepo.GetAll();
                }
            }
            catch
            {
                return new List<Core.Models.Picture>();
            }
        }

        public static async Task<List<Core.Models.Picture>> GetAll(string include)
        {
            try
            {
                using (var ctx = new Data.AmstramgramContext())
                {
                    var pictureRepo = new Data.Repositories.PictureRepository(ctx, null);
                    return await pictureRepo.GetAll(include);
                }
            }
            catch
            {
                return new List<Core.Models.Picture>();
            }
        }

        public static async Task<List<Core.Models.Picture>> GetAll(IEnumerable<string> includes)
        {
            try
            {
                using (var ctx = new Data.AmstramgramContext())
                {
                    var pictureRepo = new Data.Repositories.PictureRepository(ctx, null);
                    return await pictureRepo.GetAll(includes);
                }
            }
            catch
            {
                return new List<Core.Models.Picture>();
            }
        }

        public static async Task<ICollection<Core.Models.Comment>> GetComments(long id)
        {
            try
            {
                using (var ctx = new Data.AmstramgramContext())
                {
                    var pictureRepo = new Data.Repositories.PictureRepository(ctx, null);
                    return await pictureRepo.GetComments(id);
                }
            }
            catch
            {
                return new List<Core.Models.Comment>();
            }
        }

        public static async Task<ICollection<Core.Models.Like>> GetLikes(long id)
        {
            try
            {
                using (var ctx = new Data.AmstramgramContext())
                {
                    var pictureRepo = new Data.Repositories.PictureRepository(ctx, null);
                    return await pictureRepo.GetLikes(id);
                }
            }
            catch
            {
                return new List<Core.Models.Like>();
            }
        }

        public static async Task<ICollection<Tag>> GetTags(long id)
        {
            try
            {
                using (var ctx = new Data.AmstramgramContext())
                {
                    var pictureRepo = new Data.Repositories.PictureRepository(ctx, null);
                    return await pictureRepo.GetTags(id);
                }
            }
            catch
            {
                return new List<Tag>();
            }
        }

        public static async Task<Core.Models.User> GetUser(long id)
        {
            try
            {
                using (var ctx = new Data.AmstramgramContext())
                {
                    var pictureRepo = new Data.Repositories.PictureRepository(ctx, null);
                    return await pictureRepo.GetUser(id);
                }
            }
            catch
            {
                return null;
            }
        }

        public static bool Update(Core.Models.Picture entity)
        {
            try
            {
                using (var ctx = new Data.AmstramgramContext())
                {
                    var pictureRepo = new Data.Repositories.PictureRepository(ctx, null);
                    pictureRepo.Update(entity);
                    return pictureRepo.SaveChanges();
                }
            }
            catch
            {
                return false;
            }
        }
    }
}