using E_commerce.Models.Models;
using E_commerce.Repository.UserRepository;
using E_commerce.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce_project.Controllers.Admin
{
    public class AdminUsersController : Controller
    {
        private readonly IUserRepository _userRepository;
        public AdminUsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<List<PayloadVM>> GetallUsers()
        {
            var users= await _userRepository.GetAllUsers();
            return users;
        }

        public async Task<PayloadVM> GetUserdetails(string email)
        {
            var user = await _userRepository.GetUserDetails( email);
            return user;
        }
        public async Task<Usermanagement> UpdateUserdetails(UserUpdateVM userupdate)
        {
            var user = await _userRepository.UpdateUserProfile(userupdate);
            return user;
        }

    }
}
