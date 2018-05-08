using GraphQL.Types;

namespace Api.Models
{
    public class TagType : ObjectGraphType<Tag>
    {
        public TagType()
        {
            Name = "Tag";
            Description = "A tag";

            Field(x => x.Text, nullable: true).Description("The text of the tag.");
        }
    }
}
