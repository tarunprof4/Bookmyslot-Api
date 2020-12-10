using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using Bookmyslot.Api.SlotScheduler.Repositories.Enitites;
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

        public SlotRepository(IDbConnection connection)
        {
            this.connection = connection;
        }

        public async Task<Response<Guid>> CreateSlot(SlotModel slotModel)
        {
            var slotEntity = EntityFactory.EntityFactory.CreateSlotEntity(slotModel);
            await this.connection.InsertAsync<Guid, SlotEntity>(slotEntity);
            return new Response<Guid>() { Result = slotEntity.Id };
        }

        public async Task<Response<bool>> DeleteSlot(SlotModel slotModel)
        {
            var slotEntity = EntityFactory.EntityFactory.DeleteSlotEntity(slotModel);
            await this.connection.UpdateAsync<SlotEntity>(slotEntity);
            return new Response<bool>() { Result = true };
        }

        public async Task<Response<IEnumerable<SlotModel>>> GetAllSlotsDateRange(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<SlotModel>> GetSlot(Guid slotId)
        {
            var slotEntity = await this.connection.GetAsync<SlotEntity>(slotId);
            if (slotEntity == null)
            {
                return new Response<SlotModel>();
            }

            var customerModel = ModelFactory.ModelFactory.CreateSlotModel(slotEntity);
            return new Response<SlotModel>() { Result = customerModel };
        }

        public async Task<Response<bool>> UpdateSlot(SlotModel slotModel)
        {
            var slotEntity = EntityFactory.EntityFactory.UpdateSlotEntity(slotModel);
            await this.connection.UpdateAsync<SlotEntity>(slotEntity);
            return new Response<bool>() { Result = true };
        }
    }
}
