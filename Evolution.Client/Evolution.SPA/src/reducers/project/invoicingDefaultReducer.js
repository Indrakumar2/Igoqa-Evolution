import { projectActionTypes } from '../../constants/actionTypes';

export const ProjectInvoicingDefaults = (state, action) => {
    const { type, data } = action;
    switch (type) {
        case projectActionTypes.invoicingDefaults.PROJECT_CUSTOMER_CONTACT:
            state = {
                ...state,
                customerContractContact: data,
            };
            return state;
        case projectActionTypes.invoicingDefaults.PROJECT_FETCH_TAXES:
            state = {
                ...state,
                taxes: data,
            };
            return state;
        case projectActionTypes.invoicingDefaults.FETCH_CUSTOMER_TAX:
           state={
               ...state.projectDetail.ProjectInfo,
               euvatPrefix:data.euvatPrefix,
               vatRegistrationNumber:data.vatRegistrationNumber,
           };
        case projectActionTypes.invoicingDefaults.PROJECT_CUSTOMER_CONTRACT_ADDRESS:
            state = {
                ...state,
                customerContractAddress: data,
            };
            return state;
        case projectActionTypes.invoicingDefaults.PROJECT_INVOICE_REMITTANCE_FOOTER_TEXT:
            state = {
                ...state,
                invoiceRemittanceandFooterText: data,
            };
            return state;
        // case projectActionTypes.invoicingDefaults.PROJECT_ASSIGNMENT_TYPE:
        //     state = {
        //         ...state,
        //         projectDocumentTypeMasterData: data
        //     };
        //     return state;
        case projectActionTypes.invoicingDefaults.ADD_PROJECT_INVOICE_DEFAULT:
            if (state.projectDetail.ProjectInvoiceReferences == null) {
                state = {
                    ...state,
                    projectDetail: {
                        ...state.projectDetail,
                        ProjectInvoiceReferences: []
                    },
                    isbtnDisable: false
                };
            }
            state = {
                ...state,
                projectDetail: {
                    ...state.projectDetail,
                    ProjectInvoiceReferences: data
                },
                isbtnDisable: false
            };
            return state;        
        case projectActionTypes.invoicingDefaults.UPDATE_PROJECT_INVOICE_DEFAULT:
            state = {
                ...state,
                projectDetail: {
                    ...state.projectDetail,
                    ProjectInvoiceReferences: data
                },
                isbtnDisable: false
            };
            return state;
        case projectActionTypes.invoicingDefaults.DELETE_PROJECT_INVOICE_DEFAULT:
            state = {
                ...state,
                projectDetail: {
                    ...state.projectDetail,
                    ProjectInvoiceReferences: data
                },
                isbtnDisable: false
            };
            return state;
        case projectActionTypes.invoicingDefaults.ADD_PROJECT_ATTACHMENT_TYPES:
            if (state.projectDetail.ProjectInvoiceAttachments == null) {
                state = {
                    ...state,
                    projectDetail: {
                        ...state.projectDetail,
                        ProjectInvoiceAttachments: []
                    },
                    isbtnDisable: false
                };
            }
            state = {
                ...state,
                projectDetail: {
                    ...state.projectDetail,
                    ProjectInvoiceAttachments: [
                        ...state.projectDetail.ProjectInvoiceAttachments, data
                    ]
                },
                isbtnDisable: false
            };
            return state;        
        case projectActionTypes.invoicingDefaults.UPDATE_PROJECT_ATTACHMENT_TYPES:
            state = {
                ...state,
                projectDetail: {
                    ...state.projectDetail,
                    ProjectInvoiceAttachments: data
                },
                isbtnDisable: false
            };
            return state;
        case projectActionTypes.invoicingDefaults.DELETE_PROJECT_ATTACHMENT_TYPES:
            state = {
                ...state,
                projectDetail: {
                    ...state.projectDetail,
                    ProjectInvoiceAttachments: data
                },
                isbtnDisable: false
            };
            return state;
        case projectActionTypes.invoicingDefaults.ADD_UPDATE_PROJECT_INVOICING_DEFAULTS:
            state = {
                ...state,
                projectDetail: {
                    ...state.projectDetail,
                    ProjectInfo: data
                },
                isbtnDisable: false
            };
            return state;
        default:
            return state;
    }
};