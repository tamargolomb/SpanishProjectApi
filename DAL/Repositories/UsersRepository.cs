using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class UsersRepository: IUsersRepository
    {
        private readonly SpanishDB _context;

        public UsersRepository(SpanishDB context)
        {
            _context = context;
        }
        public void Add(Users User)
        {
            _context.Users.Add(User);
            _context.SaveChanges();
        }
        public List<Users> GetAll()
        {
            return _context.Users
              .Include(r => r.LevelId)
              .ToList();
        }

        public Users? GetByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.UserEmail == email);
        }

        public Users? GetById(int id)
        {
            return _context.Users.Find(id);
        }

        public bool EmailExists(string email)
        {
            return _context.Users.Any(u => u.UserEmail == email);
        }
        public void Update(Users User)
        {
            _context.Users.Update(User);
            _context.SaveChanges();
        }
       
    }
}
