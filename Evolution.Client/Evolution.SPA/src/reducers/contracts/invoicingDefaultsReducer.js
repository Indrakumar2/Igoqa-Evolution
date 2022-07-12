import { contractActionTypes } from '../../constants/actionTypes';

export const ContractInvoicingDefaults = (state, action) => {
    const { type, data } = action;
    switch (type) {
        case contractActionTypes.invoiceDefaultActionTypes.SHOW_CUSTOMER_CONTACT:
            state = {
                ...state,
                customerContractContact: data,
            };
            return state;
        case contractActionTypes.invoiceDefaultActionTypes.FETCH_TAXES:
            state = {
                ...state,
                taxes: data,
            };
            return state;
        case contractActionTypes.invoiceDefaultActionTypes.SHOW_CUSTOMER_CONTRACT_ADDRESS:
            state = {
                ...state,
                customerContractAddress: data,
            };
            return state;
        case contractActionTypes.invoiceDefaultActionTypes.SHOW_INVOICE_CURRENCY:
            state = {
                ...state,
                invoiceCurrency: data,
            };
            return state;
        case contractActionTypes.invoiceDefaultActionTypes.SHOW_INVOICE_REMITTANCE_FOOTER_TEXT:
            state = {
                ...state,
                invoiceRemittanceandFooterText: data,
            };
            return state;
        case contractActionTypes.invoiceDefaultActionTypes.SHOW_DEFAULT_INVOICE_REFERENCES:
            state = {
                ...state,
                defaultInvoiceRefernces: data,
            };
            return state;        
        case contractActionTypes.invoiceDefaultActionTypes.ADD_NEW_INVOICE_DEFAULT:
            if (state.contractDetail.ContractInvoiceReferences == null) {
                state = {
                    ...state,
                    contractDetail: {
                        ...state.contractDetail,
                        ContractInvoiceReferences: []
                    },
                    isbtnDisable: false
                };
            }
            state = {
                ...state,
                contractDetail: {
                    ...state.contractDetail,
                    ContractInvoiceReferences: data
                },
                isbtnDisable: false
            };
            return state;
        case contractActionTypes.invoiceDefaultActionTypes.DELETE_INVOICE_DEFAULT:
            state = {
                ...state,
                contractDetail: {
                    ...state.contractDetail,
                    ContractInvoiceReferences: data
                },
                isbtnDisable: false
            };
            return state;
        case contractActionTypes.invoiceDefaultActionTypes.EDIT_INVOICE_DEFAULT:
            state = {
                ...state,
                editInvoiceReferences: data
            };
            return state;
        case contractActionTypes.invoiceDefaultActionTypes.UPDATE_INVOICE_DEFAULT:
            state = {
                ...state,
                contractDetail: {
                    ...state.contractDetail,
                    ContractInvoiceReferences: data
                },
                isbtnDisable: false
            };
            return state;
        case contractActionTypes.invoiceDefaultActionTypes.SHOW_DEFAULT_INVOICE_ATTACHMENT_TYPES:
            state = {
                ...state,
                defaultInvoiceAttachmentTypes: data,
            };
            return state;
        case contractActionTypes.invoiceDefaultActionTypes.ADD_ATTACHMENT_TYPES:
            if (state.contractDetail.ContractInvoiceAttachments == null) {
                state = {
                    ...state,
                    contractDetail: {
                        ...state.contractDetail,
                        ContractInvoiceAttachments: []
                    },
                    isbtnDisable: false
                };
            }
            state = {
                ...state,
                contractDetail: {
                    ...state.contractDetail,
                    ContractInvoiceAttachments: [
                        ...state.contractDetail.ContractInvoiceAttachments, data
                    ]
                },
                isbtnDisable: false
            };
            return state;
        case contractActionTypes.invoiceDefaultActionTypes.DELETE_ATTACHMENT_TYPES:
            state = {
                ...state,
                contractDetail: {
                    ...state.contractDetail,
                    ContractInvoiceAttachments: data
                },
                isbtnDisable: false
            };
            return state;
        case contractActionTypes.invoiceDefaultActionTypes.EDIT_ATTACHMENT_TYPES:
            state = {
                ...state,
                editAttachmentTypes: data
            };
            return state;
        case contractActionTypes.invoiceDefaultActionTypes.UPDATE_ATTACHMENT_TYPES:
            state = {
                ...state,
                contractDetail: {
                    ...state.contractDetail,
                    ContractInvoiceAttachments: data
                },
                isbtnDisable: false
            };
            return state;
        case contractActionTypes.invoiceDefaultActionTypes.ADD_UPDATE_INVOICING_DEFAULTS:        
            state = {
                ...state,
                contractDetail: {
                    ...state.contractDetail,
                    ContractInfo: data
                },
                isbtnDisable: false
            };
            return state;
        case contractActionTypes.invoiceDefaultActionTypes.IS_INVOICE_REFERENCE_MODAL_OPEN:
            if (data === false) {
                state = {
                    ...state,
                    editInvoiceReferences: {}
                };
            }
            state = {
                ...state,
                isInvoiceReferenceModalOpen: data
            };
            return state;
        case contractActionTypes.invoiceDefaultActionTypes.IS_INVOICE_REFERENCE_EDIT:
            state = {
                ...state,
                isInvoiceReferenceEdit: data
            };
            return state;
        // case contractActionTypes.invoiceDefaultActionTypes.CONTRACT_DOCUMENT_TYPE_MASTER:
        //     state = {
        //         ...state,
        //         contractDocumentTypeMasterData: data
        //     };
        //     return state;
        case contractActionTypes.invoiceDefaultActionTypes.IS_INVOICE_ATTACHMENT_TYPES_MODAL_OPEN:
            if (data === false) {
                state = {
                    ...state,
                    editAttachmentTypes: {}
                };
            }
            state = {
                ...state,
                isInvoiceAttachmentTypesModalOpen: data
            };
            return state;
        case contractActionTypes.invoiceDefaultActionTypes.IS_INVOICE_ATTACHMENT_TYPES_EDIT:
            state = {
                ...state,
                isInvoiceAttachmentTypesEdit: data
            };
            return state;
        default:
            return state;
    }
};