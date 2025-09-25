using E_commerce.Models.Models;
using E_commerce.Repository.AddressRepository;
using E_commerce.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_commerce_project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AddressManagementController : Controller
    {
        private readonly IAddressRepository _addressRepository;
        public AddressManagementController(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }
        [HttpPost("addAddress")]
        public async Task<Address> StoreAddress(AddressVM addressVM)
        {
            var userid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var address = await _addressRepository.StoreAddress(addressVM, userid);
            return address;
        }
        [HttpPut("updateAddress")]
        public async Task<Address> UpdateAddress(AddressVM addressVM)
        {//which address of the user because there are multiple addresses
            var userid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var address = await _addressRepository.UpdateAddress(addressVM, userid);
            return address;
        }
        [HttpGet("GetAddresses")]
        public async Task<List<Address>> GetAddresses()
        {
            var userid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var address = await _addressRepository.GetUserSavedAddresses(userid);
            return address;
        }
        [HttpDelete("Remove Address")]
        public async Task<bool> RemoveAddress(int addressid)
        {
            var removedaddress = await _addressRepository.RemoveAddress(addressid);
            return removedaddress;
        }


    }
}
