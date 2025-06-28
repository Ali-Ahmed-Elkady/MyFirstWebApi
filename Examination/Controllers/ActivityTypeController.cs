using BLL.Dto;
using BLL.Services.ActivityTypeService;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]/[action]")]

public class ActivityTypeController : ControllerBase
{
    private readonly IActivityTypeService activity;

    public ActivityTypeController(IActivityTypeService activity)
    {
        this.activity = activity;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var result = await activity.GetAll();
        return Ok(result);
    }
    [HttpPost]
    public async Task<IActionResult> AddActivity([FromBody] ActivityTypeDto activity)
    {
         var result  = await this.activity.Add(activity);
         if (result.Success)
            return Ok(result);
            return BadRequest();
    }
    [HttpPut]
    public async Task<IActionResult> Update(ActivityTypeDto Activity)
    {
        var result = await activity.Edit(Activity);
        return Ok(result);
    }
    [HttpDelete]
    public async Task<IActionResult> Delete(ActivityTypeDto Activity)
    {
        var result = await activity.Delete(Activity);
        if (result.Success)
            return Ok(result);
        return BadRequest(result);
    }
}
