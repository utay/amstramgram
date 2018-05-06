using AutoMapper;
using GraphQL.Types;

namespace Api.Models
{
    public class CommentType : ObjectGraphType<Comment>
    {
        public CommentType(Core.Data.ICommentRepository commentRepository, IMapper mapper)
        {
            Name = "Comment";
            Description = "A comment";

            Field(x => x.Id, nullable: true).Description("The id of the comment.");
            Field(x => x.Text, nullable: true).Description("The text of the comment.");
        }
    }
}
