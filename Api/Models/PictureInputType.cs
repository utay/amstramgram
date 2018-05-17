using GraphQL.Types;

public class PictureInputType : InputObjectGraphType
{
    public PictureInputType()
    {
        Name = "PictureInput";

        Field<StringGraphType>("image");
        Field<StringGraphType>("description");
        Field<StringGraphType>("color");
        Field<IntGraphType>("userId");
        Field<ListGraphType<TagInputType>>("tags");
    }
}
