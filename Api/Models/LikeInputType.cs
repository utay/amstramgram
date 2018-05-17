using GraphQL.Types;

public class LikeInputType : InputObjectGraphType
{
    public LikeInputType()
    {
        Name = "LikeInput";

        Field<IntGraphType>("userId");
        Field<IntGraphType>("pictureId");
    }
}
