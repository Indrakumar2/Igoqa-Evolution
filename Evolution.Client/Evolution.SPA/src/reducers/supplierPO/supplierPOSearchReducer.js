import { supplierPOActionTypes } from '../../constants/actionTypes';

export const SupplierPOSearchReducer = (state, actions) => {
    const { type, data } = actions;
    switch (type) {
        case supplierPOActionTypes.FETCH_SUPPLIER_PO_SEARCH:

            state = {
                ...state,
                supplierPOSearchData: data
            };
            return state;

        case supplierPOActionTypes.CLEAR_SUPPLIER_PO_SEARCH:
            state = {
                ...state,
                supplierPOSearchData: []
            };
            return state;

        case supplierPOActionTypes.SAVE_SALECTED_SUPPLIER_PO_ID:
            state = {
                ...state,
                supplierPOData: data
            };
            return state;
        case supplierPOActionTypes.FETCH_SUPPLIER_LIST_FOR_SUPPLIER_PO:
            state = {
                ...state,
                supplierList: data
            };
            return state;
        case supplierPOActionTypes.CLEAR_SUPPLIER_LIST_FOR_SUPPLIER_PO:
            state = {
                ...state,
                supplierList: []
            };
            return state;
        default:
            return state;
    }
};