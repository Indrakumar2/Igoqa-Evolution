export default {
    company: [
        {
            name: 'GeneralDetails',
            componentType: '',
            component: require('./components/applicationComponents/company/generalDetails')
        },
        {
            name: 'EMailTemplates',
            componentType: '',
            component: require('./components/applicationComponents/company/emailTemplates')
        },
        {
            name: 'DivisionCostCenter',
            componentType: '',
            component: require('./components/applicationComponents/company/divisionCostCenterMargin')
        },
        {
            name: 'ExpectedMargin',
            componentType: '',
            component: require('./components/applicationComponents/company/expectedMarginByBusinessUnit')
        },
        {
            name: 'CompanyOffices',
            componentType: '',
            component: require('./components/applicationComponents/company/companyOffices')
        },
        {
            name: 'Payroll',
            componentType: '',
            component: require('./components/applicationComponents/company/payroll')
        },
        {
            name: 'Contracts',
            componentType: '',
            component: require('./components/applicationComponents/company/contracts')
        },
        {
            name: 'Documents',
            componentType: '',
            component: require('./components/applicationComponents/company/documents')
        },
        {
            name: 'Notes',
            componentType: '',
            component: require('./components/applicationComponents/company/notes')
        }
    ],
    customer: [
        {
            name: 'GeneralDetails',
            componentType: '',
            component: require('./components/applicationComponents/customer/generalDetails')
        },
        {
            name: 'PortalAccess',
            componentType: '',
            component: require('./components/applicationComponents/customer/extranet')
        },
        {
            name: 'AssignmentAccountReference',
            componentType: '',
            component: require('./components/applicationComponents/customer/assignment')
        },
        {
            name: 'Contracts',
            componentType: '',
            component: require('./components/applicationComponents/customer/contracts')
        },
        {
            name: 'Documents',
            componentType: '',
            component: require('./components/applicationComponents/customer/documents')
        },
        {
            name: 'Notes',
            componentType: '',
            component: require('./components/applicationComponents/customer/notes')
        }
    ],
    contract: [
        {
            name: 'GeneralDetails',
            componentType: '',
            component: require('./components/applicationComponents/contracts/generalDetails')
        },
        {
            name: 'ChildContracts',
            componentType: '',
            component: require('./components/applicationComponents/contracts/childContracts')
        },
        {
            name: 'InvoicingDefaults',
            componentType: '',
            component: require('./components/applicationComponents/contracts/invoicingDefaults')
        },
        {
            name: 'FixedExchangeRates',
            componentType: '',
            component: require('./components/applicationComponents/contracts/fixedExchangeRates')
        },
        {
            name: 'RateSchedule',
            componentType: '',
            component: require('./components/applicationComponents/contracts/rateSchedule')
        },
        {
            name: 'Documents',
            componentType: '',
            component: require('./components/applicationComponents/contracts/documents')
        },
        {
            name: 'Project',
            componentType: '',
            component: require('./components/applicationComponents/contracts/project')
        },
        {
            name: 'Notes',
            componentType: '',
            component: require('./components/applicationComponents/contracts/notes')
        }
    ],
    project: [
        {
            name: 'GeneralDetails',
            componentType: '',
            component: require('./components/applicationComponents/projects/generalDetails')
        },
        {
            name: 'InvoicingDefaults',
            componentType: '',
            component: require('./components/applicationComponents/projects/invoicingDefaults')
        },
        {
            name: 'Documents',
            componentType: '',
            component: require('./components/applicationComponents/projects/documents')
        },
        {
            name: 'SupplierPO',
            componentType: '',
            component: require('./components/applicationComponents/projects/supplierPO')
        },
        {
            name: 'Assignments',
            componentType: '',
            component: require('./components/applicationComponents/projects/assignments')
        },
        {
            name: 'Notes',
            componentType: '',
            component: require('./components/applicationComponents/projects/notes')
        },
        {
            name: 'ClientNotification',
            componentType: '',
            component: require('./components/applicationComponents/projects/clientNotification')
        },
    ],
    supplier: [
        {
            name: 'SupplierDetails',
            componentType: '',
            component: require('./components/applicationComponents/supplier/supplierDetail')
        },
        {
            name: 'Documents',
            componentType: '',
            component: require('./components/applicationComponents/supplier/documents')
        },
        {
            name: 'Notes',
            componentType: '',
            component: require('./components/applicationComponents/supplier/notes')
        },
    ],
    supplierPo: [
        {
            name: 'SupplierDetails',
            componentType: '',
            component: require('./components/applicationComponents/supplierPO/supplierDetails')
        },
        {
            name: 'Assignments',
            componentType: '',
            component: require('./components/applicationComponents/supplierPO/assignments')
        },

        {
            name: 'Documents',
            componentType: '',
            component: require('./components/applicationComponents/supplierPO/documents')
        },
        {
            name: 'Notes',
            componentType: '',
            component: require('./components/applicationComponents/supplierPO/notes')
        },
    ],
    assignment: [
        {
            name: 'generalDetails',
            componentType: '',
            component: require('./components/applicationComponents/assignment/generalDetails')
        },
        {
            name: 'technicalDisciplines',
            componentType: '',
            component: require('./components/applicationComponents/assignment/technicalDisciplines')
        },
        {
            name: 'supplierInformation',
            componentType: '',
            component: require('./components/applicationComponents/assignment/supplierInformation')
        },
        {
            name: 'contractRateSchedules',
            componentType: '',
            component: require('./components/applicationComponents/assignment/contractRateSchedules')
        },
        {
            name: 'assignmentReferences',
            componentType: '',
            component: require('./components/applicationComponents/assignment/assignmentReferences')
        },
        {
            name: 'assignedSpecialists',
            componentType: '',
            component: require('./components/applicationComponents/assignment/assignedSpecialists')
        },
        {
            name: 'assignmentInstructions', //assigniedInstructions changed as a assignmentInstructions
            componentType: '',
            component: require('./components/applicationComponents/assignment/assignmentInstructions')
        },
        {
            name: 'additionalExpenses',
            componentType: '',
            component: require('./components/applicationComponents/assignment/additionalExpenses')
        },
        {
            name: 'interCompanyDiscounts',
            componentType: '',
            component: require('./components/applicationComponents/assignment/interCompanyDiscounts')
        },
        {
            name: 'documents',
            componentType: '',
            component: require('./components/applicationComponents/assignment/documents')
        },
        {
            name: 'notes',
            componentType: '',
            component: require('./components/applicationComponents/assignment/notes')
        },
        {
            name: 'timesheets',
            componentType: '',
            component: require('./components/applicationComponents/assignment/timesheets')
        },
        {
            name: 'visits',
            componentType: '',
            component: require('./components/applicationComponents/assignment/visits')
        },
    ],
    techSpec: [
        {
            name: 'ResourceStatus',
            componentType: '',
            component: require('./grm/components/techSpec/resourceStatus')
        },
        {
            name: 'ContactInformation',
            componentType: '',
            component: require('./grm/components/techSpec/contactInformation')
        },
        {
            name: 'PayRate',
            componentType: '',
            component: require('./grm/components/techSpec/payRate')
        },
        {
            name: 'Taxonomy',
            componentType: '',
            component: require('./grm/components/techSpec/taxonomy')
        },
        {
            name: 'ProfessionalEducational',
            componentType: '',
            component: require('./grm/components/techSpec/professionalEducationalDetails')
        },
        {
            name: 'ResourceCapability',
            componentType: '',
            component: require('./grm/components/techSpec/resourceCapability')
        },
        {
            name: 'SensitiveDocuments',
            componentType: '',
            component: require('./grm/components/techSpec/sensitiveDocuments')
        },
        {
            name: 'Documents',
            componentType: '',
            component: require('./grm/components/techSpec/documents')
        },
        {
            name: 'Comments',
            componentType: '',
            component: require('./grm/components/techSpec/comments')
        },
        {
            name: 'TaxonomyApproval',
            componentType: '',
            component: require('./grm/components/techSpec/taxonomyApproval')
        }

    ],
    visits: [
        {
            name: 'GeneralDetails',
            componentType: '',
            component: require('./components/applicationComponents/visit/generalDetails')
        },
        {
            name: 'HistorialVisit',
            componentType: '',
            component: require('./components/applicationComponents/visit/historialVisit')
        },
        {
            name: 'TechnicalSpecialistAccounts',
            componentType: '',
            component: require('./components/applicationComponents/visit/technicalSpecialistAccounts')
        },
        {
            name: 'VisitReference',
            componentType: '',
            component: require('./components/applicationComponents/visit/visitReference')
        },
        {
            name: 'SupplierPerformance',
            componentType: '',
            component: require('./components/applicationComponents/visit/supplierPerformance')
        },
        {
            name: 'InterCompanyDiscounts',
            componentType: '',
            component: require('./components/applicationComponents/visit/interCompanyDiscount')
        },
        {
            name: 'Documents',
            componentType: '',
            component: require('./components/applicationComponents/visit/documents')
        },
        {
            name: 'Notes',
            componentType: '',
            component: require('./components/applicationComponents/visit/notes')
        },
        {
            name: 'Invoice',
            componentType: '',
            component: require('./components/applicationComponents/visit/invoice')
        }
    ],
    timesheets: [
        {
            name: 'GeneralDetails',
            componentType: '',
            component: require('./components/applicationComponents/timesheet/generalDetails')
        },
        {
            name: 'TechnicalSpecialistAccounts',
            componentType: '',
            component: require('./components/applicationComponents/timesheet/technicalSpecialistAccounts')
        },
        {
            name: 'TimesheetReference',
            componentType: '',
            component: require('./components/applicationComponents/timesheet/timesheetReference')
        },
        {
            name: 'Documents',
            componentType: '',
            component: require('./components/applicationComponents/timesheet/documents')
        },
        {
            name: 'Notes',
            componentType: '',
            component: require('./components/applicationComponents/timesheet/notes')
        },
        {
            name: 'Invoice',
            componentType: '',
            component: require('./components/applicationComponents/timesheet/invoice')
        }
    ],
     
    assignmentLifecycleStatus: [        
        {
            name: 'Lifecycle',
            componentType: '',
            component: require('./components/applicationComponents/admin/manageAssignmentLifeCycle/lifeCycle')
        },
        {
            name: 'Status',
            componentType: '',
            component: require('./components/applicationComponents/admin/manageAssignmentLifeCycle/status')
        }         
        
    ],
    common: [
        {
            name: 'CustomInput',
            componentType: '',
            component: require('./common/baseComponents/inputControlls')
        },
        {
            name: 'CustomerAndCountrySearch',
            componentType: '',
            component: require('./components/applicationComponents/customerAndCountrySearch')
        },
        {
            name: 'InputWithPopUpSearch',
            componentType: '',
            component: require('./components/applicationComponents/inputWithPopUpSearch')
        },
    ]
};
