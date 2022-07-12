export const fieldLengthConstants = {
    common:{
        notes:{
            NOTES_MAXLENGTH: 4000,
            VISIT_TIMESHEET_MAXLENGTH:1000,
        },
        selectType:{
            VISIT_TIMESHEET_TEXT_MAXLENGTH: 20,
            DEFAULT_DESCRIPTION_MAXLENGTH: 500,
        },
        budget:{
            BUDGET_VALUE_MAXLENGTH: 17,
            BUDGET_HOURS_MAXLENGTH: 14,
            BUDGET_VALUE_PREFIX_LIMIT: 11,
            BUDGET_HOURS_PREFIX_LIMIT: 9,
            BUDGET_VALUE_SUFFIX_LIMIT: 2,
            BUDGET_HOURS_SUFFIX_LIMIT: 2,
        },
    },
    
    company: {
        generalDetails:{
           VATTAX_REGISTRATION_NUMBER_MAXLENGTH:60,
           REVERSE_CHARGE_DISCLAIMER_MAXLENGTH:150,
           IDENTIFIER_REMITTANCE_MAXLENGTH:100,
           INVOICE_REMITTANCE_TEXT_MAXLENGTH:1500,
           IDENTIFIER_FOOTER_MAXLENGTH:100,
           INVOICE_FOOTER_TEXT_MAAXLENGTH:500,
           INVOICE_DRAFT_TEXT_MAXLENGTH:10,
           INVOICE_DESCRIPTION_MAXLENGTH:25,
           INTERCO_DRAFT_TEXT_MAXLENGTH:10,
           INTER_COMPANY_DESCRIPTION_MAXLENGTH:25,
           INTER_COMPANY_TEXT_MAXLENGTH:100,
           INVOICE_HEADER_MAXLENGTH:250,
           INVOICE_SUMMARY_PAGE_FREE_TEXT_MAXLENGTH:500,
           RESOURCE_OUTSIDE_DISTANCE_MAXLENGTH:4,
           VAT_REGUATION_TEXT_WITHIN_EC_MAXLENGTH:135, // CR560
           VAT_REGUATION_TEXT_OUTSIDE_EC_MAXLENGTH:135, // CR560

        },
        CompanyEmail:{
            COMPANY_EMAIL_TEMPLATE:4000,
        },
        divisionCostCenter:{
            DIVISION_MAXLENGTH:30,
            ACOUNT_REFERENCE_MAXLENGTH:15,
            COST_CENTER_CODE_MAXLENGTH:20,
            COST_CENTER_NAME_MAXLENGTH:100

        },
        expectedMargin:{
            MINIMUM_EXPECTED_MARGIN_MAXLENGTH:10,
        },
        companyOffices:{
           OFFICE_NAME_MAXLENGTH:30,
           ACCOUNT_REFERENCE_MAXLENGTH:15,
           PINCODE_MAXLENGTH:15,
           COMPANY_FULL_ADDRESS_MAXLENGTH: 200,

        },
        payRoll:{
            PAYROLL_NAME_MAXLENGTH:20,
            EXPORT_PREFIX_MAXLENGTH:20,
            PERIOD_NAME_MAXLENGTH:10,
        }
    },
    Customer:{
        generalDetails:{
            FULL_ADDRESS_MAXLENGTH:500,
            POSTAL_CODE_MAXLENGTH:15,
            VAT_TAX_REGISTRATION_NO_MAXLENGTH:60,
            CONTACT_NAME_MAXLENGTH:50,
            POSITION_MAXLENGTH:60,
            TELEPHONE_NO_MAXLENGTH:60,
            FAX_NO_MAXLENGTH:60,
            MOBILE_NO_MAXLENGTH:60,
            EMAIL_MAXLENGTH:60,
            CUST_OTHER_CONTACT_DETAILS_MAXLENGTH:150,

        },
        portalAccess:{
            USER_NAME_MAXLENGTH:50,
            EMAIL_MAXLENGTH:128,
            PASSWORD_MAXLENGTH:128, //Changes for IGO D946
            CONFIRM_PASSWORD_MAXLENGTH:128,
            PASSWORD_QUESTION_MAXLENGTH:255,
            PASSWORD_ANSWER_MAXLENGTH:255,
            COMMENTS_MAXLENGTH:255

        },
        Assignment_Account_Reference:
        {
          ACCOUNT_REFERENCE_MAXLENGTH:20,
        },
    },

    Contract:{
        generalDetails:{
            CUSTOMER_CONTRACT_NUMBER_MAXLENGTH:40,
            CRM_CONFLICTS_OF_BID_REVIEW_COMMENTS_MAXLENGTH:500,
            CRM_REASON_MAXLENGTH:50,
            BUDGET_WARNING_MAXLENGTH:4,
            BUDGETHOURS_WARNING_MAXLENGTH:4,
            CLIENT_REPORTING_REQUIREMENTS_MAXLENGTH:1000,
            ASSIGNMENT_INSTRUCTIONAL_OPERATIONAL_NOTES_MAXLENGTH:4000,
            BUDGET_VALUE_PREFIX_LIMIT:16,
            BUDGET_VALUE_SUFFIX_LIMIT:2,
            BUDGET_HOURS_PREFIX_LIMIT:16,
            BUDGET_HOURS_SUFFIX_LIMIT:2

        },
     invoiceDefaults:
     {
        INVOICE_NOTES_MAXLENGTH:4000,
        INVOICE_FREE_TEXT_MAXLENGTH:1000,
     },
     rateSchedule:
     {
         SCHEDULE_NAME_MAXLENGTH:200,
         SCHEDULE_NAME_PRINTED_ON_INVOICE_MAXLENGTH:20,
         CHARGE_RATE_DESCRIPTION_MAXLENGTH:100,

     },

    },
    Project:{
        generalDetails:{
            CUSTOMER_PROJECT_NUMBER_MAXLENGTH:40,
            CUSTOMER_PROJECT_NAME_MAXLENGTH:60,
            CUSTOMER_REPORTING_REQUIREMENTS_MAXLENGTH:1000,
            ASSIGNMENT_INSTRUCTIONAL_OPERATIONAL_NOTES_MAXLENGTH:4000,
            BUDGET_HOURS_PREFIX_LIMIT:16,
            BUDGET_HOURS_SUFFIX_LIMIT:2,
            BUDGET_VALUE_PREFIX_LIMIT:16,
            BUDGET_VALUE_SUFFIX_LIMIT:2
            
        },
        invoiceDefaults:
        {
            INVOICE_NOTES_MAXLENGTH:4000,
            INVOICE_FREE_TEXT_MAXLENGTH:1000

        }
    },
    Resource:
    {
        contactInformation:{
            
        FIRST_NAME_MAXLENGTH:30,
        LAST_NAME_MAXLENGTH:30,
        CONTACT_COMMENTS_MAXLENGTH:1200, //D1394
        MOBILE_NUMBER_MAXLENGTH:60,
        OTHER_PHONE_NUMBER_MAXLENGTH:60,
        EMAIL_MAXLENGTH:60,
        EMERGENCY_CONTACT_NAME_MAXLENGTH:250,
        EMERGENCY_CONTACT_NUMBER_MAXLENGTH:60,
        STREET_ADDRESS_MAXLENGTH:200,
        PASSWORD_MAXLENGTH:128,
        PASSWORD_ANSWER_MAXLENGTH:255,
        USER_NAME_MAXLENGTH:50,
        ZIP_POSTAL_CODE_MAXLENGTH:15,
        TS_HOME_PAGE_COMMENTS_MAXLENGTH:2000,

        },
        payRate:
        {
            PAYROL_REFERNCE_MAXLENGTH:15,
            PAYROLL_NOTES_MAXLENGTH:2000,
            TAX_REF_NO_MAXLENGTH:30,
            PAY_RATE_SCEDULEE_MAXLENGTH:100,
            SCHEDULE_NOTES_MAXLENGTH:200,
            DESCRIPTION_TEXT_MAXLENGTH:100,
        }

    },

    supplier: {
       supplierDetails:{
           SUPPLIER_NAME_MAXLENGTH: 60,
           POSTAL_CODE_MAXLENGTH: 15,
           FULL_ADDRESS_MAXLENGTH: 200,
           CONTACT_NAME_MAXLENGTH: 60,
           TELEPHONE_NUMBER_MAXLENGTH: 60,
           FAX_NUMBER_MAXLENGTH: 60,
           MOBILE_NUMBER_MAXLENGTH: 60,
           SUPPLIER_EMAIL_MAXLENGTH: 60,
           OTHER_CONTACT_DETAILS_MAXLENGTH: 150,
       },
    },

    supplierpo: {
        supplierDetails:{
            SUPPLIER_PO_NUMBER_MAXLENGTH: 150,
            MATERIAL_DESCRIPTION_MAXLENGTH: 200,
            SUPPLIER_FULL_ADDRESS_MAXLENGTH: 150,
            SUPPLIERPO_BUDGET_VALUE_MAXLENGTH: 14,
            SUPPLIERPO_BUDGET_WARNING_MAXLENGTH: 2,
            SUPPLIERPO_BUDGET_UNITS_MAXLENGTH: 12,
        },
    },

    assignment: {
        generalDetails:{
            ASSIGNMENT_REFERENCE_MAXLENGTH: 75,
            CONTRACT_HOLDING_COMPANY_MAXLENGTH: 40,
            OPERATING_COMPANY_MAXLENGTH: 40,
            WORK_LOCATION_ZIP_CODE_MAXLENGTH: 15,
            CUSTOMER_REPORTING_REQUIREMENTS: 1000,
            ASSIGNMENT_BUDGET_VALUE_MAXLENGTH: 14,
            ASSIGNMENT_BUDGET_WARNING_MAXLENGTH: 3,
            ASSIGNMENT_BUDGET_UNITS_MAXLENGTH: 12,
        },
        assignmentReferences:{
            REFERENCE_VALUE_MAXLENGTH: 60,
        },
        assignedSpecialists:{
            SCHEDULE_NOTES_ON_INVOICE_MAXLENGTH: 30,
        },
        assignmentInstructions:{
            INTER_COMPANY_INSTRUCTIONS_MAXLENGTH: 4000,
            RESOURCE_INSTRUCTIONS_MAXLENGTH: 4000,
        },
        additionalExpenses:{
            DESCRIPTION_MAXLENGTH: 50,
            RATE_MAXLENGTH: 15,
            UNITS_MAXLENGTH: 15,
        },
        interCompanyDiscounts:{
            PERCENTAGE_MAXLENGTH: 6,
            DESCRIPTION_MAXLENGTH: 20
            
        },
    },

    visit: {
        generalDetails:{
            CUSTOMER_NAME_MAXLENGTH: 60,
            REPORT_NUMBER_MAXLENGTH: 65,
            VISIT_DATE_PERIOD_MAXLENGTH: 30,
            VISIT_SUMMARY_REPORT_MAXLENGTH: 400,
            VISIT_REFERENCE_MAXLENGTH: 50,
            PERCENTAGE_COMPLETE_MAXLENGTH: 60,
            NOTIFICATION_REFERENCE_MAXLENGTH: 25,
        },
        supplierPerformance:{
            NCR_REFERENCE_MAXLENGTH: 50,
        },
        visitReference:{
            REFERENCE_VALUE_MAXLENGTH: 50,
        },
        resourceAccounts:{
            time:{
                EXPENSE_DESCRIPTION_MAXLENGTH: 50,
                CHARGE_RATE_DESCRIPTION_MAXLENGTH: 100,
                PAY_RATE_DESCRIPTION_MAXLENGTH: 100,
            },
            expense:{
                EXPENSE_DESCRIPTION_MAXLENGTH: 50,
            },
            travel:{
                EXPENSE_DESCRIPTION_MAXLENGTH: 50,
            },
            consumable:{
                CHARGE_DESCRIPTION_MAXLENGTH: 100,
                PAY_RATE_DESCRIPTION_MAXLENGTH: 100,
            },
        },
        email:{
            SUBJECT_MAXLENGTH:160
        }
    },

    timesheet: {
        generalDetails:{
            TIMESHEET_DATE_PERIOD_MAXLENGTH: 30,
            TIMESHEET_DESCRIPTION_MAXLENGTH: 65,
            TIMESHEET_REFERENCE_MAXLENGTH: 50,
        },
        timesheetReference:{
            REFERENCE_VALUE_MAXLENGTH: 50,
        },
        resourceAccounts:{
            time:{
                EXPENSE_DESCRIPTION_MAXLENGTH: 50,
                CHARGE_RATE_DESCRIPTION_MAXLENGTH: 100,
                PAY_RATE_DESCRIPTION_MAXLENGTH: 100,
            },
            expense:{
                EXPENSE_DESCRIPTION_MAXLENGTH: 50,
            },
            travel:{
                EXPENSE_DESCRIPTION_MAXLENGTH: 50,
            },
            consumable:{
                CHARGE_DESCRIPTION_MAXLENGTH: 100,
                PAY_RATE_DESCRIPTION_MAXLENGTH: 100,
            },
        },
    },

    admin: {
        userRoles:{
            ROLE_NAME_MAXLENGTH: 50,
            DESCRIPTION_MAXLENGTH: 200,
        },
        users:{
            LOGON_NAME_MAXLENGTH: 50,
            USER_NAME_MAXLENGTH: 50,
            USER_EMAIL_MAXLENGTH:50,
        },
    },

};