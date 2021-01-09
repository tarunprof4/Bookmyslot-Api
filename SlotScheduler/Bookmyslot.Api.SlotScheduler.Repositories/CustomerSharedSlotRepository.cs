using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.SlotScheduler.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using Bookmyslot.Api.SlotScheduler.Repositories.Enitites;
using Bookmyslot.Api.SlotScheduler.Repositories.Queries;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Repositories
{
    public class CustomerSharedSlotRepository : ICustomerSharedSlotRepository
    {
        private readonly IDbConnection connection;

        public CustomerSharedSlotRepository(IDbConnection connection)
        {
            this.connection = connection;
        }


        public async Task<Response<IEnumerable<SlotModel>>> GetCustomerYetToBeBookedSlots(string customerId)
        {
            var parameters = new { IsDeleted = false, CreatedBy = customerId };
            var sql = SlotTableQueries.GetCustomerYetToBeBookedSlotsQuery;

            return await GetCustomerSlots(sql, parameters);
        }


        public async Task<Response<IEnumerable<SlotModel>>> GetCustomerBookedSlots(string customerId)
        {
            var parameters = new { IsDeleted = false, CreatedBy = customerId };
            var sql = SlotTableQueries.GetCustomerBookedSlotsQuery;

            return await GetCustomerSlots(sql, parameters);
        }

        public async Task<Response<IEnumerable<SlotModel>>> GetCustomerCompletedSlots(string customerId)
        {
            var parameters = new { IsDeleted = false, CreatedBy = customerId };
            var sql = SlotTableQueries.GetCustomerCompletedSlotsQuery;

            return await GetCustomerSlots(sql, parameters);
        }

        public async Task<Response<IEnumerable<SlotModel>>> GetCustomerCancelledSlots(string customerId)
        {
            var parameters = new { IsDeleted = true, CreatedBy = customerId };
            var sql = SlotTableQueries.GetCustomerCancelledSlotsQuery;

            return await GetCustomerSlots(sql, parameters);
        }


        private async Task<Response<IEnumerable<SlotModel>>> GetCustomerSlots(string sql, object parameters)
        {
            var slotEntities = await this.connection.QueryAsync<SlotEntity>(sql, parameters);

            var slotModels = ModelFactory.ModelFactory.CreateSlotModels(slotEntities);
            if (slotModels.Count == 0)
            {
                return Response<IEnumerable<SlotModel>>.Empty(new List<string>() { AppBusinessMessages.NoRecordsFound });
            }

            return new Response<IEnumerable<SlotModel>>() { Result = slotModels };
        }
    }
}
