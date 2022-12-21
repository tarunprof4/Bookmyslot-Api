using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using Bookmyslot.Api.SlotScheduler.Domain;
using Bookmyslot.Api.SlotScheduler.Repositories.Enitites;
using Bookmyslot.Api.SlotScheduler.Repositories.ModelFactory;
using Bookmyslot.Api.SlotScheduler.Repositories.Queries;
using Bookmyslot.SharedKernel.Contracts.Database;
using Bookmyslot.SharedKernel.Contracts.Event;
using Bookmyslot.SharedKernel.ValueObject;
using Dapper;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Repositories
{
    public class SlotRepository : ISlotRepository
    {
        private readonly IDbConnection connection;
        private readonly IDbInterceptor dbInterceptor;
        private readonly IEventDispatcher eventDispatcher;

        public SlotRepository(IDbConnection connection, IDbInterceptor dbInterceptor, IEventDispatcher eventDispatcher)
        {
            this.connection = connection;
            this.dbInterceptor = dbInterceptor;
            this.eventDispatcher = eventDispatcher;
        }



        public async Task<Result<string>> CreateSlot(SlotModel slotModel)
        {
            var slotEntity = EntityFactory.EntityFactory.CreateSlotEntity(slotModel);

            var sql = SlotTableQueries.CreateSlotQuery;
            var parameters = new
            {
                Id = slotEntity.Id,
                Title = slotEntity.Title,
                CreatedBy = slotEntity.CreatedBy,
                Country = slotEntity.Country,
                TimeZone = slotEntity.TimeZone,
                SlotDate = slotEntity.SlotDate,
                SlotStartDateTimeUtc = slotEntity.SlotStartDateTimeUtc,
                SlotEndDateTimeUtc = slotEntity.SlotEndDateTimeUtc,
                SlotStartTime = slotEntity.SlotStartTime,
                SlotEndTime = slotEntity.SlotEndTime,
                CreatedDateUtc = slotEntity.CreatedDateUtc,
                IsDeleted = slotEntity.IsDeleted,
            };


            await this.dbInterceptor.GetQueryResults("CreateSlot", parameters, () => this.connection.ExecuteAsync(sql, parameters));

            return new Result<string>() { Value = slotEntity.Id };
        }

        public async Task<Result<bool>> DeleteSlot(string slotId)
        {
            var sql = SlotTableQueries.DeleteSlotQuery;
            var parameters = new
            {
                Id = slotId,
                ModifiedDateUtc = DateTime.UtcNow,
                IsDeleted = true
            };
            await this.dbInterceptor.GetQueryResults("DeleteSlot", parameters, () => this.connection.ExecuteAsync(sql, parameters));

            return new Result<bool>() { Value = true };
        }

        public async Task<Result<SlotModel>> GetSlot(string slotId)
        {
            var sql = SlotTableQueries.GetSlotQuery;
            var parameters = new { Id = slotId, IsDeleted = false };

            var slotEntity = await this.dbInterceptor.GetQueryResults("GetSlot", parameters, () => this.connection.QueryFirstOrDefaultAsync<SlotEntity>(sql, parameters));

            return ResponseModelFactory.CreateSlotModelResponse(slotEntity);
        }



        public async Task<Result<bool>> UpdateSlotBooking(SlotModel slotModel)
        {
            var sql = SlotTableQueries.UpdateSlotQuery;
            var parameters = new
            {
                Id = slotModel.Id,
                BookedBy = slotModel.BookedBy,
                SlotMeetingLink = slotModel.SlotMeetingLink,
                ModifiedDateUtc = DateTime.UtcNow,
            };
            await this.dbInterceptor.GetQueryResults("UpdateSlot", parameters, () => this.connection.ExecuteAsync(sql, parameters));
            await this.eventDispatcher.DispatchEvents(slotModel.Events);

            return new Result<bool>() { Value = true };
        }

    }
}
