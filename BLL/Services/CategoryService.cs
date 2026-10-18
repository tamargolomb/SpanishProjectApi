using AutoMapper;
using DAL;
using DAL.Repositories;
using DTO;
using DTO.Categories;
using DTO.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoriesRepository _CategoriesRepository;
        private readonly IMapper _mapper;
        public CategoryService(ICategoriesRepository categoriesRepository, IMapper mapper)
        {
            _CategoriesRepository = categoriesRepository;
            _mapper = mapper;
        }
        public void Add(Categories category)
        {
            _CategoriesRepository.Add(category);
        }
        public List<ShowCategoriesDTO> GetAllCategories()
        {
            var categories = _CategoriesRepository.GetAll();
            return _mapper.Map<List<ShowCategoriesDTO>>(categories);
        }
    }
}
