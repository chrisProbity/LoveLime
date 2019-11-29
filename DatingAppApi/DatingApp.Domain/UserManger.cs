using System.Threading.Tasks;
using System;
using DatingApp.Core.Interfaces;
using DatingApp.Data;
using DatingApp.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DatingApp.Domain
{
    public class UserManager : IUserManager
    {
        private readonly DataContext _context;

        public UserManager(DataContext context)
        {
            _context = context;
            
        }
        public async Task<User> Login(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
            if (user != null)
            {
                if (!VerifyPassword(password, user.PasswordHash, user.PasswordSalt))
                {
                    return null;
                }
            }
            return user;
        }

        private bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; ++i)
                {
                    if (computedHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public async Task<User> Register(User user, string password)
        {
            //try
            //{
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;

                await _context.Users.AddAsync(user);
                var save = await _context.SaveChangesAsync();

                if (save > 0)
                {
                    return user;
                }
                return null;
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(ex.Message);
            //}

        }

        public void CreatePasswordHash(string password, out byte[] PasswordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<bool> UserExists(string username)
        {
            var user = await _context.Users.AnyAsync(x => x.Username == username);
            if (user)
            {
                return true;
            }
            return false;
        }

        public async void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async void DeleteUser(int userId)
        {
            var userToDelete = await _context.Users.FirstAsync(x => x.ID == userId);
            if(userToDelete != null)
            {
                _context.Remove(userToDelete);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            var users = await _context.Users.Include(p => p.Photos).ToListAsync();
           if (users != null)
           {
                return users;
           }
            return null;
        }

        public Task<User> GetUser(int id)
        {
            var user = _context.Users.Include(p=>p.Photos).FirstOrDefaultAsync(x => x.ID == id);
            if(user == null)
            {
                return null;
            }
            return user;
        }

        public async Task<bool> SaveChanges()
        {
           var saveChanges = await _context.SaveChangesAsync();
            if(saveChanges > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<Photo> GetPhoto(int id)
        {
            var photo = await _context.Photos.FirstOrDefaultAsync(x => x.ID == id);
            if(photo != null)
            {
                return photo;
            }
            return null;
        }

        public async Task<Photo> GetMainPhoto(int userId)
        {
            var photo = await _context.Photos.Where(x => x.UserId == userId).FirstOrDefaultAsync(p => p.IsMain);
            if(photo != null)
            {
                return photo;
            }
            return null;
        }
    }
}