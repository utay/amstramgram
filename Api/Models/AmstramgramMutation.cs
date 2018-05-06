using AutoMapper;
using GraphQL.Types;
using Core;

namespace Api.Models
{
    public class AmstramgramMutation : ObjectGraphType
    {
        public AmstramgramMutation() { }

        public AmstramgramMutation(Core.Data.IUserRepository userRepository, Core.Data.IPictureRepository pictureRepository, IMapper mapper)
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
                    var picture = pictureRepository.Add(data);
                    pictureRepository.SaveChangesAsync();
                    return mapper.Map<Picture>(picture);
                }
            );
        }
    }
}
