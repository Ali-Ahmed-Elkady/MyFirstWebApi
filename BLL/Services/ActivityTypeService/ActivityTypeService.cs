using AutoMapper;
using BLL.Dto;
using BLL.Services.Unified_Response;
using DAL.Entities;
using DAL.Repo.Abstraction;

namespace BLL.Services.ActivityTypeService
{
    public class ActivityTypeService : IActivityTypeService
    {
        private readonly IRepo<ActivityType> repo;
        private readonly IMapper mapper;
        public ActivityTypeService(IRepo<ActivityType> Repo , IMapper Mapper)
        {
            repo = Repo;
            mapper = Mapper;
        }
        public async Task<UnifiedResponse<ActivityTypeDto>> Add(ActivityTypeDto ActivityType)
        {
            try
            {
                if (ActivityType is null)
                    throw new Exception("Activity Type Can Not be null");
                if (ActivityType.Name is null)
                    throw new Exception("Name Cannot be null");
                var activity = mapper.Map<ActivityType>(ActivityType);
                await repo.Add(activity);
                return UnifiedResponse<ActivityTypeDto>.SuccessResult(ActivityType);
            }
            catch(Exception ex)
            {
                return UnifiedResponse<ActivityTypeDto>.ErrorResult(ex.Message);
            }
        }

        public async Task<UnifiedResponse<ActivityTypeDto>> Delete(ActivityTypeDto activity)
        {
            try
            {
                var result = await repo.Get(a=>a.Code == activity.Code);
                if (result is null || result.Count==0)
                    throw new Exception("Activity Not Found");
                await repo.Delete(activity.Code);
                return UnifiedResponse<ActivityTypeDto>.SuccessResult(activity); 
            }
            catch(Exception ex)
            {
                return UnifiedResponse<ActivityTypeDto>.ErrorResult(ex.Message);
            }
        }

        public async Task<UnifiedResponse<ActivityTypeDto>> Edit(ActivityTypeDto activity)
        {
            try
            {
                var existing = await repo.Get(a => a.Code == activity.Code);
                if (existing is null)
                    throw new Exception("Activity Not Found In DB!");

                var updated = mapper.Map<ActivityType>(activity);

                await repo.Edit(updated);
                return UnifiedResponse<ActivityTypeDto>.SuccessResult(activity);
            }
            catch (Exception ex)
            {
                return UnifiedResponse<ActivityTypeDto>.ErrorResult(ex.Message);
            }
        }


        public async Task<List<ActivityTypeDto>> GetAll()
        {
            var result = await repo.Get();
            var result2 = mapper.Map<List<ActivityTypeDto>>(result);
            return result2;
        }

        public async Task<List<ActivityTypeDto>> GetByCode(int code)
        {
            if (code is 0)
                throw new Exception("Activity Code Cannot Be 0");
                var Activity = await repo.Get(a => a.Code == code);
                if (Activity is null)
                    throw new Exception("Activity Not Found");
                var result = mapper.Map<List<ActivityTypeDto>>(Activity);
                return (result);   
        }
    }
}
