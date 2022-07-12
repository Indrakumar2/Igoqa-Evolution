export const AppMainRoutes={
    default:'/',
    login:'/Login',
    dashboard:'/Dashboard',
    company:'/Company',
    companyDetails:'/CompanyDetails',
    customer:'/Customer',
    customerDetails:'/CustomerDetails',
    contracts:'/Contracts',
    viewContracts:'/ViewContracts',
    editContracts:'/EditContract',
    contractsDetails:'/ContractsDetails',
    createContracts:'/CreateContract',
    project:'/Project',
    projectDetail:'/ProjectDetails',
    createProject:'/CreateProject',
    viewProject:'/ViewProject',
    editProject:'/EditProject', 
    supplierDetail:'/SupplierDetails',
    createSupplier:'/CreateSupplier',
    viewSupplier:'/ViewSupplier',
    editSupplier:'/EditSupplier',
    searchContract:'/SearchContract',
    projectSearchContract:'/Project/SearchContract',
    error:'/Error',
    techSpec: '/EditResource',
    createProfile: '/CreateProfile',
    profileDetails: '/ProfileDetails',
    searchProject: '/SearchProject',
    searchAssignment: '/EditAssignment',
    createSupplierPO:'/SupplierPO/SearchProject',
    assignmentSearchProject:'/Assignment/SearchProject',
    visitSearchAssignment:'/Visit/SearchAssignment',
    timesheetSearchAssignment:'/Timesheet/SearchAssignment',
    createAssignment: '/CreateAssignment',
    editAssignment: '/AssignmentDetails',    
    editSupplierPO:'/EditSupplierPO',
    supplierPoDetails:'/SupplierPODetails',
    userRoles:'/UserRoles',    
    createRole:'/CreateRole',
    updateRole:'/UpdateRole',
    users:'/Users',
    createUser:'/CreateUser',
    updateUser:'/UpdateUser',
    preAssignment:'/PreAssignment',
    quickSearch:'/QuickSearch',
    timeOffRequest:'/TimeOffRequest',

    forbidden:'/Forbidden',
    createVisit:'/Visit/SearchAssignment',		   
    createTimesheet:'/Timesheet/SearchAssignment',		

    visitSearch:'/EditVisit',		

    visitDetails:'/VisitDetails',  

    editVisit: '/EditVisit',		
    timesheetSearch:'/EditTimesheet',	

    timesheetDetails:'/TimesheetDetails',
    // Calendar - CR - Request #1
    globalCalendar:'/globalCalendar',
    companyCalendar:'/companyCalendar',
    myCalendar:'/myCalendar',

    forgotPassword:'/ForgotPassword',
    intertekCompanyDocuments:'/intertekCompanyDocuments',
    intertekOperationalDocuments:'intertekOperationalDocuments',
    auditSearch:'/AuditSearch',
    adminDetails:'/AdminDetails',
    manageReferencesType:'/ManageReferencesType',
    assignmentLifecycleAndStatus:'/AssignmentLifecycleAndStatus',
    /**WonLost Report URL */
   wonLostReport:'/WonLostReport',
   searchTimesheet:'/searchTimesheet',
   companySpecificMatrixReport:'/companySpecificMatrixReport',
   taaxonomyReport:'/taxonomyReport',
   calendarScheduleReport: '/CalendarScheduleDetailsReport',
   downloadServerReport: '/downloadServerReport',
   downloadedReportsToView:'/downloadedreportstoview'
};

export const AppDashBoardRoutes={
    mysearch:'/Dashboard/MySearch', 
    mytasks:'/Dashboard/MyTasks', 
    assignments:'/Dashboard/ActiveAssignments', 
    inactiveassignments:'/Dashboard/InactiveAssignments',

    generaldetails:'/Dashboard/GeneralDetails',

    visitStatus:'/Dashboard/VisitStatusAndApproval',
    timesheet:'/Dashboard/TimesheetPendingApproval',  
    contractsNearExpiry:'/Dashboard/ContractsNearExpiry',
    documentAproval:'/Dashboard/DocumentApproval',
    budgetMonetary:'/Dashboard/BudgetMonetary',
    BudgetHours:'/Dashboard/BudgetHours' ,
    documentation:'/dashboard/Documentation',
};

export const AppCompanyRoutes={
    generalDetails:'/Company/GeneralDetails',
    // extranet:'company/extranet',
    emailTemplates:'/Company/EmailTemplates',
    divisionCostCenterMargin:'/Company/DivisionCostCenterMargin',

    expectedMarginByBusinessUnit:'/Company/ExpectedMarginByBusinessUnit',
    payroll:'/Company/Payroll',
    contracts:'/Company/Contracts',
    documents:'/Company/Documents',
    notes:'/Company/Notes',
    changeLog:'/Company/ChangeLog',    
    companyOffices: '/Company/CompanyOffices',
    companyTaxes: '/Company/CompanyTaxes'
};

export const AppCustomerRoutes={

    generalDetails:'/Customer/GeneralDetails',
    portalAccess:'/Customer/PortalAccess',
    assignments:'/Customer/AssignmentAccountReference',
    contracts:'/Customer/Contracts',
    documents:'/Customer/Documents',
    notes:'/Customer/Notes',
    changeLog:'/Customer/ChangeLog', 

};

export const AppContractsRoutes={
    generalDetails:'/Contracts/GeneralDetails',
    childContracts:'/Contracts/ChildContracts',
    documents:'/Contracts/Documents',
    invoicingDefaults:'/Contracts/InvoicingDefaults',
    fixedExchangeRates:'/Contracts/FixedExchangeRates',
    rateSchedules:'/Contracts/RateSchedule',
    project:'/Contracts/Projects',
    notes:'/Contracts/Notes'
};

export const AppTechSpecRoutes={
    resourceStatus: '/TechSpec/ResourceStatus',
    contactInformation: '/TechSpec/ContactInformation',
    payRate: '/TechSpec/PayRate',
    taxonomyOtherDetails: '/TechSpec/TaxonomyOtherDetails',
    professionalEducationalDetails: '/TechSpec/ProfessionalEducationalDetails',
    resourceCapabilityCertification: '/TechSpec/ResourceCapabilityCertification',
    sensitiveDocuments: '/TechSpec/SensitiveDocuments',
    comments: '/TechSpec/Comments',
    taxonomyApproval: '/TechSpec/TaxonomyApproval',
     
};

export const AppProjectsRoutes={
    generalDetails:'/Projects/GeneralDetails',
    // invoicingDefaults:'/projects/invoicingDefaults',
    // documents:'/projects/documents',
    // assignments:'/projects/assignments',
    // notes:'/projects/notes',
    // clientNotification:'./projects/clientNotification',
    // changeLog:'/projects/changeLog'
};