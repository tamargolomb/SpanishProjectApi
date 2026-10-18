using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class CategoriesRepository: ICategoriesRepository
    {
        private readonly SpanishDB _context;
        public CategoriesRepository(SpanishDB context)
        {
            _context = context;
        }


        public void Add(Categories category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
        }
        public List<Categories> GetAll()
        {
            return _context.Categories
             .Include(r => r.CategoryName)
             .Include(r => r.CategoryDescription)
             .ToList();
        }
      
  
    }
}
