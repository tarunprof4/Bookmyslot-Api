using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using Bookmyslot.Api.SlotScheduler.Domain;
using Bookmyslot.Api.SlotScheduler.Repositories.Enitites;
using Bookmyslot.Api.SlotScheduler.Repositories.ModelFactory;
using Bookmyslot.Api.SlotScheduler.Repositories.Queries;
using Bookmyslot.SharedKernel.Contracts.Database;
using Bookmyslot.SharedKernel.Contracts.Event;
using Bookmyslot.SharedKernel.ValueObject;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Repositories
{
    public class CustomerCancelledSlotRepository : ICustomerCancelledSlotRepository
    {
        private readonly IDbConnection connection;
        private readonly IDbInterceptor dbInterceptor;
        private readonly IEventDispatcher eventDispatcher;
        public CustomerCancelledSlotRepository(IDbConnection connection, IDbInterceptor dbInterceptor, IEventDispatcher eventDispatcher)
        {
            this.connection = connection;
            this.dbInterceptor = dbInterceptor;
            this.eventDispatcher = eventDispatcher;
        }

        public async Task<Result<bool>> CreateCustomerCancelledSlot(CancelledSlotModel cancelledSlotModel)
        {
            var cancelledSlotEntity = EntityFactory.EntityFactory.CreateCancelledSlotEntity(cancelledSlotModel);

            var sql = CancelledSlotTableQueries.CreateCancelledSlotQuery;
            var parameters = new
            {
                Id = cancelledSlotEntity.Id,
                Title = cancelledSlotEntity.Title,
                CreatedBy = cancelledSlotEntity.CreatedBy,
                CancelledBy = cancelledSlotEntity.CancelledBy,
                BookedBy = cancelledSlotEntity.BookedBy,
                Country = cancelledSlotEntity.Country,
                TimeZone = cancelledSlotEntity.TimeZone,
                SlotDate = cancelledSlotEntity.SlotDate,
                SlotStartDateTimeUtc = cancelledSlotEntity.SlotStartDateTimeUtc,
                SlotEndDateTimeUtc = cancelledSlotEntity.SlotEndDateTimeUtc,
                SlotStartTime = cancelledSlotEntity.SlotStartTime,
                SlotEndTime = cancelledSlotEntity.SlotEndTime,
                CreatedDateUtc = cancelledSlotEntity.CreatedDateUtc,
            };


            await this.dbInterceptor.GetQueryResults("CreateCustomerCancelledSlot", parameters, () => this.connection.ExecuteAsync(sql, parameters));
            await this.eventDispatcher.DispatchEvents(cancelledSlotModel.Events);

            return new Result<bool>() { Value = true };
        }


        public async Task<Result<IEnumerable<CancelledSlotModel>>> GetCustomerSharedCancelledSlots(string customerId)
        {
            var parameters = new { IsDeleted = true, CancelledBy = customerId };
            var sql = CancelledSlotTableQueries.GetCustomerSharedByCancelledSlotsQuery;

            var cancelledSlotEntities = await this.dbInterceptor.GetQueryResults("GetCustomerSharedCancelledSlots", parameters, () => this.connection.QueryAsync<CancelledSlotEntity>(sql, parameters));

            return ResponseModelFactory.CreateCancelledSlotModels(cancelledSlotEntities);
        }



        public async Task<Result<IEnumerable<CancelledSlotModel>>> GetCustomerBookedCancelledSlots(string customerId)
        {
            var parameters = new { IsDeleted = true, CancelledBy = customerId, BookedBy = customerId };
            var sql = CancelledSlotTableQueries.GetCustomerBookedByCancelledSlotsQuery;

            var cancelledSlotEntities = await this.dbInterceptor.GetQueryResults("GetCustomerBookedCancelledSlots", parameters, () => this.connection.QueryAsync<CancelledSlotEntity>(sql, parameters));

            return ResponseModelFactory.CreateCancelledSlotModels(cancelledSlotEntities);
        }


    }
}
