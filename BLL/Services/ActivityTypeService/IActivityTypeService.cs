using BLL.Dto;
using BLL.Services.Unified_Response;

namespace BLL.Services.ActivityTypeService
{
    public interface IActivityTypeService
    {
        public Task<UnifiedResponse<ActivityTypeDto>> Add(ActivityTypeDto Customer);
        public Task<UnifiedResponse<ActivityTypeDto>> Edit(ActivityTypeDto Customer);
        public Task<UnifiedResponse<ActivityTypeDto>> Delete(ActivityTypeDto activity);
        public Task<List<ActivityTypeDto>> GetByCode(int Code);
        public Task<List<ActivityTypeDto>> GetAll();
    }
}
