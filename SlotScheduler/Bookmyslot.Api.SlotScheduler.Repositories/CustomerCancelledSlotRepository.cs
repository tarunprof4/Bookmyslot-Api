using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.SlotScheduler.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using Bookmyslot.Api.SlotScheduler.Repositories.Enitites;
using Bookmyslot.Api.SlotScheduler.Repositories.Queries;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Repositories
{
    public class CustomerCancelledSlotRepository : ICustomerCancelledSlotRepository
    {
        private readonly IDbConnection connection;

        public CustomerCancelledSlotRepository(IDbConnection connection)
        {
            this.connection = connection;
        }

        public async Task<Response<bool>> CreateCustomerCancelledSlot(CancelledSlotModel cancelledSlotModel)
        {
            var customerEntity = EntityFactory.EntityFactory.CreateCancelledSlotEntity(cancelledSlotModel);
            await this.connection.InsertAsync<Guid, CancelledSlotEntity>(customerEntity);
            return new Response<bool>() { Result = true };
        }


        public async Task<Response<IEnumerable<CancelledSlotModel>>> GetCustomerSharedCancelledSlots(string customerId)
        {
            var parameters = new { IsDeleted = true, CancelledBy = customerId };
            var sql = SlotTableQueries.GetCustomerSharedByCancelledSlotsQuery;

            var cancelledSlotEntities = await this.connection.QueryAsync<CancelledSlotEntity>(sql, parameters);

            return GetCancelledSlotModels(cancelledSlotEntities);
        }

        

        public async Task<Response<IEnumerable<CancelledSlotModel>>> GetCustomerBookedCancelledSlots(string customerId)
        {
            var parameters = new { IsDeleted = true, CancelledBy = customerId, BookedBy = customerId };
            var sql = SlotTableQueries.GetCustomerBookedByCancelledSlotsQuery;

            var cancelledSlotEntities = await this.connection.QueryAsync<CancelledSlotEntity>(sql, parameters);

            return GetCancelledSlotModels(cancelledSlotEntities);
        }

        private static Response<IEnumerable<CancelledSlotModel>> GetCancelledSlotModels(IEnumerable<CancelledSlotEntity> cancelledSlotEntities)
        {
            var slotModels = ModelFactory.ModelFactory.CreateCancelledSlotModels(cancelledSlotEntities);
            if (slotModels.Count == 0)
            {
                return Response<IEnumerable<CancelledSlotModel>>.Empty(new List<string>() { AppBusinessMessages.NoRecordsFound });
            }

            return new Response<IEnumerable<CancelledSlotModel>>() { Result = slotModels };
        }


    }
}
