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
    public class CustomerCancelledSlotRepository : ICustomerCancelledSlotRepository
    {
        private readonly IDbConnection connection;

        public CustomerCancelledSlotRepository(IDbConnection connection)
        {
            this.connection = connection;
        }

        public Task<Response<CancelledSlotModel>> CreateCustomerCancelledSlots(CancelledSlotModel cancelledSlotModel)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Response<IEnumerable<CancelledSlotModel>>> GetCustomerCancelledSlots(string customerId)
        {
            var parameters = new { IsDeleted = true, CancelledBy = customerId };
            var sql = SlotTableQueries.GetCustomerBookedByCancelledSlotsQuery;

            var cancelledSlotEntities = await this.connection.QueryAsync<CancelledSlotEntity>(sql, parameters);

            var slotModels = ModelFactory.ModelFactory.CreateCancelledSlotModels(cancelledSlotEntities);
            if (slotModels.Count == 0)
            {
                return Response<IEnumerable<CancelledSlotModel>>.Empty(new List<string>() { AppBusinessMessages.NoRecordsFound });
            }

            return new Response<IEnumerable<CancelledSlotModel>>() { Result = slotModels };
        }
    }
}
