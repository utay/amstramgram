using GraphQL.Types;

public class UserInputType : InputObjectGraphType
{
    public UserInputType()
    {
        Name = "UserInput";

        Field<IntGraphType>("id");
        Field<StringGraphType>("nickname");
        Field<StringGraphType>("email");
        Field<StringGraphType>("password");
        Field<StringGraphType>("firstname");
        Field<StringGraphType>("lastname");
        Field<StringGraphType>("picture");
        Field<StringGraphType>("phone");
        Field<StringGraphType>("gender");
        Field<StringGraphType>("description");
        Field<BooleanGraphType>("private");
    }
}