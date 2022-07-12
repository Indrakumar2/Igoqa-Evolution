import { assignmentsActionTypes } from '../../constants/actionTypes';

export const GeneralDetailsReducer = (state, action) => {
    const { type, data } = action;
    switch (type) {
        case assignmentsActionTypes.FETCH_COORDINATOR_FOR_CONTRACT_HOLDING_COMPANY:
            state = {
                ...state,
                contractHoldingCompanyCoordinators: data
            };
            return state;
        case assignmentsActionTypes.FETCH_COORDINATOR_FOR_OPERATING_COMPANY:
            state = {
                ...state,
                operatingCompanyCoordinators: data
            };
            return state;
        case assignmentsActionTypes.FETCH_CUSTOMER_ASSIGNMENT_CONTACT:
            state = {
                ...state,
                customerAssignmentContact: data
            };
            return state;
        case assignmentsActionTypes.FETCH_ASSIGNMENT_COMPANY_ADDRESS:
            state = {
                ...state,
                assignmentCompanyAddress: data
            };
            return state;
        case assignmentsActionTypes.FETCH_SUPPLIER_PO:
            state = {
                ...state,
                supplierPO: data
            };
            return state;
        case assignmentsActionTypes.UPDATE_ASSIGNMENT_GENERAL_DETAILS:
            state = {
                ...state,
                assignmentDetail: {
                    ...state.assignmentDetail,
                    AssignmentInfo: data,
                    AssignmentInterCompanyDiscounts:{
                        ...state.assignmentDetail.AssignmentInterCompanyDiscounts,
                        assignmentContractHoldingCompanyName: data.assignmentContractHoldingCompany,
                        assignmentContractHoldingCompanyCode: data.assignmentContractHoldingCompanyCode,
                        assignmentOperatingCompanyName: data.assignmentOperatingCompany,
                        assignmentOperatingCompanyCode: data.assignmentOperatingCompanyCode,
                    },
                },
                AssignmentLifeCycleOld:data.AssignmentLifeCycleOld,
                AssignmentSupplierPOOld:data.AssignmentSupplierPOOld,
                isbtnDisable: false
            };
            return state;
        case assignmentsActionTypes.FETCH_ASSIGNMENT_TIMESHEET_STATE:
            state = {
                ...state,
                workLocationState: data
            };
            return state;
        case assignmentsActionTypes.FETCH_ASSIGNMENT_TIMESHEET_CITY:
            state = {
                ...state,
                workLocationCity: data
            };
            return state;
        default: return state;
    }
};