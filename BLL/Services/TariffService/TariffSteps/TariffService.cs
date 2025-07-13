using BLL.Dto;
using DAL.Entities;

namespace BLL.Services.TariffService
{
    public partial class TariffService
    {
        public async Task<(bool, string)> Add(TariffStepsDto tariff)
        {
            try
            {
                if (tariff != null)
                {
                    var CurrentTariff = await repo.Get(a => a.Id == tariff.TariffId);
                    var Activity = await ActivityRepo.Get(a => a.Code == CurrentTariff.ActivityTypeId);
                    var Tariff = mapper.Map<TariffSteps>(tariff);
                    
                    if (tariff.IsRecalculated)
                    {
                        Tariff.RecalculationEdge = tariff.From -1;
                        (decimal Total ,decimal bure) result = await customer.CalculateConsumptions(Tariff.RecalculationEdge,Activity.Code);
                        Tariff.RecalculationAddedAmount = (Tariff.RecalculationEdge * tariff.Price)-(result.bure);
                    }                  
                    await steps.Add(Tariff);
                    return (true, "Tariff Step Added Successfully");
                }
                throw new Exception("Entity Cannot be Null");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
        public async Task<(bool, string)> Edit(TariffStepsDto tariff)
        {
            try
            {
                if (tariff is null)
                    throw new Exception("Invalid Tariff Data!");
                var Tariff = tariff.TariffId;
                var ExistingTariff =await steps.Get(a=>a.Id == Tariff);
                if (ExistingTariff is null)
                    throw new Exception("Tariff not Found!");
                 mapper.Map(tariff,ExistingTariff);
                await steps.Edit(ExistingTariff);
                return (true, "user edited Successfully");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
        public async Task<(bool, string)> DeleteTariffStep(int id)
        {
            try
            {
                var exists = await steps.Get(a=>a.Id == id);
                if (exists is null)
                    throw new Exception("Tariff Steps not Found in DB!");
                var result = steps.Delete(exists);
                return (true, "Tariff Step Removed Successfully");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
        public async Task<List<TariffStepsDto>> GetByTariffId(int code)
        {
            var exists = await steps.Get(a => a.Id == code);
            if (exists is null)
                throw new Exception("No Tariffs For this Activity");
            var result = mapper.Map<List<TariffStepsDto>>(exists);
            return result;
        }
    }
}
