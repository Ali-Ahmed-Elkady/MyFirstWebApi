using BLL.Dto;
using BLL.Services.Unified_Response;

namespace BLL.Services.TariffService
{
    public interface ITariffService
    {
        public Task<UnifiedResponse<TariffDto>> Add(TariffDto tariff);
        public Task<(bool, string)> Edit(TariffDto tariff);
        public Task<(bool, string)> Delete(int id);
        public Task<List<TariffDto>> GetByActivityType(int code);
        //Tariff Steps 
        public Task<UnifiedResponse<TariffStepsDto>> Add(TariffStepsDto tariff);
        public Task<UnifiedResponse<TariffStepsDto>> Edit(TariffStepsDto tariff);
        public Task<(bool, string)> DeleteTariffStep(int id);
        public Task<List<TariffStepsDto>> GetByTariffId(int code);
    }
}
