using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Common.Database.Interfaces;
using Bookmyslot.Api.SlotScheduler.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using Bookmyslot.Api.SlotScheduler.Repositories.Enitites;
using Bookmyslot.Api.SlotScheduler.Repositories.ModelFactory;
using Bookmyslot.Api.SlotScheduler.Repositories.Queries;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Repositories
{
    public class CustomerBookedSlotRepository : ICustomerBookedSlotRepository
    {
        private readonly IDbConnection connection;
        private readonly ISqlInterceptor sqlInterceptor;

        public CustomerBookedSlotRepository(IDbConnection connection, ISqlInterceptor sqlInterceptor)
        {
            this.connection = connection;
            this.sqlInterceptor = sqlInterceptor;
        }
      
        public async Task<Response<IEnumerable<SlotModel>>> GetCustomerBookedSlots(string customerId)
        {
            var parameters = new { IsDeleted = false, BookedBy = customerId };
            var sql = SlotTableQueries.GetCustomerBookedByBookedSlotsQuery;

            return await GetCustomerSlots("GetCustomerBookedSlots", sql, parameters);
        }

        public async Task<Response<IEnumerable<SlotModel>>> GetCustomerCompletedSlots(string customerId)
        {
            var parameters = new { IsDeleted = false, BookedBy = customerId };
            var sql = SlotTableQueries.GetCustomerBookedByCompletedSlotsQuery;

            return await GetCustomerSlots("GetCustomerCompletedSlots", sql, parameters);
        }

        private async Task<Response<IEnumerable<SlotModel>>> GetCustomerSlots(string operationName, string sql, object parameters)
        {
            var slotEntities = await this.sqlInterceptor.GetQueryResults(operationName, parameters, () => this.connection.QueryAsync<SlotEntity>(sql, parameters));

            return ResponseModelFactory.CreateSlotModelsResponse(slotEntities);
        }
    }
}
