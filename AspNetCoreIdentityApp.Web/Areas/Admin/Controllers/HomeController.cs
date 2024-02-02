using AspNetCoreIdentityApp.Web.Areas.Admin.Models;
using AspNetCoreIdentityApp.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreIdentityApp.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public HomeController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> UserList()
        {
           var UserList= await _userManager.Users.ToListAsync();

            var UserViewModelList = UserList.Select(x => new UserViewModel()
            {
                Id = x.Id,
                UserName=x.UserName,
                Email=x.Email

            }).ToList();

            return View(UserViewModelList);
        }
    }
}
