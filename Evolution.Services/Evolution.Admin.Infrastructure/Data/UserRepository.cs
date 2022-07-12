using AutoMapper;
using Evolution.Admin.Domain.Enums;
using Evolution.Admin.Domain.Interfaces.Data;
using Evolution.Common.Extensions;
using Evolution.GenericDbRepository.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using DbModels = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Admin.Domain.Models.Admins;

namespace Evolution.Admin.Infrastructure.Data
{
    public class UserRepository : GenericRepository<Evolution.DbRepository.Models.SqlDatabaseContext.User>, IUserRepository
    {
        private readonly DbModels.EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;

        public UserRepository(IMapper mapper, DbModels.EvolutionSqlDbContext dbContext) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }

        public IList<DomainModel.User> Search(DomainModel.User searchModel)
        {
            var dbSearchModel = _mapper.Map<DbModels.User>(searchModel);
            IQueryable<DbModels.User> whereClause = null;

            var user = _dbContext.User;

            if (!string.IsNullOrEmpty(searchModel.CompanyCode))
                whereClause = user.Where(x => x.UserRole.Any(x1 => x1.Company.Code == searchModel.CompanyCode));

            if (searchModel.CompanyCode.HasEvoWildCardChar())
                whereClause = user.WhereLike(x => x.Company.Code, searchModel.CompanyCode, '*');

            if (searchModel.CompanyName.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.Company.Name, searchModel.CompanyName, '*');
            else
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.CompanyName) || x.Company.Name == searchModel.CompanyName);

            if (searchModel.DisplayName.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.Name, searchModel.DisplayName, '*');
            else
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.DisplayName) || x.Name == searchModel.DisplayName);
            if (searchModel.Username.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.SamaccountName, searchModel.Username, '*');
            else
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.Username) || x.SamaccountName == searchModel.Username);

            var expression = dbSearchModel.ToExpression(new List<string> { nameof(dbSearchModel.PhoneNumberConfirmed),
                                                                           nameof(dbSearchModel.EmailConfirmed),
                                                                           nameof(dbSearchModel.IsActive)});
            //Added null check since from report we will not be sending IsActive parameter.
            if (searchModel.IsActive.HasValue && searchModel.IsActive != false)
                whereClause = whereClause.Where(x => x.IsActive == searchModel.IsActive);

            if (!string.IsNullOrEmpty(searchModel.UserType))
                whereClause = whereClause.Where(x => x.UserType.Any(x1 => x1.UserTypeName == searchModel.UserType && x1.Company.Code == searchModel.CompanyCode && x1.IsActive == true)); //IGO_QC def 854 #1

            if (expression == null)
                return _mapper.Map<IList<DomainModel.User>>(whereClause.ToList());
            else
                return _mapper.Map<IList<DomainModel.User>>(whereClause.Where(expression).ToList())?.OrderBy(x => x.DisplayName).ToList();
        }

        public IList<DomainModel.User> GetUsers(IList<string> companyCodes)
        {
            IQueryable<DbModels.User> whereClause = _dbContext.User;
            IQueryable<DbModels.User> whereClause1 = _dbContext.User;
            var coordinators1 = whereClause.Include(a => a.UserType).Include(a => a.CompanyOffice).Include("UserType.Company")
                          .Where(x => x.UserType.Any(x1 => x1.UserTypeName == EnumExtension.DisplayName(UserType.MICoordinator) && companyCodes.Contains(x1.Company.Code)))
                          .Select(x => new DbModels.User
                          {
                              Id = x.Id,
                              Name = x.Name,
                              IsActive = x.IsActive,
                              UserType = x.UserType.Select(x2 => new DbModels.UserType { UserId = x2.UserId, UserTypeName = x2.UserTypeName, CompanyId = x2.CompanyId, Company = new DbModels.Company { Code = x2.Company.Code }, IsActive = x2.IsActive }).ToList(),
                              SamaccountName = x.SamaccountName,
                              Email = x.Email,
                              PhoneNumber = x.PhoneNumber,
                              CompanyOffice = new DbModels.CompanyOffice { OfficeName = x.CompanyOffice.OfficeName },
                              Company = new DbModels.Company { Code = x.Company.Code, Name = x.Company.Name }
                          })?.AsNoTracking()?.ToList();

            var coordinators2 = whereClause1.Include("UserType").Include("CompanyOffice").Include("UserType.Company")
                            .Where(x => x.AuthenticationMode == "AD" &&
                             x.UserType.Any(x1 => x1.UserTypeName != EnumExtension.DisplayName(UserType.MICoordinator)
                             && companyCodes.Contains(x1.Company.Code)) &&
                                                 (x.ProjectCoordinator.Count(x1 => companyCodes.Contains(x1.Contract.ContractHolderCompany.Code)) > 0
                                                 || x.AssignmentOperatingCompanyCoordinator.Any(a => companyCodes.Contains(a.OperatingCompany.Code))
                                                 || x.AssignmentContractCompanyCoordinator.Any(a => companyCodes.Contains(a.ContractCompany.Code))))
                            .Select(x => new DbModels.User
                            {
                                Id = x.Id,
                                Name = x.Name,
                                IsActive = x.IsActive,
                                UserType = x.UserType.Select(x2 => new DbModels.UserType { UserId = x2.UserId, UserTypeName = x2.UserTypeName, CompanyId = x2.CompanyId, Company = new DbModels.Company { Code = x2.Company.Code }, IsActive = x2.IsActive }).ToList(),
                                SamaccountName = x.SamaccountName,
                                Email = x.Email,
                                PhoneNumber = x.PhoneNumber,
                                CompanyOffice = new DbModels.CompanyOffice { OfficeName = x.CompanyOffice.OfficeName },
                                Company = new DbModels.Company { Code = x.Company.Code, Name = x.Company.Name }
                            })?.AsNoTracking()?.ToList();

            var finalData = coordinators1.Union(coordinators2)
                                   .Select(a => new DomainModel.User()
                                   {
                                       Id = a.Id,
                                       DisplayName = GetStatus(a.IsActive ?? false, a.Name, a.UserType, companyCodes),
                                       IsActive = a.IsActive,
                                       Username = a.SamaccountName,
                                       Email = a.Email,
                                       CompanyOffice = a.CompanyOffice.OfficeName,
                                       CompanyCode = a.Company.Code,
                                       CompanyName = a.Company.Name,
                                   })?
                                   .GroupBy(a => new { a.Id, a.Username })?
                                   .Select(x => x.FirstOrDefault())?
                                   .ToList();


            return finalData?.ToList();
        }

        public IList<DomainModel.User> GetUsers(string loggedInUser, bool isVisit, bool isOperating)
        {
            if (isVisit)
                return GetVisitCoordinators(loggedInUser, isOperating);
            else
                return GetTimesheetCoordinators(loggedInUser, isOperating);
        }

        public IList<DomainModel.User> GetUsers(IList<int> companyIds)
        {
            IQueryable<DbModels.User> whereClause = _dbContext.User;
            IQueryable<DbModels.User> whereClause1 = _dbContext.User;
            var coordinators1 = whereClause.Include(a => a.UserType).Include(a => a.CompanyOffice).Include("UserType.Company")
                          .Where(x => x.UserType.Any(x1 => x1.UserTypeName == EnumExtension.DisplayName(UserType.MICoordinator) && companyIds.Contains(x1.CompanyId)))
                          .Select(x => new DbModels.User
                          {
                              Id = x.Id,
                              Name = x.Name,
                              IsActive = x.IsActive,
                              UserType = x.UserType.Select(x2 => new DbModels.UserType { UserId = x2.UserId, UserTypeName = x2.UserTypeName, CompanyId = x2.CompanyId, Company = new DbModels.Company { Code = x2.Company.Code }, IsActive = x2.IsActive }).ToList(),
                              SamaccountName = x.SamaccountName,
                              Email = x.Email,
                              PhoneNumber = x.PhoneNumber,
                              CompanyOffice = new DbModels.CompanyOffice { OfficeName = x.CompanyOffice.OfficeName },
                              Company = new DbModels.Company { Code = x.Company.Code, Name = x.Company.Name }
                          })?.AsNoTracking()?.ToList();

            var coordinators2 = whereClause1.Include("UserType").Include("CompanyOffice").Include("UserType.Company")
                            .Where(x => x.AuthenticationMode == "AD" &&
                             x.UserType.Any(x1 => x1.UserTypeName != EnumExtension.DisplayName(UserType.MICoordinator)
                             && companyIds.Contains(x1.CompanyId)) &&
                                                 (x.ProjectCoordinator.Count(x1 => companyIds.Contains(x1.Contract.ContractHolderCompanyId)) > 0
                                                 || x.AssignmentOperatingCompanyCoordinator.Any(a => companyIds.Contains(a.OperatingCompanyId))
                                                 || x.AssignmentContractCompanyCoordinator.Any(a => companyIds.Contains(a.ContractCompanyId))))
                            .Select(x => new DbModels.User
                            {
                                Id = x.Id,
                                Name = x.Name,
                                IsActive = x.IsActive,
                                UserType = x.UserType.Select(x2 => new DbModels.UserType { UserId = x2.UserId, UserTypeName = x2.UserTypeName, CompanyId = x2.CompanyId, Company = new DbModels.Company { Code = x2.Company.Code }, IsActive = x2.IsActive }).ToList(),
                                SamaccountName = x.SamaccountName,
                                Email = x.Email,
                                PhoneNumber = x.PhoneNumber,
                                CompanyOffice = new DbModels.CompanyOffice { OfficeName = x.CompanyOffice.OfficeName },
                                Company = new DbModels.Company { Code = x.Company.Code, Name = x.Company.Name }
                            })?.AsNoTracking()?.ToList();

            var finalData = coordinators1.Union(coordinators2)
                                   .Select(a => new DomainModel.User()
                                   {
                                       Id = a.Id,
                                       DisplayName = GetStatus(a.IsActive ?? false, a.Name, a.UserType, companyIds),
                                       IsActive = a.IsActive,
                                       Username = a.SamaccountName,
                                       Email = a.Email,
                                       CompanyOffice = a.CompanyOffice.OfficeName,
                                       CompanyCode = a.Company.Code,
                                       CompanyName = a.Company.Name,
                                   })?
                                   .GroupBy(a => new { a.Id, a.Username })?
                                   .Select(x => x.FirstOrDefault())?
                                   .ToList();


            return finalData?.ToList();
        }

        public IList<DomainModel.User> Get(string companyCode, IList<string> userTypes, bool isFilterCompanyActiveCoordinators = false)
        {
            return _mapper.Map<IList<DbModels.User>, IList<DomainModel.User>>(_dbContext.User.Include("Company").Include("CompanyOffice").Join(_dbContext.UserType,
                                        usr => new { id = usr.Id },
                                        usrTy => new { id = usrTy.UserId },
                                        (usr, usrType) => new { usr, usrType })
                             .Where(x => x.usrType.Company.Code == companyCode
                                         && userTypes.Contains(x.usrType.UserTypeName)
                                         && (isFilterCompanyActiveCoordinators && (x.usr.IsActive == true && x.usrType.IsActive == true)) //Def 245
                ).Select(x => x.usr).GroupBy(x => x.Id).Select(x => x.FirstOrDefault()).ToList());

        }

        public IList<DomainModel.User> Get(IList<string> companyCode, IList<string> userTypes, bool isFilterCompanyActiveCoordinators = false)
        {
            return _mapper.Map<IList<DbModels.User>, IList<DomainModel.User>>(_dbContext.User.Include("Company").Include("CompanyOffice").Join(_dbContext.UserType,
                                        usr => new { id = usr.Id },
                                        usrTy => new { id = usrTy.UserId },
                                        (usr, usrType) => new { usr, usrType })
                             .Where(x => companyCode.Contains(x.usrType.Company.Code)
                                         && userTypes.Contains(x.usrType.UserTypeName)
                                         && (isFilterCompanyActiveCoordinators && (x.usr.IsActive == true && x.usrType.IsActive == true))
                ).Select(x => x.usr).GroupBy(x => x.Id).Select(x => x.FirstOrDefault()).ToList());

        }

        public void GetUsers(IList<string> companyCodes, ref IList<DbModels.User> dbUser)
        {
            IQueryable<DbModels.User> whereClause = _dbContext.User.Include("UserType").Include("UserType.Company").Include("ProjectCoordinator.Contract.ContractHolderCompany").AsNoTracking();
            whereClause = whereClause.Where(x => ((x.UserType.Any(x1 => x1.UserTypeName == EnumExtension.DisplayName(UserType.MICoordinator) && companyCodes.Contains(x1.Company.Code)))
                                                        || x.ProjectCoordinator.Count(x1 => companyCodes.Contains(x1.Contract.ContractHolderCompany.Code)) > 0)).AsNoTracking();

            dbUser = whereClause?.ToList();
        }

        private IList<DomainModel.User> GetVisitCoordinators(string loggedInCompany, bool isOperating)
        {
            int? ContractHolderCompanyId = _dbContext.Company.FirstOrDefault(a => a.Code == loggedInCompany)?.Id;
            List<string> status = !isOperating ? new List<string>() { "C", "N", "O", "J", "R" } : new List<string>() { "C", "N", "J", "R" };
            IQueryable<DbModels.User> whereClause = _dbContext.User;

            whereClause = whereClause.Include(a => a.UserType).Include(a => a.CompanyOffice).Include("UserType.Company")
                          .Where(x => x.UserType.Any(x1 => (x1.UserTypeName == EnumExtension.DisplayName(UserType.MICoordinator)
                          || x1.UserTypeName != EnumExtension.DisplayName(UserType.MICoordinator))
                          && x1.CompanyId == ContractHolderCompanyId));

            if (isOperating)
            {
                whereClause = whereClause?.Where(visit =>
                visit.AssignmentContractCompanyCoordinator.Any(a => a.OperatingCompany.Id == ContractHolderCompanyId)
                && visit.AssignmentContractCompanyCoordinator.Any(b => b.Visit.Any(c => status.Contains(c.VisitStatus))));
            }
            else
            {
                whereClause = whereClause?.Where(visit =>
               (visit.AssignmentContractCompanyCoordinator.Any(a => a.ContractCompany.Id == ContractHolderCompanyId)
                || visit.AssignmentContractCompanyCoordinator.Any(d => d.Project.Contract.InvoicingCompanyId == ContractHolderCompanyId))
               && visit.AssignmentContractCompanyCoordinator.Any(b => b.Visit.Any(c => status.Contains(c.VisitStatus))));
            }
                        
            var ContractCoordinators = whereClause?
                            .Select(x => new DbModels.User
                            {
                                Id = x.Id,
                                Name = x.Name,
                                IsActive = x.IsActive,
                                UserType = x.UserType.Select(x2 => new DbModels.UserType { UserId = x2.UserId, UserTypeName = x2.UserTypeName, Company = new DbModels.Company { Code = x2.Company.Code }, IsActive = x2.IsActive }).ToList(),
                                SamaccountName = x.SamaccountName,
                                Email = x.Email,
                                PhoneNumber = x.PhoneNumber,
                                CompanyOffice = new DbModels.CompanyOffice { OfficeName = x.CompanyOffice.OfficeName },
                                Company = new DbModels.Company { Code = x.Company.Code, Name = x.Company.Name }
                            })?.AsNoTracking()?.ToList();
            List<DomainModel.User> finalCoordinators = ContractCoordinators?.Select(a => new DomainModel.User
            {
                Id = a.Id,
                DisplayName = GetStatus(a.IsActive ?? false, a.Name, a.UserType, new List<string>() { loggedInCompany })
            })?.Distinct()?.ToList();
            return finalCoordinators;
        }

        private IList<DomainModel.User> GetTimesheetCoordinators(string loggedInCompany, bool isOperating)
        {
            List<string> status = !isOperating ? new List<string>() { "C", "O", "J", "R" } : new List<string>() { "C", "J", "R" };
            int? ContractHolderCompanyId = _dbContext.Company.FirstOrDefault(a => a.Code == loggedInCompany)?.Id;
            IQueryable<DbModels.User> whereClause = _dbContext.User;

            whereClause = whereClause.Include(a => a.UserType).Include(a => a.CompanyOffice).Include("UserType.Company")
                          .Where(x => x.UserType.Any(x1 => (x1.UserTypeName == EnumExtension.DisplayName(UserType.MICoordinator)
                          || x1.UserTypeName != EnumExtension.DisplayName(UserType.MICoordinator))
                          && x1.CompanyId == ContractHolderCompanyId));

            if (isOperating)
            {
                whereClause = whereClause?.Where(visit =>
                visit.AssignmentContractCompanyCoordinator.Any(a => a.OperatingCompany.Id == ContractHolderCompanyId)
                && visit.AssignmentContractCompanyCoordinator.Any(b => b.Timesheet.Any(c => status.Contains(c.TimesheetStatus))));
            }
            else
            {
                whereClause = whereClause?.Where(visit =>
               (visit.AssignmentContractCompanyCoordinator.Any(a => a.ContractCompany.Id == ContractHolderCompanyId)
                || visit.AssignmentContractCompanyCoordinator.Any(d => d.Project.Contract.InvoicingCompanyId == ContractHolderCompanyId))
               && visit.AssignmentContractCompanyCoordinator.Any(b => b.Timesheet.Any(c => status.Contains(c.TimesheetStatus))));
            }

            var ContractCoordinators = whereClause?
                            .Select(x => new DbModels.User
                            {
                                Id = x.Id,
                                Name = x.Name,
                                IsActive = x.IsActive,
                                UserType = x.UserType.Select(x2 => new DbModels.UserType { UserId = x2.UserId, UserTypeName = x2.UserTypeName, Company = new DbModels.Company { Code = x2.Company.Code }, IsActive = x2.IsActive }).ToList(),
                                SamaccountName = x.SamaccountName,
                                Email = x.Email,
                                PhoneNumber = x.PhoneNumber,
                                CompanyOffice = new DbModels.CompanyOffice { OfficeName = x.CompanyOffice.OfficeName },
                                Company = new DbModels.Company { Code = x.Company.Code, Name = x.Company.Name }
                            })?.AsNoTracking()?.ToList();
            List<DomainModel.User> finalCoordinators = ContractCoordinators?.Select(a => new DomainModel.User
            {
                Id = a.Id,
                DisplayName = GetStatus(a.IsActive ?? false, a.Name, a.UserType, new List<string>() { loggedInCompany })
            })?.Distinct()?.ToList();
            return finalCoordinators;
        }

        private string GetStatus(bool IsActive, string userName, IEnumerable<DbModels.UserType> userType, IList<string> companyCodes)
        {
            List<DbModels.UserType> users = userType.Where(a => companyCodes.Contains(a.Company.Code)).ToList();
            string lstrName = string.Empty;
            if (IsActive)
            {
                if (users.Any(a => a.UserTypeName == EnumExtension.DisplayName(UserType.MICoordinator)))
                {
                    if (users.Any(a => a.UserTypeName == EnumExtension.DisplayName(UserType.MICoordinator) && a.IsActive == true))
                        lstrName = userName;
                    else if (users.Any(a => a.UserTypeName == EnumExtension.DisplayName(UserType.MICoordinator) && a.IsActive == false))
                        lstrName = userName + " (" + MICoordinatorStatus.InActive.DisplayName() + ")";
                    else
                        lstrName = userName + " (" + MICoordinatorStatus.DONOTUSE.DisplayName() + ")";
                }
                else
                    lstrName = userName + " (" + MICoordinatorStatus.DONOTUSE.DisplayName() + ")";
            }
            else
                lstrName = userName + " (" + MICoordinatorStatus.InActive.DisplayName() + ")";
            return lstrName;
        }

        private string GetStatus(bool IsActive, string userName, IEnumerable<DbModels.UserType> userType, IList<int> companyIds)
        {
            List<DbModels.UserType> users = userType.Where(a => companyIds.Contains(a.CompanyId)).ToList();
            string lstrName = string.Empty;
            if (IsActive)
            {
                if (users.Any(a => a.UserTypeName == EnumExtension.DisplayName(UserType.MICoordinator)))
                {
                    if (users.Any(a => a.UserTypeName == EnumExtension.DisplayName(UserType.MICoordinator) && a.IsActive == true))
                        lstrName = userName;
                    else if (users.Any(a => a.UserTypeName == EnumExtension.DisplayName(UserType.MICoordinator) && a.IsActive == false))
                        lstrName = userName + " (" + MICoordinatorStatus.InActive.DisplayName() + ")";
                    else
                        lstrName = userName + " (" + MICoordinatorStatus.DONOTUSE.DisplayName() + ")";
                }
                else
                    lstrName = userName + " (" + MICoordinatorStatus.DONOTUSE.DisplayName() + ")";
            }
            else
                lstrName = userName + " (" + MICoordinatorStatus.InActive.DisplayName() + ")";
            return lstrName;
        }

    }
}
