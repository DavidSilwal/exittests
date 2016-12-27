using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication.Controllers
{
    [Authorize()]
    public class ProfileController : Controller
    {        
        public IActionResult Index()
        {
            return View();
            }   
        
    }
}
