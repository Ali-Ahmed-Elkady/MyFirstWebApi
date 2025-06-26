using BLL.Dto;
using BLL.Helper;
using DAL.Entities;
using Microsoft.AspNetCore.Http;

namespace BLL.Services.CustomersService
{
    public partial class Customer
    {  
        public async Task<(bool, string)> AddConsumption(CustomerConsumptionDTO CustomerConsumption)
        {
            try
            {
                if (CustomerConsumption != null)
                {
                    var customerConsumption = mapper.Map<CustomerConsumptions>(CustomerConsumption);
                    await RepoConsumption.Add(customerConsumption);
                    return (true, "Customer Added Successfully");
                }
                throw new Exception("Entity Cannot be Null");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
        public async Task<bool> UploadConsumption(IFormFile file)
        {
            var result = mapper.Map<List<CustomerConsumptionDTO>, List<CustomerConsumptions>>(await file.UploadSheet<CustomerConsumptionDTO>());
            await RepoConsumption.AddRange(result);
            return true;
        }
        public async Task<List<CustomerConsumptionDTO>> GetCustomerConsumptions(long CustomerCode)
        {
            var Customer = await RepoConsumption.Get(a => a.CustomerCode == CustomerCode);
            var result = mapper.Map<List<CustomerConsumptionDTO>>(Customer);
            return result;
        }
        public async Task<List<CustomerConsumptionDTO>> GetAllConsumptions()
        {
            var Customer = await RepoConsumption.Get();
            var result = mapper.Map<List<CustomerConsumptionDTO>>(Customer);
            return result;
        }
        public async Task<(bool, string)> EditConsumption(CustomerConsumptionDTO Customer)
        {
            try
            {
                if (Customer is null)
                    throw new Exception("Invalid Customer Data!");
                var customerCode = Customer.CustomerCode;
                var ExistingCustomer = GetCustomerConsumptions(customerCode);
                if (ExistingCustomer is null)
                    throw new Exception("Customer not Found!");
                var customer = mapper.Map<CustomerConsumptions>(Customer);
                await RepoConsumption.Edit(customer);
                return (true, "user edited Successfully");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
        public async Task<(bool, string)> DeleteConsumptions(long CustomerCode)
        {
            try
            {
                var result = await RepoConsumption.Get(a => a.CustomerCode == CustomerCode);

                if (result is null)
                    throw new Exception("Customer Not Found!");
                await RepoConsumption.Delete(CustomerCode);
                
                return (true, "Customer deleted Successfully");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}
