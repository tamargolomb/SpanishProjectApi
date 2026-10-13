using System;
using System.Collections.Generic;
using System.Text;
using DAL;
using DTO;
using Microsoft.EntityFrameworkCore;

namespace BLL.Repositories
{
    public interface ILevelsRepository
    {
        void Add(Levels Level);
        List<Levels> GetAll();
        Levels GetById(int id);
        void Update(Levels Level);
        void Delete(int id);
    }
}
