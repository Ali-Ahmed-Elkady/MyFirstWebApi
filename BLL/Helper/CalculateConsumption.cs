using BLL.Dto;
using DAL.Entities;
using DAL.Repo.Abstraction;

namespace BLL.Helper
{
    public  class CalculateConsumption
    {
        private readonly IRepo<Customers> repo;
        private readonly IRepo<Tariff> repoTariff;
        private readonly IRepo<TariffSteps> repoSteps;
        public CalculateConsumption(IRepo<Customers> repo, IRepo<Tariff> repoTariff, IRepo<TariffSteps> repoSteps)
        {
            this.repo = repo;   
            this.repoTariff = repoTariff;
            this.repoSteps = repoSteps;
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
                        totalAmount += (remainingKW * step.Price) + step.RecalculationAddedAmount+step.ServicePrice;
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

    }
}
