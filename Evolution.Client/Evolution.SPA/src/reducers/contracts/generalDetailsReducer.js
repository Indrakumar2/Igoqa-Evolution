import { customerActionTypes, companyActionTypes, contractActionTypes,sideMenu } from '../../constants/actionTypes';

export const GeneralDetailReducer = (state, actions) => {
    const { type, data } = actions;
    switch (type) {
        case companyActionTypes.FETCH_COMPANY_OFFICES:
            state = {
                ...state,
                companyOffices: data,
            };
            return state;
        case companyActionTypes.FETCH_PARENT_CONTRACT_NUMBER:
            state = {
                ...state,
                parentContractNumber: data,
            };
            return state;
        case companyActionTypes.FETCH_PARENT_CONTRACT_GENERAL_DETAILS:
            state = {
                ...state,
                contractDetail: data,
            };
            return state;
        case companyActionTypes.CLEAR_PARENT_CONTRACT_GENERAL_DETAILS:
            state = {
                ...state,
                contractDetail: []
            };
            return state;
        case contractActionTypes.CRM_YES:
            state = {
                ...state,
                CustomerCodeInCRM: true,
            };
            return state;

        case contractActionTypes.CRM_NO:
            state = {
                ...state,
                CustomerCodeInCRM: false,
            };
            return state;
        case contractActionTypes.CRM_SELECT:
            state = {
                ...state,
                CustomerCodeInCRM: "",
            };
            return state;
        case customerActionTypes.FETCH_CUSTOMER_LIST:
            state = {
                ...state,
                customerList: data
            };
            return state;
        case contractActionTypes.CLEAR_CUSTOMER_LIST:
            state = {
                ...state,
                customerList: []
            };
            return state;
        case contractActionTypes.CLEAR_SEARCH_DATA:
            state = {
                ...state,
                customerContract: []
            };
            return state;
        case contractActionTypes.GENERAL_DETAILS_SELECTED_CUSTOMER_CODE:
            state = {
                ...state,
                selectedCustomerName: data
            };
            return state;
        case contractActionTypes.SELECTED_CUSTOMER_NAME:
            state = {
                ...state,
                customerName: data[0]
            };
            return state;
        case contractActionTypes.CUSTOMER_SHOW_MODAL:
            state = {
                ...state,
                isShowModal: !state.isShowModal,
                countryMasterData: state.countryMasterData,
            };
            return state;
        case contractActionTypes.CUSTOMER_HIDE_MODAL:
            state = {
                ...state,
                isShowModal: false,
            };
            return state;
        case contractActionTypes.ADD_UPDATE_GENERAL_DETAILS:
            state = {
                ...state,
                contractDetail: {
                    ...state.contractDetail,
                    ContractInfo: data
                },
                isbtnDisable: false
            };
            return state;
        case contractActionTypes.SELECTED_CONTRACT_TYPE:
            state = {
                ...state,
                selectedContractType: data
            };
            return state;
            case sideMenu.CREATE_CONTRACT :
            state={
                ...state,
                parentContractNumber:[]
            };
            return state;
        default:
            return state;
    };
};
