using GraphQL.Types;

public class PictureInputType : InputObjectGraphType
{
    public PictureInputType()
    {
        Name = "PictureInput";

        Field<StringGraphType>("image");
        Field<StringGraphType>("description");
        Field<IntGraphType>("userId");
        Field<ListGraphType<TagInputType>>("tags");
    }
}
