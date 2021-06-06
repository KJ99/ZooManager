using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZooManager.Data.Contexts;

namespace ZooManager.Data.Seeders
{
    public class UsersDataSeeder
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AuthDbContext _dbContext;
        public UsersDataSeeder(IApplicationBuilder app)
        {
            IServiceProvider serviceProvider = app.ApplicationServices.CreateScope().ServiceProvider;
            DbContextOptions<AuthDbContext> dbOptions = serviceProvider.GetRequiredService<DbContextOptions<AuthDbContext>>();
            
            _dbContext = new AuthDbContext(dbOptions);
            _userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            _roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        }

        public async void Seed()
        {
            bool roleSeeded = true;

            if(!_dbContext.Roles.Any())
            {
                roleSeeded = await SeedRoles();
            }
            if(roleSeeded && !_dbContext.Users.Any())
            {
                await SeedUsers();
            }
        }

        private async Task<bool> SeedRoles()
        {
            IdentityRole apiAdmin = new IdentityRole();
            apiAdmin.Name = "api.admin";
            apiAdmin.NormalizedName = "API.ADMIN";
            await _roleManager.CreateAsync(apiAdmin);

            IdentityRole zooManager = new IdentityRole();
            zooManager.Name = "zoo.manager";
            zooManager.NormalizedName = "ZOO.MANAGER";
            await _roleManager.CreateAsync(zooManager);

            IdentityRole zooEmployee = new IdentityRole();
            zooEmployee.Name = "zoo.employee";
            zooEmployee.NormalizedName = "ZOO.EMPLOYEE";
            await _roleManager.CreateAsync(zooEmployee);

            return true;

        }

        private async Task<bool> SeedUsers()
        {
            IdentityUser admin = new IdentityUser();
            admin.Email = "admin@zoo.pl";
            admin.UserName = "admion";
            admin.NormalizedEmail = "ADMIN@ZOO.PL";
            admin.NormalizedUserName = "ADMION";
            admin.PasswordHash = _userManager.PasswordHasher.HashPassword(admin, "zaq1@WSX");

            await _userManager.CreateAsync(admin);
            await _userManager.AddToRoleAsync(admin, "api.admin");

            return true;
        }
    }
}
