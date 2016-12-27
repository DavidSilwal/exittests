using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using WebApplication.Data;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        public HomeController(
            UserManager<User> userManager, ApplicationDbContext context) : base(userManager, context)
        {
        }

        //[Authorize(Policy = "HasPermission")]
        public IActionResult Index()
        {

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
