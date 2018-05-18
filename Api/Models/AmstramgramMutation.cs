using AutoMapper;
using GraphQL.Types;
using Core;
using System;
using Algolia.Search;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;

namespace Api.Models
{
    public class AmstramgramMutation : ObjectGraphType
    {
        public AmstramgramMutation() { }

        public AmstramgramMutation(Core.Data.IUserRepository userRepository, Core.Data.IPictureRepository pictureRepository,
            Core.Data.ICommentRepository commentRepository, Core.Data.ILikeRepository likeRepository,
            Core.Data.IUserFollowerRepository userFollowerRepository, IMapper mapper)
        {
            Name = "Mutation";

            AlgoliaClient client = new AlgoliaClient("A71NP8C36C", "ac1a68327b713553e3d21307968adab7");
            Index usersIndex = client.InitIndex("Amstramgram_users");
            Index picturesIndex = client.InitIndex("Amstramgram_pictures");
            Index tagsIndex = client.InitIndex("Amstramgram_tags");

            Field<UserType>(
                "createUser",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<UserInputType>> { Name = "user" }
                ),
                resolve: context =>
                {
                    var data = context.GetArgument<Core.Models.User>("user");
                    var user = userRepository.Add(data);
                    userRepository.SaveChanges();
                    data.objectID = data.Id.ToString();
                    usersIndex.AddObject(data);
                    return mapper.Map<User>(user);
                }
            );

            Field<UserType>(
                "updateUser",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<UserInputType>> { Name = "user" }
                ),
                resolve: context =>
                {
                    var data = context.GetArgument<Core.Models.User>("user");
                    userRepository.Update(data);
                    userRepository.SaveChanges();
                    usersIndex.PartialUpdateObject(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(data)));
                    return mapper.Map<User>(null);
                }
            );

            Field<PictureType>(
                "createPicture",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<PictureInputType>> { Name = "picture" }
                ),
                resolve: context =>
                {
                    var data = context.GetArgument<Core.Models.Picture>("picture");
                    if (userRepository.Get(data.UserId).Result == null)
                    {
                        return null;
                    }
                    var date = ((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
                    data.CreatedAt = date;
                    data.UpdatedAt = date;
                    var picture = pictureRepository.Add(data);
                    pictureRepository.SaveChanges();
                    foreach (var tag in data.Tags)
                    {
                        tagsIndex.AddObject(tag);
                    }
                    data.objectID = data.Id.ToString();
                    picturesIndex.AddObject(data);
                    return mapper.Map<Picture>(picture);
                }
            );

            Field<CommentType>(
                "createComment",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<CommentInputType>> { Name = "comment" }
                ),
                resolve: context =>
                {
                    var data = context.GetArgument<Core.Models.Comment>("comment");
                    if (userRepository.Get(data.UserId).Result == null || pictureRepository.Get(data.PictureId).Result == null)
                    {
                        return null;
                    }
                    if (data.Text == "")
                    {
                        return null;
                    }
                    var date = ((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
                    data.CreatedAt = date;
                    var comment = commentRepository.Add(data);
                    commentRepository.SaveChanges();
                    return mapper.Map<Comment>(comment);
                }
            );

            Field<CommentType>(
                "deleteComment",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<CommentInputType>> { Name = "comment" }
                ),
                resolve: context =>
                {
                    var data = context.GetArgument<Core.Models.Comment>("comment");
                    if (userRepository.Get(data.UserId).Result == null || pictureRepository.Get(data.PictureId).Result == null)
                    {
                        return null;
                    }
                    commentRepository.Delete(data);
                    commentRepository.SaveChanges();
                    return mapper.Map<Comment>(data);
                }
            );

            Field<LikeType>(
                "createLike",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<LikeInputType>> { Name = "like" }
                ),
                resolve: context =>
                {
                    var data = context.GetArgument<Core.Models.Like>("like");
                    if (userRepository.Get(data.UserId).Result == null || pictureRepository.Get(data.PictureId).Result == null)
                    {
                        return null;
                    }
                    if (likeRepository.Find(data.UserId, data.PictureId) != null)
                    {
                        return null;
                    }
                    var date = ((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
                    data.CreatedAt = date;
                    var like = likeRepository.Add(data);
                    likeRepository.SaveChanges();
                    return mapper.Map<Like>(like);
                }
            );

            Field<LikeType>(
                "deleteLike",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<LikeInputType>> { Name = "like" }
                ),
                resolve: context =>
                {
                    var data = context.GetArgument<Core.Models.Like>("like");
                    if (userRepository.Get(data.UserId).Result == null || pictureRepository.Get(data.PictureId).Result == null)
                    {
                        return null;
                    }
                    var like = likeRepository.Find(data.UserId, data.PictureId);
                    if (like == null)
                    {
                        return null;
                    }
                    likeRepository.Delete(like);
                    likeRepository.SaveChanges();
                    return mapper.Map<Like>(like);
                }
            );

            Field<UserFollowerType>(
                "createFollower",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<UserFollowerInputType>> { Name = "follower" }
                ),
                resolve: context =>
                {
                    var data = context.GetArgument<Core.Models.UserFollower>("follower");
                    if (userRepository.Get(data.UserId).Result == null || userRepository.Get(data.FollowerId).Result == null)
                    {
                        return null;
                    }
                    if (data.UserId == data.FollowerId)
                    {
                        return null;
                    }
                    if (userFollowerRepository.Find(data.UserId, data.FollowerId).Result != null)
                    {
                        return null;
                    }
                    var follower = userFollowerRepository.Add(data);
                    userFollowerRepository.SaveChanges();
                    return mapper.Map<UserFollower>(follower);
                }
            );

            Field<UserFollowerType>(
                "deleteFollower",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<UserFollowerInputType>> { Name = "follower" }
                ),
                resolve: context =>
                {
                    var data = context.GetArgument<Core.Models.UserFollower>("follower");
                    if (userRepository.Get(data.UserId).Result == null || userRepository.Get(data.FollowerId).Result == null)
                    {
                        return null;
                    }
                    var follower = userFollowerRepository.Find(data.UserId, data.FollowerId).Result;
                    if (follower == null)
                    {
                        return null;
                    }
                    userFollowerRepository.Delete(follower);
                    userFollowerRepository.SaveChanges();
                    return mapper.Map<UserFollower>(follower);
                }
            );

            Field<UserType>(
               "registerUser",
               arguments: new QueryArguments(
                   new QueryArgument<NonNullGraphType<UserInputType>> { Name = "user" }
               ),
               resolve: context =>
               {
                   var data = context.GetArgument<Core.Models.User>("user");
                   if (data.Email == null || data.Email == "" || data.Password == null || data.Password == "")
                       return null;
                   data.Password = Helper.Users.HashPassword(data.Password);
                   var user = userRepository.Add(data);
                   userRepository.SaveChanges();
                   user.objectID = data.Id.ToString();
                   usersIndex.AddObject(data);
                   userRepository.Update(data);
                   userRepository.SaveChanges();
                   Helper.AppHttpContext.HttpContext.Response.Cookies.Append(".Amstramgram.Cookie", user.Password);
                   return mapper.Map<User>(user);
               }
           );

            Field<UserType>(
                "connectUser",
                arguments: new QueryArguments(
                   new QueryArgument<NonNullGraphType<UserInputType>> { Name = "user" }
               ),
                resolve: context =>
                {
                    var data = context.GetArgument<Core.Models.User>("user");
                    if (data.Email == null || data.Email == "" || data.Password == null || data.Password == "")
                        return null;
                    data.Password = Helper.Users.HashPassword(data.Password);
                    var user = userRepository.SignInUser(data).Result;
                    Helper.AppHttpContext.HttpContext.Response.Cookies.Append(".Amstramgram.Cookie", user.AccessToken);
                    return mapper.Map<User>(user);
                }
            );
        }
    }
}
