using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using QuestionApp.Models;
using QuestionApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace QuestionApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepository userRepository;

        public AccountController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login(string returnUrl = "")
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

       
        [HttpPost]
        public async Task<IActionResult> Login(User model, string returnUrl)
        {
            var user = userRepository.ValidUser(model.Username, model.Password);
            if (user != null)
            {
                var claimsPrincipal = CreateClaims(user);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
              
                if (returnUrl != null)
                {
                    return Redirect(returnUrl);
                }

                return Redirect("/Questions/List");


            }

            ModelState.AddModelError("Hata", "Kullanıcı adı yanlış");
            ViewBag.ReturnUrl = returnUrl;
            return View();



        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/Account/Login");
        }

        public IActionResult Register(string returnUrl = "")
        {

            ViewBag.ReturnUrl = returnUrl;


            return View();
        }


        [HttpPost]
        public IActionResult Register(User user, string returnUrl = "")
        {
            if (ModelState.IsValid)
            {
                userRepository.AddUser(user); 
                return RedirectToAction("Login", new { returnUrl=returnUrl } );


            }
            ViewBag.ReturnUrl = returnUrl;
            return View(user);

        }
        [HttpPost]
        public IActionResult IsUserExist(string username)
        {
            if (userRepository.IsUserExist(username))
            {
                return Json(true);
            }

            return Json(false);
        }
        private ClaimsPrincipal CreateClaims(User user)
        {
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, user.Username));
            claims.Add(new Claim(ClaimTypes.Role, user.Role.ToString()));
            ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);
            return claimsPrincipal;
        }
    }
}
