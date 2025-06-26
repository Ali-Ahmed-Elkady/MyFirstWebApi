using BLL.Dto;
using Microsoft.AspNetCore.Http;

namespace BLL.Services.CustomersService
{
    public interface ICustomer
    {
        public Task<(bool, string)> Add(CustomerDto Customer);
        public Task<(bool, string)> Edit(CustomerDto Customer);
        public Task<(bool, string)> Delete(long id);
        public Task<List<CustomerDto>> GetByCustomerCode(long CustomerCode);
        public Task<List<CustomerDto>> GetAll( );
        public Task<bool> Upload(IFormFile file);
        //Consumptions
        public Task<(bool, string)> AddConsumption(CustomerConsumptionDTO Customer);
        public Task<bool> UploadConsumption(IFormFile file);
        public Task<List<CustomerConsumptionDTO>> GetCustomerConsumptions(long CustomerCode);
        public Task<List<CustomerConsumptionDTO>> GetAllConsumptions();
        public Task<(bool, string)> DeleteConsumptions(long CustomerCode);
        public Task<(bool, string)> EditConsumption(CustomerConsumptionDTO Customer);
    }
}
