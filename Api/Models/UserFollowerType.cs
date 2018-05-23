using AutoMapper;
using GraphQL.Types;

namespace Api.Models
{
    public class UserFollowerType : ObjectGraphType<UserFollower>
    {
        public UserFollowerType(IMapper mapper)
        {
            Name = "UserFollower";
            Description = "A follower";

            Field<UserType>(
                "user",
                resolve: context =>
                {
                    var userFollower = DataAccess.UserFollower.GetUser(context.Source.Id).Result;
                    var mapped = mapper.Map<UserFollower>(userFollower);
                    return mapped;
                }
            );

            Field<UserType>(
                "follower",
                resolve: context =>
                {
                    var userFollower = DataAccess.UserFollower.GetFollower(context.Source.Id).Result;
                    var mapped = mapper.Map<UserFollower>(userFollower);
                    return mapped;
                }
            );
        }
    }
}