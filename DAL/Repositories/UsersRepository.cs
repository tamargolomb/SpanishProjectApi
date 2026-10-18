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
              .Include(r => r.UserId)
              .Include(r => r.UserName)
              .Include(r => r.UserEmail)
              .Include(r => r.LevelId)
              .Include(r => r.JoinDate)
              .Include(r => r.Score)
              .ToList();
        }
        public Users? GetById(int id)
        {
            return _context.Users
                .Include(r => r.UserId)
                .Include(r => r.UserName)
                .Include(r => r.UserEmail)
                .Include(r => r.LevelId)
                .Include(r => r.JoinDate)
                .Include(r => r.Score)
           .FirstOrDefault(r => r.UserId == id);
        }
        public void Update(Users User)
        {
            _context.Users.Update(User);
            _context.SaveChanges();
        }
       
    }
}
