using BLL.Dto;
using BLL.Helper;
using BLL.Services.Unified_Response;
using DAL.Entities;
using Microsoft.AspNetCore.Http;

namespace BLL.Services.CustomersService
{
    public partial class Customer
    {  
        public async Task<UnifiedResponse<CustomerConsumptionDTO>> AddConsumption(CustomerConsumptionDTO CustomerConsumption)
        {
            try
            {
                if (CustomerConsumption != null)
                {
                    var customerConsumption = mapper.Map<CustomerConsumptions>(CustomerConsumption);
                    await RepoConsumption.Add(customerConsumption);
                    return UnifiedResponse<CustomerConsumptionDTO>.SuccessResult(CustomerConsumption);
                }
                throw new Exception("Entity Cannot be Null");
            }
            catch (Exception ex)
            {
                return UnifiedResponse<CustomerConsumptionDTO>.ErrorResult(ex.Message);
            }
        }
        public async Task<bool> UploadConsumption(IFormFile file)
        {
            var result = mapper.Map<List<CustomerConsumptionDTO>, List<CustomerConsumptions>>(await file.UploadSheet<CustomerConsumptionDTO>());
            await RepoConsumption.AddRange(result);
            return true;
        }
        public async Task<UnifiedResponse<List<CustomerConsumptionDTO>>> GetCustomerConsumptions(long CustomerCode)
        {
            try
            {
                var CustomerConsumptions = await RepoConsumption.GetAll(a => a.CustomerCode == CustomerCode);
                if (CustomerConsumptions is null || CustomerConsumptions.Count == 0)
                    throw new Exception("Customer has no consumptions");
                var result = mapper.Map<List<CustomerConsumptionDTO>>(CustomerConsumptions);
                return UnifiedResponse<List<CustomerConsumptionDTO>>.SuccessResult(result);
            }catch(Exception ex)
            {
                return UnifiedResponse<List<CustomerConsumptionDTO>>.ErrorResult(ex.Message);
            }
        }
        public async Task<UnifiedResponse<List<CustomerConsumptionDTO>>> GetAllConsumptions()
        {
            try
            {
                var Customer = await RepoConsumption.GetAll();
                if (Customer is null || Customer.Count == 0)
                    throw new Exception("There is no Consumptions In DataBase");
                var result = mapper.Map<List<CustomerConsumptionDTO>>(Customer);
                return UnifiedResponse<List<CustomerConsumptionDTO>>.SuccessResult(result);
            }
            catch(Exception ex)
            {
                return UnifiedResponse<List<CustomerConsumptionDTO>>.ErrorResult(ex.Message);
            }
        }
        //public async Task<UnifiedResponse<CustomerConsumptionDTO>> EditConsumption(CustomerConsumptionDTO Customer)
        //{
        //    try
        //    {
        //        if (Customer is null)
        //            throw new Exception("Invalid Customer Data!");
        //        var ExistingCustomer = repo.Get(a => a.CustomerCode == Customer.CustomerCode);
        //        if(ExistingCustomer is null)
        //            throw new Exception("Customer not Found!");
        //        var customerConsumptions = RepoConsumption.GetAll(a => a.CustomerCode == Customer.CustomerCode);
        //        var customer = mapper.Map<CustomerConsumptions>(Customer);
        //        var result =await RepoConsumption.Edit(customer);
        //        return UnifiedResponse<CustomerConsumptionDTO>.SuccessResult(customer);
        //    }
        //    catch (Exception ex)
        //    {
        //        return UnifiedResponse<CustomerConsumptionDTO>.ErrorResult(ex.Message);
        //    }
        //}
        public async Task<UnifiedResponse<bool>> DeleteConsumptions(long CustomerCode)
        {
            try
            {
                var result = await RepoConsumption.Get(a => a.CustomerCode == CustomerCode);

                if (result is null)
                    throw new Exception("Customer Not Found!");
                var response = await RepoConsumption.Delete(result);
                
                return UnifiedResponse<bool>.SuccessResult(true);
            }
            catch (Exception ex)
            {
                return UnifiedResponse<bool>.ErrorResult(ex.Message);
            }
        }
    }
}
