using DAL;
using DTO;
using DTO.Categories;
using DTO.Lesson;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Services
{
    public interface ICategoryService
    {
        ShowCategoriesDTO AddCategory(AddCategoriesDTO dto);
        List<ShowCategoriesDTO> GetAllCategories();
        bool DeleteCategory(int Categoryid);
    }
}
