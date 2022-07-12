namespace Evolution.Common.Enums
{
    public enum EmailKey
    {
        EmailNewProject,  // On creation of new Project
        EmailInterCompanyAssignmentToCoordinator, // Send mail to OC for INTER COMPANY SCENERIO
        OperatingCoordinatorEmailToTS,
        ClientReportingRequirements, // Attachment Email from visit and timesheet
        EmailRejectedVisit, // On Rejection of  a visit or timesheet
        EmailAcceptedVisit, // On Acception of  a visit or timesheet
        EmailCustomerDirectReporting, //On visit/timesheet status changed to awaiting approval
        EmailCustomerFlashReporting, //on flash report document uploaded
        EmailCustomerInspectionReleaseNotes, //on relase notes document uploaded
        EmailNCRReporting, //on NCR report document uploaded,
        EmailCustomerReportingNotification, //Customer email report notification
        EmailVisitCompletedToCoordinator ,//Visit or Timesheet approved by OC
        EmailInterCompanyDiscountAmendmentReason, //Ineter company Discount Amendment Reason
        EmailVisitInterCompanyAmendmentReason  //Visit Inter company Discount Amendment Reason
    }
}