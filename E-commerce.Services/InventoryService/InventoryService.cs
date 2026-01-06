using E_commerce.Models.Enums;
using E_commerce.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Services.InventoryService
{
    public class InventoryService
    {
        //private readonly Iinventory
        //public InventoryService() { }
        //public async Task CreateInitialInventoryAsync(int productId, int initialStock, int sellerId)
        //{
        //    if (initialStock <= 0) return;

        //    // Add inventory record
        //    var inventory = new Inventory
        //    {
        //        Productid = productId,
        //        Quantityinstock = initialStock,
        //        Lastupdatedat = DateTime.UtcNow
        //    };
        //    await _inventoryRepository.AddAsync(inventory);

        //    // Record transaction
        //    var transaction = new InventoryTransaction
        //    {
        //        InventoryId = inventory.InventoryId,
        //        ProductId = productId,
        //        TransactionType = InventoryTransactionType.In,
        //        Quantity = initialStock,
        //        BeforeQuantity = 0,
        //        AfterQuantity = initialStock,
        //        ReferenceType = "ProductCreation",
        //        ReferenceId = productId,
        //        CreatedBy = sellerId,
        //        CreatedAt = DateTime.UtcNow
        //    };
        //    await _inventoryRepository.AddTransactionAsync(transaction);
        //}
    }
}
