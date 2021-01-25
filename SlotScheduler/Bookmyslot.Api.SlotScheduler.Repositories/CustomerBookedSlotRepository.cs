using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Common.Database.Interfaces;
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

            return await GetCustomerSlots(sql, parameters);
        }

        public async Task<Response<IEnumerable<SlotModel>>> GetCustomerCompletedSlots(string customerId)
        {
            var parameters = new { IsDeleted = false, BookedBy = customerId };
            var sql = SlotTableQueries.GetCustomerBookedByCompletedSlotsQuery;

            return await GetCustomerSlots(sql, parameters);
        }

        private async Task<Response<IEnumerable<SlotModel>>> GetCustomerSlots(string sql, object parameters)
        {
            var slotEntities = await this.sqlInterceptor.GetQueryResults(sql, parameters, () => this.connection.QueryAsync<SlotEntity>(sql, parameters));

            var slotModels = ModelFactory.ModelFactory.CreateSlotModels(slotEntities);
            if (slotModels.Count == 0)
            {
                return Response<IEnumerable<SlotModel>>.Empty(new List<string>() { AppBusinessMessages.NoRecordsFound });
            }

            return new Response<IEnumerable<SlotModel>>() { Result = slotModels };
        }
    }
}
