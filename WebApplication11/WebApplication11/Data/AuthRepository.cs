using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShoppingApplication.API.Model;

namespace ShoppingApplication.API.Data
{

    //responsible for quering the database
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;

        public AuthRepository(DataContext context)
        {
            this._context = context;
        }

        public async Task<User> Login(string username, string password)
        {
            var user = await _context.Users.Include(p => p.Photos)
            .FirstOrDefaultAsync(x => x.Username == username);

            if (user == null)
                return null;

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;
            return user;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i=0;i<computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                        return false;

                }
            }
            return true;
        }


        //Creating Password hash
        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password,out passwordHash,out passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
           // user = seedValues(user);
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        //private User seedValues(User user)
        //{
        //    string date = "1-11-1995";
        //    DateTime datetostore = DateTime.Parse(date);
        //    user.City = "pindi";
        //    user.Country = "Pakistan";
        //    user.DateofBirth = datetostore;
        //    user.Gender = "male";
        //    user.KnownAs = user.Username;
        //    user.Introduction = "hello I am " + user.Username;
        //    user.LookingFor = "Hi, why I am here ? ";
        //    user.Interest = "My interest is to play batminaton";
        //    user.LastActive = DateTime.Today;
        //    user.Created = DateTime.Today;
        //    return user;
        //}

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<bool> UserExists(string username)
        {
            if (await _context.Users.AnyAsync(x => x.Username == username))
                return true;
            return false;
        }
    }
}
