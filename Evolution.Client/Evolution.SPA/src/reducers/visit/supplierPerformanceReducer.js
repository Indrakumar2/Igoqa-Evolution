import { visitActionTypes } from '../../constants/actionTypes';

export const SupplierPerformanceReducer = (state, action) => {
    const { type, data } = action;
    switch (type) {
        case visitActionTypes.FETCH_VISIT_SUPPLIER_PERFORMANCE:     
            if (state.visitDetails.VisitSupplierPerformances == null) {
                state = {
                    ...state,
                    visitDetails: {
                        ...state.visitDetails,
                        VisitSupplierPerformances: []
                    }
                };
            }
            state = {
                ...state,
                visitDetails: {
                    ...state.visitDetails,
                    VisitSupplierPerformances: data
                    // VisitSupplierPerformances: [
                    //     ...state.visitDetails.VisitSupplierPerformances, data
                    // ]
                }
            };
            return state;
        case visitActionTypes.ADD_SUPPLIER_PERFORMANCE:     
            if (state.visitDetails.VisitSupplierPerformances == null) {
                state = {
                    ...state,
                    visitDetails: {
                        ...state.visitDetails,
                        VisitSupplierPerformances: []
                    }
                };
            }
            state = {
                ...state,
                visitDetails: {
                    ...state.visitDetails,                    
                    VisitSupplierPerformances: [
                        data,
                        ...state.visitDetails.VisitSupplierPerformances
                    ]
                },
                isbtnDisable: false
            };
            return state;   
        case visitActionTypes.UPDATE_SUPPLIER_PERFORMANCE:    
            state = {
                ...state,
                visitDetails: {
                    ...state.visitDetails,
                    VisitSupplierPerformances: data
                },
                isbtnDisable: false
            };
            return state;
        case visitActionTypes.DELETE_SUPPLIER_PERFORMANCE:
            state = {
                ...state,
                visitDetails:{
                    ...state.visitDetails,
                    VisitSupplierPerformances:data
                },
                isbtnDisable: false
            };
            return state;
        case visitActionTypes.FETCH_SUPPLIER_PERFORMANCE_TYPE:
            state = {
                ...state,
                supplierPerformanceTypeList: data
            };
            return state;
        default:
            return state;
    }
};