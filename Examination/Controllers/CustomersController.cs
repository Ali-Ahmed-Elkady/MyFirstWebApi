using BLL.Dto;
using BLL.Services.CustomersService;
using BLL.Services.Unified_Response;
using DAL.Repo.Implementation;
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
            var response = await customers.GetAll();
            if(response.Success)
            return Ok(response);
            return BadRequest(response.Message);
        }
        [HttpGet]
        public async Task<IActionResult> GetByCustomerCode(long customerCode)
        {
            var result = await customers.GetByCustomerCode(customerCode);
            if(result.Success)
            return Ok(result);
            return BadRequest(result);
        }
        [HttpPost]
        public async Task<IActionResult> AddCustomer([FromBody] CustomerDto customer)
        {
            if (customer == null)
            {
                return BadRequest("Invalid customer data.");
            }

            var response = await customers.Add(customer);
            return Ok(response);
        }
        [HttpPut]
        public async Task<IActionResult> EditCustomer(CustomerDto customer)
        {
            var response = await customers.Edit(customer);
            if(response.Success)
            return Ok(response);
            return BadRequest(response.Message);

        }
        [HttpPost]
        public async Task<IActionResult> UploadExcel(IFormFile file)
        {           
            var response =await customers.Upload(file);    
            if(response.Success)
            return Ok("File uploaded successfully");
            return BadRequest(response);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteCustomer(long CustomerCode)
        {
           var result =await customers.Delete(CustomerCode);
            if(result.Success)
            return Ok(result);
            return BadRequest(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCustomersConsumptions()
        {
            var result = await customers.GetAllConsumptions();
            if(result.Success)
            return Ok(result);
            return BadRequest(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetConsumptionByCustomerCode(long customerCode)
        {
            var result = await customers.GetCustomerConsumptions(customerCode);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPost]
        public async Task<IActionResult> AddCustomerConsumptions (CustomerConsumptionDTO consumption)
        {
            var result = await customers.AddConsumption(consumption);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteCustomerConsumption (long customerCode)
        {
            var result = await customers.DeleteConsumptions(customerCode);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPost]
        public async Task<IActionResult> CalculateConsumption (CustomerConsumptionDTO consumption)
        {
            try
            {
                var result = await customers.CalculateConsumptions(consumption);
                return Ok(new { Consumption = result });

            }
            catch (Exception ex)
            {
               return BadRequest(ex.Message);
            }
        }
    }
}
