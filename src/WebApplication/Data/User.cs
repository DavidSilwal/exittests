using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace WebApplication.Data
{
    public class User : IdentityUser<int>
    {
        public User() : base()
        {
       
            UserPermissions = new HashSet<UserPermission>();
        
        }
        
        public virtual ICollection<UserPermission> UserPermissions { get; set; }

    }
}
