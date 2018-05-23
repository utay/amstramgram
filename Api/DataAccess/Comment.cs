using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Models;

namespace Api.DataAccess
{
    public static class Comment
    {
        public static Core.Models.Comment Add(Core.Models.Comment entity)
        {
            try
            {
                using (var ctx = new Data.AmstramgramContext())
                {
                    var commentRepo = new Data.Repositories.CommentRepository(ctx, null);
                    var result = commentRepo.Add(entity);
                    if (!commentRepo.SaveChanges())
                        return null;
                    return result;
                }
            }
            catch
            {
                return null;
            }
        }

        public static bool AddRange(IEnumerable<Core.Models.Comment> entities)
        {
            try
            {
                using (var ctx = new Data.AmstramgramContext())
                {
                    var commentRepo = new Data.Repositories.CommentRepository(ctx, null);
                    commentRepo.AddRange(entities);
                    return commentRepo.SaveChanges();
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool Delete(Core.Models.Comment entity)
        {
            try
            {
                using (var ctx = new Data.AmstramgramContext())
                {
                    var commentRepo = new Data.Repositories.CommentRepository(ctx, null);
                    commentRepo.Delete(entity);
                    return commentRepo.SaveChanges();
                }
            }
            catch
            {
                return false;
            }
        }

        public static async Task<Core.Models.Comment> Get(long id)
        {
            try
            {
                using (var ctx = new Data.AmstramgramContext())
                {
                    var commentRepo = new Data.Repositories.CommentRepository(ctx, null);
                    return await commentRepo.Get(id);
                }
            }
            catch
            {
                return null;
            }
        }

        public static async Task<Core.Models.Comment> Get(long id, string include)
        {
            try
            {
                using (var ctx = new Data.AmstramgramContext())
                {
                    var commentRepo = new Data.Repositories.CommentRepository(ctx, null);
                    return await commentRepo.Get(id, include);
                }
            }
            catch
            {
                return null;
            }
        }

        public static async Task<Core.Models.Comment> Get(long id, IEnumerable<string> includes)
        {
            try
            {
                using (var ctx = new Data.AmstramgramContext())
                {
                    var commentRepo = new Data.Repositories.CommentRepository(ctx, null);
                    return await commentRepo.Get(id, includes);
                }
            }
            catch
            {
                return null;
            }
        }

        public static async Task<List<Core.Models.Comment>> GetAll()
        {
            try
            {
                using (var ctx = new Data.AmstramgramContext())
                {
                    var commentRepo = new Data.Repositories.CommentRepository(ctx, null);
                    return await commentRepo.GetAll();
                }
            }
            catch
            {
                return new List<Core.Models.Comment>();
            }
        }

        public static async Task<List<Core.Models.Comment>> GetAll(string include)
        {
            try
            {
                using (var ctx = new Data.AmstramgramContext())
                {
                    var commentRepo = new Data.Repositories.CommentRepository(ctx, null);
                    return await commentRepo.GetAll(include);
                }
            }
            catch
            {
                return new List<Core.Models.Comment>();
            }
        }

        public static async Task<List<Core.Models.Comment>> GetAll(IEnumerable<string> includes)
        {
            try
            {
                using (var ctx = new Data.AmstramgramContext())
                {
                    var commentRepo = new Data.Repositories.CommentRepository(ctx, null);
                    return await commentRepo.GetAll(includes);
                }
            }
            catch
            {
                return new List<Core.Models.Comment>();
            }
        }

        public static async Task<Core.Models.Picture> GetPicture(long id)
        {
            try
            {
                using (var ctx = new Data.AmstramgramContext())
                {
                    var commentRepo = new Data.Repositories.CommentRepository(ctx, null);
                    return await commentRepo.GetPicture(id);
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
                    var commentRepo = new Data.Repositories.CommentRepository(ctx, null);
                    return await commentRepo.GetUser(id);
                }
            }
            catch
            {
                return null;
            }
        }

        public static bool Update(Core.Models.Comment entity)
        {
            try
            {
                using (var ctx = new Data.AmstramgramContext())
                {
                    var commentRepo = new Data.Repositories.CommentRepository(ctx, null);
                    commentRepo.Update(entity);
                    return commentRepo.SaveChanges();
                }
            }
            catch
            {
                return false;
            }
        }
    }
}