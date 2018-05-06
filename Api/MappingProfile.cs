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
        }
    }
}
