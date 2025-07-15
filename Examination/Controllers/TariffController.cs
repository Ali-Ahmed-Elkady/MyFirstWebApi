using BLL.Dto;
using BLL.Services.TariffService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Examination.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    [Authorize]
    public class TariffController : Controller
    {
        private readonly ITariffService service;
        public TariffController(ITariffService Service)
        {
            service = Service;
        }
        [HttpGet]
        public async Task<IActionResult> GetByActivityType(int id)
        {
            var result = await service.GetByActivityType(id);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Add(TariffDto tariff)
        {
            var result = await service.Add(tariff);
            if (result.Success) 
            return Ok(result);
            return BadRequest(result);
        }
        [HttpPut]
        public async Task<IActionResult> Update(TariffDto tariff)
        {
            var result = await service.Edit(tariff);
            return Ok(result);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await service.Delete(id);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> AddTariffStep(TariffStepsDto tariffSteps)
        {
            var (isSuccess, message) = await service.Add(tariffSteps);

            if (isSuccess)
                return Ok(new { success = true, message });

            return BadRequest(new { success = false, message });
        }
        [HttpPut]
        public async Task<IActionResult> UpdateTariffStep(TariffStepsDto tariff)
        {
            var result = await service.Edit(tariff);
            return Ok(result);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteTariffStep(int id)
        {
            var result = await service.DeleteTariffStep(id);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetByTariffId(int id)
        {
            var result = await service.GetByTariffId(id);
            return Ok(result);
        }
    }
}
