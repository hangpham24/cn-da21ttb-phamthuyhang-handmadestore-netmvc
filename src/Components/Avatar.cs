using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebHM.Data;
using WebHM.Models;

namespace WebHM.Components
{
    public class Avatar : ViewComponent
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
            public Avatar(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            string userId = _userManager.GetUserId((ClaimsPrincipal)User);
            var user = _context.Users.Find(userId);
            return View(user);
        }
    }
}
