using AutoMapper;
using BLL.Dto;
using BLL.Helper;
using BLL.Services.Unified_Response;
using DAL.Entities;
using DAL.Repo.Abstraction;
using Microsoft.AspNetCore.Http;
using System.Net;
namespace BLL.Services.CustomersService
{
    public partial class Customer : ICustomer
    {
        private readonly IRepo<Customers> repo;
        private readonly IMapper mapper;
        private readonly IRepo<CustomerConsumptions> RepoConsumption;
        private readonly IRepo<Esdar> repoEsdar;
        public Customer(IRepo<Esdar> EsdarRepo,IRepo<Customers> Repo, IMapper Mapper,IRepo<CustomerConsumptions>repoConsumption,IRepo<Tariff>tariff,IRepo<TariffSteps>steps)
        {
            repo = Repo;
            mapper = Mapper;
            RepoConsumption = repoConsumption;
            repoTariff = tariff;
            repoSteps = steps;
            repoEsdar = EsdarRepo;
        }
        public async Task<UnifiedResponse<CustomerDto>> Add(CustomerDto Customer , string UserName)
        {
            try
            {
                if (Customer != null&& UserName !=null)
                {
                    var customer = mapper.Map<Customers>(Customer);
                    customer.Create(UserName);
                    await repo.Add(customer);
                    return UnifiedResponse<CustomerDto>.SuccessResult(Customer,HttpStatusCode.OK);
                }
                throw new Exception("Entity Cannot be Null");
            }
            catch (Exception ex)
            {
                return UnifiedResponse<CustomerDto>.ErrorResult(new List<string> { ex.Message},ex.Message, HttpStatusCode.NotFound);
            }
        }
        public async Task<UnifiedResponse<CustomerDto>> Delete(long CustomerCode)
        {
            try
            {
                var result = await repo.Get(a => a.CustomerCode == CustomerCode);
                var result2 = mapper.Map<CustomerDto>(result);
                if (result is null)
                    throw new Exception("Customer Not Found!");              
                    result.IsDeleted = true; 
                await repo.Edit(result);
                return UnifiedResponse<CustomerDto>.SuccessResult(result2, HttpStatusCode.OK);
            }
            catch(Exception ex)
            {
                return UnifiedResponse<CustomerDto>.ErrorResult(new List<string> { ex.Message},ex.Message, HttpStatusCode.NotFound);
            }
        }

        public async Task<UnifiedResponse<CustomerDto>> Edit(CustomerDto Customer , string UserName)
        {
            try
            {
                if (Customer is null)
                    throw new Exception("Invalid Customer Data!");
                var customerCode = Customer.CustomerCode;
                var ExistingCustomer = await repo.Get(a=>a.CustomerCode==Customer.CustomerCode);
                if (ExistingCustomer is null)
                    throw new Exception("Customer not Found!");
                mapper.Map(Customer, ExistingCustomer);
                ExistingCustomer.Update(UserName);
                (bool isSucess ,string message) result =await repo.Edit(ExistingCustomer);
                if(result.isSucess)
                return UnifiedResponse<CustomerDto>.SuccessResult(Customer, HttpStatusCode.OK);
                throw new Exception(result.message);
            }
            catch (Exception ex)
            {
                return UnifiedResponse<CustomerDto>.ErrorResult(new List<string> { ex.Message},ex.Message, HttpStatusCode.NotFound);
            }

        }

        public async Task<UnifiedResponse<List<CustomerDto>>> GetAll()
        {
            try
            {
                var Customer = await repo.GetAll();
                if (Customer is null || Customer.Count == 0)
                    throw new Exception("There is no customers in DB!");
                var result = mapper.Map<List<CustomerDto>>(Customer);
                return UnifiedResponse<List<CustomerDto>>.SuccessResult(result, HttpStatusCode.NotFound);
            }
            catch(Exception ex)
            {
                return UnifiedResponse<List<CustomerDto>>.ErrorResult(new List<string> { ex.Message},ex.Message, HttpStatusCode.NotFound);
            }
        }

        public async Task<UnifiedResponse<CustomerDto>> GetByCustomerCode(long CustomerCode)
        {
            try
            {
                var Customer = await repo.Get(a => a.CustomerCode == CustomerCode);
                if (Customer is null)
                    throw new Exception("No Customer Matches the current Code");
                var result = mapper.Map<CustomerDto>(Customer);
                return UnifiedResponse<CustomerDto>.SuccessResult(result, HttpStatusCode.NotFound);
            }
            catch(Exception ex)
            {
                return UnifiedResponse<CustomerDto>.ErrorResult(new List<string> { ex.Message},ex.Message, HttpStatusCode.NotFound);
            }
        }
        public async Task<UnifiedResponse<bool>> Upload(IFormFile file)
        {
            try
            {
                var result = mapper.Map<List<CustomerDto>, List<Customers>>(await file.UploadSheet<CustomerDto>());
                await repo.AddRange(result);
                return UnifiedResponse<bool>.SuccessResult(true, HttpStatusCode.NotFound);
            }
            catch(Exception ex)
            {
                return UnifiedResponse<bool>.ErrorResult(new List<string> { ex.Message},ex.Message, HttpStatusCode.NotFound);
            }
        }        
    }
    }

