using AutoMapper;
using Azure;
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

        public async Task<UnifiedResponse<(bool,string)>> Delete(int code)
        {
            try
            {
                var result = await repo.Get(a=>a.Code == code);
                if (result is null )
                    throw new Exception("Activity Not Found");
                (bool isSucess , string message) response = await repo.Delete(result);
                return UnifiedResponse<(bool,string)>.SuccessResult(response); 
                
            }
            catch(Exception ex)
            {
                return UnifiedResponse<(bool, string)>.SuccessResult((false ,ex.Message));
            }
        }

        public async Task<UnifiedResponse<ActivityTypeDto>> Edit(ActivityTypeDto activity)
        {
            try
            {
                var existing = await repo.Get(a => a.Code == activity.Code);
                if (existing is null)
                    throw new Exception("Activity Not Found In DB!");

                mapper.Map(activity, existing);

                (bool isSucess , string message) result = await repo.Edit(existing);
                if (result.isSucess)
                return UnifiedResponse<ActivityTypeDto>.SuccessResult(activity);
                throw new Exception(result.message);
            }
            catch (Exception ex)
            {
                return UnifiedResponse<ActivityTypeDto>.ErrorResult(ex.Message);
            }
        }


        public async Task<UnifiedResponse<List<ActivityTypeDto>>> GetAll()
        {
            try
            {
                var result = await repo.GetAll();
                if (result is null || result.Count == 0)
                    throw new Exception("There is no Activities in DB!");
                var response = mapper.Map<List<ActivityTypeDto>>(result);
                return UnifiedResponse<List<ActivityTypeDto>>.SuccessResult(response);
            }
            catch(Exception ex)
            {
                return UnifiedResponse<List<ActivityTypeDto>>.ErrorResult(ex.Message);
            }
        }

        public async Task<UnifiedResponse<ActivityTypeDto>> GetByCode(int code)
        {
            try 
            {
                if (code is 0)
                    throw new Exception("Activity Code Cannot Be 0");
                var Activity = await repo.Get(a => a.Code == code);
                if (Activity is null)
                    throw new Exception("Activity Not Found");
                var result = mapper.Map<ActivityTypeDto>(Activity);
                return UnifiedResponse<ActivityTypeDto>.SuccessResult(result);
            }
            catch(Exception ex)
            {
                return UnifiedResponse<ActivityTypeDto>.ErrorResult(ex.Message);
            }
        }
    }
}
