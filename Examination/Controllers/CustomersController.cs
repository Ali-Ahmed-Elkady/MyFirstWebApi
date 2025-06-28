using BLL.Dto;
using BLL.Services.CustomersService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Examination.Controllers
{
    [ApiController]
    [Route("[action]")]  
    public class CustomersController : Controller
    {
        private readonly ICustomer customers;
        public CustomersController(ICustomer customer)
        {
            customers = customer;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await customers.GetAll();
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetByCustomerCode(long customerCode)
        {
            var result = await customers.GetByCustomerCode(customerCode);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> AddCustomer([FromBody] CustomerDto customer)
        {
            if (customer == null)
            {
                return BadRequest("Invalid customer data.");
            }

            var (success, message) = await customers.Add(customer);
            return success ? Ok(message) : BadRequest(message);
        }
        [HttpPut]
        public async Task<IActionResult> EditCustomer(CustomerDto customer)
        {
            var (success, message) = await customers.Edit(customer);
            return success ? Ok(message) : BadRequest(message);

        }
        [HttpPost]
        public async Task<IActionResult> UploadExcel(IFormFile file)
        {           
            await customers.Upload(file);           
            return Ok("File uploaded successfully");
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteCustomer(long CustomerCode)
        {
           var result =await customers.Delete(CustomerCode);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCustomersConsumptions()
        {
            var result = await customers.GetAllConsumptions();
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetConsumptionByCustomerCode(long customerCode)
        {
            var result = await customers.GetCustomerConsumptions(customerCode);
            return Ok(result);
        }
    }
}
