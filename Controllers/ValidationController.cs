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
    public class ValidationController : Controller
    {
        UserManager<IdentityUser> _userManager;
        ApplicationDbContext _applicationDbContext;
        SignInManager<IdentityUser> _signInManager;
        public ValidationController(UserManager<IdentityUser> userManager, ApplicationDbContext applicationDb, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _applicationDbContext = applicationDb;
            _signInManager = signInManager;
        }

        [HttpPost]
        public async Task<IActionResult> BlockAsync(ValidationUser[] users)
        {
            foreach(var u in users)
            {
                if (u.IsChecked == "on")
                {
                    var blockedUser = await _userManager.FindByIdAsync(u.Id);
                    blockedUser.LockoutEnd = DateTime.MaxValue;
                    await _userManager.UpdateSecurityStampAsync(blockedUser);
                    await _userManager.UpdateAsync(blockedUser);
                }
            }
            await LogoutIfInArrayAsync(users, HttpContext.User);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> UnblockAsync(ValidationUser[] users)
        {
            foreach(var u in users)
            {
                if (u.IsChecked == "on")
                {
                    var unblockedUser = await _userManager.FindByIdAsync(u.Id);
                    unblockedUser.LockoutEnd = null;
                    await _userManager.UpdateAsync(unblockedUser);
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAsync(ValidationUser[] users)
        {
            foreach(var u in users)
            {
                if (u.IsChecked == "on")
                {
                    DeleteDateUserById(u.Id);
                    var deletedUser = await _userManager.FindByIdAsync(u.Id);
                    await _userManager.DeleteAsync(deletedUser);
                }
            }
            await LogoutIfInArrayAsync(users, HttpContext.User);
            return RedirectToAction("Index", "Home");
        }

        private async Task LogoutIfInArrayAsync(ValidationUser[] users, System.Security.Claims.ClaimsPrincipal claims)
        {
            if (users.FirstOrDefault(u => u.IsChecked == "on" && u.Id == _userManager.GetUserId(claims)) != null)
                await _signInManager.SignOutAsync();
        }

        private void DeleteDateUserById(string id)
        { 
            var dateUser = _applicationDbContext.DateUsers.First(d => d.Id == id);
            _applicationDbContext.DateUsers.Remove(dateUser);
            _applicationDbContext.SaveChanges();
        }
    }
}
