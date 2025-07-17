using BLL.Dto;
using BLL.Services.Unified_Response;
using DAL.Entities;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Net;

namespace BLL.Services.TariffService
{
    public partial class TariffService
    {
        public async Task<UnifiedResponse<TariffStepsDto>> Add(TariffStepsDto tariff)
        {
            try
            {
                if (tariff != null)
                {
                    var prevStep = (await steps.GetAll(a => a.TariffId == tariff.TariffId)).OrderByDescending(a => a.Id).FirstOrDefault();
                    if (prevStep == null || tariff.From > prevStep.To)
                    {

                        var CurrentTariff = await repo.Get(a => a.Id == tariff.TariffId);
                        var Activity = await ActivityRepo.Get(a => a.Code == CurrentTariff.ActivityTypeId);
                        var Tariff = mapper.Map<TariffSteps>(tariff);

                        if (tariff.IsRecalculated)
                        {
                            Tariff.RecalculationEdge = tariff.From - 1;
                            (decimal Total, decimal bure) result = await customer.CalculateConsumptions(Tariff.RecalculationEdge, Activity.Code);
                            Tariff.RecalculationAddedAmount = (Tariff.RecalculationEdge * tariff.Price) - (result.bure);
                        }
                        await steps.Add(Tariff);
                        return UnifiedResponse<TariffStepsDto>.SuccessResult(tariff,HttpStatusCode.OK);
                    }
                    throw new Exception("Tariff Step Cannot be smaller Than The previous Step");
                }
                throw new Exception("Tariff Step cannot be null");
            }
            catch (Exception ex)
            {
                return UnifiedResponse<TariffStepsDto>.ErrorResult(ex.Message, HttpStatusCode.NotFound);
            }
        }
        public async Task<UnifiedResponse<TariffStepsDto>> Edit(TariffStepsDto tariff)
        {
            try
            {
                if (tariff is null)
                    throw new Exception("Tariff Step Data Can't Be Null");
                var ExistingTariff =await steps.Get(a=>a.Id == tariff.TariffId);
                if (ExistingTariff is null)
                    throw new Exception("Tariff not Found!");
                var prevStep = (await steps.GetAll(a => a.TariffId == tariff.TariffId)).OrderByDescending(a => a.Id).FirstOrDefault();
                if (prevStep == null || tariff.From > prevStep.To)
                {
                    mapper.Map(tariff, ExistingTariff);
                    await steps.Edit(ExistingTariff);
                    return UnifiedResponse<TariffStepsDto>.SuccessResult(tariff, HttpStatusCode.NotFound);
                }
                throw new Exception("Tariff Step Cannot be smaller Than The previous Step");
            }
            catch (Exception ex)
            {
                return UnifiedResponse<TariffStepsDto>.ErrorResult(ex.Message, HttpStatusCode.NotFound);
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
            var exists = await steps.GetAll(a => a.TariffId == code);
            if (exists is null)
                throw new Exception("No Tariffs For this Activity");
            var result = mapper.Map<List<TariffStepsDto>>(exists);
            return result;
        }
    }
}
