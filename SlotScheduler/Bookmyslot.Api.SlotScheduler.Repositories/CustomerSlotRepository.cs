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
    public class CustomerSlotRepository : ICustomerSlotRepository
    {
        private readonly IDbConnection connection;
        private readonly ISqlInterceptor sqlInterceptor;

        public CustomerSlotRepository(IDbConnection connection, ISqlInterceptor sqlInterceptor)
        {
            this.connection = connection;
            this.sqlInterceptor = sqlInterceptor;
        }


        public async Task<Response<IEnumerable<SlotModel>>> GetDistinctCustomersNearestSlotFromToday(PageParameterModel pageParameterModel)
        {
            var parameters = new { IsDeleted = false, PageNumber = pageParameterModel.PageNumber, PageSize = pageParameterModel.PageSize };
            var sql = SlotTableQueries.GetDistinctCustomersNearestSlotFromTodayQuery;

            var slotEntities = await this.sqlInterceptor.GetQueryResults("GetDistinctCustomersNearestSlotFromToday", parameters, () => this.connection.QueryAsync<SlotEntity>(sql, parameters));

            return ResponseModelFactory.CreateSlotModelsResponse(slotEntities);
        }

        

        public async Task<Response<IEnumerable<SlotModel>>> GetCustomerAvailableSlots(PageParameterModel pageParameterModel, string email)
        {
            var parameters = new { IsDeleted = false, CreatedBy= email, PageNumber = pageParameterModel.PageNumber, PageSize = pageParameterModel.PageSize };
            var sql = SlotTableQueries.GetCustomerAvailableSlotsFromTodayQuery;

            var slotEntities = await this.sqlInterceptor.GetQueryResults("GetCustomerAvailableSlots", parameters, () => this.connection.QueryAsync<SlotEntity>(sql, parameters));

            return ResponseModelFactory.CreateSlotModelsResponse(slotEntities);
        }

    }
}
