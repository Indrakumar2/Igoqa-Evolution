using Evolution.Document.Domain.Models.Document;
using System.Collections.Generic;

namespace Evolution.Assignment.Domain.Models.Assignments
{
    public class AssignmentDetail
    {
        public Assignment AssignmentInfo { get; set; }

        public IList<AssignmentSubSupplier> AssignmentSubSuppliers { get; set; }

        public IList<AssignmentContractRateSchedule> AssignmentContractSchedules { get; set; }

        public IList<AssignmentReferenceType> AssignmentReferences { get; set; }

        public IList<AssignmentTaxonomy> AssignmentTaxonomy { get; set; }

        public IList<AssignmentTechnicalSpecialist> AssignmentTechnicalSpecialists { get; set; }

        public IList<AssignmentTechnicalSpecialistSchedule> AssignmentTechnicalSpecialistChargeRates { get; set; }

        public AssignmentInstructions AssignmentInstructions { get; set; }

        public IList<AssignmentAdditionalExpense> AssignmentAdditionalExpenses { get; set; }

        public AssignmentInterCoDiscountInfo AssignmentInterCompanyDiscounts { get; set; }

        public IList<AssignmentContributionCalculation> AssignmentContributionCalculators { get; set; }

        public IList<AssignmentNote> AssignmentNotes { get; set; }

        public IList<ModuleDocument> AssignmentDocuments { get; set; }

        public ResourceSearch.Domain.Models.ResourceSearch.ResourceSearch ResourceSearch { get; set; }

    }
}
