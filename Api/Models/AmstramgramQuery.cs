using AutoMapper;
using GraphQL.Types;
//using Core.Logic;

namespace Api.Models
{
    public class AmstramgramQuery : ObjectGraphType
    {
        public AmstramgramQuery() { }

        public AmstramgramQuery(Core.Data.IUserRepository userRepository, Core.Data.IPictureRepository pictureRepository, IMapper mapper)
        {
            Name = "Query";

            Field<UserType>(
                "user",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id", Description = "id of the user" }
                ),
                resolve: context =>
                {
                    var id = context.GetArgument<int>("id");
                    var user = userRepository.Get(id).Result;
                    var mapped = mapper.Map<User>(user);
                    return mapped;
                }
            );

            Field<PictureType>(
                "picture",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id", Description = "id of the picture" }
                ),
                resolve: context =>
                {
                    var id = context.GetArgument<int>("id");
                    var picture = pictureRepository.Get(id).Result;
                    var mapped = mapper.Map<Picture>(picture);
                    return mapped;
                }
            );
        }
    }
}
