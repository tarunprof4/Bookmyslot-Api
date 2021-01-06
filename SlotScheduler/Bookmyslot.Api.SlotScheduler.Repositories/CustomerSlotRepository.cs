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
    public class CustomerSlotRepository : ICustomerSlotRepository
    {
        private readonly IDbConnection connection;

        public CustomerSlotRepository(IDbConnection connection)
        {
            this.connection = connection;
        }


        public async Task<Response<IEnumerable<SlotModel>>> GetDistinctCustomersNearestSlotFromToday(PageParameterModel pageParameterModel)
        {
            var parameters = new { IsDeleted = false, PageNumber = pageParameterModel.PageNumber, PageSize = pageParameterModel.PageSize };
            var sql = SlotTableQueries.GetDistinctCustomersNearestSlotFromTodayQuery;

            var slotEntities = await this.connection.QueryAsync<SlotEntity>(sql, parameters);

            var slotModels = ModelFactory.ModelFactory.CreateSlotModels(slotEntities);
            if (slotModels.Count == 0)
            {
                return Response<IEnumerable<SlotModel>>.Empty(new List<string>() { AppBusinessMessages.NoRecordsFound });
            }

            return new Response<IEnumerable<SlotModel>>() { Result = slotModels };
        }

        public async Task<Response<IEnumerable<SlotModel>>> GetCustomerAvailableSlots(PageParameterModel pageParameterModel, string email)
        {
            var parameters = new { IsDeleted = false, CreatedBy= email, PageNumber = pageParameterModel.PageNumber, PageSize = pageParameterModel.PageSize };
            var sql = SlotTableQueries.GetCustomerAvailableSlotsFromTodayQuery;

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
