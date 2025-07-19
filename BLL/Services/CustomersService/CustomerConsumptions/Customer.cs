using BLL.Dto;
using BLL.Helper;
using BLL.Services.Unified_Response;
using DAL.Entities;
using DAL.Repo.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.Net;

namespace BLL.Services.CustomersService
{
    public partial class Customer
    {
        private readonly IRepo<Tariff> repoTariff;
        private readonly IRepo<TariffSteps> repoSteps;
        public async Task<UnifiedResponse<CustomerConsumptionDTO>> AddConsumption(CustomerConsumptionDTO CustomerConsumption)
        {
            try
            {
                var Errors = new List<string>();
                if (CustomerConsumption != null)
                {
                    var result = await repo.Get(a => a.CustomerCode == CustomerConsumption.CustomerCode);
                    if (result == null) 
                    { 
                        Errors.Add("Customer not found in database");
                        return UnifiedResponse<CustomerConsumptionDTO>.ErrorResult(Errors, "Customer not found", HttpStatusCode.NotFound);
                    }
                    if (CustomerConsumption.ConsumptionKw < 0)
                    {
                        Errors.Add("Consumption must be greater or Equal zero");
                        return UnifiedResponse<CustomerConsumptionDTO>.ErrorResult(Errors, "Invalid consumption value", HttpStatusCode.BadRequest);
                    }
                    var customerConsumption = mapper.Map<CustomerConsumptions>(CustomerConsumption);
                    await RepoConsumption.Add(customerConsumption);
                    return UnifiedResponse<CustomerConsumptionDTO>.SuccessResult(CustomerConsumption,HttpStatusCode.OK);
                }
                Errors.Add("Entity Cannot be Null");
                return UnifiedResponse<CustomerConsumptionDTO>.ErrorResult(Errors, "Entity cannot be null", HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                return UnifiedResponse<CustomerConsumptionDTO>.ErrorResult(new List<string> { ex.Message },ex.Message, HttpStatusCode.NotFound);
            }
        }
        public async Task<UnifiedResponse<List<CustomerConsumptionDTO>>> UploadConsumption(IFormFile file)
        {
            try
            {
                var UploadedCustomers = await file.UploadSheet<CustomerConsumptionDTO>();
                var result = mapper.Map<List<CustomerConsumptionDTO>, List<CustomerConsumptions>>(UploadedCustomers);
                await RepoConsumption.AddRange(result);
                return UnifiedResponse<List<CustomerConsumptionDTO>>.SuccessResult(UploadedCustomers, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return UnifiedResponse<List<CustomerConsumptionDTO>>.ErrorResult(new List<string> { ex.Message },ex.Message,HttpStatusCode.NotFound);
            }
        }
        public async Task<UnifiedResponse<List<CustomerConsumptionDTO>>> GetCustomerConsumptions(long CustomerCode)
        {
            try
            {
                var CustomerConsumptions = await RepoConsumption.GetAll(a => a.CustomerCode == CustomerCode);
                if (CustomerConsumptions is null || CustomerConsumptions.Count == 0)
                    throw new Exception("Customer has no consumptions");
                var result = mapper.Map<List<CustomerConsumptionDTO>>(CustomerConsumptions);
                return UnifiedResponse<List<CustomerConsumptionDTO>>.SuccessResult(result, HttpStatusCode.NotFound);
            }catch(Exception ex)
            {
                return UnifiedResponse<List<CustomerConsumptionDTO>>.ErrorResult(new List<string> { ex.Message },ex.Message, HttpStatusCode.NotFound);
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
                return UnifiedResponse<List<CustomerConsumptionDTO>>.SuccessResult(result, HttpStatusCode.NotFound);
            }
            catch(Exception ex)
            {
                return UnifiedResponse<List<CustomerConsumptionDTO>>.ErrorResult(new List<string> { ex.Message },ex.Message, HttpStatusCode.NotFound);
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
                
                return UnifiedResponse<bool>.SuccessResult(true, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return UnifiedResponse<bool>.ErrorResult(new List<string> { ex.Message },ex.Message, HttpStatusCode.NotFound);
            }
        }
        public async Task<decimal> CalculateConsumptions(CustomerConsumptionDTO Customer)
        {
            try
            {
                var customer = await repo.Get(a => a.CustomerCode == Customer.CustomerCode);
                if (customer == null)
                    throw new Exception("Customer not found in database");

                var tariff = await repoTariff.Get(a => a.ActivityTypeId == customer.ActivityId);
                if (tariff == null)
                    throw new Exception("No tariff assigned to this activity");

                var tariffSteps = (await repoSteps.GetAll(a => a.TariffId == tariff.Id))
                    .OrderBy(s => s.From)
                    .ToList();

                decimal remainingKW = Customer.ConsumptionKw;
                decimal totalAmount = 0.0m;

                foreach (var step in tariffSteps)
                {
                    decimal stepSize = step.To - step.From + 1;

                    if (remainingKW <= 0)
                        break;

                    if (remainingKW > stepSize)
                    {
                        totalAmount += (stepSize * step.Price) + step.RecalculationAddedAmount;
                        remainingKW -= stepSize;
                    }
                    else
                    {
                        totalAmount += (remainingKW * step.Price) + step.RecalculationAddedAmount + step.ServicePrice;
                        remainingKW = 0;
                        break;
                    }
                }

                return totalAmount;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error calculating consumption cost: {ex.Message}");
            }
        }
        public async Task<(decimal,decimal)> CalculateConsumptions(int ConsumptionKw , int ActivityCode)
        {
            try
            {
                var tariff = await repoTariff.Get(a => a.ActivityTypeId == ActivityCode);
                if (tariff == null)
                    throw new Exception("No tariff assigned to this activity");
                if(ConsumptionKw == 0)
                {
                    return (tariff.ZeroReading,tariff.ZeroReading);
                }
                var tariffSteps = (await repoSteps.GetAll(a => a.TariffId == tariff.Id))
                    .OrderBy(s => s.From)
                    .ToList();

                decimal remainingKW = ConsumptionKw;
                decimal totalAmount = 0.0m;
                var BureConsumption = 0.0m;

                foreach (var step in tariffSteps)
                {
                    decimal stepSize = step.To - step.From + 1;

                    if (remainingKW <= 0)
                        break;

                    if (remainingKW > stepSize)
                    {
                        totalAmount += (stepSize * step.Price) + step.RecalculationAddedAmount;
                        remainingKW -= stepSize;
                    }
                    else
                    {
                        totalAmount += (remainingKW * step.Price) + step.RecalculationAddedAmount + step.ServicePrice;
                         BureConsumption = totalAmount - step.ServicePrice;
                        remainingKW = 0;
                        break;
                    }
                }

                return (totalAmount,BureConsumption);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error calculating consumption cost: {ex.Message}");
            }
        }
        public async Task<UnifiedResponse<List<EsdarDto>>> IsdarConsumptions()
        {
            var CustomerConsumption = new List<Esdar>();
            var result = await RepoConsumption.GetAll();
            var Customers = mapper.Map<List<CustomerConsumptionDTO>>(result);
            if (result is null || result.Count == 0)
            {
                return UnifiedResponse<List<EsdarDto>>.ErrorResult(new List<string> { "No consumptions found" }, "You Cannot Use Isdar Before Uploading Consumptions ", HttpStatusCode.NotFound);
            }
            foreach(var item in Customers)
            {         
               var element = await CalculateConsumptions(item);
               CustomerConsumption.Add(new Esdar
               {
                   ConsumptionKw=item.ConsumptionKw,
                   EsdarDate = DateTime.Now,
                   ConsumptionAmount = element,
                   CustomerConsumptionsId= item.Id,
               });
               
            }
            await repoEsdar.AddRange(CustomerConsumption);
            var response = mapper.Map<List<EsdarDto>>(CustomerConsumption);
            return UnifiedResponse<List<EsdarDto>>.SuccessResult(response, HttpStatusCode.OK);

        }
    }
}
