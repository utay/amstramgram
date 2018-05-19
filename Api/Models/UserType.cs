using System.Collections.Generic;
using AutoMapper;
using GraphQL.Types;

namespace Api.Models
{
    public class UserType : ObjectGraphType<User>
    {
        public UserType(Core.Data.IUserRepository userRepository, IMapper mapper)
        {
            Name = "User";
            Description = "A super normal user";

            Field(x => x.Id).Description("The id of the user.");
            Field(x => x.Nickname, nullable: true).Description("The nickname of the user.");
            Field(x => x.Email, nullable: false).Description("The email of the user.");
            Field(x => x.Password, nullable: true).Description("The password of the user.");
            Field(x => x.Firstname, nullable: true).Description("The firstname of the user.");
            Field(x => x.Lastname, nullable: true).Description("The lastname of the user.");
            Field(x => x.Picture, nullable: true).Description("The picture of the user.");
            Field(x => x.Phone, nullable: true).Description("The phone of the user.");
            Field(x => x.Gender, nullable: true).Description("The gender of the user.");
            Field(x => x.Description, nullable: true).Description("The description of the user.");
            Field(x => x.Private, nullable: true).Description("Whether the account is private or not.");

            Field<ListGraphType<PictureType>>(
                "pictures",
                resolve: context =>
                {
                    var pictures = userRepository.GetPictures(context.Source.Id).Result;
                    var mapped = mapper.Map<IEnumerable<Picture>>(pictures);
                    return mapped;
                }
            );

            Field<ListGraphType<UserFollowerType>>(
                "followers",
                resolve: context =>
                {
                    var followers = userRepository.GetFollowers(context.Source.Id).Result;
                    var mapped = mapper.Map<IEnumerable<UserFollower>>(followers);
                    return mapped;
                }
            );

            Field<ListGraphType<UserFollowerType>>(
                "following",
                resolve: context =>
                {
                    var following = userRepository.GetFollowing(context.Source.Id).Result;
                    var mapped = mapper.Map<IEnumerable<UserFollower>>(following);
                    return mapped;
                }
            );
        }
    }
}
