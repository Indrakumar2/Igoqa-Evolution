export const applicationConstants = {
    Authentication:{
        ACCESS_TOKEN:'accessToken',
        REFRESH_TOKEN:'refreshToken',
        ACCESS_TOKEN_EXPIRES_AT:'accessTokenExpires',
        REFRESH_TOKEN_EXPIRES_AT:'refreshTokenExpires',
        USER_NAME:'Username',
        DEFAULT_COMPANY_CODE:'CompanyCode',
        DEFAULT_COMPANY_ID:'CompanyId',
        DISPLAY_NAME:'DisplayName',
        USER_TYPE:'UserType',
        USER_MENU: 'UserMenu',
        COMPANY_CURRENCY:"CompanyCurrency",
        IDLE_FLAG:'idleFlag',
        IDLE_START:'idleStart'
    },
    client_code:'Evo2Api',
    client_aud_code:'Evo2SPA',
    visitSearchValidateFields: {
        // "customerName": "Customer Name",
        "customerId": "Customer Name",
        "contractNo": "Contract No",
        "customerContarctNo": "Customer Contract No",
        //"assignmentNo": "Assignment No", Commented because of 500 error
        "reportNo": "Report No",
        "ProjectNo": "Project No",
        "customerProjectName": "Customer Project Name",
        "supplierPoNo": "Supplier PO Number",
        "supplierSubSupplier": "Supplier/Sub-Supplier",
        "technicalSpecialist": "Resource",
        "notificationRef": "Notification Reference",
        "eariliestvisitDate": "Earliest Visit Date",
        "lastvaisitdate": "Latest Visit Date",
        "contractHoldingCompany": "Contract Holding Company",               
        "operatingCompany": "Operating Company",      
        'visitCategory':'Category',
        'materialDescription':'Material Description'  
    },
    timesheetSearchValidateFields: {
        // "timesheetCustomerName": "Customer Name",
        "timesheetCustomerId": "Customer Name",
        "timesheetContractNumber": "Contract Number",
        "timesheetProjectNumber": "Project Number",
        //"timesheetAssignmentNumber": "Assignment Number", Commented because of 500 error        
        "timesheetStartDate": 'Earliest Date',
        "timesheetEndDate": 'Latest Date',
        "timesheetStatus": "Status",
        "timesheetContractHolderCompany": "Contract Holding Company",        
        "timesheetOperatingCompany": "Operating Company",        
        "technicalSpecialistName": "Resource(s)",
        "timesheetDescription": "Timesheet Description",
        'timesheetCategory':'Category'  
    },
    assignmentSearchValidateFields: {
        "assignmentNumber": "Assignment Number",
        // "assignmentStatus": "Assignment Status",
        "assignmentReference": "Assignment Reference",
        // "customerName": "Customer Name",
        "assignmentCustomerId": "Customer Name",
        "contractHoldingCompanyCode": "Contract Holding Company",
        "operatingCompanyCode": "Operating Company",
        "projectNumber": "Project Number",
        "supplierPurchaseOrderNumber": "Supplier Purchase Order",
        "technicalSpecialistName": "Resource",
        "assignmentCategory":"Category",
        'materialDescription':'Material Description'  
    },
    supplierPOSearchValidateFields: {
        'supplierPONumber': 'Supplier PO',
        'supplierPODeliveryDate': 'Delivery Date',
        'supplierPOCompletedDate': 'Completed Date',
        'supplierPOContractNumber': 'Contract Number',
        'SupplierPoProjectNumber': 'Project Number',
        //  'supplierPOStatus': 'Status',
        'supplierPOCustomerProjectName': 'Customer Project Name',
        'searchDocumentType': 'Search Documents',
        'documentSearchText': 'Search Document(s) For Words',
        'supplierPOContractHolderCompany': 'Contract Holding Company',
        'materialDescription': 'Material Description',
        // 'SupplierPOCustomerName': 'Customer Name',
        'customerId':'Customer Name',
        'supplierPOMainSupplierName': 'Main Supplier',
        'supplierPOSubSupplierName': 'Sub-Supplier'
    },
    resourceSearchValidateFields: {
        "firstName": "First Name",
        "lastName": "Last Name",
        "pin": "ePin",
        "employmentStatus": "Employment Status",
        "profileStatus": "Profile Status",
        "service": "Service",
        "companyCode": "Company Name", ////def 363,549
        "city": "City",
        "postalCode": "Postal Code",
        "fullAddress": "Full Address"
    },
    systemContants:[
        'ReleaseNoteText','ReleaseNoteUrl'
    ],
    //def 1100 ((nn) Home Page Documentation ) : fix
    documentationContants:[
        'DescText','URLText','DescText1','URLText1','DescText2','URLText2','DescText3','URLText3','DescText4','URLText4','DescText5','URLText5','DescText6','URLText6','DescText7','URLText7'
    ],
    
    //Defect ID 702 - customer visible switch can Enable while document Upload in Visit and Timesheet
    customerVisibleDocType:[       
        'Non Conformance Report','Report - Flash','Release Note', 'Report - Inspection'
    ],
    customerVisibleAprovedStauts:{
        "ReportInspection": "Report - Inspection"
    },
    RESOURE:{
        INTER_COMPANY_RESOURCE_USER_TYPE: "InterCompanyResourceUserType"
    }
};
export const countryCode ={
    ENGLISH:'EN'   
};