using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Evolution.Assignment.Domain.Interfaces.Data;
using Evolution.Assignment.Domain.Models.Assignments;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.GenericDbRepository.Services;
using Microsoft.EntityFrameworkCore;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Assignment.Domain.Models.Assignments;


namespace Evolution.Assignment.Infrastructure.Data
{
    public class AssignmentSubSupplierRepository : GenericRepository<DbModel.AssignmentSubSupplier>, IAssignmentSubSupplerRepository
    {
        private  DbModel.EvolutionSqlDbContext _dbContext = null;
        private  IMapper _mapper = null;

        public AssignmentSubSupplierRepository(DbModel.EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }

        public IList<DomainModel.AssignmentSubSupplier> Search(DomainModel.AssignmentSubSupplier model, params Expression<Func<DbModel.AssignmentSubSupplier, object>>[] includes)
        {
            var dbSearchModel = _mapper.Map<DbModel.AssignmentSubSupplier>(model);
            var assignmentSubsupplier = _dbContext.AssignmentSubSupplier;

            IQueryable<DbModel.AssignmentSubSupplier> whereClause = null;
            if (model.AssignmentId > 0)
                whereClause = assignmentSubsupplier.Where(x => x.AssignmentId == model.AssignmentId && x.IsDeleted!=true);

            /*Need to activate if any error occurs*/
            //if (model.SupplierId> 0)//MS-TS Link
            //    whereClause = assignmentSubsupplier.Where(x => x.SupplierId == model.SupplierId);//MS-TS Link

            //if (model.SupplierContactId > 0)
            //    whereClause = assignmentSubsupplier.Where(x => x.SupplierContactId == model.SupplierContactId);

            //if (model.SupplierName.HasEvoWildCardChar())
            //    whereClause = assignmentSubsupplier.WhereLike(x => x.Supplier.SupplierName, model.SupplierName, '*');//MS-TS Link
            //else
            //    whereClause = assignmentSubsupplier.Where(x => string.IsNullOrEmpty(model.SupplierName) || x.Supplier.SupplierName == model.SupplierName);//MS-TS Link

            //if (model.SupplierContactName.HasEvoWildCardChar())
            //    whereClause = assignmentSubsupplier.WhereLike(x => x.SupplierContact.SupplierContactName, model.SupplierContactName, '*');
            //else
            //    whereClause = assignmentSubsupplier.Where(x => string.IsNullOrEmpty(model.SupplierContactName) || x.SupplierContact.SupplierContactName == model.SupplierContactName);

            var expression = dbSearchModel.ToExpression(new List<string> { nameof(dbSearchModel.IsFirstVisit) });
            if (includes.Any())
                whereClause = includes.Aggregate(whereClause, (current, include) => current.Include(include));

            if (expression == null)
                return assignmentSubsupplier.ProjectTo<DomainModel.AssignmentSubSupplier>().ToList();
            else
            {
                var assignmentSubSupplier = assignmentSubsupplier.Where(expression).ProjectTo<DomainModel.AssignmentSubSupplier>().ToList();
                List<DomainModel.AssignmentSubSupplierTS> assignmentSubSupplierTs = null;
                assignmentSubSupplierTs = assignmentSubSupplier.Where(y => y.SupplierType == SupplierType.MainSupplier.FirstChar()).SelectMany(x => x.AssignmentSubSupplierTS).ToList();
               
                assignmentSubSupplierTs.ToList().ForEach(x =>
                    {
                        x.IsAssignedToThisSubSupplier = false;
                    });
               
                assignmentSubSupplier.ToList().ForEach(x =>
                  {
                      x.MainSupplierContactName = assignmentSubSupplier.Where(x2 => x2.SupplierType == SupplierType.MainSupplier.FirstChar()).Select(x1 => x1.MainSupplierContactName).FirstOrDefault();
                      x.MainSupplierContactId = assignmentSubSupplier.Where(x1 => x1.SupplierType == SupplierType.MainSupplier.FirstChar()).Select(x1 => x1.MainSupplierContactId).FirstOrDefault();
                      x.IsMainSupplierFirstVisit = assignmentSubSupplier.Any(x3 => x3.IsMainSupplierFirstVisit == true) ? true : false;
                      x.MainSupplierId = assignmentSubSupplier.Where(x1 => x1.SupplierType == SupplierType.MainSupplier.FirstChar()).Select(x1 => x1.MainSupplierId).FirstOrDefault();
                      x.MainSupplierName = assignmentSubSupplier.Where(x1 => x1.SupplierType == SupplierType.MainSupplier.FirstChar()).Select(x1 => x1.MainSupplierName).FirstOrDefault();
                      if(assignmentSubSupplier.Count == 1)//it means only Main SP exists(to maintain json structure as same)
                      {
                          x.AssignmentSubSupplierTS = assignmentSubSupplierTs;
                          //x.SubSupplierId = null;
                          //x.MainSupplierId = null;
                          //x.SubSupplierContactId = null;
                      }
                      else { x.AssignmentSubSupplierTS.AddRange(assignmentSubSupplierTs); }
                      
                  });
                if (assignmentSubSupplier.Count > 1)//(because - if only main supplier in DB then u dont filter)
                { 
                assignmentSubSupplier = assignmentSubSupplier.Where(y => y.SupplierType != SupplierType.MainSupplier.FirstChar()).ToList();
                }
                return assignmentSubSupplier;

                //if (assignmentSubSupplier.Count == 0) //This is never be ZERO MS-TS(MAin SUP is Mandatary)
                //{
                //    //var dbAss = _dbContext.Assignment.Include(x=>x.MainSupplierContact).FirstOrDefault(x => x.Id == model.AssignmentId);
                //    //return new List<DomainModel.AssignmentSubSupplier> { new DomainModel.AssignmentSubSupplier {
                //    //    AssignmentId=dbAss?.Id,
                //    //    MainSupplierContactId =dbAss?.MainSupplierContactId,
                //    //     MainSupplierContactName=dbAss?.MainSupplierContact?.SupplierContactName,
                //    //     MainSupplierId=dbAss?.MainSupplierContact?.SupplierId,
                //    //     SupplierName=dbAss?.SupplierPurchaseOrder?.Supplier?.SupplierName

                //    //} };
                //}
                //else
                //    return assignmentSubSupplier;
            }
        }

        public IList<DomainModel.AssignmentSubSupplierVisit> GetSubSupplierForVisit(DomainModel.AssignmentSubSupplierVisit model, params Expression<Func<DbModel.AssignmentSubSupplier, object>>[] includes)
        {
            var dbSearchModel = _mapper.Map<DbModel.AssignmentSubSupplier>(model);
            var assignmentSubsupplier = _dbContext.AssignmentSubSupplier;
            IQueryable<DbModel.AssignmentSubSupplier> whereClause = null;
            if (model.AssignmentId > 0)
                whereClause = assignmentSubsupplier.Where(x => x.AssignmentId == model.AssignmentId);     
            return whereClause.ProjectTo<DomainModel.AssignmentSubSupplierVisit>().ToList();             
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            _mapper = null;
        }

        public List<AssignmentSubSupplier> GetAssignmentSubSuppliers(int AssigenmnetID)
        {
            List<DbModel.AssignmentSubSupplier> listassigenmentSubSullplierdata = _dbContext.AssignmentSubSupplier?.Where(a => a.AssignmentId == AssigenmnetID)?.ToList();
            if (listassigenmentSubSullplierdata != null && listassigenmentSubSullplierdata.Count > 0)
                return _mapper.Map<List<AssignmentSubSupplier>>(listassigenmentSubSullplierdata);
            return null;
        }

        public void RemoveAssignementSubSupplier(List<int> assignmentIds,List<int>SupplierID,string SupplierType )
        {
            if (assignmentIds.Count == 1)
            {
                var FilesToBeDeleted = _dbContext.AssignmentSubSupplier.Where(a => assignmentIds.Contains(a.AssignmentId) && SupplierID.Contains((int)a.SupplierId)  && a.SupplierType == SupplierType && a.IsDeleted == false).ToList();
                FilesToBeDeleted.ForEach(item =>
                {
                    //item.IsFirstVisit = false;
                    item.LastModification = DateTime.UtcNow;
                    item.IsDeleted = true;
                });
                _dbContext.SaveChanges();
            }
            else {
                var FilesToBeDeleted = _dbContext.AssignmentSubSupplier.Where(a => assignmentIds.Contains(a.AssignmentId) &&SupplierID.Contains((int)a.SupplierId)  && a.IsDeleted == false && a.SupplierType==SupplierType).ToList();
                FilesToBeDeleted.ForEach(item =>
                {
                    item.IsFirstVisit = false;
                    item.LastModification = DateTime.UtcNow;
                    item.IsDeleted = true;
                });
                _dbContext.SaveChanges();
            }
        }

        public void AddAssignmentsubsupplier(IList<AssignmentSubSupplier> MainsupplierData)
        {
            var assignmentIds = MainsupplierData.Select(x => x.AssignmentId).ToList();
            var SupplierID = MainsupplierData.Select(x => x.SubSupplierId).ToList();
            var FilesToBeInsert = _dbContext.AssignmentSubSupplier.Where(a => assignmentIds.Contains(a.AssignmentId) && SupplierID.Contains((int)a.SupplierId) && a.SupplierType == "M" && a.IsDeleted == false).ToList();
            if (FilesToBeInsert.Count==0 && assignmentIds.Count >0)
            {
                foreach (var mainSupplierDatas in MainsupplierData)
                {
                    DbModel.AssignmentSubSupplier data = _mapper.Map<DbModel.AssignmentSubSupplier>(mainSupplierDatas);
                    {
                        data.AssignmentId = (int)mainSupplierDatas.AssignmentId;
                        data.SupplierId = mainSupplierDatas.SubSupplierId;
                        data.ModifiedBy = mainSupplierDatas.ModifiedBy;
                        data.SupplierType = mainSupplierDatas.SupplierType;
                        data.IsFirstVisit = false;
                        data.IsDeleted = false;
                    };
                    _dbContext.AssignmentSubSupplier.Add(data);
                    _dbContext.SaveChanges();
                }
            }
        }
    }
}
