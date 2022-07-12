using AutoMapper;
using Evolution.Assignment.Domain.Enums;
using Evolution.Assignment.Domain.Interfaces.Data;
using Evolution.GenericDbRepository.Services;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Assignment.Domain.Models.Assignments;

namespace Evolution.Assignment.Infrastructure.Data
{
    public class AssignmentInstructionsRepository : GenericRepository<DbModel.AssignmentMessage>, IAssignmentInstructionsRepository
    {
        private DbModel.EvolutionSqlDbContext _dbContext = null;
        private IMapper _mapper = null;

        public AssignmentInstructionsRepository(DbModel.EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public DomainModel.AssignmentInstructions Search(int assignmentId)
        {
            var dbAssignmentMessages = _dbContext.AssignmentMessage.Where(x => x.AssignmentId == assignmentId && x.IsActive==true 
                                                                                && x.MessageTypeId != (int)AssignmentMessageType.ReportingRequirements).ToList();
            if (dbAssignmentMessages.Count > 0)
            {
                var result = _mapper.Map<DomainModel.AssignmentInstructions>(dbAssignmentMessages);
                result.AssignmentId = assignmentId;
                return result;
            }

            return null;
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            _mapper = null;
        }
    }
}
