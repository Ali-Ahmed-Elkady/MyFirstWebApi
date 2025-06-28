using AutoMapper;
using BLL.Dto;
using BLL.Services.Unified_Response;
using DAL.Entities;
using DAL.Repo.Abstraction;

namespace BLL.Services.TariffService
{
    public partial class TariffService : ITariffService
    {
        private readonly IRepo<Tariff> repo;
        private readonly IRepo<TariffSteps> steps;
        private readonly IMapper mapper;
      
        public TariffService(IRepo<Tariff> Repo, IMapper Mapper, IRepo<TariffSteps> steps)
        {
            repo = Repo;
            mapper = Mapper;
            this.steps = steps;
        }
        public async Task<UnifiedResponse<TariffDto>> Add(TariffDto tariff)
        {
            try
            {
                if (tariff != null)
                {
                    var Tariff = mapper.Map<Tariff>(tariff);
                    (bool isSucess ,string message) result = await repo.Add(Tariff);
                    if(result.isSucess)
                    return UnifiedResponse<TariffDto>.SuccessResult(tariff,"tariff added successfully");
                    return UnifiedResponse<TariffDto>.ErrorResult("An Error Happend While Adding Tariff");
                }
                return UnifiedResponse<TariffDto>.ErrorResult("Tariff cannot be null");
            }
            catch (Exception ex)
            {
                return UnifiedResponse<TariffDto>.ErrorResult(ex.Message);
            }
        }

        public async Task<(bool, string)> Delete(int code)
        {
            try
            {
                var exists = await repo.Get(a => a.Id == code);
                if (exists is null)
                    throw new Exception("Tariff not Found in DB!");
                var result = repo.Delete(code);
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
                var Tariff = tariff.ActivityTypeId;
                var ExistingTariff = repo.Get();
                if (ExistingTariff is null)
                    throw new Exception("Tariff not Found!");
                var Tariffs = mapper.Map<Tariff>(tariff);
                await repo.Edit(Tariffs);
                return (true, "user edited Successfully");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<List<TariffDto>> GetByActivityType(int code)
        {
            var exists = await repo.Get(a => a.ActivityTypeId == code);
            if(exists is null)
            throw new Exception("No Tariffs For this Activity");
            var result = mapper.Map<List<TariffDto>>(exists);
            return result;
        }
    }
}
