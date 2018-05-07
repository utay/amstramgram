using AutoMapper;
using GraphQL.Types;
using Core;
using System;

namespace Api.Models
{
    public class AmstramgramMutation : ObjectGraphType
    {
        public AmstramgramMutation() { }

        public AmstramgramMutation(Core.Data.IUserRepository userRepository, Core.Data.IPictureRepository pictureRepository,
            Core.Data.ITagRepository tagRepository, Core.Data.ICommentRepository commentRepository,
            Core.Data.ILikeRepository likeRepository, Core.Data.IUserFollowerRepository userFollowerRepository,
            IMapper mapper)
        {
            Name = "Mutation";

            Field<UserType>(
                "createUser",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<UserInputType>> { Name = "user" }
                ),
                resolve: context =>
                {
                    var data = context.GetArgument<Core.Models.User>("user");
                    var user = userRepository.Add(data);
                    userRepository.SaveChangesAsync();
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
                    pictureRepository.SaveChangesAsync();
                    return mapper.Map<Picture>(picture);
                }
            );

            Field<TagType>(
                "createTag",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<TagInputType>> { Name = "tag" }
                ),
                resolve: context =>
                {
                    var data = context.GetArgument<Core.Models.Tag>("tag");
                    if (pictureRepository.Get(data.PictureId).Result == null)
                    {
                        return null;
                    }
                    if (data.Text == "")
                    {
                        return null;
                    }
                    var tag = tagRepository.Add(data);
                    tagRepository.SaveChangesAsync();
                    return mapper.Map<Tag>(tag);
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
                    commentRepository.SaveChangesAsync();
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
                    likeRepository.SaveChangesAsync();
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
                    if (userFollowerRepository.Find(data.UserId, data.FollowerId).Result != null)
                    {
                        return null;
                    }
                    var follower = userFollowerRepository.Add(data);
                    userFollowerRepository.SaveChangesAsync();
                    return mapper.Map<UserFollower>(follower);
                }
            );
        }
    }
}
