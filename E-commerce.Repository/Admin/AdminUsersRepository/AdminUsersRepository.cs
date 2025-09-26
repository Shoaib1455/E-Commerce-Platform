using E_commerce.Models.Data;
using E_commerce.ViewModels;
using E_commerce.ViewModels.Admin;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Repository.Admin.AdminUsersRepository
{
    public class AdminUsersRepository
    {
       private readonly EcommerceContext _context;
        public AdminUsersRepository() {
        }
        public async Task <List<UserDetailsVM>> ListAllUsers(string role)
        {
            var users = await _context.Usermanagements.Where(u=> u.Role==role).ToListAsync();
            var userdetails = users.Select(u => new UserDetailsVM
            {
                Name=u.Name,
                Email=u.Email,
                Role=u.Role,
            }).ToList();

           
            return userdetails;
        }
    }
}
