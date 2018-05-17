using System.Collections.Generic;
using AutoMapper;
using GraphQL.Types;

namespace Api.Models
{
    public class PictureType : ObjectGraphType<Picture>
    {
        public PictureType(Core.Data.IPictureRepository pictureRepository, IMapper mapper)
        {
            Name = "Picture";
            Description = "A picture";

            Field(x => x.Id).Description("The Id of the picture.");
            Field(x => x.Image, nullable: true).Description("The image of the picture.");
            Field(x => x.Description, nullable: true).Description("The description of the picture.");
            Field(x => x.Color, nullable: true).Description("The accent color of the picture.");
            Field(x => x.CreatedAt, nullable: true).Description("The creation date of the picture.");
            Field(x => x.UpdatedAt, nullable: true).Description("The update date of the picture.");

            Field<UserType>(
                "user",
                resolve: context =>
                {
                    var user = pictureRepository.GetUser(context.Source.Id).Result;
                    var mapped = mapper.Map<User>(user);
                    return mapped;
                }
            );

            Field<ListGraphType<TagType>>(
                "tags",
                resolve: context =>
                {
                    var tags = pictureRepository.GetTags(context.Source.Id).Result;
                    var mapped = mapper.Map<IEnumerable<Tag>>(tags);
                    return mapped;
                }
            );

            Field<ListGraphType<CommentType>>(
                "comments",
                resolve: context =>
                {
                    var comments = pictureRepository.GetComments(context.Source.Id).Result;
                    var mapped = mapper.Map<IEnumerable<Comment>>(comments);
                    return mapped;
                }
            );

            Field<ListGraphType<LikeType>>(
                "likes",
                resolve: context =>
                {
                    var likes = pictureRepository.GetLikes(context.Source.Id).Result;
                    var mapped = mapper.Map<IEnumerable<Like>>(likes);
                    return mapped;
                }
            );
        }
    }
}
