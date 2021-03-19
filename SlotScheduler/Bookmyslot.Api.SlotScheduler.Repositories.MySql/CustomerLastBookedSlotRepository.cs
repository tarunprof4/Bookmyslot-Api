using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Database.Interfaces;
using Bookmyslot.Api.SlotScheduler.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces.Repository;
using Bookmyslot.Api.SlotScheduler.Repositories.Enitites;
using Bookmyslot.Api.SlotScheduler.Repositories.ModelFactory;
using Bookmyslot.Api.SlotScheduler.Repositories.Queries;
using Dapper;
using System.Data;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Repositories
{
    public class CustomerLastBookedSlotRepository : ICustomerLastBookedSlotRepository
    {
        private readonly IDbConnection connection;
        private readonly IDbInterceptor dbInterceptor;

        public CustomerLastBookedSlotRepository(IDbConnection connection, IDbInterceptor dbInterceptor)
        {
            this.connection = connection;
            this.dbInterceptor = dbInterceptor;
        }

        public async Task<Response<bool>> SaveCustomerLatestSlot(CustomerLastBookedSlotModel customerLastBookedSlotModel)
        {
            var customerLastBookedSlotEntity = EntityFactory.EntityFactory.CreateCustomerLastBookedSlotEntity(customerLastBookedSlotModel);

            var sql = SlotTableQueries.InsertOrUpdateCustomerLastBookedSlotQuery;
            var parameters = new
            {
                CreatedBy = customerLastBookedSlotEntity.CreatedBy,
                Title = customerLastBookedSlotEntity.Title,
                Country = customerLastBookedSlotEntity.Country,
                TimeZone = customerLastBookedSlotEntity.TimeZone,
                SlotDate = customerLastBookedSlotEntity.SlotDate,
                SlotDateUtc = customerLastBookedSlotEntity.SlotDateUtc,
                SlotStartTime = customerLastBookedSlotEntity.SlotStartTime,
                SlotEndTime = customerLastBookedSlotEntity.SlotEndTime,
                ModifiedDateUtc = customerLastBookedSlotEntity.ModifiedDateUtc,
            };


            await this.dbInterceptor.GetQueryResults("SaveCustomerLatestSlot", parameters, () => this.connection.ExecuteAsync(sql, parameters));

            return new Response<bool>() { Result = true };
        }


        public async Task<Response<CustomerLastBookedSlotModel>> GetCustomerLatestSlot(string customerId)
        {
            var sql = SlotTableQueries.GetCustomerLastBookedSlotQuery;
            var parameters = new { CreatedBy = customerId };

            var customerLastBookedSlotEntity = await this.dbInterceptor.GetQueryResults("GetSlot", parameters, () => this.connection.QueryFirstOrDefaultAsync<CustomerLastBookedSlotEntity>(sql, parameters));


            return ResponseModelFactory.CreateCustomerLastBookedSlotModelResponse(customerLastBookedSlotEntity);
        }

      
    }
}
