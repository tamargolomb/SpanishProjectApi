using DAL;
using DAL.Repositories;
using DTO;
using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using DTO.User;

namespace BLL.Services
{
    public class UserService: IUserService
    {
        private readonly IUsersRepository _UsersRepository;
        private readonly IMapper _mapper;
        public UserService(IUsersRepository userRepository, IMapper mapper)
        {
            _UsersRepository = userRepository;
            _mapper = mapper;
        }

        public Users? GetById(int id)
        {
            return _UsersRepository.GetById(id);
        }

        public void Register(Users user)
        {
            _UsersRepository.Add(user);
        }
        public List<ShowUserDTO> GetAllUsers()
        {
            var users = _UsersRepository.GetAll();
            return _mapper.Map<List<ShowUserDTO>>(users);
        }
        public void Update(Users user)
        {
            _UsersRepository.Update(user);
        }
    }
}
