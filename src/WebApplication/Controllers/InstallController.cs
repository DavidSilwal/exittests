using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using WebApplication.Data;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class InstallController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly ILogger _logger;

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        
        private ApplicationDbContext _context;

        public InstallController(
            ApplicationDbContext context,
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            ILoggerFactory loggerFactory)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = loggerFactory.CreateLogger<InstallController>();
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(InstallationViewModel model)
        {
            if(ModelState.IsValid)
            {
                var permissions = new List<Permission>()
                {
                    new Permission() { Group = "user_management", Name = "CREATE_USERS", Description = "Create Users" },
                    new Permission() { Group = "user_management", Name = "EDIT_USERS", Description = "Edit Users" },
                    new Permission() { Group = "user_management", Name = "VIEW_USERS", Description = "View Users" },
                    new Permission() { Group = "user_management", Name = "DELETE_USERS", Description = "Delete Users" },

                 
                    new Permission() { Group = "settings", Name = "CREATE_ROLES", Description = "Create Roles" },
                    new Permission() { Group = "settings", Name = "EDIT_ROLES", Description = "Edit Roles" },
                    new Permission() { Group = "settings", Name = "VIEW_ROLES", Description = "View Roles" },
                    new Permission() { Group = "settings", Name = "DELETE_ROLES", Description = "Delete Roles" },
};

                foreach(var permission in permissions)
                {
                    _context.Permissions.Add(permission);
                }

                _context.SaveChanges();

                //var roles = new List<Role>()
                //{
                //    new Role() { Name = "Administrator" }
                //};

                var user = new User
                {
                    UserName = model.UserName,
                
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    var adminRole = new Role() {
                        Name = "Administrator"
                    };

                    result = await _roleManager.CreateAsync(adminRole);

                    if(result.Succeeded)
                    {
                        foreach (var _permission in _context.Permissions)
                        {
                            _context.RolePermissions.Add(new RolePermission()
                            {
                                RoleId = adminRole.Id,
                                PermissionId = _permission.Id
                            });
                        }

                        _context.SaveChanges();

                        await _userManager.AddToRoleAsync(user, adminRole.Name);
                    }

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    AddErrors(result);
                }
            }

            return View(model);
        }
    }
}
