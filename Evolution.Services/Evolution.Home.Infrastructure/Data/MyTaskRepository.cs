using AutoMapper;
using AutoMapper.QueryableExtensions;
using Evolution.Common.Constants;
using Evolution.Common.Extensions;
using Evolution.Common.Enums;
using Evolution.GenericDbRepository.Services;
using Evolution.Home.Domain.Interfaces.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Home.Domain.Models.Homes;
using Evolution.Logging.Interfaces;
using System.Linq.Expressions;
using Evolution.TechnicalSpecialist.Domain.Interfaces.Data;

namespace Evolution.Home.Infrastructure.Data
{
    public class MyTaskRepository : GenericRepository<DbModel.Task>, IMyTaskRepository
    {
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;
        private readonly IAppLogger<MyTaskRepository> _logger = null;
        private readonly ITechnicalSpecialistRepository _technicalSpecialistRepository = null;

        public MyTaskRepository(DbModel.EvolutionSqlDbContext dbContext, IMapper mapper, IAppLogger<MyTaskRepository> logger, ITechnicalSpecialistRepository technicalSpecialistRepository) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
            this._logger = logger;
            this._technicalSpecialistRepository = technicalSpecialistRepository;
        }

        public IList<DomainModel.MyTask> Search(DomainModel.MyTask model)
        {
            var dbSearchModel = _mapper.Map<DbModel.Task>(model);
            IQueryable<DbModel.Task> whereClause = _dbContext.Task;

            if (model.AssignedBy.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.AssignedBy.SamaccountName, model.AssignedBy, '*');
            else
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(model.AssignedBy) || x.AssignedBy.SamaccountName == model.AssignedBy);

            if (model.AssignedTo.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.AssignedBy.SamaccountName, model.AssignedTo, '*');
            else
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(model.AssignedTo) || x.AssignedTo.SamaccountName == model.AssignedTo);
             
            if (model.Moduletype.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.Moduletype, model.Moduletype, '*');
            else
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(model.Moduletype) || x.Moduletype == model.Moduletype);

            if (model.TaskRefCode.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.TaskRefCode, model.TaskRefCode, '*');
            else
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(model.TaskRefCode) || x.TaskRefCode == model.TaskRefCode);
             
            if (model.TaskType.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.TaskType, model.TaskType, '*');
            else
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(model.TaskType) || x.TaskRefCode == model.TaskType);

            if (!string.IsNullOrEmpty(model.CompanyCode))//D661 issue1 myTask CR //Commented for D363 CR Change ---- //D702 #18issue (Ref ALM Doc 11-06-2020)
                whereClause = whereClause.Where(x => x.Company.Code == model.CompanyCode); //  D363 CR Change

            var expression = dbSearchModel.ToExpression();

            if (expression == null)
                return whereClause.ProjectTo<DomainModel.MyTask>().ToList();
            else
                return whereClause.Where(expression).ProjectTo<DomainModel.MyTask>().ToList();
        }

        public IList<DomainModel.MyTask> Search(string taskType, IList<string> taskRefCode=null, IList<string> assignedBy = null, IList<string> assignedTo = null)
        { 
            IQueryable<DbModel.Task> whereClause = _dbContext.Task.Where(x=>x.TaskType == taskType);

            if (assignedBy?.Count>0) 
                whereClause = whereClause.Where(x => assignedBy.Contains(x.AssignedBy.SamaccountName));

            if (assignedTo?.Count > 0)
                whereClause = whereClause.Where(x => assignedTo.Contains(x.AssignedTo.SamaccountName));

            if (taskRefCode?.Count > 0)
                whereClause = whereClause.Where(x => taskRefCode.Contains(x.TaskRefCode));
              
             
            return whereClause.ProjectTo<DomainModel.MyTask>().ToList(); 
        }

        public IList<DomainModel.MyTask> Search(IList<string> moduleType , IList<string> assignedTo , IList<string> assignedBy = null, IList<string> taskRefCode = null)
        {
            IQueryable<DbModel.Task> whereClause = _dbContext.Task;

            if (assignedBy?.Count > 0)
                whereClause = whereClause.Where(x => assignedBy.Contains(x.AssignedBy.SamaccountName));

            if (assignedTo?.Count > 0)
                whereClause = whereClause.Where(x => assignedTo.Contains(x.AssignedTo.SamaccountName));

            if (moduleType?.Count > 0)
                whereClause = whereClause.Where(x => moduleType.Contains(x.Moduletype));

            if (taskRefCode?.Count > 0)
                whereClause = whereClause.Where(x => taskRefCode.Contains(x.TaskRefCode));


            return whereClause.ProjectTo<DomainModel.MyTask>().ToList();
        }

        //Added for D761 CR - Starts ---//Function changed for D946
        //Added for D761 CR - Ends
    //D946 CR Start
        public void UpdateTechSpec(IList<DomainModel.MyTask> myTasks, int? pendingWithId) //UAT 07-08Dec20 Doc Ref: Resource #2 Issue
        {
            Exception exception = null;
            try
            {
                List<int> techSpecId = null;
                int? ProfileActionId = null;
                List<string> taskTypes = myTasks.Select(taskType => Convert.ToString(taskType.TaskType)).Distinct().ToList();
                List <KeyValuePair<string, object>> updateValueProps = new List<KeyValuePair<string, object>>();
                techSpecId = myTasks.Select(taskRef => Convert.ToInt32(taskRef.TaskRefCode)).Distinct().ToList();
                List<DbModel.TechnicalSpecialist> technicalSpecialist = _dbContext.TechnicalSpecialist.Where(x => techSpecId.Contains(x.Id)).ToList();
                List<KeyValuePair<DbModel.TechnicalSpecialist,  List<KeyValuePair<string, object>>>> technicalSpecialistUpdate = new List<KeyValuePair<DbModel.TechnicalSpecialist, List<KeyValuePair<string, object>>>>();
                myTasks?.ToList().ForEach(taskData =>
                {
                    var techSpec = technicalSpecialist?.FirstOrDefault(x => x.Id == Convert.ToInt32(taskData.TaskRefCode));
                    if(techSpec != null)
                    {
                        if (taskData.TaskType == TechnicalSpecialistConstants.Task_Type_Resource_To_Update_Profile)
                        {
                            var profileType = _dbContext.Data.FirstOrDefault(x => x.MasterDataTypeId == Convert.ToInt32(MasterType.ProfileAction) &&
                                                 x.Name == TechnicalSpecialistConstants.Profile_Action_Create_Update_Profile);
                            ProfileActionId = profileType.Id;
                        }
                        else
                        {
                            ProfileActionId = techSpec.ProfileActionId;
                        }
                        updateValueProps.AddRange(new List<KeyValuePair<string, object>> {
                                            new KeyValuePair<string, object>("AssignedToUser", taskData.AssignedTo),
                                            new KeyValuePair<string, object>("AssignedByUser",  taskData.AssignedBy),
                                            new KeyValuePair<string, object>("PendingWithId", pendingWithId),
                                            new KeyValuePair<string, object>("ProfileActionId", ProfileActionId)
                                           });
                        technicalSpecialistUpdate.Add(new KeyValuePair<DbModel.TechnicalSpecialist, List<KeyValuePair<string, object>>>(techSpec, updateValueProps));
                    }
                   
                });
                // d770 fix for random update in pendingwith column
                // _technicalSpecialistRepository.Update(technicalSpecialistUpdate, a => a.AssignedToUser, b => b.AssignedByUser, c => c.PendingWithId, d => d.ProfileActionId);
                string tasktypedata = taskTypes.ElementAt(0);
                if (tasktypedata != "OM Verify and Validate" && tasktypedata != "OM Validated")
                {
                    _technicalSpecialistRepository.Update(technicalSpecialistUpdate, a => a.AssignedToUser, b => b.AssignedByUser, c => c.PendingWithId, d => d.ProfileActionId);
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), myTasks);
            }
        }
        //D946 CR End

        public IList<DbModel.ResourceSearch> GetResourceSearchByIds(IList<int> resourceSearchIds)
        {
            IList<DbModel.ResourceSearch> dbResourceSearch = new List<DbModel.ResourceSearch>();
            if (resourceSearchIds?.Count > 0)
            {
                dbResourceSearch = _dbContext.ResourceSearch.Where(x => resourceSearchIds.Contains(x.Id))?.Select(x1 => x1)?.ToList();
            }
            return dbResourceSearch;
        }

        public void UpdateResourceSearchOnReassign(IList<DbModel.ResourceSearch> resourceSearch)
        {
            Exception exception = null;
            try
            {
                if (resourceSearch?.Count > 0)
                {
                    resourceSearch?.ToList().ForEach(x =>
                    {
                        _dbContext.ResourceSearch.Update(x);
                        _dbContext.SaveChanges();
                    });
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), resourceSearch);
            }
        }
    }
}
