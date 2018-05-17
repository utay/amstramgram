using GraphQL.Types;

public class UserFollowerInputType : InputObjectGraphType
{
    public UserFollowerInputType()
    {
        Name = "UserFollowerInput";

        Field<IntGraphType>("userId");
        Field<IntGraphType>("followerId");
    }
}
