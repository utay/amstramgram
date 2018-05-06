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
                    var data = userFollowerRepository.GetUser(context.Source.Id).Result;
                    var mapped = mapper.Map<User>(data);
                    return mapped;
                }
            );

            Field<UserType>(
                "follower",
                resolve: context =>
                {
                    var data = userFollowerRepository.GetFollower(context.Source.Id).Result;
                    var mapped = mapper.Map<User>(data);
                    return mapped;
                }
            );
        }
    }
}
