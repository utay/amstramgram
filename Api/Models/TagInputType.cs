using GraphQL.Types;

public class TagInputType : InputObjectGraphType
{
    public TagInputType()
    {
        Name = "TagInput";

        Field<StringGraphType>("text");
    }
}
