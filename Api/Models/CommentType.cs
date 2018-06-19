using AutoMapper;
using GraphQL.Types;

namespace Api.Models
{
    public class CommentType : ObjectGraphType<Comment>
    {
        public CommentType(IMapper mapper)
        {
            Name = "Comment";
            Description = "A comment";

            Field(x => x.Id, nullable: true).Description("The id of the comment.");
            Field(x => x.Text, nullable: true).Description("The text of the comment.");
            Field(x => x.CreatedAt, nullable: true).Description("The date of the creation of the comment.");

            Field<UserType>(
                "user",
                resolve: context =>
                {
                    var user = DataAccess.Comment.GetUser(context.Source.Id).Result;
                    var mapped = mapper.Map<User>(user);
                    return mapped;
                }
            );
        }
    }
}