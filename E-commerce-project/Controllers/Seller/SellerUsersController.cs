using E_commerce.Repository.UserRepository;
using E_commerce.ViewModels;
using E_commerce.Repository.UserRepository;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce_project.Controllers.Seller
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class SellerUsersController : Controller
    {
        private readonly IUserRepository _userRepository;
        public SellerUsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser(UserSignInVM userdetails)
        {
            if (!string.IsNullOrEmpty(userdetails.Email) && !string.IsNullOrEmpty(userdetails.Password))
            {
                var result = await _userRepository.Signup(userdetails, "seller");
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
    }
}
