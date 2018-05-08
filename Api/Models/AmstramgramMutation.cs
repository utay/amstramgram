using AutoMapper;
using GraphQL.Types;
using Core;
using System;
using Algolia.Search;

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
                    usersIndex.AddObject(data);
                    return mapper.Map<User>(user);
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
                    data.CreatedAt = DateTime.Now.ToString();
                    data.UpdatedAt = DateTime.Now.ToString();
                    var picture = pictureRepository.Add(data);
                    pictureRepository.SaveChanges();
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
                    data.CreatedAt = DateTime.Now.ToString();
                    var comment = commentRepository.Add(data);
                    commentRepository.SaveChanges();
                    return mapper.Map<Comment>(comment);
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
                    if (likeRepository.Find(data.UserId, data.PictureId).Result != null)
                    {
                        return null;
                    }
                    data.CreatedAt = DateTime.Now.ToString();
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
                    var like = likeRepository.Find(data.UserId, data.PictureId).Result;
                    if (like == null)
                    {
                        return null;
                    }
                    likeRepository.Detach(like);
                    likeRepository.Delete(data.UserId, data.PictureId);
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
                    userFollowerRepository.Detach(follower);
                    userFollowerRepository.Delete(data.UserId, data.FollowerId);
                    userFollowerRepository.SaveChanges();
                    return mapper.Map<UserFollower>(follower);
                }
            );
        }
    }
}
