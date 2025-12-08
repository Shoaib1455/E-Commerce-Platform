using E_commerce.Models.Models;
using E_commerce.Repository.UserRepository;
using E_commerce.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce_project.Controllers.Admin
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AdminUsersController : Controller
    {
        private readonly IUserRepository _userRepository;
        public AdminUsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser(UserSignInVM userdetails)
        {
            if (!string.IsNullOrEmpty(userdetails.Email) && !string.IsNullOrEmpty(userdetails.Password))
            {
                var result = await _userRepository.Signup(userdetails, "admin");
                if (!result)
                {

                    return BadRequest("User Already Exist");
                }
            }
            else
            {
                return BadRequest("User details required");
            }

                return Ok(new { message = "Registration Successful" });
        }
        [HttpGet]
        public async Task<List<PayloadVM>> GetallUsers()
        {
            var users= await _userRepository.GetAllUsers();
            return users;
        }
        [HttpGet]
        public async Task<PayloadVM> GetUserdetails(string email)
        {
            var user = await _userRepository.GetUserDetails( email);
            return user;
        }
        [HttpGet]
        public async Task<Usermanagement> UpdateUserdetails(UserUpdateVM userupdate)
        {
            var user = await _userRepository.UpdateUserProfile(userupdate);
            return user;
        }

    }
}
