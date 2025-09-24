using E_commerce.Models.Data;
using E_commerce.Models.Models;
using E_commerce.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Repository.AddressRepository
{
    public class AddressRepository
    {
        private readonly EcommerceContext _context;
        public AddressRepository(EcommerceContext context ) 
        {
            _context = context;
        }

        public async Task<Address> StoreAddress(AddressVM address,int userid)
        {
            Address add = new Address()
            {
                Street = address.Street,
                City = address.City,
                State = address.State,
                Postalcode = address.Postalcode,
                Country = address.Country,
                Isdefault = address.Isdefault,
                Userid = userid,

            };
            await _context.Addresses.AddAsync(add);
            await _context.SaveChangesAsync();
            return add;
        }
        public async Task<Address> UpdateAddress(AddressVM address, int userid)
        {
            var oldaddress= await _context.Addresses.Where(a=> a.Id==address.Id).FirstOrDefaultAsync();
            Address add = new Address()
            {
                Street = address.Street,
                City = address.City,
                State = address.State,
                Postalcode = address.Postalcode,
                Country = address.Country,
                Isdefault = address.Isdefault,
                Userid = userid,

            };
             _context.Addresses.Update(add);
            await _context.SaveChangesAsync();
            return add;

        }
        public async Task<List<Address>> GetUserSavedAddresses(int userid)
        {
            var useraddresses= await _context.Addresses.Where(a=> a.Userid==userid).ToListAsync();
            return useraddresses;
        }

        public async Task<bool> RemoveAddress(int addressid)
        {
            var address= await _context.Addresses.FindAsync(addressid);
            if (address == null) { 
                return false;
            }
             _context.Addresses.Remove(address);
            await _context.SaveChangesAsync();
            return true;
        }


    }
}
