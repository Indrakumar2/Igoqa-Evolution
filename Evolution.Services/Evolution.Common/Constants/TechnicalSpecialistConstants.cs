namespace Evolution.Common.Constants
{
    public static class TechnicalSpecialistConstants
    {
        // ----- Send To Profile action Types--
        public const string Profile_Action_Send_To_TM = "Send to TM";
        public const string Profile_Action_Send_To_RC_RM = "Send to RC/RM";
        public const string Profile_Action_Send_To_TS = "Send to TS";
        public const string Profile_Action_Create_Update_Profile = "Create/Update Profile";
        // ----- end--
        // ----- Create profile workflow task types--
        public const string Task_Type_Taxonomy_Approval_Request = "Taxonomy Approval Request";
        public const string Task_Type_TS_Updated_Profile = "TS Updated Profile";
        public const string Task_Type_Taxonomy_Updated = "Taxonomy Updated";
        public const string Task_Type_Resource_To_Update_Profile = "Resource to update Profile";
        // ----- end--
        // ----- Create profile workflow task type descriptions--
        public const string Task_Description_TM_Verify_And_Validate = "TM Verify and Validate for {0}, {1} ({2})";
        public const string Task_Description_TM_Validated = "TM Validated for {0}, {1} ({2})";
        public const string Task_Description_TS_Updated_Profile = "Profile Modified/Updated for {0}, {1} ({2})";
        public const string Task_Description_Resource_To_Update_Profile = "Resource to update Profile for {0}, {1} ({2})";
        // ----- end-- 

        // ----- Create profile workflow User Types --
        public const string User_Type_TM = "TechnicalManager";
        public const string User_Type_RC = "ResourceCoordinator";
        public const string User_Type_RM = "ResourceManager";
        public const string User_Type_TS = "TechnicalSpecialist";
        public const string User_Type_OC = "MICoordinator";
        public const string User_Type_OM = "OperationManager";
        // ----- end-- 

        // ----- Email notification --
        public const string Email_Notification_CustomerApprovalAdd_Subject = "Notification: Profile ePin {0} has a new Customer Approval"; 
        public const string Email_Notification_RecordsNearExpiry_Subject = "Notification: You have a Document or Certification near expiry";
        public const string Email_Notification_TsProfileLogin_Create_Subject = "Notification: Your Evolution2 Profile Login Details Created";
        public const string Email_Notification_TsProfileLogin_Info_Update_Subject = "Notification: Your Evolution2 Profile Login Details updated";
        public const string Email_Notification_TmProfileValidation_Subject = "Notification: Profile ePin {0} requires TM Evaluation";
        public const string Email_Notification_ProfileChangeUpdate_Subject = "Notification: Updated Profile Information for ePin {0} is now available.";
        public const string Email_Notification_TmApproval_Subject = "Notification: Taxonomy has been updated for Profile ePin {0}";
        public const string Email_Notification_ResourceStatusChangeUpdate_Subject = "Notification: Resource Status has been changed {0} to {1}";
        public const string Email_Notification_ProfileActivation_Subject = "Notification: New Resource Profile ePin {0} has been made active";
        public const string Email_Notification_ProfileStatusChange_Subject = "Notification: Your Evolution2 Profile Status has been changed";
        public const string Email_Notification_ClientApprovalCertification_Subject = "Notification: {0},{1}> <{2} has new Client Approved Certificate.";
        public const string Email_Notification_ResourceAssignmentNotification_Subject = "Notification: You have been assigned to Evolution2 Assignment {0}-{1}";
        public const string Email_Notification_ResourceDuplicateAllocation_Subject = "Notification: Duplicate Allocation on {0}";
        public const string Email_Notification_TsProfileChangeRejected_Subject = "Notification: Your profile updates have been rejected";
        public const string Email_Notification_TimeOffRequest_Subject = "Notification: Time Off Request submitted by [RESOURCE_NAME] , Profile ePin [PROFILE_EPIN] for approval";
        public const string Email_Notification_TimeOffRequestByCoordinator_Subject = "Notification: Time Off Request submitted for Profile ePin [PROFILE_EPIN]";
        // ----- end-- 

        public const string Email_Content_Document_Type = "@DOCUMENT_TYPE@";
        public const string Email_Content_Expiry_Date = "@EXPIRY_DATE@";
        public const string Email_Content_First_Name = "@FIRST_NAME@";
        public const string Email_Content_Last_Name = "@LAST_NAME@";
        public const string Email_Content_User_Name = "@USER_NAME@";
        public const string Email_Content_Application_URL = "@APPLICATION_URL@";
        public const string Email_Content_Password = "@PASSWORD@";
        public const string Email_Content_Date_Form = "[TIMEOFF_FORM]";
        public const string Email_Content_Date_Till = "[TIMEOFF_THROUGH]";
        public const string Email_Content_Resource_Name = "[RESOURCE_NAME]";
        public const string Email_Content_Coordinator_Name = "[COORDINATOR_NAME]";
        public const string Email_Content_Epin = "[PROFILE_EPIN]";
        public const string Email_Content_PROFILE_ID = "@PROFILE_ID@";
        public const string Email_Content_Status_From = "@STATUS_FROM@";
        public const string Email_Content_Status_To = "@STATUS_TO@";
        public const string Email_Content_Project_Number = "@PROJECT_NO@";
        public const string Email_Content_Assignment_Number = "@ASSIGNMENT_NO@";
        public const string Email_Content_Customer_Name = "@CUSTOMER_NAME@";
        public const string Email_Content_Supplier_Name = "@SUPPLIER_NAME@";
        public const string Email_Content_Old_Assignment_ID = "@OLD_ASSIGNMENT_ID@";
        public const string Email_Content__New_Assignment_ID = "@New_ASSIGNMENT_ID@";

        public const string TS_Task_Type_Draft = "Create Profile";

        //Export
        public const string File_Export_Format = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
        public const string Chevron_Header_Text = "Inspector Curriculum Vitae (CV) Format";
        public const string Header_Text = "CURRICULUM VITAE";
        public const string Footer_Text = "By accepting this document you confirm that you will use and secure any personal data in it only for the purposes of the services Intertek provide you and in accordance with applicable data protection and privacy law.";
        public const string Footer_URL = "intertek.com";

        public const string TS_Change_Approval_Status_Reject = "R";
        public const string TS_Change_Approval_Status_Approved = "A";

        public const string ProfileChangeHistory = "ProfileChangeHistory";

    }
}
