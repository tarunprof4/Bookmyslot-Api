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
    public class CustomerLastSharedSlotRepository : ICustomerLastSharedSlotRepository
    {
        private readonly IDbConnection connection;
        private readonly IDbInterceptor dbInterceptor;

        public CustomerLastSharedSlotRepository(IDbConnection connection, IDbInterceptor dbInterceptor)
        {
            this.connection = connection;
            this.dbInterceptor = dbInterceptor;
        }

        public async Task<Response<bool>> SaveCustomerLatestSharedSlot(CustomerLastSharedSlotModel customerLastSharedSlotModel)
        {
            var customerLastSharedSlotEntity = EntityFactory.EntityFactory.CreateCustomerLastSharedSlotEntity(customerLastSharedSlotModel);

            var sql = LastSlotSharedTableQueries.InsertOrUpdateCustomerLastSharedSlotQuery;
            var parameters = new
            {
                CreatedBy = customerLastSharedSlotEntity.CreatedBy,
                Title = customerLastSharedSlotEntity.Title,
                Country = customerLastSharedSlotEntity.Country,
                TimeZone = customerLastSharedSlotEntity.TimeZone,
                SlotDate = customerLastSharedSlotEntity.SlotDate,
                SlotStartDateTimeUtc = customerLastSharedSlotEntity.SlotStartDateTimeUtc,
                SlotEndDateTimeUtc = customerLastSharedSlotEntity.SlotEndDateTimeUtc,
                SlotStartTime = customerLastSharedSlotEntity.SlotStartTime,
                SlotEndTime = customerLastSharedSlotEntity.SlotEndTime,
                ModifiedDateUtc = customerLastSharedSlotEntity.ModifiedDateUtc,
            };


            await this.dbInterceptor.GetQueryResults("SaveCustomerLatestSlot", parameters, () => this.connection.ExecuteAsync(sql, parameters));

            return new Response<bool>() { Result = true };
        }


        public async Task<Response<CustomerLastSharedSlotModel>> GetCustomerLatestSharedSlot(string customerId)
        {
            var sql = LastSlotSharedTableQueries.GetCustomerLastSharedSlotQuery;
            var parameters = new { CreatedBy = customerId };

            var customerLastSharedSlotEntity = await this.dbInterceptor.GetQueryResults("GetSlot", parameters, () => this.connection.QueryFirstOrDefaultAsync<CustomerLastSharedSlotEntity>(sql, parameters));


            return ResponseModelFactory.CreateCustomerLastSharedSlotModelResponse(customerLastSharedSlotEntity);
        }

      
    }
}
