using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Common.Database.Interfaces;
using Bookmyslot.Api.SlotScheduler.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using Bookmyslot.Api.SlotScheduler.Repositories.Enitites;
using Bookmyslot.Api.SlotScheduler.Repositories.ModelFactory;
using Bookmyslot.Api.SlotScheduler.Repositories.Queries;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Repositories
{
    public class SlotRepository : ISlotRepository
    {
        private readonly IDbConnection connection;
        private readonly ISqlInterceptor sqlInterceptor;

        public SlotRepository(IDbConnection connection, ISqlInterceptor sqlInterceptor)
        {
            this.connection = connection;
            this.sqlInterceptor = sqlInterceptor;
        }

        public async Task<Response<IEnumerable<SlotModel>>> GetAllSlots(PageParameterModel pageParameterModel)
        {
            var parameters = new { PageNumber = pageParameterModel.PageNumber, PageSize = pageParameterModel.PageSize };
            var sql = SlotTableQueries.GetAllSlotsQuery;

            var slotEntities = await this.sqlInterceptor.GetQueryResults("GetAllSlots", parameters, () => this.connection.QueryAsync<SlotEntity>(sql, parameters));

            return ResponseModelFactory.CreateSlotModelsResponse(slotEntities);
        }



        public async Task<Response<Guid>> CreateSlot(SlotModel slotModel)
        {
            var slotEntity = EntityFactory.EntityFactory.CreateSlotEntity(slotModel);

            var parameters = new { slotEntity = slotEntity };
            await this.sqlInterceptor.GetQueryResults("CreateSlot", parameters, () => this.connection.InsertAsync<Guid, SlotEntity>(slotEntity));

            return new Response<Guid>() { Result = slotEntity.Id };
        }

        public async Task<Response<bool>> DeleteSlot(SlotModel slotModel)
        {
            var slotEntity = EntityFactory.EntityFactory.DeleteSlotEntity(slotModel);

            var parameters = new { slotEntity = slotEntity };
            await this.sqlInterceptor.GetQueryResults("DeleteSlot", parameters, () => this.connection.UpdateAsync<SlotEntity>(slotEntity));
            
            return new Response<bool>() { Result = true };
        }

        public async Task<Response<SlotModel>> GetSlot(Guid slotId)
        {
            var parameters = new { slotId = slotId };
            var slotEntity = await this.sqlInterceptor.GetQueryResults("GetSlot", parameters, () => this.connection.GetAsync<SlotEntity>(slotId));

            
            return ResponseModelFactory.CreateSlotModelResponse(slotEntity);
        }

     

        public async Task<Response<bool>> UpdateSlot(SlotModel slotModel)
        {
            var slotEntity = EntityFactory.EntityFactory.UpdateSlotEntity(slotModel);

            var parameters = new { slotEntity = slotEntity };
            await this.sqlInterceptor.GetQueryResults("UpdateSlot", parameters, () => this.connection.UpdateAsync<SlotEntity>(slotEntity));
            
            return new Response<bool>() { Result = true };
        }

      
    }
}
