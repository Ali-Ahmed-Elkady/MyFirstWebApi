using BLL.Dto;
using BLL.Services.TariffService;
using DAL.Repo.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace Examination.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
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
            return Ok(result);
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
    }
}
