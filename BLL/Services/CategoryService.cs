using AutoMapper;
using DAL;
using DAL.Repositories;
using DTO;
using DTO.Categories;
using DTO.Lesson;
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
        public ShowCategoriesDTO AddCategory(AddCategoriesDTO dto)
        {
            var category = _mapper.Map<Categories>(dto);
            _CategoriesRepository.Add(category);
            return _mapper.Map<ShowCategoriesDTO>(category);
        }
        public List<ShowCategoriesDTO> GetAllCategories()
        {
            var categories = _CategoriesRepository.GetAll();
            return _mapper.Map<List<ShowCategoriesDTO>>(categories);
        }
        public bool DeleteCategory(int Categoryid)
        {
            var category = _CategoriesRepository.GetOwned(Categoryid);
            if (category is null)
                return false;

            _CategoriesRepository.Delete(Categoryid);
            return true;
        }
    }
}
