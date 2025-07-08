using BLL.Dto;
using BLL.Services.Unified_Response;

namespace BLL.Services.ActivityTypeService
{
    public interface IActivityTypeService
    {
        public Task<UnifiedResponse<ActivityTypeDto>> Add(ActivityTypeDto Customer);
        public Task<UnifiedResponse<ActivityTypeDto>> Edit(ActivityTypeDto Customer);
        public Task<UnifiedResponse<(bool, string)>> Delete(int code);
        public Task<UnifiedResponse<ActivityTypeDto>> GetByCode(int Code);
        public Task<UnifiedResponse<List<ActivityTypeDto>>> GetAll();
    }
}
