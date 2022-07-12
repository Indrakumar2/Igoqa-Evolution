import { supplierActionTypes, sideMenu } from '../../constants/actionTypes';
import { commonActionTypes } from '../../grm/constants/actionTypes';

export const SupplierReducer = (state, actions) => {
    const { type, data } = actions;
    switch (type) {
        case supplierActionTypes.FETCH_SUPPLIER_DATA:
            state = {
                ...state,
                supplierData: data
            };
            return state;
        case supplierActionTypes.SAVE_SELECTED_SUPPLIER:
            state = {
                ...state,
                selectedSupplier: data
            };
            return state;
        case supplierActionTypes.UPDATE_SUPPLIER_BTN_ENABLE_STATUS:
            state = {
                ...state,
                isbtnDisable: true
            };
            return state;
        case commonActionTypes.INTERACTION_MODE:
            state = {
                ...state,
                interactionMode: data
            };
            return state;
        case sideMenu.CREATE_SUPPLIER:
            state = {
                ...state,
                selectedSupplier: '',
                isbtnDisable: true,
                //supplierData: {},
                supplierDetails: {
                    stateForAddress: [],
                    cityForAddress: [],
                },
                supplierSearch: {
                    state: [],
                    city: [],
                    supplierSearchList: []
                }
            };
            return state;
        case sideMenu.EDIT_SUPPLIER:
            state = {
                ...state,
                supplierData: {},
                // supplierDetails: {
                //     stateForAddress: [],
                //     cityForAddress: [],
                // },
                supplierSearchList:[],
                supplierSearch: {
                    state: [],
                    city: [],
                    supplierSearchList: []
                },
                isbtnDisable: true,
            };
            return state;
        case supplierActionTypes.SUPPLIER_DUPLICATE_NAME:
            state = {
                ...state,
                duplicateMessage: data
            };
            return state;
            
            case sideMenu.HANDLE_MENU_ACTION:
            state = {
                ...state,
                supplierSearch:{
                    ...state.supplierSearch,
                    supplierSearchList:[],
                    state:[],
                    city:[],
                }
            };
            return state;
            case sideMenu.CLEAR_SUPPLIER:
            state = {
                ...state,
                supplierData: {}
                
            };
            return state;
            case supplierActionTypes.UPDATE_REPORT_CUSTOMER:
            state = {
                ...state,
                reportsCustomer: data,

            };
            return state;
            case supplierActionTypes.CLEAR_CUSTOMER_DATA:
                state ={
                    ...state,
                    reportsCustomer:[],
                };
                return state;
        default:
            return state;
    };
};