using BLL.Dto;

namespace BLL.Services.TariffService
{
    public interface ITariffService
    {
        public Task<(bool, string)> Add(TariffDto tariff);
        public Task<(bool, string)> Edit(TariffDto tariff);
        public Task<(bool, string)> Delete(int id);
        public Task<List<TariffDto>> GetByActivityType(int code);
    }
}
