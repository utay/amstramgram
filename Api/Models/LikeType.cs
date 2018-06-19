using AutoMapper;
using GraphQL.Types;

namespace Api.Models
{
    public class LikeType : ObjectGraphType<Like>
    {
        public LikeType(IMapper mapper)
        {
            Name = "Like";
            Description = "A like";

            Field(x => x.CreatedAt, nullable: true).Description("The creation date of the like.");

            Field<UserType>(
                "user",
                resolve: context =>
                {
                    var user = DataAccess.Like.GetUser(context.Source.Id).Result;
                    var mapped = mapper.Map<User>(user);
                    return mapped;
                }
            );
        }
    }
}