using DatingApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.Core.Interfaces
{
    public interface IUserManager
    {
        Task<User> Register(User user, string password);
        Task<User> Login(string username, string password);
        Task<bool> UserExists(string username);
        void Add<T>(T entity) where T : class;
        void DeleteUser(int userId);
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUser(int id);
        Task<bool> SaveChanges();
        Task<Photo> GetPhoto(int id);
        Task<Photo> GetMainPhoto(int userId);

    }
}
