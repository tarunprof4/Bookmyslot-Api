using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Database.Interfaces;
using Bookmyslot.Api.SlotScheduler.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
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

        public SlotRepository(IDbConnection connection, IDbInterceptor dbInterceptor)
        {
            this.connection = connection;
            this.dbInterceptor = dbInterceptor;
        }

     

        public async Task<Response<string>> CreateSlot(SlotModel slotModel)
        {
            var slotEntity = EntityFactory.EntityFactory.CreateSlotEntity(slotModel);

            var sql = SlotQueries.CreateSlotQuery;
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
            var sql = SlotQueries.DeleteSlotQuery;
            var parameters = new {
                Id = slotId,
                ModifiedDateUtc = DateTime.UtcNow,
                IsDeleted = true
            };
            await this.dbInterceptor.GetQueryResults("DeleteSlot", parameters, () => this.connection.ExecuteAsync(sql, parameters));
            
            return new Response<bool>() { Result = true };
        }

        public async Task<Response<SlotModel>> GetSlot(string slotId)
        {
            var sql = SlotQueries.GetSlotQuery;
            var parameters = new { Id = slotId, IsDeleted = false };

            var slotEntity = await this.dbInterceptor.GetQueryResults("GetSlot", parameters, () => this.connection.QueryFirstOrDefaultAsync<SlotEntity>(sql, parameters));

            return ResponseModelFactory.CreateSlotModelResponse(slotEntity);
        }

     

        public async Task<Response<bool>> UpdateSlotBooking(string slotId, string slotMeetingLink, string bookedBy)
        {
            var sql = SlotQueries.UpdateSlotQuery;
            var parameters = new
            {
                Id = slotId,
                BookedBy = bookedBy,
                SlotMeetingLink = slotMeetingLink,
                ModifiedDateUtc = DateTime.UtcNow,
            };
            await this.dbInterceptor.GetQueryResults("UpdateSlot", parameters, () => this.connection.ExecuteAsync(sql, parameters));
            
            return new Response<bool>() { Result = true };
        }

      
    }
}
