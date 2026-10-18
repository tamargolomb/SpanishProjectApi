using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class LevelsRepository: ILevelsRepository
    {
        private readonly SpanishDB _context;

        public LevelsRepository(SpanishDB context)
        {
            _context = context;
        }
        public void Add(Levels Level)
        {
            _context.Levels.Add(Level);
            _context.SaveChanges();
        }
        public List<Levels> GetAll()
        {
            return _context.Levels
        .Include(r => r.LevelName)
        .Include(r => r.LevelDescription)
        .ToList();
        }
        public Levels GetById(int id)
        {
            return _context.Levels.Find(id);
        }
        public void Update(Levels Level)
        {
            _context.Levels.Update(Level);
            _context.SaveChanges();
        }
       
    }
}
