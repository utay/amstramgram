using AutoMapper;
using GraphQL.Types;
using Core;
using System;
using Algolia.Search;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Configuration;

namespace Api.Models
{
    public class AmstramgramMutation : ObjectGraphType
    {
        public AmstramgramMutation()
        {
        }

        public AmstramgramMutation(IMapper mapper, IConfiguration configuration)
        {
            Name = "Mutation";

            AlgoliaClient client = new AlgoliaClient(configuration.GetValue<string>("Algolia:AppId"),
                configuration.GetValue<string>("Algolia:AppSecret"));
            Index usersIndex = client.InitIndex(configuration.GetValue<string>("Algolia:IndexUser"));
            Index picturesIndex = client.InitIndex(configuration.GetValue<string>("Algolia:IndexPictures"));
            Index tagsIndex = client.InitIndex(configuration.GetValue<string>("Algolia:IndexTags"));

            Field<UserType>(
                "createUser",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<UserInputType>> { Name = "user" }
                ),
                resolve: context =>
                {
                    var data = context.GetArgument<Core.Models.User>("user");
                    var user = DataAccess.User.Add(data);
                    if (user != null)
                    {
                        data.objectID = data.Id.ToString();
                        usersIndex.AddObject(data);
                        //Save object id
                        DataAccess.User.Update(data);
                    }
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
                    var userInDb = DataAccess.User.Get(data.Id).Result;
                    if (userInDb == null || userInDb.Id == 0)
                        //User doesn't exist
                        return null;
                    data.objectID = userInDb.objectID;
                    data.AccessToken = userInDb.AccessToken;
                    bool result = DataAccess.User.Update(data);
                    if (result)
                    {
                        usersIndex.PartialUpdateObject(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(data)));
                    }
                    return mapper.Map<User>(result ? data : null);
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
                    if (DataAccess.User.Get(data.UserId).Result == null)
                    {
                        return null;
                    }
                    var date = ((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
                    data.CreatedAt = date;
                    data.UpdatedAt = date;
                    var picture = DataAccess.Picture.Add(data);
                    foreach (var tag in data.Tags)
                    {
                        tagsIndex.AddObject(tag);
                    }
                    data.objectID = data.Id.ToString();
                    picturesIndex.AddObject(data);
                    DataAccess.Picture.Update(data);
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
                    if (DataAccess.User.Get(data.UserId).Result == null || DataAccess.Picture.Get(data.PictureId).Result == null)
                    {
                        return null;
                    }
                    if (data.Text == "")
                    {
                        return null;
                    }
                    var date = ((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
                    data.CreatedAt = date;
                    var comment = DataAccess.Comment.Add(data);
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
                    if (DataAccess.User.Get(data.UserId).Result == null || DataAccess.Picture.Get(data.PictureId).Result == null)
                    {
                        return null;
                    }
                    DataAccess.Comment.Delete(data);
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
                    if (DataAccess.User.Get(data.UserId).Result == null || DataAccess.Picture.Get(data.PictureId).Result == null)
                    {
                        return null;
                    }
                    if (DataAccess.Like.Find(data.UserId, data.PictureId) != null)
                    {
                        return null;
                    }
                    var date = ((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
                    data.CreatedAt = date;
                    var like = DataAccess.Like.Add(data);
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
                    if (DataAccess.User.Get(data.UserId).Result == null || DataAccess.Picture.Get(data.PictureId).Result == null)
                    {
                        return null;
                    }
                    var like = DataAccess.Like.Find(data.UserId, data.PictureId);
                    if (like == null)
                    {
                        return null;
                    }
                    DataAccess.Like.Delete(like);
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
                    if (DataAccess.User.Get(data.UserId).Result == null || DataAccess.User.Get(data.FollowerId).Result == null)
                    {
                        return null;
                    }
                    if (data.UserId == data.FollowerId)
                    {
                        return null;
                    }
                    if (DataAccess.UserFollower.Find(data.UserId, data.FollowerId) != null)
                    {
                        return null;
                    }
                    var follower = DataAccess.UserFollower.Add(data);
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
                    if (DataAccess.User.Get(data.UserId).Result == null || DataAccess.User.Get(data.FollowerId).Result == null)
                    {
                        return null;
                    }
                    var follower = DataAccess.UserFollower.Find(data.UserId, data.FollowerId);
                    if (follower == null)
                    {
                        return null;
                    }
                    DataAccess.UserFollower.Delete(follower);
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
                   var user = DataAccess.User.Add(data);
                   user.objectID = data.Id.ToString();
                   usersIndex.AddObject(data);
                   DataAccess.User.Update(data);
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
                    var user = DataAccess.User.SignInUser(data).Result;
                    Helper.AppHttpContext.HttpContext.Response.Cookies.Append(".Amstramgram.Cookie", user.AccessToken);
                    return mapper.Map<User>(user);
                }
            );
        }
    }
}