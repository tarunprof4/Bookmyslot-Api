using Bookmyslot.Api.Customers.Contracts;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Bookmyslot.Api.Customers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerBusiness customerBusiness;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerController"/> class. 
        /// </summary>
        /// <param name="customerBusiness">customer Business</param>
        /// <param name="customerRepository">The customer repository</param>
        public CustomerController(ICustomerBusiness customerBusiness)
        {
            this.customerBusiness = customerBusiness;
        }

        //GET: api/<CustomerController>
        [HttpGet]
        public async Task<IEnumerable<CustomerModel>> Get()
        {
            Log.Information("Get all Customers ");
            var customerResponse = await customerBusiness.GetAllCustomers();
            return customerResponse.Result;
        }


        [HttpGet("{email}")]
        public async Task<CustomerModel> Get(string email)
        {
            Log.Information("Get Customer Email " + email);
            var customerResponse = await customerBusiness.GetCustomer(email);
            return customerResponse.Result;
        }


        // POST api/<CustomerController>
        [HttpPost]
        public async Task Post([FromBody] CustomerModel customerModel)
        {
            Log.Information("Create Customer " + customerModel);
            var customerResponse = await customerBusiness.CreateCustomer(customerModel);
            //return customerResponse.Result;
        }

        // PUT api/<CustomerController>/5
        [HttpPut("{email}")]
        public async Task Put([FromBody] CustomerModel customerModel)
        {
            Log.Information("Update Customer " + customerModel);
            var customerResponse = await customerBusiness.UpdateCustomer(customerModel);

        }

        // DELETE api/<CustomerController>/5
        [HttpDelete("{email}")]
        public async Task Delete(string email)
        {
            Log.Information("Delete Customer " + email);
            var customerResponse = await customerBusiness.DeleteCustomer(email);
        }
    }
}
