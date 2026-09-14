using FlashSale.OrderManager.Application.Interfaces;
using FlashSale.OrderManager.Domain.Enums;
using FlashSale.OrderManager.Domain.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace FlashSale.OrderManager.Infrastructure.Persistance.Repositories
{
    public class OutboxRepository:IOutboxRepository
    {
        private readonly ApplicationDbContext _db;

        public OutboxRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(OutBoxMessage message)
        {
            await _db.OutboxMessages.AddAsync(message);
        }

        public OutBoxMessage CreateOutBoxMessage(
            OutboxEventType eventType,
            object payload,
            DateTimeOffset occurredOnUtc)
        {
            return new OutBoxMessage
            {
                Id = Guid.NewGuid(),
                EventType = eventType,
                Payload = JsonSerializer.Serialize(payload),
                OccurredOnUtc = occurredOnUtc,
                RetryCount = 0
            };
        }

        public async Task<OutBoxMessage?> GetOutBoxMessageByIdAsync(Guid messageId)
        {
            return await _db.OutboxMessages
                .FirstOrDefaultAsync(x => x.Id == messageId);
        }

        public async Task<List<OutBoxMessage>> GetUnpublishedAsync(int batchSize, int leaseSeconds = 30)
        {
            var batchSizeParam = new SqlParameter("@BatchSize", batchSize);
            var leaseSecondsParam = new SqlParameter("@LeaseSeconds", leaseSeconds);

            return await _db.OutboxMessages
                .FromSqlRaw(
                    "EXEC dbo.ClaimOutboxMessages @BatchSize, @LeaseSeconds",
                    batchSizeParam, leaseSecondsParam)
                .ToListAsync();
        }

        public Task MarkAsFailedAsync(OutBoxMessage message, string error, int maxRetries = 5)
        {
            message.RetryCount += 1;
            message.Error = error;

            if(message.RetryCount >= maxRetries)
            {
                // dead-lettered: parked out of the claim query permanently
                // (consider a dedicated Status/DeadLetteredOnUtc column instead)
                message.LockedUntilUtc = DateTimeOffset.MaxValue;
            }
            else
            {
                var backoffSeconds = Math.Min(30 * Math.Pow(2, message.RetryCount), 3600);
                message.LockedUntilUtc = DateTimeOffset.UtcNow.AddSeconds(backoffSeconds);
            }

            _db.OutboxMessages.Update(message);

            return Task.CompletedTask;
        }

        public Task MarkAsPublishedAsync(OutBoxMessage message)
        {
            message.PublishedOnUtc = DateTimeOffset.UtcNow;
            message.LockedUntilUtc = null;

            _db.OutboxMessages.Update(message);

            return Task.CompletedTask;
        }
    }
}