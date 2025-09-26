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
    public class AddressRepository : IAddressRepository
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
            var oldaddress= await _context.Addresses.Where(a=> a.Id==address.Id && a.Userid==userid).FirstOrDefaultAsync();
            if (oldaddress != null)
            {
                oldaddress.Street = (address.Street != oldaddress.Street) ? address.Street : oldaddress.Street;
                oldaddress.City = (address.City != oldaddress.City) ? address.City : oldaddress.City;
                oldaddress.State = (address.State != oldaddress.State) ? address.State : oldaddress.State;
                oldaddress.Postalcode = (address.Postalcode != oldaddress.Postalcode) ? address.Postalcode : oldaddress.Postalcode;
                oldaddress.Country = (address.Country != oldaddress.Country) ? address.Country : oldaddress.Country;


                _context.Addresses.Update(oldaddress);
                await _context.SaveChangesAsync();
                return oldaddress;
            }
            return new Address();

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
