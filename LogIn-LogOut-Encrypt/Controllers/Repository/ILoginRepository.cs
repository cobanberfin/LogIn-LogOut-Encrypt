using LogIn_LogOut_Encrypt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogIn_LogOut_Encrypt.Controllers.Repository
{
   public interface ILoginRepository
    {

        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);

        void CreateUser(User user);
        bool UserExists(string userName);
        User GetUser(string userName);

        bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
    }
}
