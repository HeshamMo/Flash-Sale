using AutoMapper;
using FlashSale.InventoryManager.Application.Messaging.Messages.OrderCancelled;
using FlashSale.InventoryManager.Application.Messaging.Messages.OrderCreated;
using FlashSale.InventoryManager.Domain.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace FlashSale.InventoryManager.Infrastructure.Persistance.Repositories
{
    public class InventoryRepository:IInventoryRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly TimeProvider _timeProvide;
        public InventoryRepository(ApplicationDbContext context, IMapper mapper, TimeProvider timeProvide)
        {
            _db = context;
            _mapper = mapper;
            _timeProvide = timeProvide;
        }

        public async Task<Product> CreateAsync(Product product)
        {

            await _db.Products.AddAsync(product);
            return product;
        }



        public Task<Product> GetByIdAsync(Guid id)
        {
            return _db.Products.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }


        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _db.Products.AsNoTracking().ToListAsync();
        }

        public Task UpdateAsync(Product product)
        {
            _db.Products.Update(product);
            return Task.CompletedTask; ;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existing = await _db.Products.FirstOrDefaultAsync(x => x.Id == id);
            if(existing == null) return false;
            _db.Products.Remove(existing);
            return true;
        }



        public async Task<bool> ReserveStockAsync(
            Guid orderId,
            ICollection<OrderCreatedProduct> orderItems)
        {
            var orderItemsAsTable = new DataTable();

            orderItemsAsTable.Columns.Add(
                DbQueryOptions.ProductIdColumn,
                typeof(Guid));

            orderItemsAsTable.Columns.Add(
                DbQueryOptions.ProductQuantityColumn,
                typeof(int));

            foreach(var item in orderItems)
            {
                orderItemsAsTable.Rows.Add(
                    item.ProductId,
                    item.ProductQuantity);
            }

            var orderIdParameter = new SqlParameter(
                DbQueryOptions.OrderIdParameter,
                orderId);


            var createdAtParameter = new SqlParameter(
                DbQueryOptions.CreatedAtParameter,
                 _timeProvide.GetUtcNow());

            var orderItemsParameter = new SqlParameter(
                DbQueryOptions.OrderItemsParameter,
                orderItemsAsTable)
            {
                SqlDbType = SqlDbType.Structured,
                TypeName = DbQueryOptions.OrderItemTableType
            };

            var result = (await _db.Database
                .SqlQueryRaw<bool>(
                    DbQueryOptions.ReserveStockCommand,
                    orderIdParameter,
                    createdAtParameter,
                    orderItemsParameter)
                .ToListAsync()).First();

            return result;
        }


        public async Task<bool> ReleaseStockAsync(
            Guid orderId,
            ICollection<OrderCancelledProduct> orderItems)
        {
            var orderItemsAsTable = new DataTable();

            orderItemsAsTable.Columns.Add(
                DbQueryOptions.ProductIdColumn,
                typeof(Guid));

            orderItemsAsTable.Columns.Add(
                DbQueryOptions.ProductQuantityColumn,
                typeof(int));

            foreach(var item in orderItems)
            {
                orderItemsAsTable.Rows.Add(
                    item.ProductId,
                    item.ProductQuantity);
            }

            var orderIdParameter = new SqlParameter(
                DbQueryOptions.OrderIdParameter,
                orderId);


            var createdAtParameter = new SqlParameter(
                DbQueryOptions.CreatedAtParameter,
                _timeProvide.GetUtcNow());

            var orderItemsParameter = new SqlParameter(
                DbQueryOptions.OrderItemsParameter,
                orderItemsAsTable)
            {
                SqlDbType = SqlDbType.Structured,
                TypeName = DbQueryOptions.OrderItemTableType
            };

            var result = (await _db.Database
                .SqlQueryRaw<bool>(
                    DbQueryOptions.ReleaseStockCommand,
                    orderIdParameter,
                    createdAtParameter,
                    orderItemsParameter)
                .ToListAsync()).First();

            return result;
        }

    }
}
