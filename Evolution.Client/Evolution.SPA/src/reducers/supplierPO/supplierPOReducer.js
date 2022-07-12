import { supplierPOActionTypes, sideMenu } from '../../constants/actionTypes';

export const SupplierPOReducer = (state, actions) => {
    const { type, data } = actions;
    switch (type) {
        case supplierPOActionTypes.FETCH_SUPPLIER_PO_DATA:
            state = {
                ...state,
                supplierPOData: data,
                isbtnDisable: true
            };            
            return state;
        case supplierPOActionTypes.CLEAR_SUPPLIER_PO_DATA:
            state = {
                ...state,
                supplierPOData: {}
            };
            return state;
        case supplierPOActionTypes.ADD_SUB_SUPPLIER:
            if (state.supplierPOData == null || state.supplierPOData.SupplierPOSubSupplier == null) {
                state = {
                    ...state,
                    supplierPOData: {
                        ...state.supplierPOData,
                        SupplierPOSubSupplier: [],
                    },
                    isbtnDisable: false
                };
            }
            state = {
                ...state,
                supplierPOData: {
                    ...state.supplierPOData,
                    SupplierPOSubSupplier: [
                        ...state.supplierPOData.SupplierPOSubSupplier, data
                    ],
                },
                isbtnDisable: false
            };
            return state;
        case supplierPOActionTypes.UPDATE_SUB_SUPPLIER:
            state = {
                ...state,
                supplierPOData: {
                    ...state.supplierPOData,
                    SupplierPOSubSupplier: data
                },
                isbtnDisable: false
            };
            return state;
        case supplierPOActionTypes.DELETE_SUB_SUPPLIER:
            state = {
                ...state,
                supplierPOData: {
                    ...state.supplierPOData,
                    SupplierPOSubSupplier: data,
                },
                isbtnDisable: false
            };
            return state;
        case supplierPOActionTypes.UPDATE_SUPPLIER_DETAILS:
            if (state.supplierPOData == null) {
                state = {
                    ...state,
                    supplierPOData: {
                        ...state.supplierPOData,
                        SupplierPOInfo: {},
                    },
                };
            }

            state = {
                ...state,
                supplierPOData: {
                    ...state.supplierPOData,
                    SupplierPOInfo: data
                },
                isbtnDisable: false
            };
            return state; 
            case supplierPOActionTypes.SAVE_PROJECT_DETAILS_FOR_SUPPLIER:
            state = {
                ...state,
                projectDetails:data,
            };
            return state; 

            case sideMenu.HANDLE_MENU_ACTION:
                    state = {
                        ...state,
                        supplierPOSearchData: []
                    };
                    return state;
            //For ITK D-456 -Starts
            case supplierPOActionTypes.SUPPLIER_PO_VIEW_ACCESS:
                    state = {
                        ...state,
                        supplierPOViewMode:data,
                    };
                    return state; 
            //For ITK D-456 -Ends
        default:
            return state;
    };
};