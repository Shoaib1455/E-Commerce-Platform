using E_commerce.Models;
using E_commerce.Models.Models;
using E_commerce.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Repository.UserRepository
{
    public interface IUserRepository
    {
        public Task<bool> Signup(UserSignInVM userdetails,string role);
        public Task<string> Login(UserVM user);
        public Task<string> Logout(string token);
        public Task<PayloadVM> GetUserProfile(string email, string token);
        public Task<List<PayloadVM>> GetAllUsers();
        public Task<Usermanagement> UpdateUserProfile(UserUpdateVM updateduser);
        public Task<bool> ForgotPassword(ForgotPasswordVM forgetpassword);
        public Task<bool> ResetPassword(ResetPasswordVM resetpass);
        public Task<PayloadVM> GetUserDetails(string email);
    }
}
