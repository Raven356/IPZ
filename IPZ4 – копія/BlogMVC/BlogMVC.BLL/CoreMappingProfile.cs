using AutoMapper;
using BlogMVC.BLL.Models;
using BlogMVC.DAL.Models;
using BlogMVC.Models;

namespace BlogMVC.BLL
{
    public class CoreMappingProfile : Profile
    {
        public CoreMappingProfile() 
        {
            CreateMap<EditBlogPostDTO, BlogPost>()
                .ForMember(dst => dst.AuthorId, opt => opt.MapFrom(s => s.AuthorId))
                .ForMember(dst => dst.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(dst => dst.Text, opt => opt.MapFrom(s => s.Text))
                .ForMember(dst => dst.CategoryId, opt => opt.MapFrom(s => s.CategoryId))
                .ForMember(dst => dst.Date, opt => opt.MapFrom(s => s.Date))
                .ForMember(dst => dst.Title, opt => opt.MapFrom(s => s.Title))
                .ForMember(dst => dst.Author, opt => opt.Ignore())
                .ForMember(dst => dst.Category, opt => opt.Ignore());

            CreateMap<Author, AuthorDTO>().ReverseMap();
            CreateMap<BlogPost, BlogPostDTO>().ReverseMap();
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<BlogPost, CreateBlogPostDTO>().ReverseMap();

            CreateMap<Tags, TagsDTO>();
            CreateMap<CommentDTO, Comment>()
                .ForMember(dst => dst.BlogPost, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
