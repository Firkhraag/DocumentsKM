using System;
using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    // Мок репозитория для тестирования
    public class MockUserRepo : IUserRepo
    {
        private readonly List<User> _users;

        public MockUserRepo(IEmployeeRepo employeeRepo)
        {
            // Начальные значения
            _users = new List<User>
            {
                new User
                {
                    Id=0,
                    Login="1",
                    Password=BCrypt.Net.BCrypt.HashPassword("1"),
                    Employee=employeeRepo.GetById(0),
                },
                new User
                {
                    Id=1,
                    Login="2",
                    Password=BCrypt.Net.BCrypt.HashPassword("2"),
                    Employee=employeeRepo.GetById(1),
                },
                new User
                {
                    Id=2,
                    Login="3",
                    Password=BCrypt.Net.BCrypt.HashPassword("3"),
                    Employee=employeeRepo.GetById(2),
                },
            };
        }

        // public IEnumerable<User> GetAll()
        // {
        //     return _users;
        // }

        public User GetById(int id)
        {
            try
            {
                return _users[id];
            }
            catch (ArgumentOutOfRangeException)
            {
                return null;
            }
        }

        public User GetByLogin(string login)
        {
            foreach (User user in _users)
            {
                if (user.Login == login)
                    return user;
            }
            return null;
        }

        public bool SaveChanges()
        {
            return true;
        }
    }
}
