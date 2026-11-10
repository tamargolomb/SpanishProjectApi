using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public interface ILessonsRepository
    {
        void Add(Lesson Lesson);
        List<Lesson> GetAll();
        Lesson? GetById(int id);
        void Update(Lesson Lesson);
        bool Delete(int id);
        Lesson? GetOwned(int id, int userId);
        List<Lesson> GetByUser(int userId);

    }
}
