﻿using Api.Helper;
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
        public AmstramgramQuery()
        {
        }

        public AmstramgramQuery(IMapper mapper)
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
                    var user = DataAccess.User.Get(id).Result;
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
                    var picture = DataAccess.Picture.Get(id).Result;
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
                    if (id == null || id == 0)
                        return null;
                    var user = DataAccess.User.Get(id.Value).Result;
                    var mapped = mapper.Map<User>(user);
                    return mapped;
                }
            );
        }
    }
}