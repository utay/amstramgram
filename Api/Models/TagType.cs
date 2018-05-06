using AutoMapper;
using GraphQL.Types;

namespace Api.Models
{
    public class TagType : ObjectGraphType<Tag>
    {
        public TagType(Core.Data.ITagRepository tagRepository, IMapper mapper)
        {
            Name = "Tag";
            Description = "A tag";

            Field(x => x.Id, nullable: true).Description("The id of the tag.");
            Field(x => x.Text, nullable: true).Description("The text of the tag.");

            Field<PictureType>(
                "picture",
                resolve: context =>
                {
                    var picture = tagRepository.GetPicture(context.Source.Id).Result;
                    var mapped = mapper.Map<Picture>(picture);
                    return mapped;
                }
            );
        }
    }
}
