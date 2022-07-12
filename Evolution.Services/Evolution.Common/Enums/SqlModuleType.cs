namespace Evolution.Common.Enums
{
    public enum SQLModuleType
    {
        Contract,
        Contract_Detail,
        Contract_Schedule,
        Contract_ScheduleRate,
        Contract_Note,
        Contract_InvoiceReference,
        Contract_InvoiceAttachment,
        Contract_ExchangeRate,
        Project,
        Project_Detail,
        TechSpecialist,
        TechSpecialist_Stamp,
        Customer_Contact,
        Customer_Note,
        Customer_AccountReference,
        Customer_AssignmentReference,
        Customer_Address,
        Security,
        SupplierPO_Detail,
        Supplier_Detail,
        Assignment_Detail,
        Timesheet_Detail,
        Visit_Reference,
        Visit_Detail
    }

    public enum SQLModuleActionType
    {
        Add,
        Modify,
        Delete,
        Select
    }
}
