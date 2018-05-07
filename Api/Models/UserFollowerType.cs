using AutoMapper;
using GraphQL.Types;

namespace Api.Models
{
    public class UserFollowerType : ObjectGraphType<UserFollower>
    {
        public UserFollowerType(Core.Data.IUserFollowerRepository userFollowerRepository, IMapper mapper)
        {
            Name = "UserFollower";
            Description = "A follower";

            Field<UserType>(
                "user",
                resolve: context =>
                {
                    return context.Source.User;
                }
            );

            Field<UserType>(
                "follower",
                resolve: context =>
                {
                    return context.Source.Follower;
                }
            );
        }
    }
}
