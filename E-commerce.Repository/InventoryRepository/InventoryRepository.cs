using E_commerce.Models.Data;
using E_commerce.Models.Enums;
using E_commerce.Models.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static E_commerce.Repository.InventoryRepository.IInventoryRepository;

namespace E_commerce.Repository.InventoryRepository
{
   
    public class InventoryRepository:IInventoryRepository
    {
        private readonly EcommerceContext _context;
        public InventoryRepository(EcommerceContext context)
        {
            _context = context;
        }

        public async Task<Inventory> AddInitialInventoryAsync(int productId, int quantity, int sellerId)
        {
            if (quantity <= 0) return null;

            // Add inventory
            var inventoryRecord = new Inventory
            {
                Productid = productId,
                Quantityinstock = quantity,
                Lastupdatedat = DateTime.UtcNow
            };
            _context.Inventories.Add(inventoryRecord);
            await _context.SaveChangesAsync();

            // Record transaction
            var transaction = new Inventorytransaction
            {
                Inventoryid = inventoryRecord.Id,
                Productid = productId,
                Transactiontype = InventoryTransactionType.In.ToString(),
                Quantity = quantity,
                Beforequantity = 0,
                Afterquantity = quantity,
                Referencetype = "ProductCreation",
                Referenceid = productId,
                Createdby = sellerId,
                Createdat = DateTime.UtcNow
            };
            _context.Inventorytransactions.Add(transaction);
            await _context.SaveChangesAsync();

            return inventoryRecord;
        }

        // ================================
        // 2️⃣ Increase stock (e.g., new shipment)
        // ================================
        public async Task<Inventory> AddStockAsync(int inventoryId, int quantity, int sellerId, string referenceType, int referenceId)
        {
            var inventoryRecord = await _context.Inventories.FirstOrDefaultAsync(i => i.Id == inventoryId);
            if (inventoryRecord == null) return null;

            int beforeQty = inventoryRecord.Quantityinstock;
            inventoryRecord.Quantityinstock += quantity;
            inventoryRecord.Lastupdatedat = DateTime.UtcNow;
            _context.Inventories.Update(inventoryRecord);
            await _context.SaveChangesAsync();

            var transaction = new Inventorytransaction
            {
                Inventoryid = inventoryId,
                Productid = inventoryRecord.Productid,
                Transactiontype = InventoryTransactionType.In.ToString(),
                Quantity = quantity,
                Beforequantity = beforeQty,
                Afterquantity = inventoryRecord.Quantityinstock,
                Referencetype = referenceType,
                Referenceid = referenceId,
                Createdby = sellerId,
                Createdat = DateTime.UtcNow
            };
            _context.Inventorytransactions.Add(transaction);
            await _context.SaveChangesAsync();

            return inventoryRecord;
        }

        // ================================
        // 3️⃣ Decrease stock (e.g., order placed)
        // ================================
        public async Task<Inventory> ReduceStockAsync(int productId, int quantity, int sellerId, string referenceType, int referenceId)
        {
            var inventoryRecord = await _context.Inventories.FirstOrDefaultAsync(i => i.Id == productId);
            if (inventoryRecord == null || inventoryRecord.Quantityinstock < quantity)
                throw new InvalidOperationException("Insufficient stock");

            int beforeQty = inventoryRecord.Quantityinstock;
            inventoryRecord.Quantityinstock -= quantity;
            inventoryRecord.Lastupdatedat = DateTime.UtcNow;
            _context.Inventories.Update(inventoryRecord);
            await _context.SaveChangesAsync();

            var transaction = new Inventorytransaction
            {
                Inventoryid = inventoryRecord.Id,
                Productid = productId,
                Transactiontype = InventoryTransactionType.Out.ToString(),
                Quantity = quantity,
                Beforequantity = beforeQty,
                Afterquantity = inventoryRecord.Quantityinstock,
                Referencetype = referenceType,
                Referenceid = referenceId,
                Createdby = sellerId,
                Createdat = DateTime.UtcNow
            };
            _context.Inventorytransactions.Add(transaction);
            await _context.SaveChangesAsync();

            return inventoryRecord;
        }

        // ================================
        // 4️⃣ Get inventory by product
        // ================================
        public async Task<Inventory> GetInventoryByProductIdAsync(int productId)
        {
            return await _context.Inventories.FirstOrDefaultAsync(i => i.Productid == productId);
        }

        // ================================
        // 5️⃣ Get all inventories
        // ================================
        public async Task<List<Inventory>> GetAllInventoriesAsync()
        {
            return await _context.Inventories.ToListAsync();
        }

        // ================================
        // 6️⃣ Get all transactions for a product
        // ================================
        public async Task<List<Inventorytransaction>> GetTransactionsByProductIdAsync(int productId)
        {
            return await _context.Inventorytransactions
                .Where(t => t.Productid == productId)
                .OrderByDescending(t => t.Createdat)
                .ToListAsync();
        }

        // ================================
        // 7️⃣ Optional: Delete inventory
        // ================================
        public async Task<bool> DeleteInventoryAsync(int inventoryId)
        {
            var inventoryRecord = await _context.Inventories.FirstOrDefaultAsync(i => i.Id == inventoryId);
            if (inventoryRecord == null) return false;

            _context.Inventories.Remove(inventoryRecord);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task ReserveStockAsync(int productId, int quantity, int userId, int orderId)
        {
            using var transaction = await _context.Database
                .BeginTransactionAsync(System.Data.IsolationLevel.Serializable);

            var inventory = await _context.Inventories
                .FirstOrDefaultAsync(i => i.Productid == productId);

            if (inventory == null)
                throw new Exception("Inventory not found");

            var availableStock = inventory.Quantityinstock - inventory.Reservedquantity;

            if (availableStock < quantity)
                throw new Exception("Insufficient stock");

            inventory.Reservedquantity += quantity;
            inventory.Lastupdatedat = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            _context.Inventorytransactions.Add(new Inventorytransaction
            {
                Inventoryid = inventory.Id,
                Productid = inventory.Productid,
                Transactiontype = InventoryTransactionType.Reserved.ToString(),
                Quantity = quantity,
                Beforequantity = availableStock,
                Afterquantity = availableStock - quantity,
                Referencetype = "Checkout",
                Referenceid = orderId,
                Createdby = userId,
                Createdat = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        //release reserve stock 
        public async Task ReleaseReservedStockAsync(int inventoryId, int quantity, int userId, int orderId)
        {
            var inventory = await _context.Inventories
                .FirstOrDefaultAsync(i => i.Id == inventoryId);

            inventory.Reservedquantity -= quantity;
            inventory.Lastupdatedat = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            _context.Inventorytransactions.Add(new Inventorytransaction
            {
                Inventoryid = inventory.Id,
                Productid = inventory.Productid,
                Transactiontype = InventoryTransactionType.In.ToString(),
                Quantity = quantity,
                //Referencetype = "PaymentFailed",
                Referenceid = orderId,
                Createdby = userId,
                Createdat = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
        }

        //private readonly EcommerceContext _context;
        //private readonly IInventoryRepository _inventoryRepository;
        //public InventoryRepository(EcommerceContext context, IInventoryRepository inventoryRepository) 
        //{

        //    _context = context;
        //    _inventoryRepository = inventoryRepository;
        //}

        //public async Task AddInventoryAsync(Inventory inventory)
        //{
        //    _context.Inventories.Add(inventory);
        //    await _context.SaveChangesAsync();
        //}

        //public async Task AddInventoryTransactionAsync(Inventorytransaction transaction)
        //{
        //    _context.Inventorytransactions.Add(transaction);
        //    await _context.SaveChangesAsync();
        //}

        //public async Task<Inventory?> GetInventoryByProductIdAsync(int productId)
        //{
        //    return await _context.Inventories.FirstOrDefaultAsync(i => i.Productid == productId);
        //}

        //public async Task UpdateInventoryAsync(Inventory inventory)
        //{
        //    _context.Inventories.Update(inventory);
        //    await _context.SaveChangesAsync();
        //}
        //

        //--------------------------------------
        //public async Task CreateInitialInventoryAsync(int productId, int initialStock, int sellerId)
        //{
        //    var inventory = new Inventory
        //    {
        //        Productid = productId,
        //        Quantityinstock = initialStock,
        //        Lastupdatedat = DateTime.UtcNow
        //    };

        //    await _inventoryRepository.AddInventoryAsync(inventory);

        //    if (initialStock > 0)
        //    {
        //        var transaction = new Inventorytransaction
        //        {
        //            Inventoryid = inventory.Id,
        //            Productid = productId,
        //            Transactiontype = (string)InventoryTransactionType.In,
        //            Quantity = initialStock,
        //            Beforequantity = 0,
        //            Afterquantity = initialStock,
        //            Referencetype = "ProductCreation",
        //            Referenceid = productId,
        //            Createdby = sellerId,
        //            Createdat = DateTime.UtcNow
        //        };

        //        await _inventoryRepository.AddInventoryTransactionAsync(transaction);
        //    }
        //}

        //public async Task ReduceStockAsync(int productId, int quantity, int userId, string referenceType, int referenceId)
        //{
        //    var inventory = await _inventoryRepository.GetInventoryByProductIdAsync(productId);
        //    if (inventory == null || inventory.QuantityInStock < quantity)
        //        throw new InvalidOperationException("Insufficient stock");

        //    var before = inventory.QuantityInStock;
        //    inventory.QuantityInStock -= quantity;
        //    inventory.LastUpdatedAt = DateTime.UtcNow;

        //    await _inventoryRepository.UpdateInventoryAsync(inventory);

        //    var transaction = new Inventorytransaction
        //    {
        //        InventoryId = inventory.InventoryId,
        //        ProductId = productId,
        //        TransactionType = InventoryTransactionType.Out,
        //        Quantity = quantity,
        //        BeforeQuantity = before,
        //        AfterQuantity = inventory.QuantityInStock,
        //        ReferenceType = referenceType,
        //        ReferenceId = referenceId,
        //        CreatedBy = userId,
        //        CreatedAt = DateTime.UtcNow
        //    };

        //    await _inventoryRepository.AddInventoryTransactionAsync(transaction);
        //}



    }
}
