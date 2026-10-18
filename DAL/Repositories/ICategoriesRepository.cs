using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;


namespace DAL.Repositories
{
    public interface ICategoriesRepository
    {
        void Add(Categories category);            
        List<Categories> GetAll();               

    }
}
