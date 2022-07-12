using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Company.Domain.Enums
{
    public enum CompanyMessageType
    {
        //InvoiceRemittanceText = 1,
        //InvoiceFooterText = 2,
        //InvoiceDraftText = 3,  //u
        //TechSpecialistExtranetComment = 4,
        //CustomerExtranetComment = 5,
        //EmailCustomerReportingNotification = 6, //u
        //EmailCustomerDirectReporting = 7, //u
        //EmailRejectedVisit = 8, //u
        //InvoiceDescriptionText = 9,   //u
        //EmailVisitCompletedToCoordinator = 10, //u
        //EmailInterCompanyAssignmentToCoordinator = 11, //u
        //InvoiceInterCompDraftText = 12,
        //InterCompanyDescription = 13,  //u
        //InvoiceInterCompText = 14,  //u
        //ReverseChargeDisclaimer = 15,
        //LogoText = 16,  //u
        //InvoiceSummarryText = 17,  //u
        //InvoiceHeader = 18,
        //OperatingCoordinatorEmailToTS = 19,
        //PayrollMasterTypeId=31,
        //QualificationMasterTypeId = 13


        InvoiceRemittanceText = 1,
        InvoiceFooterText = 2,
        InvoiceDraftText = 3,
        TechSpecialistExtranetComment = 4,
        CustomerExtranetComment = 5,
        EmailCustomerReportingNotification = 6,
        EmailCustomerDirectReporting = 7,
        EmailRejectedVisit = 8,
        InvoiceDescriptionText = 9,
        EmailVisitCompletedToCoordinator = 10,
        EmailInterCompanyAssignmentToCoordinator = 11,
        InterCompanyDraftText = 12,
        InterCompanyDescription = 13,
        InterCompanyText = 14,
        ReverseChargeDisclaimer = 15,
        LogoText = 16,
        FreeText = 17,
        InvoiceHeader = 18,
        OperatingCoordinatorEmailToTS = 19,
        NotRequired = 20,
        EmailInterCompanyDiscountAmendmentReason=21,
        EmailVisitInterCompanyDiscountAmendmentReason=22
    }
}
