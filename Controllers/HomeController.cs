using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext _applicationDbContext;
        public HomeController(ApplicationDbContext applicationDb)
        {
            _applicationDbContext = applicationDb;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Index()
        {
            var users = from u in _applicationDbContext.Users 
                        join d in _applicationDbContext.DateUsers on u.Id equals d.Id
                        select
                        new IndexUser { Id = u.Id, 
                                        Email = u.Email,
                                        Name = u.UserName,
                                        IsBlocked = u.LockoutEnd != null,
                                        LastLoginDate = d.LastLoginDate,
                                        RegistrationDate = d.RegistrationDate }; 
            return View(users.ToList());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [AllowAnonymous]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
