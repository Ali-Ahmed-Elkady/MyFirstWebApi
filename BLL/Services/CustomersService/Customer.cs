using AutoMapper;
using BLL.Dto;
using BLL.Helper;
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
        public async Task<(bool, string)> Add(CustomerDto Customer)
        {
            try
            {
                if (Customer != null)
                {
                    var customer = mapper.Map<Customers>(Customer);
                    await repo.Add(customer);
                    return (true, "Customer Added Successfully");
                }
                throw new Exception("Entity Cannot be Null");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
        public async Task<(bool, string)>Delete(long CustomerCode)
        {
            try
            {
                var result = await repo.Get(a => a.CustomerCode == CustomerCode);

                if (result is null)
                    throw new Exception("Customer Not Found!");
               foreach(var item in result)
                {
                    item.IsDeleted = true;
                }
                await repo.EditRange(result);
                return (true, "Customer deleted Successfully");
            }catch(Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool, string)> Edit(CustomerDto Customer)
        {
            try
            {
                if (Customer is null)
                    throw new Exception("Invalid Customer Data!");
                var customerCode = Customer.CustomerCode;
                var ExistingCustomer = GetByCustomerCode(customerCode);
                if (ExistingCustomer is null)
                    throw new Exception("Customer not Found!");
                var customer = mapper.Map<Customers>(Customer);
                customer.ModifiedAt = DateTime.Now;
                await repo.Edit(customer);
                return (true, "user edited Successfully");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }

        }

        public async Task<List<CustomerDto>> GetAll()
        {
            var Customer = await repo.Get();
            var result = mapper.Map<List<CustomerDto>>(Customer);
            return result;
        }

        public async Task<List<CustomerDto>> GetByCustomerCode(long CustomerCode)
        {
          var Customer = await repo.Get(a => a.CustomerCode == CustomerCode);
          var result = mapper.Map<List<CustomerDto>>(Customer);
            return result;
        }
        public async Task<bool> Upload(IFormFile file)
        {
            var result = mapper.Map<List<CustomerDto>, List<Customers>>(await file.UploadSheet<CustomerDto>());
            await repo.AddRange(result);
            return true;
        }        
    }
    }

