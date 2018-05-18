using Api.Helper;
using AutoMapper;
using Data;
using GraphQL.Types;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Threading.Tasks;
//using Core.Logic;

namespace Api.Models
{
    public class AmstramgramQuery : ObjectGraphType
    {
        private HttpContext _context;

        public AmstramgramQuery() { _context = null;  }

        public AmstramgramQuery(HttpContext context)
        {
            _context = context;
        }

        public void AddHttpContext(HttpContext context)
        {
            _context = context;
        }

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

            Field<UserType>(
                "currentUser",
                arguments: new QueryArguments(),
                resolve: context =>
                {
                    long? id = Helper.AppHttpContext.HttpContext.Session.GetObject<long>("currentUserId");
                    if (id == null ||id == 0)
                        return null;
                    var user = userRepository.Get(id.Value).Result;
                    var mapped = mapper.Map<User>(user);
                    return mapped;
                }
            );
        }
    }
}
