export const securitymodule = {
    COMPANY: "Company",
    CUSTOMER: "Customer",
    CONTRACT: "Contract",
    PROJECT: "Project",
    ASSIGNMENT: "Assignment",
    TECHSPECIALIST: "TechSpecialist",
    SUPPLIER: "Supplier",
    SUPPLIERPO: "SupplierPO",
    SECURITY: "Security",
    MYPROFILE: "MyProfile",
    PREASSIGNMENT: "PreAssignment",
    TIMEOFFREQUEST: "TimeOffRequest",
    MYSCHEDULE: "MySchedule",
    MYASSIGNMENT: "MyAssignment",
    MYDOCUMENT: "MyDocument",
    DOCUMENTS: "Documents",
    VISIT:"Visit",
    TIMESHEET: "TimeSheet",
    QUICKSEARCH:"QuickSearch", //def 669
};

export const activitycode = {
    VIEW: "V00001",
    MODIFY: "M00001",
    DELETE: "D00001",
    NEW: "N00001",
    INTER_COMP_LEVEL_0_VIEW:"ICS00001",
    LEVEL_0_VIEW:"S00001",
    LEVEL_0_MODIFY:"S00002", //D667
    LEVEL_1_VIEW:"S00003",
    LEVEL_1_MODIFY:"S00004",
    LEVEL_2_VIEW:"S00005",
    LEVEL_2_MODIFY:"S00006",
    VIEW_SENSITIVE_DOC:"S00007",
    EDIT_SENSITIVE_DOC:"S00008",
    EDIT_PAYRATE:"S00009",
    VIEW_PAYRATE:"S00010",
    VIEW_TM:"S00011",
    EDIT_TM:"S00012",
    TIMESHEET_CHARTE_RATE_VIEW: "TI0003",
    TIMESHEET_PAY_RATE_VIEW: "TI0006",
    VISIT_CHARTE_RATE_VIEW: "V00006",
    VISIT_PAY_RATE_VIEW: "V00007",
    VISIT_MODIFY_PAYUNIT_PAYRATE: "V00004",
    TIMESHEET_MODIFY_PAYUNIT_PAYRATE: "TI0004",
    VIEW_ALL: "A00001",
    VISIT_MODIFY_ADD_APPROVED_LINES: "V00013",
    TIMESHEET_MODIFY_ADD_APPROVED_LINES: "TI0002"
};

//Global View for Contract
export const contractActivityCode={
  View_Global:"C00001",
};

//Global View for SupplierPO
export const supplierPOActivityCode={
  View_Global:"SU0003",
};

//Global View for Assignment
export const assignmentActivityCode={
  View_ALL_Assignments:"A00001",
};
//Custome Module Name Sequence order
export const custmoduleList=[
  "Company",
  "Customer",
  "Contract",
  "Project",
  "TechSpecialist",
  "MyProfile",
  "MyAssignment",
  "PreAssignment",
  "QuickSearch",
  "TimeOffRequest",
  "Supplier",
  "SupplierPO",
  "Assignment",
  "Visit",
  "TimeSheet",
  "Finance",
  "Reporting",
  "DocumentLibrary",
  "Admin",
  "UserRoles",
  "Security",
  "MySchedule",
  "MyDocuments",
  "Document",
  "Audit",
  "Master",
   ];
   
export const levelSpecificActivities ={ 
      viewActivitiesInterCompLevel0 : [
        activitycode.INTER_COMP_LEVEL_0_VIEW //activitycode.VIEW,activitycode.LEVEL_0_VIEW,
      ],
      viewActivitiesLevel0 : [
        activitycode.VIEW,activitycode.LEVEL_0_VIEW
      ],
      viewActivitiesLevel1 : [
        activitycode.VIEW, activitycode.LEVEL_1_VIEW, activitycode.LEVEL_2_VIEW, activitycode.VIEW_SENSITIVE_DOC, activitycode.VIEW_PAYRATE
      ],
      viewActivitiesLevel2 : [
        activitycode.VIEW,activitycode.LEVEL_2_VIEW, activitycode.VIEW_SENSITIVE_DOC, activitycode.VIEW_PAYRATE
      ],
      viewActivitiesLevel3 : [
        activitycode.VIEW,activitycode.EDIT_SENSITIVE_DOC, activitycode.EDIT_PAYRATE 
       ],
       viewActivitiesLevel4 : [
        activitycode.VIEW, activitycode.EDIT_PAYRATE 
       ],
       viewActivitiesTM : [
        activitycode.VIEW,activitycode.VIEW_TM
       ],
       editActivitiesLevel0 : [
        activitycode.MODIFY,activitycode.LEVEL_0_MODIFY,activitycode.LEVEL_1_MODIFY, activitycode.LEVEL_2_MODIFY, activitycode.EDIT_SENSITIVE_DOC, activitycode.EDIT_PAYRATE
      ],
       editActivitiesLevel1 : [
        activitycode.MODIFY,activitycode.LEVEL_1_MODIFY, activitycode.LEVEL_2_MODIFY, activitycode.EDIT_SENSITIVE_DOC, activitycode.EDIT_PAYRATE
      ],
      editActivitiesLevel2 : [
        activitycode.MODIFY,activitycode.LEVEL_2_MODIFY, activitycode.EDIT_SENSITIVE_DOC, activitycode.EDIT_PAYRATE
      ],
      editActivitiesLevel3 : [
        activitycode.MODIFY,activitycode.EDIT_SENSITIVE_DOC, activitycode.EDIT_PAYRATE
      ],
      editActivitiesLevel4 : [
        activitycode.MODIFY, activitycode.EDIT_PAYRATE
      ],
      editActivitiesTM : [
        activitycode.MODIFY,activitycode.EDIT_TM
      ]
};

export const viewAllRightsModules = [
  securitymodule.ASSIGNMENT,
  securitymodule.VISIT,
  securitymodule.TIMESHEET
];

export const interCompany_viewActivitiesLevel0 ={
  activities: [
    {
      module: 'TechSpecialist',
      activity: 'S00001',
      hasPermission: true
    },
    {
      module: 'TechSpecialist',
      activity: 'ICS00001',
      hasPermission: true
    },
  ]
};