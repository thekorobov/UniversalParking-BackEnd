using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace UniversalParking.DAL.EF
{
    public class DbUserInitializer
    {
        public static async Task RoleInitializeAsync(
            RoleManager<IdentityRole<int>> roleManager)
        {
            if (await roleManager.FindByNameAsync("Administrator") == null)
            {
                await roleManager.CreateAsync(new IdentityRole<int>("Administrator"));
            }
            if (await roleManager.FindByNameAsync("Driver") == null)
            {
                await roleManager.CreateAsync(new IdentityRole<int>("Driver"));
            }
            if (await roleManager.FindByNameAsync("Owner") == null)
            {
                await roleManager.CreateAsync(new IdentityRole<int>("Owner"));
            }
        }
    }
}
