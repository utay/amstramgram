using System;
using GraphQL.Types;

namespace Api.Models
{
    public class AmstramgramSchema : Schema
    {
        public AmstramgramSchema(Func<Type, GraphType> resolveType)
            : base(resolveType)
        {
            Query = (AmstramgramQuery)resolveType(typeof(AmstramgramQuery));
            Mutation = (AmstramgramMutation)resolveType(typeof(AmstramgramMutation));
        }
    }
}
