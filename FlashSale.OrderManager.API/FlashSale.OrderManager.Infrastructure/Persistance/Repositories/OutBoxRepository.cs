using AutoMapper;
using FlashSale.OrderManager.Application.Interfaces;
using FlashSale.OrderManager.Application.Messaging.Messages;
using FlashSale.OrderManager.Application.Options;
using FlashSale.OrderManager.Domain.Enums;
using FlashSale.OrderManager.Domain.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Text.Json;
namespace FlashSale.OrderManager.Infrastructure.Persistance.Repositories
{
    public class OutboxRepository:IOutboxRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly OutboxOptions _options;

        public OutboxRepository(ApplicationDbContext db, IMapper mapper, IOptions<OutboxOptions> options)
        {
            _db = db;
            _mapper = mapper;
            _options = options.Value;
        }

        public async Task AddAsync(OutBoxMessage message)
        {
            await _db.OutboxMessages.AddAsync(message);
        }

        public OutBoxMessage CreateOrderCreatedOutBoxMessage(
            Order order,
            DateTimeOffset occurredOnUtc)
        {

            var OrderCreatedMessage = _mapper.Map<OrderCreatedMessage>(order);

            return CreateOutBoxMessage(
                OutboxEventType.OrderCreated,
                OrderCreatedMessage,
                occurredOnUtc);
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

        public async Task<List<OutBoxMessage>> GetUnpublishedAsync(OutboxEventType eventType)
        {
            var batchSizeParam = new SqlParameter("@BatchSize", _options.BatchSize);
            var leaseSecondsParam = new SqlParameter("@LeaseSeconds", _options.LeaseSeconds);
            var eventTypeParam = new SqlParameter("@EventType", eventType.ToString());

            return await _db.OutboxMessages
                .FromSqlRaw(
                    "EXEC dbo.ClaimOutboxMessages @BatchSize, @LeaseSeconds, @EventType",
                    batchSizeParam, leaseSecondsParam, eventTypeParam)
                .ToListAsync();
        }

        public Task MarkAsFailedAsync(OutBoxMessage message, string error)
        {
            message.RetryCount += 1;
            message.Error = error;

            if(message.RetryCount >= _options.MaxRetries)
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