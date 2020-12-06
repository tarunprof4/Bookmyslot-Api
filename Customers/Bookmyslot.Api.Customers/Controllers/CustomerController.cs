using Bookmyslot.Api.Common;
using Bookmyslot.Api.Customers.Contracts;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Collections.Generic;

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
        public IEnumerable<CustomerModel> Get()
        {
            Log.Information("Get all Customers ");
            var customerResponse = customerBusiness.GetAllCustomers();
            return customerResponse.Result;
        }


        [HttpGet("{email}")]
        public CustomerModel Get(string email)
        {
            Log.Information("Get Customer Email " + email);
            var customerResponse =  customerBusiness.GetCustomer(email);
            return customerResponse.Result;
        }


        //[HttpGet]
        //public Customer Get(string email)
        //{
        //    return customerBusiness.GetCustomer(email);
        //}

        // POST api/<CustomerController>
        [HttpPost]
        public void Post([FromBody] CustomerModel customerModel)
        {
            Log.Information("Create Customer " + customerModel);
            var customerResponse = customerBusiness.CreateCustomer(customerModel);
            //return customerResponse.Result;
        }

        // PUT api/<CustomerController>/5
        [HttpPut("{email}")]
        public void Put(string email, [FromBody] CustomerModel customerModel)
        {
            Log.Information("Update Customer " + customerModel);
            var customerResponse = customerBusiness.UpdateCustomer(customerModel);

        }

        // DELETE api/<CustomerController>/5
        [HttpDelete("{email}")]
        public void Delete(string email)
        {
            Log.Information("Delete Customer " + email);
            var customerResponse = customerBusiness.DeleteCustomer(email);

        }
    }
}
