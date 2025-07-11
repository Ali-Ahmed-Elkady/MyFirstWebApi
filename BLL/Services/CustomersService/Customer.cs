﻿using AutoMapper;
using BLL.Dto;
using BLL.Helper;
using BLL.Services.Unified_Response;
using DAL.Entities;
using DAL.Repo.Abstraction;
using Microsoft.AspNetCore.Http;
namespace BLL.Services.CustomersService
{
    public partial class Customer : ICustomer
    {
        private readonly IRepo<Customers> repo;
        private readonly IMapper mapper;
        private readonly IRepo<CustomerConsumptions> RepoConsumption;
        public Customer(IRepo<Customers> Repo, IMapper Mapper,IRepo<CustomerConsumptions>repoConsumption)
        {
            repo = Repo;
            mapper = Mapper;
            RepoConsumption = repoConsumption;
        }
        public async Task<UnifiedResponse<CustomerDto>> Add(CustomerDto Customer)
        {
            try
            {
                if (Customer != null)
                {
                    var customer = mapper.Map<Customers>(Customer);
                    await repo.Add(customer);
                    return UnifiedResponse<CustomerDto>.SuccessResult(Customer);
                }
                throw new Exception("Entity Cannot be Null");
            }
            catch (Exception ex)
            {
                return UnifiedResponse<CustomerDto>.ErrorResult(ex.Message);
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
                return UnifiedResponse<CustomerDto>.SuccessResult(result2);
            }
            catch(Exception ex)
            {
                return UnifiedResponse<CustomerDto>.ErrorResult(ex.Message);
            }
        }

        public async Task<UnifiedResponse<CustomerDto>> Edit(CustomerDto Customer)
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
                ExistingCustomer.ModifiedAt = DateTime.Now;
                (bool isSucess ,string message) result =await repo.Edit(ExistingCustomer);
                if(result.isSucess)
                return UnifiedResponse<CustomerDto>.SuccessResult(Customer);
                throw new Exception(result.message);
            }
            catch (Exception ex)
            {
                return UnifiedResponse<CustomerDto>.ErrorResult(ex.Message);
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
                return UnifiedResponse<List<CustomerDto>>.SuccessResult(result);
            }
            catch(Exception ex)
            {
                return UnifiedResponse<List<CustomerDto>>.ErrorResult(ex.Message);
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
                return UnifiedResponse<CustomerDto>.SuccessResult(result);
            }
            catch(Exception ex)
            {
                return UnifiedResponse<CustomerDto>.ErrorResult(ex.Message);
            }
        }
        public async Task<UnifiedResponse<bool>> Upload(IFormFile file)
        {
            try
            {
                var result = mapper.Map<List<CustomerDto>, List<Customers>>(await file.UploadSheet<CustomerDto>());
                await repo.AddRange(result);
                return UnifiedResponse<bool>.SuccessResult(true);
            }
            catch(Exception ex)
            {
                return UnifiedResponse<bool>.ErrorResult(ex.Message);
            }
        }        
    }
    }

