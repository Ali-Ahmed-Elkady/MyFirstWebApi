﻿using AutoMapper;
using BLL.Dto;
using BLL.Services.CustomersService;
using BLL.Services.Unified_Response;
using DAL.Entities;
using DAL.Repo.Abstraction;
using System.Net;

namespace BLL.Services.TariffService
{
    public partial class TariffService : ITariffService
    {
        private readonly IRepo<ActivityType> ActivityRepo;
        private readonly IRepo<Tariff> repo;
        private readonly IRepo<TariffSteps> steps;
        private readonly IMapper mapper;
        private readonly ICustomer customer;
      
        public TariffService(IRepo<Tariff> Repo, IMapper Mapper, IRepo<TariffSteps> steps, IRepo<ActivityType> activityRepo ,ICustomer customer)
        {
            repo = Repo;
            mapper = Mapper;
            this.steps = steps;
            ActivityRepo = activityRepo;
            this.customer = customer;       }
        public async Task<UnifiedResponse<TariffDto>> Add(TariffDto tariff)
        {
            try
            {
                if (tariff != null)
                {
                    List<string> errors = new();
                    var Activity =await ActivityRepo.Get(a => a.Code == tariff.ActivityTypeId);
                    tariff.ActivityTypeId = Activity.Id;
                    var Tariff = mapper.Map<Tariff>(tariff);
                    (bool isSucess ,string message) result = await repo.Add(Tariff);
                    if(result.isSucess)
                    return UnifiedResponse<TariffDto>.SuccessResult(tariff,HttpStatusCode.OK,"tariff added successfully");
                    errors.Add("An error occurred while adding the tariff");
                    return UnifiedResponse<TariffDto>.ErrorResult(errors,"An Error Happened While Adding Tariff",HttpStatusCode.NotFound);
                }
                return UnifiedResponse<TariffDto>.ErrorResult(new List<string> { "Error"},"Tariff cannot be null", HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                return UnifiedResponse<TariffDto>.ErrorResult(new List<string> { ex.Message},ex.Message, HttpStatusCode.NotFound);
            }
        }

        public async Task<(bool, string)> Delete(int code)
        {
            try
            {
                var exists = await repo.Get(a => a.Id == code);
                if (exists is null)
                    throw new Exception("Tariff not Found in DB!");
                var result = repo.Delete(exists);
                return (true, "Tariff Removed Successfully");
            }
            catch(Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool, string)> Edit(TariffDto tariff)
        {
            try
            {
                if (tariff is null)
                    throw new Exception("Invalid Tariff Data!");
                var ExistingTariff =await repo.Get(a=>a.ActivityTypeId==tariff.ActivityTypeId);
                if (ExistingTariff is null)
                    throw new Exception("Tariff not Found!");
                 mapper.Map(tariff,ExistingTariff);
                await repo.Edit(ExistingTariff);
                return (true, "user edited Successfully");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<List<TariffDto>> GetByActivityType(int code)
        {
            var exists = await repo.GetAll(a => a.ActivityTypeId == code);
            if(exists is null)
            throw new Exception("No Tariffs For this Activity");
            var result = mapper.Map<List<TariffDto>>(exists);
            return result;
        }
    }
}
