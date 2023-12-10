using AutoMapper;
using BlogMVC.BLL.Models;
using BlogMVC.Models;

namespace BlogMVC
{
    public class WebMappingProfile : Profile
    {
        public WebMappingProfile() 
        { 
            CreateMap<RegisterViewModel, UserDTO>()
                .ForMember(u => u.UserName, opt => opt.MapFrom(rm => rm.Email));
            CreateMap<LoginViewModel, LoginDTO>();
            CreateMap<EditBlogPostViewModel, EditBlogPostDTO>()
                .ForMember(d => d.CategoryId, opt => opt.Ignore());
            CreateMap<BlogPostDTO, EditBlogPostViewModel>()
                .ForMember(d => d.CategoryName, opt => opt.MapFrom(s => s.Category.Name));
        }
    }
}
