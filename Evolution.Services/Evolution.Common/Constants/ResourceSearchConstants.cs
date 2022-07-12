namespace Evolution.Common.Constants
{
    public static class ResourceSearchConstants
    {
        public const string TSTaxonomy_ApprovalStatus_Approve = "Approve";
        public const string TSTaxonomy_Exception_Comment = "{0} Taxonomy";

        public const string TS_Profile_Status_Active = "Active";
        public const string TS_Profile_Status_Inactive = "Inactive";

        //EmailNotification
        public const string Email_Content_RC_RM_Full_Name = "@RC/RM_FULL_NAME@";
        public const string Email_Content_Customer_Name = "@CUSTOMER_NAME@";
        public const string Email_Content_CH_Coordinator_Name = "@CH_Coordinator_Name@";
        public const string Email_Content_OC_Coordinator_Name = "@OP_Coordinator_Name@";

        public const string Email_Content_OPERATIONS_MANAGER_FULL_NAME = "@OPERATIONS_MANAGER_FULL_NAME@";
        public const string Email_Content_COORDINATOR_FULL_NAME = "@COORDINATOR_FULL_NAME@";
        public const string Email_Content_RESOURCE_FULLNAME_LIST = "@RESOURCE_FULLNAME_LIST@";
        public const string Email_Content_ASSIGNMENT_ID = "@ASSIGNMENT_ID@";
        public const string Email_Content_PRE_ASSIGNMENT_ID = "@PRE_ASSIGNMENT_ID@";
        public const string Email_Content_DISPOSITION_DETAIL = "@DISPOSITION_DETAIL@";


        //Email Subject
        public const string Email_Notification_Pre_Assignment_Created_Subject = "Notification: Pre-Assignment Search {0} has been created/updated";
        public const string Email_Notification_Pre_Assignment_Updated_Subject = "Notification: Pre-Assignment Search {0} has been updated";
        public const string Email_Notification_PreAssignment_Won = "Notification: Pre-Assignment Search {0} was won";
        public const string Email_Notification_PreAssignment_Lost = "Notification: Pre-Assignment Search {0} was Lost.";
        public const string Email_Notification_Resource_NotFound = "Notification: Suitable Resource not found for Assignment {0}.";
        public const string Email_Notification_Search_Disposition = "Notification: A Search Disposition has been submitted for Evolution2 Assignment {0}.";
        public const string Email_Notification_Potential_Resource_not_Found = "Notification: Potential Resource not found for Evolution2 Assignment {0}";
        public const string Email_Notification_Potential_Resource_Found = "Notification: Potential Resource found for Evolution2 Assignment {0}";
        public const string Email_Notification_PendingApproval = "Notification : Pending approval for Suitable Resource found for {0}";
        public const string Email_Notification_ResourceDispositionStatusChange = "Notification : Initiate status change for Resource found for {0}";

        public const string Email_Notification_Override_Approval_Required = "Notification: Override Approval is requested for Evolution2 Assignment {0}";
        public const string Email_Notification_Preferred_Resource_Rejected = "Notification: Preferred Resource is Rejected for Evolution2 Assignment {0}";
        public const string Email_Notification_Preferred_Resource_Approved = "Notification: Preferred Resource is Approved for Evolution2 Assignment {0}";
        public const string Email_Notification_Search_Assistance_Requested = "Notification: Search Assistance is requested for Evolution2 Assignment {0}";


        // ----- ARS workflow task type descriptions--
        public const string Task_Description_OM_Verify_And_Validate = "OM Verify and Validate ({0})"; //D874 removed "for" text
        public const string Task_Description_OM_Validated = "OM Validated ({0})"; 
        // ----- end-- 

        // ----- ARS profile workflow task types--
        public const string Task_Type_Override_Approval_Request = "OM Verify and Validate"; 
        public const string Task_Type_OM_Approve_Reject_Resource = "OM Validated";
        // ----- end--

        // ----- PLO workflow task types--
        public const string Task_Type_PLO_To_RC = "PLO to RC";
        public const string Task_Type_PLO_No_Match_GRM = "PLO - No Match in GRM";
        public const string Task_Type_PLO_Search_And_Save_Resources = "PLO - Search and Save Resources";
        // ----- end--

        // ----- PLO workflow task type descriptions-- 
        public const string Task_Description_PLO_to_RC = "PLO to RC ({0})";
        public const string Task_Description_PLO_No_Match_GRM = "PLO - No Match in Evolution ({0})";
        public const string Task_Description_PLO_Search_And_Save_Resources = "PLO - Search and Save Resources ({0})";
        // ----- end-- 


        // ----- PLO workflow User Types -- 
        public const string User_Type_RC = "ResourceCoordinator";
        public const string User_Type_RM = "ResourceManager";
        // ----- end-- 


        // ----------EmploymentType

        public const string Employment_Type_Former = "Former";
        public const string Employment_Type_OfficeStaff = "Office Staff";
        // public const string Employment_Type_FT_Employee = "FT Employee";
        // public const string Employment_Type_Independent_Contractor = "Independent Contractor";
        // public const string Employment_Type_Office_Staff = "Office Staff";
        // public const string Employment_Type_PT_Employee = "PT Employee";
        // public const string Employment_Type_Temp_Employee = "Temp Employee";
        // public const string Employment_Type_Third_Party_Contractor = "Third Party Contractor";

        //-----------
    }
}
