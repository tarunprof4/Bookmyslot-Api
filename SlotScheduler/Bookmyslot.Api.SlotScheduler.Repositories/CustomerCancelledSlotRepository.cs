﻿using Bookmyslot.Api.Common.Contracts;
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
    public class CustomerCancelledSlotRepository : ICustomerCancelledSlotRepository
    {
        private readonly IDbConnection connection;
        private readonly ISqlInterceptor sqlInterceptor;
        public CustomerCancelledSlotRepository(IDbConnection connection, ISqlInterceptor sqlInterceptor)
        {
            this.connection = connection;
            this.sqlInterceptor = sqlInterceptor;
        }

        public async Task<Response<bool>> CreateCustomerCancelledSlot(CancelledSlotModel cancelledSlotModel)
        {
            var customerEntity = EntityFactory.EntityFactory.CreateCancelledSlotEntity(cancelledSlotModel);

            var parameters = new { cancelledSlotModel = cancelledSlotModel };
            await this.sqlInterceptor.GetQueryResults("CreateCustomerCancelledSlot", parameters, () => this.connection.InsertAsync<Guid, CancelledSlotEntity>(customerEntity));

            return new Response<bool>() { Result = true };
        }


        public async Task<Response<IEnumerable<CancelledSlotModel>>> GetCustomerSharedCancelledSlots(string customerId)
        {
            var parameters = new { IsDeleted = true, CancelledBy = customerId };
            var sql = SlotTableQueries.GetCustomerSharedByCancelledSlotsQuery;

            var cancelledSlotEntities = await this.sqlInterceptor.GetQueryResults("GetCustomerSharedCancelledSlots", parameters, () => this.connection.QueryAsync<CancelledSlotEntity>(sql, parameters));

            return ResponseModelFactory.CreateCancelledSlotModels(cancelledSlotEntities);
        }

        

        public async Task<Response<IEnumerable<CancelledSlotModel>>> GetCustomerBookedCancelledSlots(string customerId)
        {
            var parameters = new { IsDeleted = true, CancelledBy = customerId, BookedBy = customerId };
            var sql = SlotTableQueries.GetCustomerBookedByCancelledSlotsQuery;

            var cancelledSlotEntities = await this.sqlInterceptor.GetQueryResults("GetCustomerBookedCancelledSlots", parameters, () => this.connection.QueryAsync<CancelledSlotEntity>(sql, parameters));

            return ResponseModelFactory.CreateCancelledSlotModels(cancelledSlotEntities);
        }


    }
}
