using System.ComponentModel.DataAnnotations;

namespace Evolution.Assignment.Domain.Enums
{
    public enum AssignmentMessageType
    {
        InterCompanyInstructions = 1,
        OperationalNotes = 2,
        ReportingRequirements = 3
    }

    public enum AssignmentInterCo
    {
        [Display(Name = "CompanyCode")]
        CompanyCode,
        [Display(Name = "CompanyName")]
        CompanyName,
        [Display(Name = "Description")]
        Description,
        [Display(Name = "AmendmentReason")]
        AmendmentReason
    }

    public enum AssignmentInterCompanyDiscountType
    {
        [Display(Name = "P")]
        ParentContract,
        [Display(Name = "C")]
        Contract,
        [Display(Name = "O")]
        OperatingCountryCompany,
        [Display(Name = "A")]
        AdditionalIntercoOfficeCountryId,
        [Display(Name = "D")]
        AdditionalIntercoOfficeCountryId2
    }


}
