using LogIn_LogOut_Encrypt.Controllers.Repository;
using LogIn_LogOut_Encrypt.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogIn_LogOut_Encrypt.Controllers
{
    public class AccountController : Controller
    {
        private ILoginRepository rep;
        public AccountController(ILoginRepository _rep)
        {
            rep=_rep;
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("id").HasValue)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public IActionResult SignUp()
        {

            return View();
        }
        public IActionResult Register(User user,string password)
        {
            byte[] passwordHash, passwordSalt;
            rep.CreatePasswordHash(password, out passwordHash, out passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordHash = passwordSalt;
            //bu usernaem ait değe yoksa yap
            if (!rep.UserExists(user.UserName))
            {
                rep.CreateUser(user);

            }
            else
            {
                return View("Hata");
            }

            return RedirectToAction("Index");

        }
        public IActionResult Login(User u,string password)
        {
            User user = rep.GetUser(u.UserName);

           if(user!=null && rep.VerifyPasswordHash(password,user.PasswordHash,user.PasswordSalt))
               
            {
                //kullanıc ve sıfre esıtse
                HttpContext.Session.SetInt32("id", user.Id);
                HttpContext.Session.SetString("fullname", user.Name + " " + user.SurName);
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index");
        }

        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");

        }
    }
}
