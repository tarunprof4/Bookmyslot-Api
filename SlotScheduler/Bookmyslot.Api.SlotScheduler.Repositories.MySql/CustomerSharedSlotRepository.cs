using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using Bookmyslot.Api.SlotScheduler.Domain;
using Bookmyslot.Api.SlotScheduler.Repositories.Enitites;
using Bookmyslot.Api.SlotScheduler.Repositories.ModelFactory;
using Bookmyslot.Api.SlotScheduler.Repositories.Queries;
using Bookmyslot.SharedKernel.Contracts.Database;
using Bookmyslot.SharedKernel.ValueObject;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Repositories
{
    public class CustomerSharedSlotRepository : ICustomerSharedSlotRepository
    {
        private readonly IDbConnection connection;
        private readonly IDbInterceptor dbInterceptor;

        public CustomerSharedSlotRepository(IDbConnection connection, IDbInterceptor dbInterceptor)
        {
            this.connection = connection;
            this.dbInterceptor = dbInterceptor;
        }


        public async Task<Result<IEnumerable<SlotModel>>> GetCustomerYetToBeBookedSlots(string customerId)
        {
            var parameters = new { IsDeleted = false, CreatedBy = customerId };
            var sql = SlotTableQueries.GetCustomerSharedByYetToBeBookedSlotsQuery;

            return await GetCustomerSlots("GetCustomerYetToBeBookedSlots", sql, parameters);
        }


        public async Task<Result<IEnumerable<SlotModel>>> GetCustomerBookedSlots(string customerId)
        {
            var parameters = new { IsDeleted = false, CreatedBy = customerId };
            var sql = SlotTableQueries.GetCustomerSharedByBookedSlotsQuery;

            return await GetCustomerSlots("GetCustomerBookedSlots", sql, parameters);
        }

        public async Task<Result<IEnumerable<SlotModel>>> GetCustomerCompletedSlots(string customerId)
        {
            var parameters = new { IsDeleted = false, CreatedBy = customerId };
            var sql = SlotTableQueries.GetCustomerSharedByCompletedSlotsQuery;

            return await GetCustomerSlots("GetCustomerCompletedSlots", sql, parameters);
        }




        private async Task<Result<IEnumerable<SlotModel>>> GetCustomerSlots(string operationName, string sql, object parameters)
        {
            var slotEntities = await this.dbInterceptor.GetQueryResults(operationName, parameters, () => this.connection.QueryAsync<SlotEntity>(sql, parameters));

            return ResponseModelFactory.CreateSlotModelsResponse(slotEntities);
        }

    }
}
