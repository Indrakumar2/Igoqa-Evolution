using System.ComponentModel.DataAnnotations;

namespace Evolution.Common.Enums
{ 
    public enum EmailType
    {
        [Display(Name = "Notification")]
        Notification,
        [Display(Name = "Profile Login Detail Created")]
        PLDC,
        [Display(Name = "Profile Login Detail Updated")]
        PLDU,
        [Display(Name = "Updated Profile Information New")]
        UPIN,
        [Display(Name = "Updated Profile Information Active")]
        UPIA,
        [Display(Name = "Updated Profile Information by Resource")]
        UPIR,
        [Display(Name = "Updated Profile Information by TM")]
        UPIM,
        [Display(Name = "Pending TM Review")]
        PTMR,
        [Display(Name = "Taxonomy Added/Updated")]
        TMA,
        [Display(Name = "New Resource Profile made active")]
        NRP,
        [Display(Name = "Document/Certificate Expiry")]
        DCE,
        [Display(Name = "Profile Status Change ")]
        PSC,
        [Display(Name = "New Customer Approval Request")]
        NCAR,
        //[Display(Name = "New Customer Approval Request and Accepted by RC ")]
        //NCAA,
        //[Display(Name = "New Customer Approval Request by RC ")]
        //NCAC,

        [Display(Name = "New Customer Approval")]
        NCA,
        [Display(Name = "Profile Updates have been Rejected - Resource")]
        PURR,

        [Display(Name = "Intercompany Pre-Assignment Created")]
        IPAC,
        [Display(Name = "Intercompany Pre-Assignment Updated by OC")]
        IPAU,
        [Display(Name = "Intercompany Pre-Assignment Won")]
        IPAW,
        [Display(Name = "Intercompany Pre-Assignment Lost")]
        IPAL,

        [Display(Name = "Resource assigned to Evo2 Assignment")]
        RAA,
        [Display(Name = "Override-Preferred Resource Request")]
        OPRR,
        [Display(Name = "Override-Preferred Resource Approved")]
        OPRA,
        [Display(Name = "Override-Preferred Resource")]
        OPRJ,

        [Display(Name = "Potential Lost Opportunity Request")]
        PLOR,
        [Display(Name = "Potential Lost Opportunity Found")]
        PLOF,
        [Display(Name = "Potential Lost Opportunity Not Found")]
        PLON,
        [Display(Name = "Search Disposition")]
        SDS,

        [Display(Name = "Time Off Request – Resource")]
        TORR,
        [Display(Name = "Time Off Request – User")]
        TORU,
        [Display(Name = "Assignment Instruction Download")]
        AID,
        [Display(Name = "Customer Direct Reporting")]
        CDR,
        [Display(Name = "Cost Of Sales")]
        COS,        
        [Display(Name = "Customer Reporting Notification")]
        CRN,
        [Display(Name = "Customer Visit/Timesheet Approval")]
        CVA,
        [Display(Name = "Customer Visit/Timesheet Rejection")]
        CVR,
        [Display(Name = "Flash Report Notification")]
        FRN,
        [Display(Name = "Intercompany Assignment")]
        ICA,
        [Display(Name = "Inspection Release Notes")]
        IRN,
        [Display(Name = "Intercompany Visit/Timeseet Approval")]
        IVA,
        [Display(Name = "NCR Reporting")]
        NCR,
        [Display(Name = "New Project Creation")]
        NPC,
        [Display(Name = "Reject Visit/Timesheet by CH coordinator")]
        RVC,
        [Display(Name = "Reject Visit Timesheet")]
        RVT,
        [Display(Name = "TAAP")]
        TAAP,
        [Display(Name = "Visit Completed Email to Coordinator")]
        VCC,
        [Display(Name = "Document Library")]
        DLB ,
        [Display(Name = "Amendment Reason")]
        ICR

    }
}
