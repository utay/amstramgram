using GraphQL.Types;

public class CommentInputType : InputObjectGraphType
{
    public CommentInputType()
    {
        Name = "CommentInput";

        Field<StringGraphType>("text");
        Field<IntGraphType>("userId");
        Field<IntGraphType>("pictureId");
    }
}
