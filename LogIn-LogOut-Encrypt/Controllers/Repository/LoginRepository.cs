using LogIn_LogOut_Encrypt.Models;
using LogIn_LogOut_Encrypt.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogIn_LogOut_Encrypt.Controllers.Repository
{
    public class LoginRepository:ILoginRepository
    {
        private DataContext db;
        public LoginRepository(DataContext _db)
        {
            db = _db; 
        }

        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            var hmac = new System.Security.Cryptography.HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }

        public void CreateUser(User user)
        {
            db.Users.Add(user);
            db.SaveChanges();
        }

        public User GetUser(string userName)
        {
            return db.Users.FirstOrDefault(x => x.UserName == userName);
        }

        public bool UserExists(string userName)
        {
            if (db.Users.Any(x => x.UserName == userName))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt);
            var computehash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            for (int i = 0; i < computehash.Length; i++)
            {
                if (computehash[i] != passwordHash[i])
                {
                    return false;
                }
               
            }
            return true;
        }
    }
}
