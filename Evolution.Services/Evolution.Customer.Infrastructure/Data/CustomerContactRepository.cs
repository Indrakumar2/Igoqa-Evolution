using Evolution.Common.Enums;
using Evolution.Customer.Domain.Interfaces.Data;
using Evolution.GenericDbRepository.Services;
using Evolution.Security.Domain.Models.Security;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Customer.Domain.Models;

namespace Evolution.Customer.Infrastructure.Data
{
    public class CustomerContactRepository : GenericRepository<DbModel.CustomerContact>, ICustomerContactRepository
    {
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;

        public CustomerContactRepository(DbModel.EvolutionSqlDbContext dbContext) : base(dbContext)
        {
            this._dbContext = dbContext;
        }

        public IList<DomainModel.Customers.Contact> Search(string customerCode, int? addressId, DomainModel.Customers.Contact searchModel)
        {
            IList<int> addressIds = new List<int>();
            IList<DomainModel.Customers.Contact> result = null;

            if (this._dbContext == null)
                throw new System.InvalidOperationException("Datacontext is not intitialized.");

            if (!addressId.HasValue)
                addressIds = _dbContext.CustomerAddress.Where(a => a.Customer.Code == customerCode).Select(x => x.Id).ToList();

            var custContact = _dbContext.CustomerContact.Where(x => (searchModel.ContactId == 0 || x.Id == searchModel.ContactId)
                                     && (string.IsNullOrEmpty(searchModel.Salutation) || x.Salutation == searchModel.Salutation)
                                     && (string.IsNullOrEmpty(searchModel.Position) || x.Position == searchModel.Position)
                                     && (string.IsNullOrEmpty(searchModel.ContactPersonName) || x.ContactName == searchModel.ContactPersonName)
                                     && (string.IsNullOrEmpty(searchModel.Landline) || x.TelephoneNumber == searchModel.Landline)
                                     && (string.IsNullOrEmpty(searchModel.Fax) || x.FaxNumber == searchModel.Fax)
                                     && (string.IsNullOrEmpty(searchModel.Mobile) || x.MobileNumber == searchModel.Mobile)
                                     && (string.IsNullOrEmpty(searchModel.Email) || x.EmailAddress == searchModel.Email)
                                     && (string.IsNullOrEmpty(searchModel.OtherDetail) || x.OtherContactDetails == searchModel.OtherDetail)
                                    && ((addressId.HasValue && x.CustomerAddressId == addressId.Value && x.CustomerAddress.Customer.Code == customerCode) || (!addressId.HasValue && addressIds.Contains(x.CustomerAddressId))))
                                   .Select(c => new DomainModel.Customers.Contact
                                   {
                                       ContactId = c.Id,
                                       CustomerAddressId = c.CustomerAddressId,
                                       Salutation = c.Salutation,
                                       Position = c.Position,
                                       ContactPersonName = c.ContactName,
                                       Landline = c.TelephoneNumber,
                                       Fax = c.FaxNumber,
                                       Mobile = c.MobileNumber,
                                       Email = c.EmailAddress,
                                       OtherDetail = c.OtherContactDetails,
                                       UpdateCount = c.UpdateCount,
                                       ModifiedBy = c.ModifiedBy,
                                       LastModifiedOn = (DateTime)c.LastModification,
                                       LogonName = c.LoginName
                                   }).ToList();

            if (custContact != null && custContact.Any())
            {
                var loginNames = custContact.Where(x => !string.IsNullOrEmpty(x.LogonName)).Select(x => x.LogonName).ToList();
                var dbUsers = this._dbContext.User
                     .Include(x => x.Company)
                     .Include(x => x.CustomerUserProjectAccess).ThenInclude(x => x.Project)
                     .Where(x => loginNames.Contains(x.SamaccountName))
                        .Select(c => new ExtranetUserInfo
                        {
                            UserId = c.Id,
                            UserName = c.Name,
                            LogonName = c.SamaccountName,
                            Email = c.Email,
                            Password = c.PasswordHash,
                            IsAccountLocked = c.LockoutEnabled,
                            CompanyCode = c.Company.Code,
                            IsActive = c.IsActive.Value,
                            UserType = UserType.Customer.ToString(),
                            SecurityQuestion1Answer = c.SecurityQuestion1Answer,
                            SecurityQuestion1 = c.SecurityQuestion1,
                            IsShowNewVisit = Convert.ToBoolean(c.IsShowNewVisit),
                            ExtranetAccessLevel = c.ExtranetAccessLevel,
                            Comments = c.Comments,
                            UpdateCount = c.UpdateCount,
                            CreatedDate = c.CreatedDate,
                            LastLoginDate = c.LastLoginDate,
                            CustomerUserProjectNumbers = c.CustomerUserProjectAccess.Select(x => new CustomerUserProject
                            {
                                Id = x.Id,
                                UserId = x.UserId,
                                ProjectNumber = x.Project.ProjectNumber.Value,
                            }).ToList()
                        }).ToList();

                result = custContact.GroupJoin(dbUsers,
                                   custCont => new { LoginName = custCont.LogonName },
                                   usr => new { LoginName = usr.LogonName },
                                   (custCont, usr) => new { custCont, usr }).Select(x =>
                                   {
                                       if (x.usr != null && x.usr.Any())
                                       {
                                           x.custCont.UserInfo = x.usr.FirstOrDefault();
                                           x.custCont.UserInfo.CustomerAddressId = x.custCont.CustomerAddressId;
                                           x.custCont.UserInfo.ContactId = x.custCont.ContactId;
                                           x.custCont.IsPortalUser = x.usr.FirstOrDefault().IsActive;
                                       }
                                       return x.custCont;
                                   }).ToList();
            }

            return result;

        }

        /*Added for Assignment Clean up*/
        public IList<DomainModel.Customers.Contact> SearchContact(string customerCode)
        {
           return  _dbContext.CustomerContact.Where(x => x.CustomerAddress.Customer.Code == customerCode)?.Select(x =>
                  new DomainModel.Customers.Contact
                  {
                      ContactId = x.Id,
                      ContactPersonName = x.ContactName,
                      Email = x.EmailAddress,
                      CustomerAddressId = x.CustomerAddressId,
                      LastModifiedOn = x.LastModification,
                      ModifiedBy = x.ModifiedBy,
                      UpdateCount = x.UpdateCount

                  })?.ToList();
        }
    }
}
