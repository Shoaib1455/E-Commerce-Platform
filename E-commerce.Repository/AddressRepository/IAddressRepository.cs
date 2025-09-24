using E_commerce.Models.Models;
using E_commerce.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Repository.AddressRepository
{
    public interface IAddressRepository
    {
        public  Task<Address> StoreAddress(AddressVM address, int userid);
        public  Task<Address> UpdateAddress(AddressVM address, int userid);
        public  Task<List<Address>> GetUserSavedAddresses(int userid);
        public  Task<bool> RemoveAddress(int addressid);
    }
}
