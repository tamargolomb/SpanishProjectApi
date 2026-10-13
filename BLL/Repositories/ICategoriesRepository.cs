using System;
using System.Collections.Generic;
using System.Text;
using DAL;
using DTO;
using Microsoft.EntityFrameworkCore;

namespace BLL.Repositories
{
    public interface ICategoriesRepository
    {
        void Add(Categories category);            
        List<Categories> GetAll();               
        Categories GetById(int id);                
        void Update(Categories category);         
        void Delete(int id);
    }
}
