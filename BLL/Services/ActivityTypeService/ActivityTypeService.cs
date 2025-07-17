using AutoMapper;
using Azure;
using BLL.Dto;
using BLL.Services.Unified_Response;
using DAL.Entities;
using DAL.Repo.Abstraction;
using System.Net;

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
                List<string> errors = new();

                if (ActivityType is null)
                {
                    errors.Add("ActivityType cannot be null");
                    return UnifiedResponse<ActivityTypeDto>.ErrorResult(errors, "ActivityType cannot be null", HttpStatusCode.BadRequest);
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(ActivityType.Name))
                        errors.Add("Activity Name cannot be null or empty");

                    if (ActivityType.Code <= 0)
                        errors.Add("Activity Code must be greater than 0");
                }

                if (errors.Any())
                {
                    return UnifiedResponse<ActivityTypeDto>.ErrorResult(errors,"Validation failed",HttpStatusCode.BadRequest);
                }

                var activity = mapper.Map<ActivityType>(ActivityType);
                await repo.Add(activity);

                return UnifiedResponse<ActivityTypeDto>.SuccessResult(ActivityType,HttpStatusCode.Created,message: "Activity type created successfully");
            }
            catch (Exception ex)
            {
                return UnifiedResponse<ActivityTypeDto>.ErrorResult(new List<string> { ex.Message },"Unexpected error occurred",HttpStatusCode.InternalServerError);
            }
        }

        public async Task<UnifiedResponse<ActivityTypeDto>> Delete(int code)
        {
            try
            {
                var result = await repo.Get(a=>a.Code == code);              
                if (result is null)
                    return UnifiedResponse<ActivityTypeDto>.ErrorResult(new List<string> { "Activity not found" }, "Activity Not Found", HttpStatusCode.NotFound);
                var Response =mapper.Map<ActivityTypeDto>(result);
                (bool isSucess , string message) response = await repo.Delete(result);
                return UnifiedResponse<ActivityTypeDto>.SuccessResult(Response, HttpStatusCode.OK,response.message);   
            }
            catch(Exception ex)
            {
                return UnifiedResponse<ActivityTypeDto>.ErrorResult(new List<string> { ex.Message },"An Error Happened While Deleting Activity",HttpStatusCode.NotAcceptable);
            }
        }

        public async Task<UnifiedResponse<ActivityTypeDto>> Edit(ActivityTypeDto activity)
        {
            try
            {
                var Errors = new List<string>();
                if (activity is null)
                {
                    Errors.Add("Activity cannot be null");
                    return UnifiedResponse<ActivityTypeDto>.ErrorResult(Errors, "Activity cannot be null", HttpStatusCode.BadRequest);
                }
                else
                {
                    var Activity = await repo.Get(a => a.Code == activity.Code);

                    if (Activity is null)
                    {
                        Errors.Add("Activity Not Found In DB!");
                        return UnifiedResponse<ActivityTypeDto>.ErrorResult(Errors, "Activity Not Found", HttpStatusCode.NotFound);
                    }
                    else
                    {
                        mapper.Map(activity, Activity);
                        (bool isSucess, string message) result = await repo.Edit(Activity);
                        if (result.isSucess)
                            return UnifiedResponse<ActivityTypeDto>.SuccessResult(activity, HttpStatusCode.OK);
                        throw new Exception(result.message);
                    }
                }
            }
            catch (Exception ex)
            {
                return UnifiedResponse<ActivityTypeDto>.ErrorResult(new List<string> { ex.Message },"Error While Editing Activity", HttpStatusCode.NotFound);
            }
        }


        public async Task<UnifiedResponse<List<ActivityTypeDto>>> GetAll()
        {
            try
            {
                var Errors = new List<string>();
                var result = await repo.GetAll();
                if (result is null || result.Count == 0)
                    Errors.Add("There is no Activities in DB!");
                if(Errors.Any())
                    return UnifiedResponse<List<ActivityTypeDto>>.ErrorResult(Errors, "No Activities Found", HttpStatusCode.NotFound);
                var response = mapper.Map<List<ActivityTypeDto>>(result);
                return UnifiedResponse<List<ActivityTypeDto>>.SuccessResult(response, HttpStatusCode.OK);
            }
            catch(Exception ex)
            {
                return UnifiedResponse<List<ActivityTypeDto>>.ErrorResult(new List<string> { ex.Message },"Error While Getting Activities", HttpStatusCode.NotFound);
            }
        }

        public async Task<UnifiedResponse<ActivityTypeDto>> GetByCode(int code)
        {
            try
            {
                if (code <= 0)
                {
                    return UnifiedResponse<ActivityTypeDto>.ErrorResult(new List<string> { "Code must be greater than 0" },"Validation failed", HttpStatusCode.BadRequest);
                }

                var activity = await repo.Get(a => a.Code == code);

                if (activity is null)
                {
                    return UnifiedResponse<ActivityTypeDto>.ErrorResult(new List<string> { $"No activity found with code {code}" },"Activity not found",HttpStatusCode.NotFound);
                }

                var result = mapper.Map<ActivityTypeDto>(activity);

                return UnifiedResponse<ActivityTypeDto>.SuccessResult(result,HttpStatusCode.OK,message: "Activity retrieved successfully");
            }
            catch (Exception ex)
            {
                return UnifiedResponse<ActivityTypeDto>.ErrorResult(new List<string> { ex.Message },"Unexpected error occurred",HttpStatusCode.InternalServerError);
            }
        }

    }
}
