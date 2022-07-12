using System;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.Document.Domain.Models.Document;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainTsModel = Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;


namespace Evolution.Visit.Domain.Models.Visits
{
    public class VisitDetail
    {
        public Visit VisitInfo { get; set; }
        public IList<Visit> HistoricalVisits { get; set; }
        public IList<VisitTechnicalSpecialist> VisitTechnicalSpecialists { get; set; }
        public IList<VisitSpecialistAccountItemTime> VisitTechnicalSpecialistTimes { get; set; }
        public IList<VisitSpecialistAccountItemExpense> VisitTechnicalSpecialistExpenses { get; set; }
        public IList<VisitSpecialistAccountItemTravel> VisitTechnicalSpecialistTravels { get; set; }
        public IList<VisitSpecialistAccountItemConsumable> VisitTechnicalSpecialistConsumables { get; set; }        
        public IList<VisitReference> VisitReferences { get; set; }
        public IList<VisitSupplierPerformanceType> VisitSupplierPerformances { get; set; }
        public VisitInterCoDiscountInfo VisitInterCompanyDiscounts { get; set; }
        public IList<ModuleDocument> VisitDocuments { get; set; }        
        public IList<VisitNote> VisitNotes { get; set; }
        public IList<DomainTsModel.TechnicalSpecialistCalendar> TechnicalSpecialistCalendarList { get; set; }
    }

    public class DbVisit : IDisposable
    {
        // Flag: Has Dispose already been called?
        bool disposed = false;
        public IList<DbModel.Visit> DbVisits = null;
        public IList<DbModel.Assignment> DbAssignments = null;
        public IList<DbModel.Project> DbProjects = null;
        public IList<DbModel.Contract> DbContracts = null;
        public IList<DbModel.VisitTechnicalSpecialist> DbVisitTechSpecialists = null;
        public IList<DbModel.VisitReference> DbVisitReference = null;
        public IList<DbModel.VisitSupplierPerformance> DbVisitSupplierPerformance = null;
        public IList<DbModel.VisitTechnicalSpecialistAccountItemConsumable> DbVisitTechSpecConsumables = null;
        public IList<DbModel.VisitTechnicalSpecialistAccountItemExpense> DbVisitTechSpecExpenses = null;
        public IList<DbModel.VisitTechnicalSpecialistAccountItemTime> DbVisitTechSpecTimes = null;
        public IList<DbModel.VisitTechnicalSpecialistAccountItemTravel> DbVisitTechSpecTravels = null;
        public IList<DbModel.VisitNote> DbVisitNotes = null;
        public IList<DbModel.VisitDocument> DbVisitDocuments = null;
        public IList<DbModel.Data> DbChargeExpenses = null;
        public IList<DbModel.Data> DbPayExpenses = null;
        public IList<DbModel.Data> DbData = null;
        public IList<DbModel.TechnicalSpecialist> DbTechnicalSpecialists = null;
        public IList<DomainTsModel.TechnicalSpecialistCalendar> filteredAddTSCalendarInfo = null;
        public IList<DomainTsModel.TechnicalSpecialistCalendar> filteredModifyTSCalendarInfo = null;
        public IList<DomainTsModel.TechnicalSpecialistCalendar> filteredDeleteTSCalendarInfo = null;

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                DbVisits?.Clear();
                DbVisits = null;

                DbAssignments?.Clear();
                DbAssignments = null;

                DbProjects?.Clear();
                DbProjects = null;

                DbContracts?.Clear();
                DbContracts = null;

                DbVisitTechSpecialists?.Clear();
                DbVisitTechSpecialists = null;

                DbVisitReference?.Clear();
                DbVisitReference = null;

                DbVisitSupplierPerformance?.Clear();
                DbVisitSupplierPerformance = null;

                DbVisitTechSpecConsumables?.Clear();
                DbVisitTechSpecConsumables = null;

                DbVisitTechSpecExpenses?.Clear();
                DbVisitTechSpecExpenses = null;

                DbVisitTechSpecTimes?.Clear();
                DbVisitTechSpecTimes = null;

                DbVisitTechSpecTravels?.Clear();
                DbVisitTechSpecTravels = null;

                DbVisitNotes?.Clear();
                DbVisitNotes = null;

                DbVisitDocuments?.Clear();
                DbVisitDocuments = null;

                DbChargeExpenses?.Clear();
                DbChargeExpenses = null;

                DbPayExpenses?.Clear();
                DbPayExpenses = null;

                DbData?.Clear();
                DbData = null;

                DbTechnicalSpecialists?.Clear();
                DbTechnicalSpecialists = null;

                filteredAddTSCalendarInfo?.Clear();
                filteredAddTSCalendarInfo = null;

                filteredModifyTSCalendarInfo?.Clear();
                filteredModifyTSCalendarInfo = null;

                filteredDeleteTSCalendarInfo?.Clear();
                filteredDeleteTSCalendarInfo = null;

            }
            disposed = true;
        }
    }
}
