using AutoMapper;
using BLL.Dto;
using DAL.Entities;
using DAL.Repo.Abstraction;

namespace BLL.Services.TariffService
{
    public class TariffService : ITariffService
    {
        private readonly IRepo<Tariff> repo;
        private readonly IMapper mapper;
        public TariffService(IRepo<Tariff> Repo, IMapper Mapper)
        {
            repo = Repo;
            mapper = Mapper;
        }
        public async Task<(bool, string)> Add(TariffDto tariff)
        {
            try
            {
                if (tariff != null)
                {
                    var Tariff = mapper.Map<Tariff>(tariff);
                    await repo.Add(Tariff);
                    return (true, "Tariff Added Successfully");
                }
                throw new Exception("Entity Cannot be Null");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
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
