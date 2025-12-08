using BCrypt.Net;
using E_commerce.Models;
using E_commerce.Models.Data;
using E_commerce.Models.Models;
using E_commerce.Services;
using E_commerce.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace E_commerce.Repository.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly EcommerceContext _context;
        private readonly TokenService _tokenservice;
        public UserRepository(EcommerceContext context, TokenService tokenservice)
        {
            _context = context;
            _tokenservice = tokenservice;
        }

        public async Task<bool> Signup(UserSignInVM userdetails ,string role)
        {
            var existing = await _context.Usermanagements.Where(s => s.Email == userdetails.Email).FirstOrDefaultAsync();
            if (existing != null || userdetails==null)
            {
                return false;
            }
            Usermanagement user = new Usermanagement()
            {

                Email = userdetails.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(userdetails.Password),
                Role = role,
                Name = userdetails.Name,

            };
            _context.Usermanagements.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<string> Login(UserVM user)
        {
            var existingUser = await _context.Usermanagements.Where(s => s.Email == user.Email).FirstOrDefaultAsync();
            if (existingUser == null || !BCrypt.Net.BCrypt.Verify(user.Password, existingUser.Password))
            {
                return null;
            }
            var token = _tokenservice.GenerateToken(existingUser);
            existingUser.Usertoken = token;
            existingUser.Isexpired = false;
            await _context.SaveChangesAsync();
            return token;

        }

        public async Task<string> Logout(string token)
        {
            var usertoken = await _context.Usermanagements.Where(t => t.Usertoken == token && !t.Isexpired).FirstOrDefaultAsync();

            if (usertoken == null)
            {
                return null;
            }
            usertoken.Isexpired = true;
            //_context.Usermanagements.Update(usertoken);
            await _context.SaveChangesAsync();
            return usertoken.Usertoken;
        }
        public async Task<PayloadVM> GetUserProfile(string email, string token)
        {
            var userdetails = await _context.Usermanagements.Where(u => u.Email == email).FirstOrDefaultAsync();
            if (userdetails != null)
            {
                PayloadVM user = new PayloadVM
                {
                    Name = userdetails.Name,
                    Email = userdetails.Email,
                    Role = userdetails.Role,
                };
                return user;

            }
            return null;
        }
        public async Task<Usermanagement> UpdateUserProfile(UserUpdateVM updateduser)
        {

            var userdetails = await _context.Usermanagements.Where(u => u.Email == updateduser.Email).FirstOrDefaultAsync();
            Usermanagement user = new Usermanagement()
            {

                Email = updateduser.Email ?? userdetails.Email,
                Role = updateduser.Role?? userdetails.Role,
                Name = updateduser.Name?? userdetails.Name

            };
            _context.Usermanagements.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task<bool> ForgotPassword(ForgotPasswordVM forgetpassword)
        {
            var user =  _context.Usermanagements.Where(u => u.Email == forgetpassword.Email).FirstOrDefault();

            if (user == null)
            {
                return false;
            }
            var resetToken = Guid.NewGuid().ToString();
            user.Passwordresettoken = resetToken;
            user.Resettokenexpiry = DateTime.UtcNow.AddMinutes(15);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> ResetPassword(ResetPasswordVM resetpass)
        {
            var user = _context.Usermanagements.Where(w=>w.Email==resetpass.Email).FirstOrDefault();
            if (user == null ||(user.Passwordresettoken==null || user.Resettokenexpiry<DateTime.UtcNow)) 
            { 
            return false;
            }
            var Newpassword = BCrypt.Net.BCrypt.HashPassword(resetpass.NewPassword);
            user.Password = Newpassword;
            user.Passwordresettoken = null;
            user.Resettokenexpiry = null;

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<List<PayloadVM>> GetAllUsers()
        {
            var userdetails = await _context.Usermanagements.ToListAsync();
            if (userdetails != null)
            {

                List<PayloadVM> user = userdetails.Select(u=> new PayloadVM
                {
                    Name = u.Name,
                    Email = u.Email,
                    Role = u.Role
                }).ToList();
                
                return user;

            }
            return new List<PayloadVM>();
        }

        public async Task<List<PayloadVM>> GetUserById()
        {
            var userdetails = await _context.Usermanagements.ToListAsync();
            if (userdetails != null)
            {

                List<PayloadVM> user = userdetails.Select(u => new PayloadVM
                {
                    Name = u.Name,
                    Email = u.Email,
                    Role = u.Role
                }).ToList();

                return user;

            }
            return new List<PayloadVM>();
        }
        public async Task<PayloadVM> GetUserDetails(string email)
        {
            var userdetails = await _context.Usermanagements.Where(u => u.Email == email).FirstOrDefaultAsync();
            if (userdetails != null)
            {
                PayloadVM user = new PayloadVM
                {
                    Name = userdetails.Name,
                    Email = userdetails.Email,
                    Role = userdetails.Role,
                };
                return user;

            }
            return null;
        }

    }
} 
