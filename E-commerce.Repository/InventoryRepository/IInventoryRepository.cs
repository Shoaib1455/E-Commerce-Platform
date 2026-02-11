using E_commerce.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Repository.InventoryRepository
{
    public interface IInventoryRepository
    {
        
        public Task<Inventory> AddInitialInventoryAsync(int productId, int quantity, int sellerId);
        public Task<Inventory> AddStockAsync(int inventoryId, int quantity, int sellerId, string referenceType, int referenceId);
        public Task<Inventory> ReduceStockAsync(int productId, int quantity, int sellerId, string referenceType, int referenceId);
        public Task ReserveStockAsync(int productId, int quantity, int userId, int orderId);
        public Task ReleaseReservedStockAsync(int productId, int quantity, int userId, int orderId);
        public Task<Inventory> GetInventoryByProductIdAsync(int productId);
        public Task<List<Inventory>> GetAllInventoriesAsync();
        public Task<List<Inventorytransaction>> GetTransactionsByProductIdAsync(int productId);
        public Task<bool> DeleteInventoryAsync(int inventoryId);

            //Task AddInventoryTransactionAsync(Inventorytransaction transaction);
            //Task<Inventory?> GetInventoryByProductIdAsync(int productId);
            //Task UpdateInventoryAsync(Inventory inventory);

    }
}
