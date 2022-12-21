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
    public class CustomerSlotRepository : ICustomerSlotRepository
    {
        private readonly IDbConnection connection;
        private readonly IDbInterceptor dbInterceptor;

        public CustomerSlotRepository(IDbConnection connection, IDbInterceptor dbInterceptor)
        {
            this.connection = connection;
            this.dbInterceptor = dbInterceptor;
        }


        public async Task<Result<IEnumerable<string>>> GetDistinctCustomersNearestSlotFromToday(PageParameter pageParameterModel)
        {
            var parameters = new { IsDeleted = false, PageNumber = pageParameterModel.PageNumber, PageSize = pageParameterModel.PageSize };
            var sql = SlotTableQueries.GetDistinctCustomersNearestSlotFromTodayQuery;

            var slotEntities = await this.dbInterceptor.GetQueryResults("GetDistinctCustomersNearestSlotFromToday", parameters, () => this.connection.QueryAsync<SlotEntity>(sql, parameters));

            return ResponseModelFactory.CreateCustomersFromSlotModelsResponse(slotEntities);
        }



        public async Task<Result<IEnumerable<SlotModel>>> GetCustomerAvailableSlots(PageParameter pageParameterModel, string email)
        {
            var parameters = new { IsDeleted = false, CreatedBy = email, PageNumber = pageParameterModel.PageNumber, PageSize = pageParameterModel.PageSize };
            var sql = SlotTableQueries.GetCustomerAvailableSlotsFromTodayQuery;

            var slotEntities = await this.dbInterceptor.GetQueryResults("GetCustomerAvailableSlots", parameters, () => this.connection.QueryAsync<SlotEntity>(sql, parameters));

            return ResponseModelFactory.CreateSlotModelsResponse(slotEntities);
        }

    }
}
