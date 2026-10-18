using DAL;
using DTO;
using DTO.Categories;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Services
{
    public interface ICategoryService
    {
        void Add(Categories category);
        List<ShowCategoriesDTO> GetAllCategories();
    }
}
