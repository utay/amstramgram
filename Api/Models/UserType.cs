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
            Description = "A fucking user";

            Field(x => x.Id).Description("The Id of the User.");
            Field(x => x.Firstname, nullable: true).Description("The firstname of the User.");

            Field<ListGraphType<PictureType>>(
                "pictures",
                resolve: context =>
                {
                    System.Console.WriteLine(context.Source.Id);
                    var pictures = userRepository.GetPictures(context.Source.Id).Result;
                    var mapped = mapper.Map<IEnumerable<Picture>>(pictures);
                    return mapped;
                }
            );
        }
    }
}
