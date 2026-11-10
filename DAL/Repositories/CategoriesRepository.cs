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
        public bool Exists(int id)
        {
            return _context.Categories.Any(category => category.CategoryId == id);
        }


        public void Add(Categories category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
        }
        public List<Categories> GetAll()
        {
            return _context.Categories
             .ToList();
        }
      public bool Delete(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null)
                return false;

            _context.Categories.Remove(category);
            _context.SaveChanges();
            return true;
        }
        public Categories? GetOwned(int id) =>
           _context.Categories.FirstOrDefault(r => r.CategoryId == id );

    }
}
