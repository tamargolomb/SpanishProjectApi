using AutoMapper;
using DAL;
using DTO;
using DTO.Categories;
using DTO.General;
using DTO.Lesson;
using DTO.Levels;
using DTO.ReviewsAndRatings;
using DTO.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Mapper
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            //Categories
            CreateMap<Categories,AddCategoriesDTO>().ReverseMap();
            CreateMap<Categories,ShowCategoriesDTO>().ReverseMap();

            //Lesson
            CreateMap<Lesson,AddViewsDTO>().ReverseMap();
            CreateMap<Lesson,AddLessonDTO>().ReverseMap();
            CreateMap<Lesson,ShowLessonDTO>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
            .ForMember(dest => dest.LevelName, opt => opt.MapFrom(src => src.Level.LevelName))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.user.UserName))
            .ReverseMap();

            //Levels
            CreateMap<Levels, AddLevelDTO>().ReverseMap();
            CreateMap<Levels,ShowLevelDTO>().ReverseMap();
            CreateMap<Levels, UpdateLevelDTO>().ReverseMap();

            //ReviewsAndRatings
            CreateMap<ReviewsAndRatings, AddResponseDTO>().ReverseMap();
            CreateMap<ReviewsAndRatings,ShowReviewsAndRatingsDTO>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName)) ;
            CreateMap<ReviewsAndRatings, UpdateRatingDTO>()
                .ReverseMap();

            //User
            CreateMap<Users,AddUserDTO>().ReverseMap();
            CreateMap<Users,LoginUserDTO>().ReverseMap();
            CreateMap<Users,ShowUserDTO>()
                .ForMember(dest => dest.LevelName, opt => opt.MapFrom(src => src.Level.LevelName));
            CreateMap<Users,UpdateUserDTO>()
                .ReverseMap();

            //General
            CreateMap<LessonSearchDTO, Lesson>().ReverseMap();

        }
    }
}
