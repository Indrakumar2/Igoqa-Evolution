import { supplierActionTypes } from '../../constants/actionTypes';

export const SupplierDetailReducer = (state, actions) => {
    const { type, data } = actions;
    switch (type) {
        case supplierActionTypes.ADD_SUPPLIER_DETAILS:
            state = {
                ...state,
                supplierData:{
                    ...state.supplierData,
                    SupplierInfo:data
                },
                isbtnDisable:false
            };
            return state;
        case supplierActionTypes.ADD_SUPPLIER_CONTACT:
            state = {
                ...state,
                supplierData: {
                    ...state.supplierData,
                    SupplierContacts:data
                },
                isbtnDisable:false
            };
            return state;
        case supplierActionTypes.UPDATE_SUPPLIER_CONTACT:
            state = {
                ...state,
                supplierData: {
                    ...state.supplierData,
                    SupplierContacts:data
                },
                isbtnDisable:false
            };
            return state;
        case supplierActionTypes.DELETE_SUPPLIER_CONTACT:
            state = {
                ...state,
                supplierData: {
                    ...state.supplierData,
                    SupplierContacts:data
                },
                isbtnDisable:false
            };
            return state;
        case supplierActionTypes.FETCH_STATE_FOR_ADDRESS:
            state = {
                ...state,
                supplierDetails:{
                    ...state.supplierDetails,
                    stateForAddress:data
                }
            };
            return state;
        case supplierActionTypes.FETCH_CITY_FOR_ADDRESS:
            state = {
                ...state,
                supplierDetails:{
                    ...state.supplierDetails,
                    cityForAddress:data
                }
            };
            return state;
        default:
            return state;
    };
};