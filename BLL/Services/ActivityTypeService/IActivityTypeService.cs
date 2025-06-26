using BLL.Dto;

namespace BLL.Services.ActivityTypeService
{
    public interface IActivityTypeService
    {
        public Task<(bool, string)> Add(ActivityTypeDto Customer);
        public Task<(bool, string)> Edit(ActivityTypeDto Customer);
        public Task<(bool, string)> Delete(int id);
        public Task<List<ActivityTypeDto>> GetByCode(int Code);
        public Task<List<ActivityTypeDto>> GetAll();
    }
}
