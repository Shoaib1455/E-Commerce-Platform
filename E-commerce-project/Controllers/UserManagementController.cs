using E_commerce.Repository.UserRepository;
using E_commerce.ViewModels;
using E_commerce_project.Common;
using E_commerce_project.Middleware;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Security.Claims;

namespace E_commerce_project.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserManagementController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserManagementController(IUserRepository userRepository )
        {
            _userRepository = userRepository;
            
        }

        [HttpPost("Register")]
        public async Task<IActionResult> CreateUser(UserSignInVM userdetails)
        {
           var result = await _userRepository.Signup(userdetails);
            if (!result) 
            {
                
                return BadRequest("User Already Exist"); 
            }

            return Ok("Registration Successful");
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserVM user) { 
            var token= await _userRepository.Login(user);
            if (token == null) {
                return BadRequest("Invalid Credentials");
            }
        
            return Ok(new { token });
        }
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Token is missing");
            }
            string validtoken=await _userRepository.Logout(token);
            return Ok("Logout Successful");
        }
        [HttpGet]
        public async Task<PayloadVM> GetsUserProfile()
        {
            //PayloadVM payload = new PayloadVM() { Email=null,Name=null,Role=null};
            //if (!TokenValidator.isValidToken(Request))
            //{
            //    return payload;
            //}
            var token = HttpContext.Request.Headers["Authorization"]
                  .FirstOrDefault()?.Split(" ").Last();
            PayloadVM payload = new PayloadVM();
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            payload=await _userRepository.GetUserProfile(email,token);

            return payload;
        }
        [HttpPut("UpdateUser")]
        public async Task<UserUpdateVM> UpdateUserProfile(UserUpdateVM updatedUser)
        {
            //var email = User.FindFirst(ClaimTypes.Email)?.Value;
            //var name = User.FindFirst(ClaimTypes.Name)?.Value;
            //var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var userUpdated =await _userRepository.UpdateUserProfile(updatedUser);
            return updatedUser;
        }
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM forgotpass)
        {
            var user=await _userRepository.ForgotPassword(forgotpass);
            if (!user)
            {
                return BadRequest("Email Doesn't Exist");
            }
            return Ok("token generated");
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM resetPassword)
        {
            if (resetPassword == null) {
                return BadRequest("Provide details that you want to update");
            }
            var isupdated= await _userRepository.ResetPassword(resetPassword);
            if (!isupdated) 
            { 
                return BadRequest("Provide correct details");
            }
                return Ok("Password Updated");
        }
        private async Task<IActionResult> OK(string v)
        {
            throw new NotImplementedException();
        }
    }
}
