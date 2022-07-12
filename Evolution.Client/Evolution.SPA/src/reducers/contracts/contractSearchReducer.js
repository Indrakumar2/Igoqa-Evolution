import { companyActionTypes, contractActionTypes, sideMenu } from '../../constants/actionTypes';
export const ContractSearchReducer = (state, action) => {
    const { type, data } = action;
    switch (type) {        
         case companyActionTypes.SHOW_HIDE_PANEL:
            state = {
                ...state,
                isPanelOpen: !state.isPanelOpen
            };
            return state;        
        case contractActionTypes.commonActionTypes.CONTRACT_FETCH_CUSTOMER:
            state = {
                ...state,
                customerContract: data
            };
            return state;
        case contractActionTypes.CONTRACT_SELECTED_CUSTOMER_CODE:
            state = {
                ...state,
                selectedCustomerData: data
            };
            return state;
            case contractActionTypes.CONTRACT_SELECTED_CONTRACT_NUMBER:
            state = {
                ...state,
                selectedContract: data
            };
            return state;
        case contractActionTypes.CLEAR_SEARCH_DATA:
                state = {
                ...state,
                customerContract: [],
                selectedCustomerData: {},               
                contractStatus: 'O'
            };
            return state;
            case contractActionTypes.CLEAR_GRID_FORM_SEARCH_DATAS:
            state = {
                ...state,
                customerContract: [],
            };
            return state;
        case sideMenu.EDIT_CONTRACT:
            state = {
                ...state,
                currentPage: 'Edit Contract',
                interactionMode: false,
                isPanelOpen: true,              
                customerContract: [],
                selectedCustomerData: {},
                customerName: '',
                defaultCustomerName: ' ',
                contractStatus: 'O',
                contractNo: '',
                customerContractNo: '',
                contractHoldingCompany: '',
                contractDetail: {},
                ContractSchedulesOnCancel: [],
                ContractScheduleRatesOnCancel: [],
                //Invoicing Defaults
                isInvoiceAttachmentTypesModalOpen: false,
                isInvoiceReferenceModalOpen: false,
                isInvoiceReferenceEdit: false,
                isInvoiceAttachmentTypesEdit: false,
                // Documents
                isShowDocumentModal: false,
                //copyDocumentDetails:[],

                isRateScheduleEdit: false,
                isChargeTypeEdit: false,
                isRateScheduleOpen: false,
                isChargeTypeOpen: false,
                isCopyChargeTypeOpen: false,
                rateScheduleEditData: {},
                chargeTypeEditData: {},
                isAdminContractRatesOpen: false,
                adminRateToCopy: [],

                showButton: false,
                ContractFixedRate: [],
                editFixedExchangeDetails: {},
                chechBoxHideButton: true,
                isExchangeRateEdit: false,
                isExchangeRateModalOpen: false,
                isExchangeEdit: false,
                addFixedData: {},
                currencyData: [],
                selectedContractType: "",
                selectedContract: "",
                isbtnDisable:true
            };
            return state;
        case sideMenu.VIEW_CONTRACT:
            state = {
                ...state,
                currentPage: 'View Contract',
                interactionMode: true,
                isPanelOpen: true,              
                customerContract: [],
                selectedCustomerData: {},
                customerName: '',
                defaultCustomerName: ' ',
                contractStatus: 'O',
                contractNo: '',
                customerContractNo: '',
                contractHoldingCompany: '',
                contractDetail: {},
                ContractSchedulesOnCancel: [],
                ContractScheduleRatesOnCancel: [],

                //copyDocumentDetails:[],

                //Invoicing Defaults
                isInvoiceAttachmentTypesModalOpen: false,
                isInvoiceReferenceModalOpen: false,
                isInvoiceReferenceEdit: false,
                isInvoiceAttachmentTypesEdit: false,
                // Documents
                isShowDocumentModal: false,
                isRateScheduleEdit: false,
                isChargeTypeEdit: false,
                isRateScheduleOpen: false,
                isChargeTypeOpen: false,
                isCopyChargeTypeOpen: false,
                rateScheduleEditData: {},
                chargeTypeEditData: {},
                isAdminContractRatesOpen: false,
                adminRateToCopy: [],

                showButton: false,
                ContractFixedRate: [],
                editFixedExchangeDetails: {},
                chechBoxHideButton: true,
                isExchangeRateEdit: false,
                isExchangeRateModalOpen: false,
                isExchangeEdit: false,
                addFixedData: {},
                currencyData: [],
                selectedContractType: "",
                selectedContract: "",
                isbtnDisable:true
            };
            return state;
        case sideMenu.CREATE_CONTRACT:       
            state = {
                ...state,
                currentPage: 'Create Contract',
                interactionMode: false,
                isPanelOpen: true,             
                customerContract: [],
                selectedCustomerData: {},
                customerName: '',
                customerList:[],
                defaultCustomerName: ' ',
                contractStatus: 'O',
                contractNo: '',
                customerContractNo: '',
                contractHoldingCompany: '',
                contractDetail: {
                    ...state.contractDetail,
                    ContractInfo: data,
                    ContractExchangeRates: null,
                    ContractInvoiceAttachments: null,
                    ContractInvoiceReferences: null,
                    ContractSchedules: null,
                    ContractScheduleRates: null,
                    ContractNotes: null,
                    ContractDocuments: null
                },
                //copyDocumentDetails:[],

                //Invoicing Defaults
                isInvoiceAttachmentTypesModalOpen: false,
                isInvoiceReferenceModalOpen: false,
                isInvoiceReferenceEdit: false,
                isInvoiceAttachmentTypesEdit: false,
                taxes: [],
                customerContractContact: [],
                customerContractAddress: [],
                invoiceRemittanceandFooterText: [],
                // Documents
                isShowDocumentModal: false,
                isRateScheduleEdit: false,
                isChargeTypeEdit: false,
                isRateScheduleOpen: false,
                isChargeTypeOpen: false,
                isCopyChargeTypeOpen: false,
                rateScheduleEditData: {},
                chargeTypeEditData: {},
                isAdminContractRatesOpen: false,
                adminRateToCopy: [],

                showButton: false,
                ContractFixedRate: [],
                editFixedExchangeDetails: {},
                chechBoxHideButton: true,
                isExchangeRateEdit: false,
                isExchangeRateModalOpen: false,
                isExchangeEdit: false,
                addFixedData: {},
                currencyData: [],
                selectedContractType: "",
                selectedContract: "",
                isbtnDisable:true
            };
            return state;
        default:
            return state;
    }
};