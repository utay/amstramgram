using AutoMapper;
using Api.Models;

namespace Api
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User
            CreateMap<Core.Models.User, User>(MemberList.Destination)
                .ForMember(dest => dest.Followers, opt => opt.Ignore())
                .ForMember(dest => dest.Pictures, opt => opt.Ignore());

            // Picture
            CreateMap<Core.Models.Picture, Picture>(MemberList.Destination)
                .ForMember(dest => dest.User, opt => opt.Ignore());

            // Tag
            CreateMap<Core.Models.Tag, Tag>(MemberList.Destination);

            // Comment
            CreateMap<Core.Models.Comment, Comment>(MemberList.Destination)
                .ForMember(dest => dest.Picture, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());

            // Like
            CreateMap<Core.Models.Like, Like>(MemberList.Destination)
                .ForMember(dest => dest.Picture, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());

            // UserFollower
            CreateMap<Core.Models.UserFollower, UserFollower>(MemberList.Destination)
                .ForMember(dest => dest.Follower, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());
        }
    }
}
