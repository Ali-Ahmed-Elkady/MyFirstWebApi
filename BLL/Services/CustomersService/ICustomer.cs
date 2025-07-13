using BLL.Dto;
using BLL.Services.Unified_Response;
using Microsoft.AspNetCore.Http;

namespace BLL.Services.CustomersService
{
    public interface ICustomer
    {
        public Task<UnifiedResponse<CustomerDto>> Add(CustomerDto Customer);
        public Task<UnifiedResponse<CustomerDto>> Edit(CustomerDto Customer);
        public Task<UnifiedResponse<CustomerDto>> Delete(long id);
        public Task<UnifiedResponse<CustomerDto>> GetByCustomerCode(long CustomerCode);
        public Task<UnifiedResponse<List<CustomerDto>>> GetAll( );
        public Task<UnifiedResponse<bool>> Upload(IFormFile file);
        //Consumptions
        public Task<UnifiedResponse<CustomerConsumptionDTO>> AddConsumption(CustomerConsumptionDTO Customer);
        public Task<bool> UploadConsumption(IFormFile file);
        public Task<UnifiedResponse<List<CustomerConsumptionDTO>>> GetCustomerConsumptions(long CustomerCode);
        public Task<UnifiedResponse<List<CustomerConsumptionDTO>>> GetAllConsumptions();
        public Task<UnifiedResponse<bool>> DeleteConsumptions(long CustomerCode);
        public Task<decimal> CalculateConsumptions(CustomerConsumptionDTO Customer);
        public Task<(decimal,decimal)> CalculateConsumptions(int ConsumptionKw, int ActivityCode);
        //public Task<UnifiedResponse<CustomerConsumptionDTO>> EditConsumption(CustomerConsumptionDTO Customer);
    }
}
