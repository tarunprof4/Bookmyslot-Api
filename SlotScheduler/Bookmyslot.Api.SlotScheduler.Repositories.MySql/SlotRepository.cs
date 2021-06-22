using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Event.Interfaces;
using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Database;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using Bookmyslot.Api.SlotScheduler.Domain;
using Bookmyslot.Api.SlotScheduler.Repositories.Enitites;
using Bookmyslot.Api.SlotScheduler.Repositories.ModelFactory;
using Bookmyslot.Api.SlotScheduler.Repositories.Queries;
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



        public async Task<Response<string>> CreateSlot(SlotModel slotModel)
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

            return new Response<string>() { Result = slotEntity.Id };
        }

        public async Task<Response<bool>> DeleteSlot(string slotId)
        {
            var sql = SlotTableQueries.DeleteSlotQuery;
            var parameters = new
            {
                Id = slotId,
                ModifiedDateUtc = DateTime.UtcNow,
                IsDeleted = true
            };
            await this.dbInterceptor.GetQueryResults("DeleteSlot", parameters, () => this.connection.ExecuteAsync(sql, parameters));

            return new Response<bool>() { Result = true };
        }

        public async Task<Response<SlotModel>> GetSlot(string slotId)
        {
            var sql = SlotTableQueries.GetSlotQuery;
            var parameters = new { Id = slotId, IsDeleted = false };

            var slotEntity = await this.dbInterceptor.GetQueryResults("GetSlot", parameters, () => this.connection.QueryFirstOrDefaultAsync<SlotEntity>(sql, parameters));

            return ResponseModelFactory.CreateSlotModelResponse(slotEntity);
        }



        public async Task<Response<bool>> UpdateSlotBooking(SlotModel slotModel)
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

            return new Response<bool>() { Result = true };
        }

    }
}
