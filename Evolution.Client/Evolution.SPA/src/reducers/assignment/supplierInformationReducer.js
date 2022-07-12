import { assignmentsActionTypes } from '../../constants/actionTypes';

export const SupplierInformationReducer = (state,action) =>{
    const { type, data } = action;
    switch (type) {
        case assignmentsActionTypes.FETCH_SUPPLIER_CONTACTS:
            state = {
                ...state,
                supplierContacts:data,
            };
            return state;
        case assignmentsActionTypes.FETCH_SUBSUPPLIERS:
            state = {
                ...state,
                subsuppliers:data
            };
            return state;

            case assignmentsActionTypes.UPDATE_SUBSUPPLIER:
                state = {
                    ...state,
                    subsuppliers:data
                };
                return state;

        case assignmentsActionTypes.FETCH_SUBSUPPLIER_CONTACTS:
            state = {
                ...state,
                subsupplierContacts:data
            };
            return state;
        case assignmentsActionTypes.FETCH_MAINSUPPLIER_NAME:
            state = {
                ...state,
                assignmentDetail: {
                    ...state.assignmentDetail,
                    AssignmentInfo:{
                        ...state.assignmentDetail.AssignmentInfo,
                        assignmentSupplierName:data,
                    }         
                }
            };
            return state;
        case assignmentsActionTypes.FETCH_MAINSUPPLIER_ID:
        state = {
            ...state,
            assignmentDetail: {
                ...state.assignmentDetail,
                AssignmentInfo:{
                    ...state.assignmentDetail.AssignmentInfo,
                    assignmentSupplierId:data,
                }         
            }
        };
        return state;
        case assignmentsActionTypes.ADD_SUBSUPPLIER_INFORMATION:
            state = {
                ...state,
                assignmentDetail: {
                    ...state.assignmentDetail,
                    AssignmentSubSuppliers: data.assignmentSubSupplier
                },
                isbtnDisable:data.isFromSupplierPO === true ? data.isFromSupplierPO : false
            };
            return state; 
        case assignmentsActionTypes.UPDATE_SUBSUPPLIER_INFORMATION:
            state = {
                ...state,
                assignmentDetail: {
                    ...state.assignmentDetail,
                    AssignmentSubSuppliers: data
                },
                isbtnDisable:false
            };                                            
            return state; 
        case assignmentsActionTypes.ADD_MAIN_SUPPLIER_INFORMATION:
            state = {
                ...state,
                assignmentDetail: {
                    ...state.assignmentDetail,
                    AssignmentInfo:data,
                },
                isbtnDisable:false
            };                                            
            return state;
        case assignmentsActionTypes.ADD_ASSIGNMENT_SUB_SUPPLIER_TECH_SPEC:
            state = {
                ...state,
                assignmentDetail: {
                    ...state.assignmentDetail,
                    AssignmentSubSuppliers: data
                },
                isbtnDisable:false
            };                                            
            return state; 
        case assignmentsActionTypes.UPDATE_ASSIGNMENT_SUB_SUPPLIER_TECH_SPEC:
            state = {
                ...state,
                assignmentDetail:{
                    ...state.assignmentDetail,
                    AssignmentTechnicalSpecialists:data
                },
                isbtnDisable:false
            };                                          
            return state; 
        case assignmentsActionTypes.DELETE_SUPPLIER_TECHSPEC:
            state = {
                ...state,
                assignmentDetail: {
                    ...state.assignmentDetail,
                    AssignmentSubSuppliers: data
                },
                isbtnDisable:false
            };
            return state;
        case assignmentsActionTypes.FETCH_ASSIGNMENT_TECHSPEC:
            state = {
                ...state,
                assignedSubSupplierTS:data
            };                                          
            return state; 
        case assignmentsActionTypes.UPDATE_MAIN_SUPPLIER_INFO:
            state = {
                ...state,
                assignmentDetail: {
                    ...state.assignmentDetail,
                    AssignmentSubSuppliers: data.AssignmentSubSupplierNewData,
                },
                // AssignmentSubSupplierOld:data.AssignmentSubSupplierOldData, // unwanted code
                isbtnDisable:false
            };
            return state;
        case assignmentsActionTypes.DELETE_SUPPLIER_INFO:
            state = {
                ...state,
                assignmentDetail: {
                    ...state.assignmentDetail,
                    AssignmentSubSuppliers: data
                },
                isSupplierPOChanged:true,
                isbtnDisable:false
            };
            return state;
        case assignmentsActionTypes.SUB_SUPPLIER_CONTACT_UPDATE_STATUS:
            state = {
                ...state,
                isSubSupplierContactUpdated: data,
            };
            return state;
        default: return state;
    }
};